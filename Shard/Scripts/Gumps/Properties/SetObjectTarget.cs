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

/* Scripts/Gumps/Properties/SetObjectTarget.cs
 * Changelog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Gumps
{
	public class SetObjectTarget : Target
	{
		private PropertyInfo m_Property;
		private Mobile m_Mobile;
		private object m_Object;
		private Stack m_Stack;
		private Type m_Type;
		private int m_Page;
		private ArrayList m_List;

		public SetObjectTarget(PropertyInfo prop, Mobile mobile, object o, Stack stack, Type type, int page, ArrayList list)
			: base(-1, false, TargetFlags.None)
		{
			m_Property = prop;
			m_Mobile = mobile;
			m_Object = o;
			m_Stack = stack;
			m_Type = type;
			m_Page = page;
			m_List = list;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			try
			{
				if (m_Type == typeof(Type))
					targeted = targeted.GetType();
				else if ((m_Type == typeof(BaseAddon) || m_Type.IsAssignableFrom(typeof(BaseAddon))) && targeted is AddonComponent)
					targeted = ((AddonComponent)targeted).Addon;

				if (m_Type.IsAssignableFrom(targeted.GetType()))
				{
					Server.Commands.CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, targeted.ToString());
					m_Property.SetValue(m_Object, targeted, null);
				}
				else
				{
					m_Mobile.SendMessage("That cannot be assigned to a property of type : {0}", m_Type.Name);
				}
			}
			catch
			{
				m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
			}
		}

		protected override void OnTargetFinish(Mobile from)
		{
			if (m_Type == typeof(Type))
				from.SendGump(new PropertiesGump(m_Mobile, m_Object, m_Stack, m_List, m_Page));
			else
				from.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
		}
	}
}