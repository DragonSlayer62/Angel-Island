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

/* Scripts\Items\Special\KukuiNut.cs
 * Changelog:
 *	05/09/06, Kit
 *		Initial creation limited one time item for reverting a pet from bonded back to unbonded.
 */

using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class KukuiNut : Item
	{

		[Constructable]
		public KukuiNut()
			: base(0xF8B)
		{
			Hue = 541;
			Stackable = false;
			Weight = 1.0;
			LootType = LootType.Regular;
			Name = "a kukui nut";

		}

		public KukuiNut(Serial serial)
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