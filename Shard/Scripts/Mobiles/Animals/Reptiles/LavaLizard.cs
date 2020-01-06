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

/* Scripts/Mobiles/Animals/Reptiles/LavaLizard.cs
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
	[CorpseName("a lava lizard corpse")]
	[TypeAlias("Server.Mobiles.Lavalizard")]
	public class LavaLizard : BaseCreature
	{
		[Constructable]
		public LavaLizard()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a lava lizard";
			Body = 0xCE;
			Hue = Utility.RandomList(0x647, 0x650, 0x659, 0x662, 0x66B, 0x674);
			BaseSoundID = 0x5A;

			SetStr(126, 150);
			SetDex(56, 75);
			SetInt(11, 20);

			SetHits(76, 90);
			SetMana(0);

			SetDamage(6, 24);

			SetSkill(SkillName.MagicResist, 55.1, 70.0);
			SetSkill(SkillName.Tactics, 60.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 80.7;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Hides { get { return 12; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public LavaLizard(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(20, 50);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020607034631/uo.stratics.com/hunters/lavalizard.shtml
					// 20 - 50 Gold
					if (Spawning)
					{
						PackGold(20, 50);
					}
					else
					{
					}
				}
				else
				{
					if (Spawning)
						PackItem(new SulfurousAsh(Utility.Random(4, 10)));

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
