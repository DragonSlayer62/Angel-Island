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
	public class CraftResCol : System.Collections.CollectionBase
	{
		public CraftResCol()
		{
		}

		public void Add(CraftRes craftRes)
		{
			List.Add(craftRes);
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

		public CraftRes GetAt(int index)
		{
			return (CraftRes)List[index];
		}
	}
}