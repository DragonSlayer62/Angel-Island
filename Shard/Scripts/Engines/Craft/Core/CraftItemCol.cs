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

namespace Server.Engines.Craft
{
	public class CraftItemCol : System.Collections.CollectionBase
	{
		public CraftItemCol()
		{
		}

		public int Add(CraftItem craftItem)
		{
			return List.Add(craftItem);
		}

		public void Remove(int index)
		{
			if (index > Count - 1 || index < 0)
			{
			}
			else
			{
				List.RemoveAt(index);
			}
		}

		public CraftItem GetAt(int index)
		{
			return (CraftItem)List[index];
		}

		public CraftItem SearchForSubclass(Type type)
		{
			for (int i = 0; i < List.Count; i++)
			{
				CraftItem craftItem = (CraftItem)List[i];

				if (craftItem.ItemType == type || type.IsSubclassOf(craftItem.ItemType))
					return craftItem;
			}

			return null;
		}

		public CraftItem SearchFor(Type type)
		{
			for (int i = 0; i < List.Count; i++)
			{
				CraftItem craftItem = (CraftItem)List[i];
				if (craftItem.ItemType == type)
				{
					return craftItem;
				}
			}
			return null;
		}
	}
}