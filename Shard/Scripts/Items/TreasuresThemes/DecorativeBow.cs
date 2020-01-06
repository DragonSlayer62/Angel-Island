
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

/* Scripts/Items/TreasureThemes/DecorativeBow.cs
 * CHANGELOG
 *	04/01/05, Kitaras	
 *		Initial Creation
 */

using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{

	public class DecorativeBow : Item
	{

		//4 differnt types 0-3
		[Constructable]
		public DecorativeBow(int type)
		{

			Name = "a decorative bow";
			Movable = true;

			if (type == 0) ItemID = 5468;
			if (type == 1) ItemID = 5469;
			if (type == 2) ItemID = 5470;
			if (type == 3) ItemID = 5471;
		}

		public DecorativeBow(Serial serial)
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