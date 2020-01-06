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
 *			August 26, 2007
 */

/* Scripts/Gumps/WelcomeGump.cs
 * CHANGELOG:
 *  1/4/07, Adam
 *      Gump turned off for now .. using a book in the starting area.
 *	8/26/07 - Pix
 *		Initial Version.
 */

using System;
using Server;

namespace Server.Gumps
{
	/*class WelcomeGump : Gump
	{
		public WelcomeGump()
			: base(20, 20)
		{
			AddPage(0);

			int width = 600;
			int height = 400;

			AddBackground(0, 0, width, height, 5054);

			AddHtml(10, 10, width - 20, 25, "<center><b>Welcome to Angel Island</b></center>", false, false);

			int y = 45;

			System.Text.StringBuilder welcome = new System.Text.StringBuilder();

			welcome.Append("We hope you enjoy playing here!  We feel that this is the way Ultima Online should be.<br>");
			welcome.Append("We are pub 15/16 based (Felucca-only), but have continued development, adding some of the better features that OSI/EA has added, while ignoring the bad ones.  ");
			welcome.Append("You'll also find that we've added many unique features that you won't find on other shards.<br>");
			welcome.Append("We've focused on making the world a balanced place to play for any type of play or type of character you wish to play.  ");
			welcome.Append("<br><br>You can find out about all the features of Angel Island at our website: <a href=\"http://www.game-master.net/\">http://www.game-master.net/</a>");
			welcome.Append("<br><br>This is a new area to get you oriented to Angel Island - explore it and see some of the custom content we have on Angel Island.");
			welcome.Append("When you're ready, exit via the black gate and you'll end up in Britain.");

			AddHtml(10, y, width - 20, 300, welcome.ToString(), false, false);

			AddButton(10, height - 30, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(40, height - 30, 170, 20, 1011036, 32767, false, false); // OKAY
		}
	}*/
}
