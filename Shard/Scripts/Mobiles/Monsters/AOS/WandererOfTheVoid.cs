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

/* Scripts/Mobiles/Monsters/AOS/WandererOfTheVoid.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a wanderer of the void corpse")]
	public class WandererOfTheVoid : BaseCreature
	{
		[Constructable]
		public WandererOfTheVoid()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a wanderer of the void";
			Body = 316;
			BaseSoundID = 377;

			SetStr(111, 200);
			SetDex(101, 125);
			SetInt(301, 390);

			SetHits(351, 400);

			SetDamage(11, 13);

			SetSkill(SkillName.EvalInt, 60.1, 70.0);
			SetSkill(SkillName.Magery, 60.1, 70.0);
			SetSkill(SkillName.Meditation, 60.1, 70.0);
			SetSkill(SkillName.MagicResist, 50.1, 75.0);
			SetSkill(SkillName.Tactics, 60.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 70.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 44;

			//int count = Utility.RandomMinMax( 2, 3 );

			//for ( int i = 0; i < count; ++i )
			//	PackItem( new MessageInABottle( this.Map ) );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public WandererOfTheVoid(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(300, 550);
			// Category 3 MID
			PackMagicItem(1, 2, 0.10);
			PackMagicItem(1, 2, 0.05);
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
