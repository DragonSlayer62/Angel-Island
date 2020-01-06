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
using Server.Items;

namespace Server.Mobiles
{
	public class SolenHelper
	{
		public static void PackPicnicBasket(BaseCreature solen)
		{
			if (1 > Utility.Random(100))
			{
				PicnicBasket basket = new PicnicBasket();

				basket.DropItem(new BeverageBottle(BeverageType.Wine));
				basket.DropItem(new CheeseWedge());

				solen.PackItem(basket);
			}
		}
	}
}