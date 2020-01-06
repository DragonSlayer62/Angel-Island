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

using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class RangerGuildmaster : BaseGuildmaster
	{
		public override NpcGuild NpcGuild { get { return NpcGuild.RangersGuild; } }

		[Constructable]
		public RangerGuildmaster()
			: base("ranger")
		{
			SetSkill(SkillName.AnimalLore, 64.0, 100.0);
			SetSkill(SkillName.Camping, 75.0, 98.0);
			SetSkill(SkillName.Hiding, 75.0, 98.0);
			SetSkill(SkillName.MagicResist, 75.0, 98.0);
			SetSkill(SkillName.Tactics, 65.0, 88.0);
			SetSkill(SkillName.Archery, 90.0, 100.0);
			SetSkill(SkillName.Tracking, 90.0, 100.0);
			SetSkill(SkillName.Stealth, 60.0, 83.0);
			SetSkill(SkillName.Fencing, 36.0, 68.0);
			SetSkill(SkillName.Herding, 36.0, 68.0);
			SetSkill(SkillName.Swords, 45.0, 68.0);
		}

		public RangerGuildmaster(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}