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

/* Scripts/Mobiles/Animals/Reptiles/LavaSnake.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a lava snake corpse")]
	[TypeAlias("Server.Mobiles.Lavasnake")]
	public class LavaSnake : BaseCreature
	{
		[Constructable]
		public LavaSnake()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a lava snake";
			Body = 52;
			Hue = Utility.RandomList(0x647, 0x650, 0x659, 0x662, 0x66B, 0x674);
			BaseSoundID = 0xDB;

			SetStr(43, 55);
			SetDex(16, 25);
			SetInt(6, 10);

			SetHits(28, 32);
			SetMana(0);

			SetDamage(1, 8);

			SetSkill(SkillName.MagicResist, 15.1, 20.0);
			SetSkill(SkillName.Tactics, 19.3, 34.0);
			SetSkill(SkillName.Wrestling, 19.3, 34.0);

			Fame = 600;
			Karma = -600;

			VirtualArmor = 24;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Meat { get { return 1; } }

		public LavaSnake(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(0, 25);
				PackItem(new SulfurousAsh());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020212100818/uo.stratics.com/hunters/lavasnake.shtml
					// 	Raw Ribs (when carved)
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
						PackItem(new SulfurousAsh());

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
