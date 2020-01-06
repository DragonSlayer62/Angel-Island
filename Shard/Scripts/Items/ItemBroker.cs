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

/* Scripts/Items/ItemBroker.cs
 * CHANGELOG:
 *	01/03/07 - Pix
 *		Finally got back to this - tested and good!
 *	11/28/06 - Pix
 *		Initial Version.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;

namespace Server.Items
{
	public class ItemBroker
	{
		public static T GetItem<T>(Serial serial, ref bool isFD) where T : Item
		{
			return GetItem(serial, ref isFD) as T;
		}

		private static Item GetItem(Serial serial, ref bool isFD)
		{
			isFD = false; //initialize

			Item item = World.FindItem(serial);
			if (item != null)
			{
				return item;
			}
			else
			{
				if (World.IsReserved(serial))
				{
					isFD = true;
				}
			}

			return null;
		}

		public static void WriteSerialList(GenericWriter writer, List<Serial> serialList)
		{
			writer.Write(serialList.Count);
			for (int i = 0; i < serialList.Count; i++)
			{
				writer.Write((int)serialList[i]);
			}
		}

		public static List<Serial> ReadSerialList(GenericReader reader)
		{
			int count = reader.ReadInt();

			List<Serial> list = new List<Serial>(count);
			for (int i = 0; i < count; i++)
			{
				int s = reader.ReadInt();
				list.Add((Serial)s);
			}

			return list;
		}
	}
}
