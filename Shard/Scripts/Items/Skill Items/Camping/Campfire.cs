/*
 *	This program is the CONFIDENTIAL and PROPRIETARY property 
 *	of Tomasello Software LLC. Any unauthorized use, reproduction or
 *	transfer of this computer program is strictly prohibited.
 *
 *      Copyright (c) 2004 Tomasello Software LLC.
 *	This is an unpublished work, and is subject to limited distribution and
 *	restricted disclosure only. ALL RIGHTS RESERVED.
 *
 *			RESTRICTED RIGHTS LEGEND
 *	Use, duplication, or disclosure by the Government is subject to
 *	restrictions set forth in subparagraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */

/* Items/Skill Items/Camping/Campfire.cs
 * CHANGELOG:
 *	2/22/11, Adam
 *		o Update to RunUO 2.0
 *  11/21/06, Plasma
 *      Changed ctor to not start secure timer if in a no camp zone
 *	11/6/06, Adam
 *		Remove debug output: Console.WriteLine("Region is a house region");
 *		*shakes fist at weaver*
 *	08/14/06, weaver
 *		Changed camping to assume you have a bedroll handy if you're in a tent.
 *	10/22/04, Pix
 *		Changed camping to not use campfireregion.
 *	9/11/04, Pixie
 *		Added OwnerUsedBedroll so that we can ensure the owner used the bedroll and log him/her
 *		out properly.
 *	7/25/04, Pixie
 *		Made sure that the region passed into new CampfireRegion isn't another CampfireRegion.
 *	7/15/04, Pixie
 *		Made it so spells and lightlevels use behavior for the region that the campfire is created in
 *	5/10/04, Pixie
 *		Initial working revision
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public enum CampfireStatus
	{
		Burning,
		Extinguishing,
		Off
	}

	public class Campfire : Item
	{
		public static readonly int SecureRange = 7;

		private static readonly Hashtable m_Table = new Hashtable();

		public static CampfireEntry GetEntry(Mobile player)
		{
			return (CampfireEntry)m_Table[player];
		}

		public static void RemoveEntry(CampfireEntry entry)
		{
			m_Table.Remove(entry.Player);
			entry.Fire.m_Entries.Remove(entry);
		}

		private Timer m_Timer;
		private DateTime m_Created;

		private ArrayList m_Entries;

		public Campfire()
			: base(0xDE3)
		{
			Movable = false;
			Light = LightType.Circle300;

			m_Entries = new ArrayList();

			m_Created = DateTime.Now;
			m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), new TimerCallback(OnTick));
		}

		public Campfire(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime Created
		{
			get { return m_Created; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public CampfireStatus Status
		{
			get
			{
				switch (this.ItemID)
				{
					case 0xDE3:
						return CampfireStatus.Burning;

					case 0xDE9:
						return CampfireStatus.Extinguishing;

					default:
						return CampfireStatus.Off;
				}
			}
			set
			{
				if (this.Status == value)
					return;

				switch (value)
				{
					case CampfireStatus.Burning:
						this.ItemID = 0xDE3;
						this.Light = LightType.Circle300;
						break;

					case CampfireStatus.Extinguishing:
						this.ItemID = 0xDE9;
						this.Light = LightType.Circle150;
						break;

					default:
						this.ItemID = 0xDEA;
						this.Light = LightType.ArchedWindowEast;
						ClearEntries();
						break;
				}
			}
		}

		private void OnTick()
		{
			DateTime now = DateTime.Now;
			TimeSpan age = now - this.Created;

			if (age >= TimeSpan.FromSeconds(100.0))
				this.Delete();
			else if (age >= TimeSpan.FromSeconds(90.0))
				this.Status = CampfireStatus.Off;
			else if (age >= TimeSpan.FromSeconds(60.0))
				this.Status = CampfireStatus.Extinguishing;

			if (this.Status == CampfireStatus.Off || this.Deleted)
				return;

			foreach (CampfireEntry entry in new ArrayList(m_Entries))
			{
				if (!entry.Valid || entry.Player.NetState == null)
				{
					RemoveEntry(entry);
				}
				else if (!entry.Safe && now - entry.Start >= TimeSpan.FromSeconds(30.0))
				{
					entry.Safe = true;
					entry.Player.SendLocalizedMessage(500621); // The camp is now secure.
				}
			}

			IPooledEnumerable eable = this.GetClientsInRange(SecureRange);

			foreach (NetState state in eable)
			{
				PlayerMobile pm = state.Mobile as PlayerMobile;

				if (pm != null && GetEntry(pm) == null)
				{
					CampfireEntry entry = new CampfireEntry(pm, this);

					m_Table[pm] = entry;
					m_Entries.Add(entry);

					pm.SendLocalizedMessage(500620); // You feel it would take a few moments to secure your camp.
				}
			}

			eable.Free();
		}

		private void ClearEntries()
		{
			if (m_Entries == null)
				return;

			foreach (CampfireEntry entry in new ArrayList(m_Entries))
			{
				RemoveEntry(entry);
			}
		}

		public override void OnAfterDelete()
		{
			if (m_Timer != null)
				m_Timer.Stop();

			ClearEntries();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			this.Delete();
		}
	}

	public class CampfireEntry
	{
		private PlayerMobile m_Player;
		private Campfire m_Fire;
		private DateTime m_Start;
		private bool m_Safe;

		public PlayerMobile Player { get { return m_Player; } }
		public Campfire Fire { get { return m_Fire; } }
		public DateTime Start { get { return m_Start; } }

		public bool Valid
		{
			get { return !Fire.Deleted && Fire.Status != CampfireStatus.Off && Player.Map == Fire.Map && Player.InRange(Fire, Campfire.SecureRange); }
		}

		public bool Safe
		{
			get { return Valid && m_Safe; }
			set { m_Safe = value; }
		}

		public CampfireEntry(PlayerMobile player, Campfire fire)
		{
			m_Player = player;
			m_Fire = fire;
			m_Start = DateTime.Now;
			m_Safe = false;
		}
	}
}