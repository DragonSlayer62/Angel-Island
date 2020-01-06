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

/* ./Scripts/Mobiles/Monsters/Humanoid/Melee/GazerLarva.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
*/

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a gazer larva corpse")]
	public class GazerLarva : BaseCreature
	{
		[Constructable]
		public GazerLarva()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a gazer larva";
			Body = 778;
			BaseSoundID = 377;

			SetStr(76, 100);
			SetDex(51, 75);
			SetInt(56, 80);

			SetHits(36, 47);

			SetDamage(2, 9);

			SetSkill(SkillName.MagicResist, 70.0);
			SetSkill(SkillName.Tactics, 70.0);
			SetSkill(SkillName.Wrestling, 70.0);

			Fame = 900;
			Karma = -900;

			VirtualArmor = 25;
		}

		public override int Meat { get { return 1; } }

		public GazerLarva(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new Nightshade(Utility.RandomMinMax(2, 3)));
				PackGold(0, 25);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// no larva circa 02 2002
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					if (Spawning)
						PackItem(new Nightshade(Utility.RandomMinMax(2, 3)));

					AddLoot(LootPack.Poor);
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
