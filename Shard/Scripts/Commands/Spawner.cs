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

/* Changelog 
 * 1/07/05, Darva
 *		First checkin.
 *
 */
using System;
using Server.Mobiles;
using Server.Targeting;
namespace Server.Commands
{
	public class SpawnerCmd
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			Server.CommandSystem.Register("Spawner", AccessLevel.GameMaster, new CommandEventHandler(Spawner_OnCommand));
		}

		[Usage("Spawner")]
		[Description("Moves you to the spawner of the targeted creature, if any.")]
		private static void Spawner_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new SpawnerTarget();
		}

		private class SpawnerTarget : Target
		{
			public SpawnerTarget()
				: base(-1, true, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is BaseCreature)
				{
					if (((BaseCreature)o).Spawner != null)
					{
						BaseCreature bc = (BaseCreature)o;
						from.MoveToWorld(bc.Spawner.Location, bc.Spawner.Map);
					}
					else
					{
						from.SendMessage("That mobile is homeless");
					}
				}
				if (o is Item)
				{
					if (((Item)o).SpawnerLocation != Point3D.Zero && (o as Item).SpawnerMap != null)
					{
						from.MoveToWorld((o as Item).SpawnerLocation, (o as Item).SpawnerMap);
					}
					else
					{
						from.SendMessage("That item is not from a spawner");
					}
				}
				else
				{
					from.SendMessage("Why would that have a spawner?");
				}
			}
		}
	}
}