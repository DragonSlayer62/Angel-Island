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

/* Scripts/Engines/RareFactory/RFViewGump.cs
 * ChangeLog:
 *	18/Mar/2007, weaver
 *		Added rarity of current group being viewed into label
 *	28/Feb/2007, weaver
 *		Initial creation.
 * 
 */

using Server.Network;
using Server.Engines;

namespace Server.Gumps
{
	public class RFViewGump : Gump
	{
		const int gx = 180;
		const int gy = 100;


		const int gw = 450;
		const int gh = 240;


		public RFViewGump()
			: base(gx, gy)
		{
			Closable = true;
			Dragable = true;
			Resizable = false;

			// Let the static RareFactory know that it's in use
			RareFactory.InUse = true;

			AddPage(0);
			AddBackground(gx, gy, gw, gh, 9270);
			AddAlphaRegion(gx + 5, gy + 5, gw - 10, gh - 10);

			AddLabel(gx + 11, gy + 11, 1152, string.Format("Rare Factory - Viewer {0}", (RareFactory.ViewingDODGroup != null ? string.Format("[Group Rarity : {0}]", RareFactory.ViewingDODGroup.Rarity) : "")));

			LoadRareView(RareFactory.ViewingDOD);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1:
					{
						break;
					}
				default:
					{
						RareFactory.InUse = false;
						return;
					}
			}
		}

		private void LoadRareView(DODInstance dodi)
		{
			if (dodi == null)
			{
				AddLabel(gx + 16, gy + 50, 1152, "No Dynamic Object Definitions defined.");
				return;
			}

			AddItem(gx + 50, gy + 60, dodi.RareTemplate.ItemID);

			AddLabel(gx + 150, gy + 60, 1152, "Name ");
			AddLabel(gx + 240, gy + 60, 1152, ": " + dodi.Name);
			AddLabel(gx + 150, gy + 75, 1152, "Item ID ");
			AddLabel(gx + 240, gy + 75, 1152, ": " + dodi.RareTemplate.ItemID.ToString());
			AddLabel(gx + 150, gy + 90, 1152, "Type ");
			AddLabel(gx + 240, gy + 90, 1152, ": " + dodi.RareTemplate.GetType().ToString());
			AddLabel(gx + 150, gy + 105, 1152, "Serial ");
			AddLabel(gx + 240, gy + 105, 1152, ": " + dodi.RareTemplate.Serial.ToString());
			AddLabel(gx + 150, gy + 120, 1152, "Cur Index ");
			AddLabel(gx + 240, gy + 120, 1152, ": " + dodi.CurIndex.ToString());
			AddLabel(gx + 150, gy + 135, 1152, "Start Index ");
			AddLabel(gx + 240, gy + 135, 1152, ": " + dodi.StartIndex.ToString());
			AddLabel(gx + 150, gy + 150, 1152, "Last Index ");
			AddLabel(gx + 240, gy + 150, 1152, ": " + dodi.LastIndex.ToString());
			AddLabel(gx + 150, gy + 165, 1152, "Start Date ");
			AddLabel(gx + 240, gy + 165, 1152, ": " + dodi.StartDate.ToString());
			AddLabel(gx + 150, gy + 180, 1152, "End Date ");
			AddLabel(gx + 240, gy + 180, 1152, ": " + dodi.EndDate.ToString());

		}

	}

}