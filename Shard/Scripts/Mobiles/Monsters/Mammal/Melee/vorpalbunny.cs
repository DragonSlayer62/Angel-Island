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

/* Scripts/Mobiles/Monsters/Mammal/Melee/VorpalBunny.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 1 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a vorpal bunny corpse")]
	public class VorpalBunny : BaseCreature
	{
		[Constructable]
		public VorpalBunny()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.175, 0.350)
		{
			Name = "a vorpal bunny";
			Body = 205;
			Hue = 0x480;
			BardImmune = true;

			SetStr(15);
			SetDex(2000);
			SetInt(1000);

			SetHits(2000);
			SetStam(500);
			SetMana(0);

			SetDamage(1);


			SetSkill(SkillName.MagicResist, 200.0);
			SetSkill(SkillName.Tactics, 5.0);
			SetSkill(SkillName.Wrestling, 5.0);

			Fame = 1000;
			Karma = 0;

			VirtualArmor = 4;

			DelayBeginTunnel();
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(250, 350);
				PackItem(new Carrot());
				// TODO: statue, eggs
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20021215144938/uo.stratics.com/hunters/vorpalbunny.shtml
					// 600-700 Gold, Carrots, Gems, Scrolls, Magic Items, Statue, Brightly Colored Eggs
					if (Spawning)
					{
						PackGold(600, 700);
					}
					else
					{
						PackItem(new Carrot(Utility.RandomMinMax(5, 10)));

						PackGem(1, .9);
						PackGem(1, .05);

						PackScroll(1, 7);
						PackScroll(1, 7, 0.2);

						if (Utility.RandomBool())
							PackMagicEquipment(1, 2);
						else
							PackMagicItem(1, 2, 0.30);

						PackStatue(0.02);

						if (Utility.Random(5) == 0)
							PackItem(new BrightlyColoredEggs());
					}
				}
				else
				{
					if (Spawning)
					{
						int carrots = Utility.RandomMinMax(5, 10);
						PackItem(new Carrot(carrots));

						if (Utility.Random(5) == 0)
							PackItem(new BrightlyColoredEggs());

						PackStatue(0.02);
					}

					AddLoot(LootPack.FilthyRich);
					AddLoot(LootPack.Rich, 2);
				}
			}
		}

		public class BunnyHole : Item
		{
			public BunnyHole()
				: base(0x913)
			{
				Movable = false;
				Hue = 1;
				Name = "a mysterious rabbit hole";

				Timer.DelayCall(TimeSpan.FromSeconds(40.0), new TimerCallback(Delete));
			}

			public BunnyHole(Serial serial)
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

				Delete();
			}
		}

		public virtual void DelayBeginTunnel()
		{
			Timer.DelayCall(TimeSpan.FromMinutes(3.0), new TimerCallback(BeginTunnel));
		}

		public virtual void BeginTunnel()
		{
			if (Deleted)
				return;

			new BunnyHole().MoveToWorld(Location, Map);

			Frozen = true;
			Say("* The bunny begins to dig a tunnel back to its underground lair *");
			PlaySound(0x247);

			Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(Delete));
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 1; } }

		public VorpalBunny(Serial serial)
			: base(serial)
		{
		}


		public override int GetAttackSound()
		{
			return 0xC9;
		}

		public override int GetHurtSound()
		{
			return 0xCA;
		}

		public override int GetDeathSound()
		{
			return 0xCB;
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

			DelayBeginTunnel();
		}
	}
}
