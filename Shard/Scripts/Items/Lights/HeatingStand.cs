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
using Server;

namespace Server.Items
{
	public class HeatingStand : BaseLight
	{
		public override int LitItemID { get { return 0x184A; } }
		public override int UnlitItemID { get { return 0x1849; } }

		[Constructable]
		public HeatingStand()
			: base(0x1849)
		{
			if (Burnout)
				Duration = TimeSpan.FromMinutes(25);
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Empty;
			Weight = 1.0;
		}

		public override void Ignite()
		{
			base.Ignite();

			if (ItemID == LitItemID)
				Light = LightType.Circle150;
			else if (ItemID == UnlitItemID)
				Light = LightType.Empty;
		}

		public override void Douse()
		{
			base.Douse();

			if (ItemID == LitItemID)
				Light = LightType.Circle150;
			else if (ItemID == UnlitItemID)
				Light = LightType.Empty;
		}

		public HeatingStand(Serial serial)
			: base(serial)
		{
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