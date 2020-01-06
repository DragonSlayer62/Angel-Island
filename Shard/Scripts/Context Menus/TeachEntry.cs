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

using System;
using Server.Mobiles;

namespace Server.ContextMenus
{
	public class TeachEntry : ContextMenuEntry
	{
		private SkillName m_Skill;
		private BaseCreature m_Mobile;
		private Mobile m_From;

		public TeachEntry(SkillName skill, BaseCreature m, Mobile from, bool enabled)
			: base(6000 + (int)skill, 4)
		{
			m_Skill = skill;
			m_Mobile = m;
			m_From = from;

			if (!enabled)
				Flags |= Network.CMEFlags.Disabled;
		}

		public override void OnClick()
		{
			if (!m_From.CheckAlive())
				return;

			m_Mobile.Teach(m_Skill, m_From, 0, false);
		}
	}
}