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

/*  Scripts\Engines\ChampionSpawn\Champs\Mephitis.cs
 *	ChangeLog:
 *  03/09/07, plasma    
 *      Removed cannedevil namespace reference
 *  01/05/07, plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 *  8/16/06, Rhiannon
 *		Added speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	5/30/04 smerX
 *		Coded/Added special abilities
 *	4/xx/04 Mith
 *		Removed spawn of gold items in pack.
 *
 */

using System;
using Server;
using Server.Items;
using Server.Engines.ChampionSpawn;

namespace Server.Mobiles
{
	public class Mephitis : BaseChampion
	{
		public override ChampionSkullType SkullType { get { return ChampionSkullType.Venom; } }

		[Constructable]
		public Mephitis()
			: base(AIType.AI_Melee, 0.175, 0.350)
		{
			Body = 173;
			Name = "Mephitis";
			BaseSoundID = 0x183;

			SetStr(505, 1000);
			SetDex(102, 300);
			SetInt(402, 600);

			SetHits(3000);
			SetStam(105, 600);

			SetDamage(20, 35);

			SetSkill(SkillName.MagicResist, 70.7, 140.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 60;
		}

		public override void GenerateLoot()
		{
			if (!Core.UOAI && !Core.UOAR)
			{
				AddLoot(LootPack.UltraRich, 4);
			}
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		//public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public override void Damage(int amount, Mobile from)
		{
			base.Damage(amount, from);

			Mobile m = VerifyValidMobile(from, 9);

			if (m != null)
			{
				ShootWeb(m, 9);
				new WarpTimer(this, m, 8, TimeSpan.FromSeconds(2.0)).Start();
			}
		}

		public Mobile VerifyValidMobile(Mobile m, int tileRange)
		{
			if (m != null && m is PlayerMobile && m.AccessLevel == AccessLevel.Player || m != null && m is BaseCreature && ((BaseCreature)m).Controlled)
			{
				if (m != null && m.Map == this.Map && m.InRange(this, tileRange) && m.Alive)
					return m;
			}

			return null;
		}

		private void ShootWeb(Mobile target, int range)
		{
			Mobile m = VerifyValidMobile(target, range);

			if (m != null && !(m is BaseCreature))
			{
				//this.MovingParticles( m, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
				this.MovingParticles(m, 0x10d4, 7, 0, false, false, 3043, 4043, 0x211);
				m.Freeze(TimeSpan.FromSeconds(4.0));
				MephiWeb w = new MephiWeb(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(4.0));
				w.MoveToWorld(new Point3D(m.X, m.Y, m.Z), m.Map);
			}
		}

		private class WarpTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_To;
			private int m_Range;

			public WarpTimer(Mobile from, Mobile to, int range, TimeSpan delay)
				: base(delay)
			{
				m_From = from;
				m_To = to;
				m_Range = range;
			}

			protected override void OnTick()
			{
				if (m_From != null && m_To != null && !(m_To.Hidden) && m_To.InRange(m_From, m_Range))
					m_From.Location = new Point3D(m_To.X, m_To.Y, m_To.Z);

				this.Stop();
			}
		}

		public Mephitis(Serial serial)
			: base(serial)
		{
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
		}
	}

	public class MephiWeb : Item
	{
		private TimeSpan m_ParaTime;

		[Constructable]
		public MephiWeb(TimeSpan delay, TimeSpan paraTime)
			: base(0x10d4)
		{
			m_ParaTime = paraTime;

			this.Movable = false;

			new DeletionTimer(delay, this).Start();
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m != null && m is PlayerMobile && m.AccessLevel == AccessLevel.Player || m != null && m is BaseCreature && ((BaseCreature)m).Controlled)
			{
				if (m != null && m.Map == this.Map && m.Alive)
				{
					m.Freeze(m_ParaTime);
					m.SendMessage("You become entangled in Mephitis' web!");
				}
			}

			return base.OnMoveOver(m);
		}

		private class DeletionTimer : Timer
		{
			private Item m_ToDelete;

			public DeletionTimer(TimeSpan delay, Item todelete)
				: base(delay)
			{
				m_ToDelete = todelete;
				Priority = TimerPriority.TwentyFiveMS;
			}

			protected override void OnTick()
			{
				m_ToDelete.Delete();
				this.Stop();
			}
		}

		public MephiWeb(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); //vers
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
