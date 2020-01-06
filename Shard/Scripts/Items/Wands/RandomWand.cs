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
	public class RandomWand
	{
		public static BaseWand CreateWand()
		{
			return CreateRandomWand();
		}

		public static BaseWand CreateRandomWand()
		{
			switch (Utility.Random(11))
			{
				default:
				case 0: return new ClumsyWand();
				case 1: return new FeebleWand();
				case 2: return new FireballWand();
				case 3: return new GreaterHealWand();
				case 4: return new HarmWand();
				case 5: return new HealWand();
				case 6: return new IDWand();
				case 7: return new LightningWand();
				case 8: return new MagicArrowWand();
				case 9: return new ManaDrainWand();
				case 10: return new WeaknessWand();
			}
		}
	}
}