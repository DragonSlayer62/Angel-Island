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

/*
 * Engines/Items/Books/BookofUpgradeContracts.cs
 * CHANGELOG:
 * 5/8/07, Adam
 *  Initial Revision.
 */

using System;
using Server;

namespace Server.Items
{
	public class BookofUpgradeContracts : BaseBook
	{
		private const string TITLE = "Upgrade Contracts Explained";
		private const int PAGES = 6;
		private const bool WRITABLE = false;
		private const int PURPLE_BOOK = 0xFF2;

		[Constructable]
		public BookofUpgradeContracts()
			: base(PURPLE_BOOK, TITLE, NameList.RandomName("female"), PAGES, WRITABLE)
		{
			// NOTE: There are 8 lines per page and
			// approx 22 to 24 characters per line.
			//  0----+----1----+----2----+
			int cnt = 0;
			string[] lines;
			Name = TITLE;

			lines = new string[]
			{
				"Modest Upgrade is",
                "",
                "500 lockdowns",
                "3 secures",
                "3 lockboxes",
                "",
                "",
                "",
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"Moderate Upgrade is",
                "",
                "900 lockdowns",
                "6 secures",
                "4 lockboxes",
                "",
                "",
                "",
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"Premium Upgrade is",
                "",
                "1300 lockdowns",
                "9 secures",
                "5 lockboxes",
                "",
                "",
                "",
			};
			Pages[cnt++].Lines = lines;


			lines = new string[]
			{
				"Extravagant Upgrade is",
                "",
                "1950 lockdowns",
                "14 secures",
                "7 lockboxes",
                "",
                "",
                ""
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{  //0123456789012345678901234
				"Your investment is safe!",
                "",
                "When you redeed your home",
                "a check for the full cost",
                "of all upgrades will be",
                "deposited in your bank.",
                "",
                ""
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"To perform an upgrade",
                "",
                "Stand under house sign",
                "Double click contract",
                "Target your house sign",
                "Your storage will be",
                "Upgraded.",
                "Enjoy your upgrade!"
			};
			Pages[cnt++].Lines = lines;
		}

		public BookofUpgradeContracts(Serial serial)
			: base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}
	}
}
