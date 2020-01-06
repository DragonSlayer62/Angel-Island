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
	public class Torch : BaseEquipableLight
	{
		public override int LitItemID { get { return 0xA12; } }
		public override int UnlitItemID { get { return 0xF6B; } }

		public override int LitSound { get { return 0x54; } }
		public override int UnlitSound { get { return 0x4BB; } }

		[Constructable]
		public Torch()
			: base(0xF6B)
		{
			if (Burnout)
				Duration = TimeSpan.FromMinutes(30);
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Circle300;
			Weight = 1.0;
		}

		public override void OnAdded(object parent)
		{
			base.OnAdded(parent);

			if (parent is Mobile && Burning)
				Mobiles.MeerMage.StopEffect((Mobile)parent, true);
		}

		public override void Ignite()
		{
			base.Ignite();

			if (Parent is Mobile && Burning)
				Mobiles.MeerMage.StopEffect((Mobile)Parent, true);
		}

		public Torch(Serial serial)
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

			if (Weight == 2.0)
				Weight = 1.0;
		}
	}
}