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

/* Scripts/Mobiles/Monsters/Misc/Melee/Golem.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 11 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a golem corpse")]
	public class Golem : BaseCreature
	{
		public override bool IsScaredOfScaryThings { get { return false; } }
		public override bool IsScaryToPets { get { return true; } }

		public override bool IsBondable { get { return false; } }

		[Constructable]
		public Golem()
			: this(false, 1.0)
		{
		}

		[Constructable]
		public Golem(bool summoned, double scalar)
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a golem";
			Body = 752;
			BardImmune = true;

			if (summoned)
				Hue = 2101;

			SetStr((int)(251 * scalar), (int)(350 * scalar));
			SetDex((int)(76 * scalar), (int)(100 * scalar));
			SetInt((int)(101 * scalar), (int)(150 * scalar));

			SetHits((int)(151 * scalar), (int)(210 * scalar));

			SetDamage((int)(13 * scalar), (int)(24 * scalar));


			SetSkill(SkillName.MagicResist, (150.1 * scalar), (190.0 * scalar));
			SetSkill(SkillName.Tactics, (60.1 * scalar), (100.0 * scalar));
			SetSkill(SkillName.Wrestling, (60.1 * scalar), (100.0 * scalar));

			if (summoned)
			{
				Fame = 10;
				Karma = 10;
			}
			else
			{
				Fame = 3500;
				Karma = -3500;
			}

			ControlSlots = 4;
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				if (!Summoned)
				{
					PackItem(new IronIngot(Utility.RandomMinMax(13, 21)));

					if (0.1 > Utility.RandomDouble())
						PackItem(new PowerCrystal());

					if (0.15 > Utility.RandomDouble())
						PackItem(new ClockworkAssembly());

					if (0.2 > Utility.RandomDouble())
						PackItem(new ArcaneGem());

					if (0.25 > Utility.RandomDouble())
						PackItem(new Gears());
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806222514/uo.stratics.com/hunters/irongolem.shtml
					// 11-25 Ingots, Arcane Gems, Gems, Gears, Power Crystals, Clockwork Assembly
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{	// don't think you can summon these in this era, but better to be safe
						if (!Summoned)
						{
							PackItem(new IronIngot(Utility.RandomMinMax(11, 25)));

							if (0.2 > Utility.RandomDouble())
								PackItem(new ArcaneGem());

							PackGem(1, .9);
							PackGem(1, .05);

							if (0.25 > Utility.RandomDouble())
								PackItem(new Gears());

							if (0.1 > Utility.RandomDouble())
								PackItem(new PowerCrystal());

							if (0.15 > Utility.RandomDouble())
								PackItem(new ClockworkAssembly());

						}
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
					{
						if (!Summoned)
						{
							PackItem(new IronIngot(Utility.RandomMinMax(13, 21)));

							if (0.1 > Utility.RandomDouble())
								PackItem(new PowerCrystal());

							if (0.15 > Utility.RandomDouble())
								PackItem(new ClockworkAssembly());

							if (0.2 > Utility.RandomDouble())
								PackItem(new ArcaneGem());

							if (0.25 > Utility.RandomDouble())
								PackItem(new Gears());
						}
					}
				}
			}
		}

		public override bool DeleteOnRelease { get { return true; } }

		public override int GetAngerSound()
		{
			return 541;
		}

		public override int GetIdleSound()
		{
			if (!Controlled)
				return 542;

			return base.GetIdleSound();
		}

		public override int GetDeathSound()
		{
			if (!Controlled)
				return 545;

			return base.GetDeathSound();
		}

		public override int GetAttackSound()
		{
			return 562;
		}

		public override int GetHurtSound()
		{
			if (Controlled)
				return 320;

			return base.GetHurtSound();
		}

		public override bool AutoDispel { get { return !Controlled; } }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (0.2 > Utility.RandomDouble())
				defender.Combatant = null;
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (Controlled || Summoned)
			{
				Mobile master = (this.ControlMaster);

				if (master == null)
					master = this.SummonMaster;

				if (master != null && master.Player && master.Map == this.Map && master.InRange(Location, 20))
				{
					if (master.Mana >= amount)
					{
						master.Mana -= amount;
					}
					else
					{
						amount -= master.Mana;
						master.Mana = 0;
						master.Damage(amount);
					}
				}
			}

			base.OnDamage(amount, from, willKill);
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public Golem(Serial serial)
			: base(serial)
		{
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
