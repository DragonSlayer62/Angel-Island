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

/* Scripts/Commands/MakeDeco.cs
 * ChangeLog
 *	05/10/05, erlein
 *		Fixed tab formatting :/
 *	05/10/05, erlein
 *      	* Added search of CustomRegion type regions to include other towns.
 *      	* Excluded all containers that have "ransom" in Name property.
 *      	* Excluded anything that IsLockedDown or IsSecure.
 *	05/10/05, erlein
 *      	Added check for region name "Ocllo Island"
 *	05/10/05, erlein
 *		Initial creation.
 */

using System;
using Server;
using Server.Items;
using Server.Regions;

namespace Server.Commands
{
	public class MakeDeco
	{

		public static void Initialize()
		{
			Server.CommandSystem.Register("MakeDeco", AccessLevel.Administrator, new CommandEventHandler(MakeDeco_OnCommand));
		}

		[Usage("MakeDeco")]
		[Description("Turns all appropriate external containers to deco only.")]
		private static void MakeDeco_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			LogHelper Logger = new LogHelper("makedeco.log", from, true);

			// Loop through town regions and search out items
			// within

			foreach (Region reg in from.Map.Regions)
			{
				if (reg is GuardedRegion)
				{
					for (int pos = 0; pos < reg.Coords.Count; pos++)
					{
						if (reg.Coords[pos] is Rectangle2D)
						{

							Rectangle2D area = (Rectangle2D)reg.Coords[pos];
							IPooledEnumerable eable = from.Map.GetItemsInBounds(area);

							foreach (object obj in eable)
							{
								if (obj is Container)
								{
									Container cont = (Container)obj;

									if (cont.Movable == false &&
										cont.PlayerCrafted == false &&
										cont.Name != "Goodwill" &&
										!(cont.RootParent is Mobile) &&
										!(cont is TrashBarrel) &&
										cont.Deco == false &&
										!(cont.IsLockedDown) &&
										!(cont.IsSecure))
									{

										// Exclude ransom chests
										if (cont.Name != null && cont.Name.ToLower().IndexOf("ransom") >= 0)
											continue;

										// Found one
										cont.Deco = true;
										Logger.Log(LogType.Item, cont);
									}
								}
							}

							eable.Free();
						}
					}
				}
			}

			Logger.Finish();
		}
	}
}
