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

/* Scripts/Gumps/Guilds/GuildWarGump.cs
 * CHANGELOG:
 *	6/16/06, Pix
 *		Fixed the 'guild list scroll off gump' bug.
 */


using System;
using System.Collections;
using Server;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
	public class GuildWarGump : Gump
	{
		private Mobile m_Mobile;
		private Guild m_Guild;

		public GuildWarGump(Mobile from, Guild guild)
			: base(20, 30)
		{
			m_Mobile = from;
			m_Guild = guild;

			Dragable = false;

			int iNextPageNumber = 0;

			AddPage(iNextPageNumber); //0
			iNextPageNumber++;

			AddBackground(0, 0, 550, 440, 5054);
			AddBackground(10, 10, 530, 420, 3000);

			AddHtmlLocalized(20, 10, 500, 35, 1011133, false, false); // <center>WARFARE STATUS</center>

			AddButton(20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, 400, 300, 35, 1011120, false, false); // Return to the main menu.

			AddPage(iNextPageNumber); //1
			iNextPageNumber++;

			AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
			AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page

			AddHtmlLocalized(20, 45, 400, 20, 1011134, false, false); // We are at war with:

			ArrayList enemies = guild.Enemies;

			if (enemies.Count == 0)
			{
				AddHtmlLocalized(20, 65, 400, 20, 1013033, false, false); // No current wars
			}
			else
			{
				for (int i = 0; i < enemies.Count; ++i)
				{
					Guild g = (Guild)enemies[i];

					if (i != 0)
					{
						if (i % 15 == 0)
						{
							AddPage(iNextPageNumber);
							iNextPageNumber++;

							AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
							AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page

							AddButton(30, 375, 5223, 5223, 0, GumpButtonType.Page, iNextPageNumber - 2);
							AddHtmlLocalized(65, 373, 150, 25, 1011067, false, false); // Previous page

							AddHtmlLocalized(20, 45, 400, 20, 1011134, false, false); // We are at war with:
						}
					}

					int y = 65 + ((i % 15) * 20);

					AddHtml(20, y, 300, 20, g.Name, false, false);
				}
			}

			AddPage(iNextPageNumber); //2
			iNextPageNumber++;

			AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
			AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page

			AddButton(30, 375, 5223, 5223, 0, GumpButtonType.Page, iNextPageNumber - 2);
			AddHtmlLocalized(65, 373, 150, 25, 1011067, false, false); // Previous page

			AddHtmlLocalized(20, 45, 400, 20, 1011136, false, false); // Guilds that we have declared war on: 

			ArrayList declared = guild.WarDeclarations;

			if (declared.Count == 0)
			{
				AddHtmlLocalized(20, 65, 400, 20, 1018012, false, false); // No current invitations received for war.
			}
			else
			{
				for (int i = 0; i < declared.Count; ++i)
				{
					Guild g = (Guild)declared[i];

					if (i != 0)
					{
						if (i % 15 == 0)
						{
							AddPage(iNextPageNumber);
							iNextPageNumber++;

							AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
							AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page

							AddButton(30, 375, 5223, 5223, 0, GumpButtonType.Page, iNextPageNumber - 2);
							AddHtmlLocalized(65, 373, 150, 25, 1011067, false, false); // Previous page

							AddHtmlLocalized(20, 45, 400, 20, 1011136, false, false); // Guilds that we have declared war on: 
						}
					}

					int y = 65 + ((i % 15) * 20);
					AddHtml(20, y, 300, 20, g.Name, false, false);
				}
			}

			AddPage(iNextPageNumber); //3
			iNextPageNumber++;

			AddButton(30, 375, 5223, 5223, 0, GumpButtonType.Page, iNextPageNumber - 2);
			AddHtmlLocalized(65, 373, 150, 25, 1011067, false, false); // Previous page

			AddHtmlLocalized(20, 45, 400, 20, 1011135, false, false); // Guilds that have declared war on us: 

			ArrayList invites = guild.WarInvitations;

			if (invites.Count > 15)
			{
				AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
				AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page
			}

			if (invites.Count == 0)
			{
				AddHtmlLocalized(20, 65, 400, 20, 1013055, false, false); // No current war declarations
			}
			else
			{
				for (int i = 0; i < invites.Count; ++i)
				{
					Guild g = (Guild)invites[i];

					if (i != 0)
					{
						if (i % 15 == 0)
						{
							AddPage(iNextPageNumber);
							iNextPageNumber++;

							if (invites.Count - i > 15)
							{
								AddButton(375, 375, 5224, 5224, 0, GumpButtonType.Page, iNextPageNumber);
								AddHtmlLocalized(410, 373, 100, 25, 1011066, false, false); // Next page
							}

							AddButton(30, 375, 5223, 5223, 0, GumpButtonType.Page, iNextPageNumber - 2);
							AddHtmlLocalized(65, 373, 150, 25, 1011067, false, false); // Previous page

							AddHtmlLocalized(20, 45, 400, 20, 1011135, false, false); // Guilds that have declared war on us: 
						}
					}

					int y = 65 + ((i % 15) * 20);
					AddHtml(20, y, 300, 20, g.Name, false, false);
				}
			}
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (GuildGump.BadMember(m_Mobile, m_Guild))
				return;

			if (info.ButtonID == 1)
			{
				GuildGump.EnsureClosed(m_Mobile);
				m_Mobile.SendGump(new GuildGump(m_Mobile, m_Guild));
			}
		}
	}
}