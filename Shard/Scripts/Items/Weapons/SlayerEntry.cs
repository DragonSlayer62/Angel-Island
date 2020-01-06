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
using Server.Mobiles;

namespace Server.Items
{
	public class SlayerEntry
	{
		private SlayerGroup m_Group;
		private SlayerName m_Name;
		private Type[] m_Types;

		public SlayerGroup Group { get { return m_Group; } set { m_Group = value; } }
		public SlayerName Name { get { return m_Name; } }
		public Type[] Types { get { return m_Types; } }

		public SlayerEntry(SlayerName name, params Type[] types)
		{
			m_Name = name;
			m_Types = types;
		}

		public bool Slays(Mobile m)
		{
			Type t = m.GetType();

			for (int i = 0; i < m_Types.Length; ++i)
				if (m_Types[i] == t)
					return true;

			return false;
		}
	}
}