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
using Server;

namespace Server.Engines.PartySystem
{
	public class PartyMemberInfo
	{
		private Mobile m_Mobile;
		private bool m_CanLoot;

		public Mobile Mobile { get { return m_Mobile; } }
		public bool CanLoot { get { return m_CanLoot; } set { m_CanLoot = value; } }

		public PartyMemberInfo(Mobile m)
		{
			m_Mobile = m;
			m_CanLoot = true;
		}
	}
}