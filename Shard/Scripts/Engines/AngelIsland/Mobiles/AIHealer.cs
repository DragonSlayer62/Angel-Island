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
// /Scripts/Mobiles/Healers/AIHealer.cs
// Created 3/28/04 by mith, copied from EvilHealer.cs
// ChangeLog
// 4/1/04, changes by mith
//	Added Frozen and Direction properties so healer won't wander and always faces south.
// 3/28/04
//	Removed code to generate AIStinger, this has been moved to the AITeleporter.
//	Removed ability to buy/sell items and teach skills.
//	Set "AlwaysMurderer" flag to false so that NPC shows as blue.

using System;
using Server;

namespace Server.Mobiles
{
	public class AIHealer : BaseHealer
	{
		public override bool CanTeach { get { return false; } }
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
		public AIHealer()
		{
			Title = "the healer";

			Karma = -10000;

			SetSkill(SkillName.Forensics, 80.0, 100.0);
			SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);
			SetSkill(SkillName.Swords, 80.0, 100.0);

			Frozen = true;
			Direction = Direction.South;
		}

		public override bool AlwaysMurderer { get { return false; } }
		public override bool IsActiveVendor { get { return false; } }

		public override void InitSBInfo()
		{
			SBInfos.Add(new SBHealer());
		}

		public override bool CheckResurrect(Mobile m)
		{
			// This code moved to AITeleporter so users only get one per visit.
			//Item aiStinger = new Server.Items.AIStinger();
			//if ( !m.AddToBackpack( aiStinger ) )
			//aiStinger.Delete();

			return true;
		}

		public AIHealer(Serial serial)
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

