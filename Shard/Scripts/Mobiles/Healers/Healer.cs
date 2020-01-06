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

/* Scripts/Mobiles/Healers/Healer.cs 
 * Changelog
 *	06/28/06, Adam
 *		Logic cleanup
 */

using System;
using Server;

namespace Server.Mobiles
{
	public class Healer : BaseHealer
	{
		public override bool CanTeach { get { return Core.UOSP ? false : true; } }

		public override bool CheckTeach(SkillName skill, Mobile from)
		{
			if (!base.CheckTeach(skill, from))
				return false;

			return (skill == SkillName.Forensics)
				|| (skill == SkillName.Healing)
				|| (skill == SkillName.SpiritSpeak)
				|| (skill == SkillName.Swords);
		}

		[Constructable]
		public Healer()
		{
			Title = "the healer";

			SetSkill(SkillName.Forensics, 80.0, 100.0);
			SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);
			SetSkill(SkillName.Swords, 80.0, 100.0);
		}

		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			SBInfos.Add(new SBHealer());
		}

		public override bool CheckResurrect(Mobile m)
		{
			if (m.Criminal)
			{
				Say(501222); // Thou art a criminal.  I shall not resurrect thee.
				return false;
			}
			else if (m.Murderer)
			{
				Say(501223); // Thou'rt not a decent and good person. I shall not resurrect thee.
				return false;
			}

			return true;
		}

		public Healer(Serial serial)
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