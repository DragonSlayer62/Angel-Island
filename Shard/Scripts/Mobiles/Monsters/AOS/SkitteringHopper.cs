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

/* ./Scripts/Mobiles/Monsters/AOS/SkitteringHopper.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 4 lines removed.
*/

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a skittering hopper corpse")]
	public class SkitteringHopper : BaseCreature
	{
		[Constructable]
		public SkitteringHopper()
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a skittering hopper";
			Body = 302;
			BaseSoundID = 959;

			SetStr(41, 65);
			SetDex(91, 115);
			SetInt(26, 50);

			SetHits(31, 45);

			SetDamage(3, 5);

			SetSkill(SkillName.MagicResist, 30.1, 45.0);
			SetSkill(SkillName.Tactics, 45.1, 70.0);
			SetSkill(SkillName.Wrestling, 40.1, 60.0);

			Fame = 300;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -12.9;

			VirtualArmor = 12;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public SkitteringHopper(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGold(10, 50);
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
