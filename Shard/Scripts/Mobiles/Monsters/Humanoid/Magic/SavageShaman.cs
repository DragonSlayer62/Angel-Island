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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/SavageShaman.cs
 * ChangeLog
 *	07/23/08, weaver
 *		Automated IPooledEnumerable optimizations. 2 loops updated.
 *  12/03/06 Taran Kain
 *      Set Female to RandomBool in body selection.
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *  03/14/05, Lego 
 *      removed bandages
 *	12/15/04, Pix
 *		Removed damage mod to big pets.
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/10/04, Froste
 *      Implemented new random IOB drop system and changed drop change to 12%
 *	11/05/04, Pigpen
 *		Made changes for Implementation of IOBSystem. Changes include:
 *		Removed IsEnemy and Aggressive Action Checks. These are now handled in BaseCreature.cs
 *		Set Creature IOBAlignment to Savage.
 *		Set DeerMask drop to set mask to IOBAlignment Savage.
 *	8/23/04, Adam
 *		Increase gold to 200-250
 *		Increase berry drop to 20%
 *	8/9/04, Adam
 *		1. Add 10-20 Ginseng to drop
 *	7/29/04 smerX
 *		Set DeerMask drop to 5%
 *	7/29/04, mith
 *		Included DeerMask()
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *	6/11/04, mith
 *		Moved the equippable combat items out of OnBeforeDeath()
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a savage corpse")]
	public class SavageShaman : BaseCreature
	{
		[Constructable]
		public SavageShaman()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{

			IOBAlignment = IOBAlignment.Savage;
			ControlSlots = 2;

			SetStr(126, 145);
			SetDex(91, 110);
			SetInt(161, 185);

			SetDamage(4, 10);

			SetSkill(SkillName.EvalInt, 77.5, 100.0);
			SetSkill(SkillName.Fencing, 62.5, 85.0);
			SetSkill(SkillName.Macing, 62.5, 85.0);
			SetSkill(SkillName.Magery, 72.5, 95.0);
			SetSkill(SkillName.Meditation, 77.5, 100.0);
			SetSkill(SkillName.MagicResist, 77.5, 100.0);
			SetSkill(SkillName.Swords, 62.5, 85.0);
			SetSkill(SkillName.Tactics, 62.5, 85.0);
			SetSkill(SkillName.Wrestling, 62.5, 85.0);

			Fame = 1000;
			Karma = -1000;

			InitBody();
			InitOutfit();

		}

		public override int Meat { get { return 1; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }

		public override void InitBody()
		{
			Name = NameList.RandomName("savage shaman");

			if (Female = Utility.RandomBool())
				Body = 184;
			else
				Body = 183;
		}
		public override void InitOutfit()
		{
			WipeLayers();

			AddItem(new BoneArms());
			AddItem(new BoneLegs());

			if (Core.UOAI || Core.UOAR)
			{
				AddItem(new DeerMask(), 1);			// always newbied on AI
			}
			else
				AddItem(new DeerMask(), 0.90);		// TODO: newbie 90% of the time (10% drop)
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy(Mobile m, RelationshipFilter filter)
		{
			if (!Core.UOAI && !Core.UOAR)
				// Ai uses HUE value and not the BodyMod as there is no sitting graphic
				if ((m.BodyMod == 183 || m.BodyMod == 184) || m.HueMod == 0)
					return false;

			return base.IsEnemy(m, filter);
		}

		public override void AggressiveAction(Mobile aggressor, bool criminal)
		{
			base.AggressiveAction(aggressor, criminal);

			if (!Core.UOAI && !Core.UOAR)
			{	// Ai uses HUE value and not the BodyMod as there is no sitting graphic
				if ((aggressor.BodyMod == 183 || aggressor.BodyMod == 184) || aggressor.HueMod == 0)
				{
					AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
					aggressor.BodyMod = 0;
					aggressor.HueMod = -1;
					aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
					aggressor.PlaySound(0x307);
					aggressor.SendLocalizedMessage(1040008); // Your skin is scorched as the tribal paint burns away!

					if (aggressor is PlayerMobile)
						((PlayerMobile)aggressor).SavagePaintExpiration = TimeSpan.Zero;
				}
			}
		}

		public override void AlterMeleeDamageTo(Mobile to, ref int damage)
		{
			if (!Core.UOAI && !Core.UOAR)
				if (to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Daemon)
					damage *= 3;
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if (0.1 > Utility.RandomDouble())
				BeginSavageDance();
		}

		public void BeginSavageDance()
		{
			ArrayList list = new ArrayList();

			IPooledEnumerable eable = this.GetMobilesInRange(8);
			foreach (Mobile m in eable)
			{
				if (m != this && m is SavageShaman)
					list.Add(m);
			}
			eable.Free();

			Animate(111, 5, 1, true, false, 0); // Do a little dance...

			if (AIObject != null)
				AIObject.NextMove = DateTime.Now + TimeSpan.FromSeconds(1.0);

			if (list.Count >= 3)
			{
				for (int i = 0; i < list.Count; ++i)
				{
					SavageShaman dancer = (SavageShaman)list[i];

					dancer.Animate(111, 5, 1, true, false, 0); // Get down tonight...

					if (dancer.AIObject != null)
						dancer.AIObject.NextMove = DateTime.Now + TimeSpan.FromSeconds(1.0);
				}

				Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(EndSavageDance));
			}
		}

		public void EndSavageDance()
		{
			if (Deleted)
				return;

			ArrayList list = new ArrayList();

			IPooledEnumerable eable = this.GetMobilesInRange(8);
			foreach (Mobile m in eable)
				list.Add(m);
			eable.Free();

			if (list.Count > 0)
			{
				switch (Utility.Random(3))
				{
					case 0: /* greater heal */
						{
							foreach (Mobile m in list)
							{
								bool isFriendly = (m is Savage || m is SavageRider || m is SavageShaman || m is SavageRidgeback);

								if (!isFriendly)
									continue;

								if (m.Poisoned || MortalStrike.IsWounded(m) || !CanBeBeneficial(m))
									continue;

								DoBeneficial(m);

								// Algorithm: (40% of magery) + (1-10)

								int toHeal = (int)(Skills[SkillName.Magery].Value * 0.4);
								toHeal += Utility.Random(1, 10);

								m.Heal(toHeal);

								m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
								m.PlaySound(0x202);
							}

							break;
						}
					case 1: /* lightning */
						{
							foreach (Mobile m in list)
							{
								bool isFriendly = (m is Savage || m is SavageRider || m is SavageShaman || m is SavageRidgeback);

								if (isFriendly)
									continue;

								if (!CanBeHarmful(m))
									continue;

								DoHarmful(m);

								double damage;

								if (Core.AOS)
								{
									int baseDamage = 6 + (int)(Skills[SkillName.EvalInt].Value / 5.0);

									damage = Utility.RandomMinMax(baseDamage, baseDamage + 3);
								}
								else
								{
									damage = Utility.Random(12, 9);
								}

								m.BoltEffect(0);

								SpellHelper.Damage(TimeSpan.FromSeconds(0.25), m, this, damage, 0, 0, 0, 0, 100);
							}

							break;
						}
					case 2: /* poison */
						{
							foreach (Mobile m in list)
							{
								bool isFriendly = (m is Savage || m is SavageRider || m is SavageShaman || m is SavageRidgeback);

								if (isFriendly)
									continue;

								if (!CanBeHarmful(m))
									continue;

								DoHarmful(m);

								if (m.Spell != null)
									m.Spell.OnCasterHurt();

								m.Paralyzed = false;

								double total = Skills[SkillName.Magery].Value + Skills[SkillName.Poisoning].Value;

								double dist = GetDistanceToSqrt(m);

								if (dist >= 3.0)
									total -= (dist - 3.0) * 10.0;

								int level;

								if (total >= 200.0 && Utility.Random(1, 100) <= 10)
									level = 3;
								else if (total > 170.0)
									level = 2;
								else if (total > 130.0)
									level = 1;
								else
									level = 0;

								m.ApplyPoison(this, Poison.GetPoison(level));

								m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
								m.PlaySound(0x474);
							}

							break;
						}
				}
			}
		}

		public SavageShaman(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(200, 250);
				PackReg(10, 15);
				//PackItem( new Bandage( Utility.RandomMinMax( 1, 15 ) ) );

				if (Utility.RandomDouble() < 0.30)
					PackItem(new TribalBerry());

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);

				// pack bulk reg
				PackItem(new Ginseng(Utility.RandomMinMax(10, 20)));

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020213040553/uo.stratics.com/hunters/savage_shaman.shtml
					// 20-40 Gold, bone arms, bone legs, deer mask, pouch of reagents (1-5 of each)
					if (Spawning)
					{
						PackGold(20, 40);
					}
					else
					{
						PackItem(typeof(BoneArms), 0.8);
						PackItem(typeof(BoneLegs), 0.8);
						PackItem(typeof(DeerMask), 0.12);
						PackItem(new BagOfReagents(Utility.Random(1, 5)));
					}
				}
				else
				{
					if (Spawning)
					{
						PackReg(10, 15);
						PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

						// http://www.uoguide.com/Savage_Empire
						// http://uo.stratics.com/secrets/archive/orcsavage.shtml
						if (Core.PublishDate >= Core.EraSAVE)
							if (0.1 > Utility.RandomDouble())
								PackItem(new TribalBerry());
					}

					AddLoot(LootPack.Average);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
