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

/* Scripts/Gumps/Properties/SetCustomEnumGump.cs
 * Changelog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Gumps
{
	public class SetCustomEnumGump : SetListOptionGump
	{
		private string[] m_Names;

		public SetCustomEnumGump(PropertyInfo prop, Mobile mobile, object o, Stack stack, int propspage, ArrayList list, string[] names)
			: base(prop, mobile, o, stack, propspage, list, names, null)
		{
			m_Names = names;
		}

		public override void OnResponse(NetState sender, RelayInfo relayInfo)
		{
			int index = relayInfo.ButtonID - 1;

			if (index >= 0 && index < m_Names.Length)
			{
				try
				{
					MethodInfo info = m_Property.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });

					Server.Commands.CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, m_Names[index]);

					if (info != null)
						m_Property.SetValue(m_Object, info.Invoke(null, new object[] { m_Names[index] }), null);
					else if (m_Property.PropertyType == typeof(Enum) || m_Property.PropertyType.IsSubclassOf(typeof(Enum)))
						m_Property.SetValue(m_Object, Enum.Parse(m_Property.PropertyType, m_Names[index], false), null);
				}
				catch
				{
					m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
				}
			}

			m_Mobile.SendGump(new PropertiesGump(m_Mobile, m_Object, m_Stack, m_List, m_Page));
		}
	}
}