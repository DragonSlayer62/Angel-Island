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

/* Scripts/Misc/WebStatus.cs
 * ChangeLog
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.IO;
using System.Text;
using System.Collections;

using Server;
using Server.Network;
using Server.Guilds;

namespace Server.Misc
{
	public class StatusPage : Timer
	{
		public static void Initialize()
		{
			new StatusPage().Start();
		}

		public StatusPage()
			: base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(60.0))
		{
			Priority = TimerPriority.FiveSeconds;
		}

		private static string Encode(string input)
		{
			StringBuilder sb = new StringBuilder(input);

			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\"", "&quot;");
			sb.Replace("'", "&apos;");

			return sb.ToString();
		}

		protected override void OnTick()
		{
			if (!Directory.Exists("web"))
				Directory.CreateDirectory("web");

			using (StreamWriter op = new StreamWriter("web/status.html"))
			{
				op.WriteLine("<html>");
				op.WriteLine("   <head>");
				op.WriteLine("      <title>RunUO Server Status</title>");
				op.WriteLine("   </head>");
				op.WriteLine("   <body bgcolor=\"white\">");
				op.WriteLine("      <h1>RunUO Server Status</h1>");
				op.WriteLine("      Online clients:<br>");
				op.WriteLine("      <table width=\"100%\">");
				op.WriteLine("         <tr>");
				op.WriteLine("            <td bgcolor=\"black\"><font color=\"white\">Name</font></td><td bgcolor=\"black\"><font color=\"white\">Location</font></td><td bgcolor=\"black\"><font coloLongTermMurdershite\">Kills</font></td><td bgcolor=\"black\"><font color=\"white\">Karma / Fame</font></td>");
				op.WriteLine("         </tr>");

				foreach (NetState state in NetState.Instances)
				{
					Mobile m = state.Mobile;

					if (m != null)
					{
						Guild g = m.Guild as Guild;

						op.Write("         <tr><td>");

						if (g != null)
						{
							op.Write(Encode(m.Name));
							op.Write(" [");

							string title = m.GuildTitle;

							if (title != null)
								title = title.Trim();
							else
								title = "";

							if (title.Length > 0)
							{
								op.Write(Encode(title));
								op.Write(", ");
							}

							op.Write(Encode(g.Abbreviation));

							op.Write(']');
						}
						else
						{
							op.Write(Encode(m.Name));
						}

						op.Write("</td><td>");
						op.Write(m.X);
						op.Write(", ");
						op.Write(m.Y);
						op.Write(", ");
						op.Write(m.Z);
						op.Write(" (");
						op.Write(m.Map);
						op.Write(")</td><td>");
						op.Write(m.LongTermMurders);
						op.Write("</td><td>");
						op.Write(m.Karma);
						op.Write(" / ");
						op.Write(m.Fame);
						op.WriteLine("</td></tr>");
					}
				}

				op.WriteLine("         <tr>");
				op.WriteLine("      </table>");
				op.WriteLine("   </body>");
				op.WriteLine("</html>");
			}
		}
	}
}