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

/* Items/Deeds/GenderChangeDeed.cs
 * ChangeLog:
 *	11/16/04 Darva
 *		Created file
 *		Made it change your gender when double clicked, removing all facial hair.
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server;

namespace Server.Items
{
	public class GenderChangeDeed : Item
	{
		[Constructable]
		public GenderChangeDeed()
			: base(0x14F0)
		{
			base.Weight = 1.0;
			base.Name = "a gender change deed";
		}

		public GenderChangeDeed(Serial serial)
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
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); //This must be in your backpack
			}
			else if (from.BodyMod != 0)
			{
				from.SendMessage("You must be in your normal form to change your gender.");
			}
			else
			{

				Body body;
				if (from.Female == false)
				{
					body = new Body(401);
				}
				else
				{
					body = new Body(400);
				}
				from.Body = body;
				from.Female = !from.Female;
				if (from.Beard != null)
					from.Beard.Delete();
				from.SendMessage("Your gender has been changed.");
				BaseArmor.ValidateMobile(from);
				this.Delete();
			}

		}
	}
}


