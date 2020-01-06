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
* 	Technical Data and Computer Software clause at DFARS 252.227-701.
*
*	Angel Island UO Shard	Version 1.0
*			Release A
*			March 25, 2004
*/

/* Scripts/Commands/Nuke.cs
 * CHANGELOG:
 *	3/15/16, Adam
 *		Set all spawner minimums and maximums if needed. Someone set them to crazy fast, like 1 minute Air Elementals
 *			And 30 second to 1 minute pirates!!
 *		While we are at it, do a complete logging of spawners and settings.
 *	2/28/11, Adam
 *		Clear all fillable containers (freezedry was making them continously refill.)
 *	2/27/11, Adam
 *		Write a nuke to locate all spawners with a respawn time over 3 hours - rares spawners.
 *		We do this because we need to remove all the ones that overlap with the FillableContainers being introduced.
 *	2/15/11, Adam
 *		Create a nuke to wipe the wandeing healers and all partol guards except for those in Skara Brae
 *		Needed for Mortalis
 *	8/27/10, adam
 *		Use Utility.BritWrap rects from boat nav to test for valid treasure map locations.
 *		Turns out that the rects defined in Utility.BritWrap are the two halves of the world excluding T2A and the dungeons
 *		which is what we want.
 *	8/14/10, adam
 *		Patch all old maps with new dynamic coordinates
 *	6/10/10, adam
 *		locate all baby rares that need patching
 *	5/2/10, adam
 *		unhue all exploited event hued clothing
 *	3/17/10, adam
 *		Patch the pm.LastDisconnect time for all players to DateTime.Now.
 *	2/26/10, Adam
 *		Add end date for the birth rare drop - March 1st
 *	2/25/10, Adam
 *		Add a birth-rares generator attached to an event sinc
 *	2/25/10, Adam
 *		delete all items in containers across the world.
 *		delete all old-school home teleporters
 *		fix checker and chess boards aftrer deleting all the pieces ;-)
 *	2/22/10, Adam
 *		The logic below that tries to skip duplicating existing region fails since NAME compare	is unreliable.
 *		For example, we have a Region Controller that handles all of Jhelom, but the XML file contains 3 separate entries foir the different islands.
 *		Another example is Ocllo Island in the XML is Island Siege in the Region Controller. We will solve this by manually turning off the duplicates
 *		generated from the XML.
 *	4/25/09, plasma	
 *		Create new DRDT regions for all XML regions where they dont already exist as DRDT
 *		Creates KinFactionCity controllers on test for faction cities
 *	2/28/09, Adam
 *		Reset all skills to 80
 *	10/21/08, Adam
 *		[IntMapOrphan set all guildstones to IsIntMapStorage = false because they were not on a spawner. This was a logic error.
 *		This Nuke patches those guildstones by setting IsIntMapStorage = true.
 *	9/29/08, Adam
 *		Add a nuke to list IDOC and near IDOC houses to the console
 *	9/21/08, Adam
 *      - Create Nuke to patch all old lables to today's date on old halloween rewards
 *  7/5/08, Adam 
 *      Prove that try/catch blocks themselves do not impose significant overhead.
 *      This is not to say 'exception handling' does not impose significant overhead as we know that does.
 *	6/18/08, Adam
 *		Report on Harrower (orphaned) instances
 *	8/23/07, Adam
 *		Write a nuke to collect all lockboxes contained in a list and move them to named pouches (one for each house) into callers backpack
 *	7/3/07, Adam
 *		reset all dragons to non breeding participants
 *  6/8/07, Adam
 *      log all rare-factory rares
 *  5/23/07, Adam
 *      Retroactively making ghosts blind
 *  4/25/07, Adam
 *		(bring this back from file revision 1.20)
 *		- Retarget all castle runes, moonstones, and runebooks to the houses ban location if the holder is not a friend of the house
 *  4/20/07, Adam
 *      ImmovableItemsOnPlayers logging
 *  03/20/07, plasma
 *      Exploit test / log
 *  02/12/07, plasma
 *      Log old CannedEvil namespace items
 *  2/6/07, Adam
 *      - initialize item.IsIntMapStorage for all items as we are not sure of the bit state that was previously used in item.cs.
 *      - Call the mobiles RemoveItem and not the ArrayLists.Remove as the mobiles needs to sep parent to null.
 *  2/5/07, Adam
 *      - System to replace all GuildRestorationDeeds and move 'deeded' guildstones to the internal map.
 *  01/02/07, Kit
 *      Validate email addresses, set to null if invalid, have logging option.
 *  12/29/06, Adam
 *      Add library patcher to set all books to im.IsLockedDown = true
 *  12/21/06, Kit
 *      changed to report location of bonded non controlled ghost pets
 *	12/19/06, Pix
 *		Nuke command to fix all BaseHouseDoorAddon.
 *  11/28/06 Taran Kain
 *      Neutralized WMD.
 *  11/27/06 Taran Kain
 *      Changed nuke back to InitializeGenes
 *  11/27/06, Adam
 *      stable all pets
 *	11/20/06 Taran Kain
 *		Change nuke to generate valid gene data for all mobiles
 *	10/20/06, Adam
 *		Remove bogus comments and watchlisting 
 *	10/19/06, Adam
 *		(bring this back from file revision 1.9)
 *		- Retarget all castle runes, moonstones, and runebooks to the houses ban location if the holder is not a friend of the house
 *		- set all houses to insecure
 *  10/15/06, Adam
 *		Make email Dumper
 *  10/09/06, Kit
 *		Disabled b1 revert nuke
 *  10/07/06, Kit
 *		B1 control slot revert(Decrease all bonded pets control slots  by one
 *	9/27/06, Adam
 *		Sandal finder
 *  8/17/06, Kit
 *		Changed back to relink forges and trees!!
 *  8/16/06, weaver
 *		Re-link tent backpacks to their respective tents!
 *  8/7/06, Kit
 *		Make nuke command relink forges to house addon list, make christmas trees be added to house addon list.
 *  7/30/06, Kit
 *		Respawn all forge addons to new animated versions!
 *	7/22/06, weaver
 *		Adapted to search through all characters' bankboxes and give them a tent each.
 *	7/6/06, Adam
 *		- Retarget all castle runes, moonstones, and runebooks to the houses ban location if the holder is not a friend of the house
 *		- set all houses to insecure
 *	05/30/06, Adam
 *		Unnewbie all reagents around the world.
 *		Note: You must use [RehydrateWorld immediatly before using this command.
 *  05/15/06, Kit
 *		New warhead wipe the world of dead bonded pets that have been released but didnt delete because of B1 bug.
 *  05/14/06, Kit
 *		Disabled this WMD.
 *  05/02/06, Kit
 *		Changed to check all pets in world add them to masters stables and then increase any bonded pets control slots by +1.
 *	03/28/06 Taran Kain
 *		Changed to reset BoneContainer hues. Checks one item per game iteration. RH's and re-FD's containers.
 *  6/16/05, Kit
 *		Changed to Wipe phantom items from vendors vs clothing hue.
 *	4/22/05: Kit
 *		Initial Version
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server;
using System.Xml;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;
using Server.Regions;
using Server.SMTP;
using Server.Accounting;
using Server.Network;
using Server.Commands;

namespace Server.Commands
{
	public class Nuke
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Nuke", AccessLevel.Administrator, new CommandEventHandler(Nuke_OnCommand));
		}

		[Usage("Nuke")]
		[Description("Does whatever.")]
		public static void Nuke_OnCommand(CommandEventArgs e)
		{
			try
			{
				e.Mobile.SendMessage("Begin nuclearization.");
				int count = NukeWorker(e);
				e.Mobile.SendMessage("Nuclearization complete with {0} items processed.", count);
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
			}
		}

		private static int NukeWorker(CommandEventArgs e)
		{
			/*if (e.Arguments.Length != 2)
			{
				e.Mobile.SendMessage("Nuke Usage: [nuke random_chance loop_counter");
				return 0;
			}*/

			int count = 0;
			foreach (Spawner sx in SpawnerCache.Spawners)
			{
				TimeSpan old_min = sx.MinDelay;
				TimeSpan old_max = sx.MaxDelay;

				// defualt RunUO is 5&10, we will default to 3&8
				if (old_min < new TimeSpan(0, 3, 0) || old_max < new TimeSpan(0, 8, 0))
				{
					count++;
					if (old_min < new TimeSpan(0, 3, 0) && old_max < new TimeSpan(0, 8, 0))
					{
						sx.LogChange(String.Format("Update MIN/MAX spawn times. Was MIN={0}, MAX={1}", old_min, old_max));
						sx.MinDelay = new TimeSpan(0, 3, 0);
						sx.MaxDelay = new TimeSpan(0, 8, 0);
					}
					else if (old_min < new TimeSpan(0, 3, 0))
					{
						sx.LogChange(String.Format("Update MIN spawn time, was {0}", old_min));
						sx.MinDelay = new TimeSpan(0, 3, 0);
					}
					else
					{
						sx.LogChange(String.Format("Update MAX spawn time, was {0}", old_max));
						sx.MaxDelay = new TimeSpan(0, 8, 0);
					}
				}
			}


			e.Mobile.SendMessage("Updated {0} Spawner(s).", count);

			return 0;
		}
	}
}
