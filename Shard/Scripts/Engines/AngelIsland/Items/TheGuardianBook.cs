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

// Engines/AngelIsland/TheGuardianBook.cs, last modified 4/12/04 by Adam.
// Bassed on Engines/AngelIsland/TemplateBook.cs, last modified 4/12/04 by Pixie.
// 4/12/04 Adam
//   Initial Revision.
// 4/12/04 Created by Adam;

using System;
using Server;

namespace Server.Items
{
	public class TheGuardianBook : BaseBook
	{
		private const string TITLE = "The Guardian";
		private const string AUTHOR = "Adam Ant";
		private const int PAGES = 6;	//This doesn't *HAVE* to be updated, it'll fill up the 
		//book with blank pages though.  It'd be cleaner if it
		//had the exact right number of pages.

		private const bool WRITABLE = false;

		//This randomly chooses one of the four types of books.
		//If you wish to only have one particular book, or a couple
		//of different types, remove the ones you don't want
		private static int[] BOOKTYPES = new int[]
			{ 
			  0xFEF, //brown
			  0xFF0, //tan
			  0xFF1, //red
			  0xFF2  //purple
			};

		[Constructable]
		public TheGuardianBook()
			: base(Utility.RandomList(BOOKTYPES), TITLE, AUTHOR, PAGES, WRITABLE)
		{
			// NOTE: There are 8 lines per page and
			// approx 22 to 24 characters per line.
			//  0----+----1----+----2----+
			int cnt = 0;
			string[] lines;

			lines = new string[]
			{
				"He who hunteth the lich",  
				"in these caves committeth",  
				"a crime of the gravest", 
				"order, deserving of death.", 
				"The liches and I dwell",  
				"together in these caves,",  
				"which we devoutly believe",  
				"to be given us as our", 
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"sacred home.",
				"I gladly give my blessing", 
				"to all who wish to hunt", 
				"down and deliver unto", 
				"death all Daemons and", 
				"Dragons. These are", 
				"creatures of vile and", 
				"loathsome aspect,", 
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"malodorous and displeasing", 
				"to the senses.",
				"If you would join in the", 
				"hunt to rid my sacred", 
				"home of these vile", 
				"monsters, you must don a",
				"scarf of the colour of", 
				"blood in token of your",
			};
			Pages[cnt++].Lines = lines;


			lines = new string[]
			{
				"respect.",
				"If you fail in this regard,", 
				"and look not upon this", 
				"magical garment, you will", 
				"die swiftly and surely to", 
				"my hand.",
				"I have given unto you fit", 
				"means of escape, which I",
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"enjoin you to take unto", 
				"you and employ.",
				"I have marked and", 
				"designed for your especial", 
				"use a rune, and this will", 
				"bring you at the end of", 
				"your travels to the great",
				"city of Britain.",
			};
			Pages[cnt++].Lines = lines;

			lines = new string[]
			{
				"May your going be safe",
				"and sure. Blessings upon", 
				"you.",
				"",
				"",
				"",
				"",
				"",
			};
			Pages[cnt++].Lines = lines;

			/* PAGE SYNTAX:
						lines = new string[]
						{
							"",
							"",
							"",
							"",
							"",
							"",
							"",
							"",
						};
						Pages[cnt++].Lines = lines;
			*/
		}

		public TheGuardianBook(Serial serial)
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
