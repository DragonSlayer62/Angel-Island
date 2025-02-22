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

/* Scripts/Mobiles/Monsters/Reptile/Magic/DeepSeaSerpent.cs
 * ChangeLog
 *	9/3/10, Adam
 *		Remove MIB and SpecialNet as these are added dynamically to the creature in the harvest system
 *		Removal also allows us to add this creature as spawn without adding special loot
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  11/10/04, Froste
 *      Removed PirateHat as loot, now restricted to "brethren only" drop
 *  9/26/04, Jade
 *      Increased gold drop from (25,50) to (150,200).
 *  7/21/04, Adam
 *		CS0654: (line 101, column 18) Method 'Server.Utility.RandomBool()' referenced without parentheses
 *		Fixed a little mith'take ;p
 *	7/21/04, mith
 *		Added PirateHat as loot, 5% drop.
 *	6/29/04, Pix
 *		Fixed MIB loot to spawn for the current facet.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a deep sea serpents corpse")]
	public class DeepSeaSerpent : BaseCreature
	{
		[Constructable]
		public DeepSeaSerpent()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a deep sea serpent";
			Body = 150;
			BaseSoundID = 447;

			SetStr(251, 425);
			SetDex(87, 135);
			SetInt(87, 155);

			SetHits(151, 255);

			SetDamage(6, 14);

			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 60.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 70.0);

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 60;
			CanSwim = true;
			CantWalk = true;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override bool HasBreath { get { return true; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Meat { get { return 10; } }
		public override int Scales { get { return (Core.UOAI || Core.UOAR || Core.PublishDate < Core.PlagueOfDespair) ? 0 : 8; } }
		public override ScaleType ScaleType { get { return ScaleType.Blue; } }

		public DeepSeaSerpent(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(150, 200);
				PackItem(new SulfurousAsh(4));
				PackItem(new BlackPearl(4));
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20011213223007/uo.stratics.com/hunters/deepseaserpent.shtml
					// 	Special Fishing Net, 1 Fish Steak (carved)

					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
						// even though loot says, SpecialFishingNet, I don't believe it should ever be added as loot.
						// the SpecialFishingNet is added dynamically by the fishing system.
						// fishing.cs
					}
				}
				else
				{
					if (Spawning)
					{
						if (Utility.RandomBool())
							PackItem(new SulfurousAsh(4));
						else
							PackItem(new BlackPearl(4));

						// even though loot says, SpecialFishingNet, I don't believe it should ever be added as loot.
						// the SpecialFishingNet is added dynamically by the fishing system.
						// fishing.cs
					}

					AddLoot(LootPack.Meager);
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
