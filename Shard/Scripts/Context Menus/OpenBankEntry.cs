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
/* ChangeLog
	*	01/03/07, plasma
 *		Remove all duel challenge system code
 *	10/15/07, Pix
 *		Fixed expired AggressorInfos from preventing banking.
 *	8/26/07, Pix
 *		Prevent duelers from accessing their bankbox.
 * 01/05/05, Darva
 * Added code to prevent players from opening their bank box for 2 minutes after
 * 	Any successful steal.
 *
*/

using System;
using Server.Items;
using Server.Mobiles;
namespace Server.ContextMenus
{
	public class OpenBankEntry : ContextMenuEntry
	{
		private Mobile m_Banker;

		public OpenBankEntry(Mobile from, Mobile banker)
			: base(6105, 12)
		{
			m_Banker = banker;
		}

		public override void OnClick()
		{
			if (!Owner.From.CheckAlive())
				return;

			if (Owner.From.Criminal)
			{
				m_Banker.Say(500378); // Thou art a criminal and cannot access thy bank box.
			}
			else
			{
				PlayerMobile pm = Owner.From as PlayerMobile;

				if (pm != null && DateTime.Now - pm.LastStoleAt < TimeSpan.FromMinutes(2))
				{
					m_Banker.Say(500378); // Thou art a criminal and cannot access thy bank box.
				}
				else if (pm != null && AggressorInFight(pm))
				{
					m_Banker.Say("You seem to be busy to bank, come back when you're not fighting.");
				}
				else
				{
					BankBox box = this.Owner.From.BankBox;

					if (box != null)
						box.Open();
				}
			}
		}

		private bool AggressorInFight(PlayerMobile pm)
		{
			if (pm == null) return false;

			//check that we're not actively involved in a fight:
			for (int i = 0; i < pm.Aggressed.Count; ++i)
			{
				AggressorInfo info = (AggressorInfo)pm.Aggressed[i];
				if (!info.Expired)
				{
					if (info.Attacker == pm && info.Defender is PlayerMobile)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}