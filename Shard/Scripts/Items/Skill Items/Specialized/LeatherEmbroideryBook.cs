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

/* Scripts/Items/Skill Items/Specialized/LeatherEmbroideryBook.cs
 * ChangeLog:
 *	04/02/2008
 *		Initial creation
 */

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class LeatherEmbroideryBook : Item
	{
		public const double c_RequiredTailoring = 90.0;
		public const double c_RequiredInscribe = 80.0;

		[Constructable]
		public LeatherEmbroideryBook()
			: base(0xFF4)
		{
			Name = "Leather Embroidering";
			Weight = 1.0;
		}

		public LeatherEmbroideryBook(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			PlayerMobile pm;

			if (from is PlayerMobile)
				pm = (PlayerMobile)from;
			else
				return;

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (pm.LeatherEmbroidering)
			{
				pm.SendMessage("You have already learned this.");
			}
			else if (pm.Skills[SkillName.Tailoring].Base < c_RequiredTailoring
					 || pm.Skills[SkillName.Inscribe].Base < c_RequiredInscribe)
			{
				pm.SendMessage("Only one who is a both a Master Tailor and an Expert Scribe can learn from this book.");
			}
			else
			{
				pm.LeatherEmbroidering = true;
				pm.SendMessage("You have learned the art of embroidering leather. Use heavy embroidery floss to customize your hand-crafted goods.");
				Delete();
			}
		}
	}
}
