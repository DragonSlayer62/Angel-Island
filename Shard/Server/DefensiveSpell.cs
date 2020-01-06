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

/* Server/DefensiveSpell.cs
 *	ChangeLog:
 *	12/26/06 - Pix.
 *		Moved DefensiveSpell class from Scripts/Spells/Base/SpellHelper.cs so 
 *		we can reference the lock object for defensive spells in the core.
 */


using System;
using Server;


namespace Server
{
	public class DefensiveSpell
	{
		public static void Nullify(Mobile from)
		{
			if (!from.CanBeginAction(typeof(DefensiveSpell)))
				new InternalTimer(from).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;

			public InternalTimer(Mobile m)
				: base(TimeSpan.FromMinutes(1.0))
			{
				m_Mobile = m;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				m_Mobile.EndAction(typeof(DefensiveSpell));
			}
		}
	}
}