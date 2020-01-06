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
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class BeverageBuyInfo : GenericBuyInfo
	{
		private BeverageType m_Content;

		public BeverageBuyInfo(Type type, BeverageType content, int price, int amount, int itemID, int hue)
			: this(null, type, content, price, amount, itemID, hue)
		{
		}

		public BeverageBuyInfo(string name, Type type, BeverageType content, int price, int amount, int itemID, int hue)
			: base(name, type, price, amount, itemID, hue)
		{
			m_Content = content;

			if (type == typeof(Pitcher))
				Name = (1048128 + (int)content).ToString();
			else if (type == typeof(BeverageBottle))
				Name = (1042959 + (int)content).ToString();
			else if (type == typeof(Jug))
				Name = (1042965 + (int)content).ToString();
		}

		public override object GetObject()
		{
			return Activator.CreateInstance(Type, new object[] { m_Content });
		}
	}
}