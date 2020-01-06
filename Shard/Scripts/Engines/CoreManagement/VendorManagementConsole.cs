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

/* Engines/CoreManagement/VendorManagementConsole.cs
 * ChangeLog
 *	1/19/08, Adam
 *		Created.
 *		Player Vendor Management console for the global values stored in Engines/AngelIsland/CoreAI.cs
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server;

namespace Server.Items
{
	[FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
	public class VendorManagementConsole : Item
	{
		[Constructable]
		public VendorManagementConsole()
			: base(0x1F14)
		{
			Weight = 1.0;
			Hue = 51;
			Name = "Vendor Management Console";
		}

		public VendorManagementConsole(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int GracePeriod
		{
			get
			{
				return CoreAI.GracePeriod;
			}
			set
			{
				CoreAI.GracePeriod = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int ConnectionFloor
		{
			get
			{
				return CoreAI.ConnectionFloor;
			}
			set
			{
				CoreAI.ConnectionFloor = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double Commission
		{
			get
			{
				return CoreAI.Commission;
			}
			set
			{
				CoreAI.Commission = value;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

}