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

/* Scripts/Mobiles/Monsters/Misc/Melee/SandVortex.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a sand vortex corpse")]
	public class SandVortex : BaseCreature
	{
		[Constructable]
		public SandVortex()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a sand vortex";
			Body = 790;
			BaseSoundID = 263;

			SetStr(96, 120);
			SetDex(171, 195);
			SetInt(76, 100);

			SetHits(51, 62);

			SetDamage(3, 16);

			SetSkill(SkillName.MagicResist, 150.0);
			SetSkill(SkillName.Tactics, 70.0);
			SetSkill(SkillName.Wrestling, 80.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 28;
		}

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
				return;

			if (DateTime.Now >= m_NextAttack)
			{
				SandAttack(combatant);
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds(10.0 + (10.0 * Utility.RandomDouble()));
			}
		}

		public void SandAttack(Mobile m)
		{
			DoHarmful(m);

			m.FixedParticles(0x36B0, 10, 25, 9540, 2413, 0, EffectLayer.Waist);

			new InternalTimer(m, this).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer(Mobile m, Mobile from)
				: base(TimeSpan.FromSeconds(1.0))
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound(0x4CF);
				AOS.Damage(m_Mobile, m_From, Utility.RandomMinMax(1, 40), 90, 10, 0, 0, 0);
			}
		}

		public SandVortex(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(60, 90);
				PackItem(new Bone());
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020606082014/uo.stratics.com/hunters/sandvortex.shtml
					// ?
					// http://web.archive.org/web/20020606082014/uo.stratics.com/hunters/sandvortex.shtml
					// ?
					// http://web.archive.org/web/20021015000744/uo.stratics.com/hunters/sandvortex.shtml
					// 250-300 Gold, Bones
					// "Loot was a few hundred gold and a spine."
					// I think the "Bone" dropped by RunUO is incorrect, bones are different than 'Bone' (bone reagent vs 'human bones'
					if (Spawning)
					{
						PackGold(250, 300);
					}
					else
					{
						switch (Utility.Random(5))
						{
							case 0: PackItem(new BoneShards()); break;
							case 1: PackItem(new SpineBone()); break;
							case 2: PackItem(new RibCage()); break; ;
							case 3: PackItem(new PelvisBone()); break;
							case 4: PackItem(new Skull()); break;
						}
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
						PackItem(new Bone());

					AddLoot(LootPack.Meager, 2);
				}
			}
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
}
