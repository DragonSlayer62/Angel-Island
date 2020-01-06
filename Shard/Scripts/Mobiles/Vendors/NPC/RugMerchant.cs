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

/* SScripts\Mobiles\Vendors\NPC\RugMerchant.CS
 * ChangeLog
 *  9/16/04, Pigpen
 * 		Created RugMerchant.cs
 */

using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class RugMerchant : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public RugMerchant()
			: base("the Rug Merchant")
		{
			//SetSkill( SkillName.Camping, 55.0, 78.0 );
			//SetSkill( SkillName.Alchemy, 60.0, 83.0 );
			//SetSkill( SkillName.AnimalLore, 85.0, 100.0 );
			//SetSkill( SkillName.Cooking, 45.0, 68.0 );
			//SetSkill( SkillName.Tracking, 36.0, 68.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBRugMerchant());
		}

		public RugMerchant(Serial serial)
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