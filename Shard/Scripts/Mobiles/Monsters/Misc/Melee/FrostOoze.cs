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

/* ./Scripts/Mobiles/Monsters/Misc/Melee/FrostOoze.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
*/

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a frost ooze corpse")]
	public class FrostOoze : BaseCreature
	{
		[Constructable]
		public FrostOoze()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a frost ooze";
			Body = 94;
			BaseSoundID = 456;

			SetStr(18, 30);
			SetDex(16, 21);
			SetInt(16, 20);

			SetHits(13, 17);

			SetDamage(3, 9);

			SetSkill(SkillName.MagicResist, 5.1, 10.0);
			SetSkill(SkillName.Tactics, 19.3, 34.0);
			SetSkill(SkillName.Wrestling, 25.3, 40.0);

			Fame = 450;
			Karma = -450;

			VirtualArmor = 38;
		}

		public FrostOoze(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();

				if (Utility.RandomBool())
					PackGem();
			}
			else
			{ 
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806203419/uo.stratics.com/hunters/frostooze.shtml
					// 	Gems
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);
					}
				}
				else
				{	// Standard RunUO
					AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
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
