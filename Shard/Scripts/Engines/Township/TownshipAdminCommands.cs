using System;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Items;
using Server.Commands;

namespace Server.Township
{
	public class TownshipAdminCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register("TSList", AccessLevel.Counselor, new CommandEventHandler(TSList_OnCommand));
		}

		[Usage("TSList")]
		[Description("Opens an interface providing access to a list of townships.")]
		public static void TSList_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendGump(new Township.TownshipStaffGump(e.Mobile));
		}
	}
}
