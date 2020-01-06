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

/* Scripts/Commands/CancelSpell.cs
*	ChangeLog:
*	4/17/04 Creation by smerX
*		Created to provide easier access to certain game features
*/
using System;
using System.Collections;
using System.Reflection;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Spells;

namespace Server.Commands
{
	public class CancelSpell
	{

		public static void Initialize()
		{
			Server.CommandSystem.Register("CancelSpell", AccessLevel.Player, new CommandEventHandler(CancelSpell_OnCommand));
		}

		[Usage("CancelSpell")]
		[Description("Cancels the spell currently being cast.")]
		private static void CancelSpell_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;
			ISpell i = m.Spell;

			if (i != null && i.IsCasting)
			{
				Spell s = (Spell)i;
				s.Disturb(DisturbType.EquipRequest, true, false);
				m.SendMessage("You snap yourself out of concentration.");
				m.FixedEffect(0x3735, 6, 30);
				return;
			}

			else
			{
				m.SendMessage("You must be casting a spell to use this feature.");
				return;
			}

		}
	}
}