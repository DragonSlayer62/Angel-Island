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
//  /Scripts/Items/Misc/PoisonCloth.cs
//	CHANGE LOG
//  04/17/2004 - Pulse
//		This item is totally new, created for the Angel Island shard.
//		This item is created when a piece of cloth is poisoned.
//		If this item is in a players root backpack, when they fire a ranged 
//		weapon, the ranged weapon will be poisoned for 1 round and a poison
//		charge is subtracted from this item.  When the item's PoisonCharges count
//		reaches 0 during use the item is deleted.

using System;
using Server;
using System.Text;
using Server.Mobiles;
using System.Collections;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PoisonCloth : Item
	{
		private Poison m_Poison;
		private int m_PoisonCharges;
		private double m_Delay;

		[CommandProperty(AccessLevel.GameMaster)]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int PoisonCharges
		{
			get { return m_PoisonCharges; }
			set { m_PoisonCharges = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public double Delay
		{
			get { return m_Delay; }
			set { m_Delay = value; }
		}

		[Constructable]
		public PoisonCloth()
			: base(0x175D)
		{
			Weight = 1.0;
			Hue = 63;
			Stackable = false;
			Amount = 1;
			Poison = null;
			PoisonCharges = 0;
			Delay = 0.0;
			Name = "a poison soaked rag";
		}

		public override void OnSingleClick(Mobile from)
		{
			ArrayList attrs = new ArrayList();

			if (Poison != null && PoisonCharges > 0)
				this.LabelTo(from, Name + "\n[Poisoned: " + PoisonCharges.ToString() + "]");
			else
				this.LabelTo(from, Name);
		}


		public PoisonCloth(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			Poison.Serialize(m_Poison, writer);
			writer.Write((int)m_PoisonCharges);
			writer.Write((double)m_Delay);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_Poison = Poison.Deserialize(reader);
			m_PoisonCharges = reader.ReadInt();
			m_Delay = reader.ReadDouble();
		}
	}
}