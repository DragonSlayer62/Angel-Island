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

/* Scripts/Gumps/Properties/SetObjectGump.cs
 * Changelog:
 *  11/17,06, Adam
 *      Add: #pragma warning disable 429
 *      The Unreachable code complaint in this file is acceptable
 *      C:\Program Files\RunUO\Scripts\Gumps\Properties\SetObjectGump.cs(77,68): warning CS0429: Unreachable expression code detected
 */

#pragma warning disable 429

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;
using Server.Prompts;

namespace Server.Gumps
{
	public class SetObjectGump : Gump
	{
		private PropertyInfo m_Property;
		private Mobile m_Mobile;
		private object m_Object;
		private Stack m_Stack;
		private Type m_Type;
		private int m_Page;
		private ArrayList m_List;

		public const bool OldStyle = PropsConfig.OldStyle;

		public const int GumpOffsetX = PropsConfig.GumpOffsetX;
		public const int GumpOffsetY = PropsConfig.GumpOffsetY;

		public const int TextHue = PropsConfig.TextHue;
		public const int TextOffsetX = PropsConfig.TextOffsetX;

		public const int OffsetGumpID = PropsConfig.OffsetGumpID;
		public const int HeaderGumpID = PropsConfig.HeaderGumpID;
		public const int EntryGumpID = PropsConfig.EntryGumpID;
		public const int BackGumpID = PropsConfig.BackGumpID;
		public const int SetGumpID = PropsConfig.SetGumpID;

		public const int SetWidth = PropsConfig.SetWidth;
		public const int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public const int SetButtonID1 = PropsConfig.SetButtonID1;
		public const int SetButtonID2 = PropsConfig.SetButtonID2;

		public const int PrevWidth = PropsConfig.PrevWidth;
		public const int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public const int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public const int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public const int NextWidth = PropsConfig.NextWidth;
		public const int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public const int NextButtonID1 = PropsConfig.NextButtonID1;
		public const int NextButtonID2 = PropsConfig.NextButtonID2;

		public const int OffsetSize = PropsConfig.OffsetSize;

		public const int EntryHeight = PropsConfig.EntryHeight;
		public const int BorderSize = PropsConfig.BorderSize;

		private const int EntryWidth = 212;

		private const int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
		private const int TotalHeight = OffsetSize + (5 * (EntryHeight + OffsetSize));

		private const int BackWidth = BorderSize + TotalWidth + BorderSize;
		private const int BackHeight = BorderSize + TotalHeight + BorderSize;

		public SetObjectGump(PropertyInfo prop, Mobile mobile, object o, Stack stack, Type type, int page, ArrayList list)
			: base(GumpOffsetX, GumpOffsetY)
		{
			m_Property = prop;
			m_Mobile = mobile;
			m_Object = o;
			m_Stack = stack;
			m_Type = type;
			m_Page = page;
			m_List = list;

			string initialText = PropertiesGump.ValueToString(o, prop);

			AddPage(0);

			AddBackground(0, 0, BackWidth, BackHeight, BackGumpID);
			AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), TotalHeight, OffsetGumpID);

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, prop.Name);
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, initialText);
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

			AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 1, GumpButtonType.Reply, 0);

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, "Change by Serial");
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

			AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 2, GumpButtonType.Reply, 0);

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, "Nullify");
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

			AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 3, GumpButtonType.Reply, 0);

			x = BorderSize + OffsetSize;
			y += EntryHeight + OffsetSize;

			AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
			AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, TextHue, "View Properties");
			x += EntryWidth + OffsetSize;

			if (SetGumpID != 0)
				AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

			AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, 4, GumpButtonType.Reply, 0);
		}

		private class InternalPrompt : Prompt
		{
			private PropertyInfo m_Property;
			private Mobile m_Mobile;
			private object m_Object;
			private Stack m_Stack;
			private Type m_Type;
			private int m_Page;
			private ArrayList m_List;

			public InternalPrompt(PropertyInfo prop, Mobile mobile, object o, Stack stack, Type type, int page, ArrayList list)
			{
				m_Property = prop;
				m_Mobile = mobile;
				m_Object = o;
				m_Stack = stack;
				m_Type = type;
				m_Page = page;
				m_List = list;
			}

			public override void OnCancel(Mobile from)
			{
				m_Mobile.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
			}

			public override void OnResponse(Mobile from, string text)
			{
				object toSet;
				bool shouldSet;

				try
				{
					int serial = Utility.ToInt32(text);

					toSet = World.FindEntity(serial);

					if (toSet == null)
					{
						shouldSet = false;
						m_Mobile.SendMessage("No object with that serial was found.");
					}
					else if (!m_Type.IsAssignableFrom(toSet.GetType()))
					{
						toSet = null;
						shouldSet = false;
						m_Mobile.SendMessage("The object with that serial could not be assigned to a property of type : {0}", m_Type.Name);
					}
					else
					{
						shouldSet = true;
					}
				}
				catch
				{
					toSet = null;
					shouldSet = false;
					m_Mobile.SendMessage("Bad format");
				}

				if (shouldSet)
				{
					try
					{
						Server.Commands.CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, toSet == null ? "(null)" : toSet.ToString());
						m_Property.SetValue(m_Object, toSet, null);
					}
					catch
					{
						m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
					}
				}

				m_Mobile.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			object toSet;
			bool shouldSet, shouldSend = true;
			object viewProps = null;

			switch (info.ButtonID)
			{
				case 0: // closed
					{
						m_Mobile.SendGump(new PropertiesGump(m_Mobile, m_Object, m_Stack, m_List, m_Page));

						toSet = null;
						shouldSet = false;
						shouldSend = false;

						break;
					}
				case 1: // Change by Target
					{
						m_Mobile.Target = new SetObjectTarget(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List);
						toSet = null;
						shouldSet = false;
						shouldSend = false;
						break;
					}
				case 2: // Change by Serial
					{
						toSet = null;
						shouldSet = false;
						shouldSend = false;

						m_Mobile.SendMessage("Enter the serial you wish to find:");
						m_Mobile.Prompt = new InternalPrompt(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List);

						break;
					}
				case 3: // Nullify
					{
						toSet = null;
						shouldSet = true;

						break;
					}
				case 4: // View Properties
					{
						toSet = null;
						shouldSet = false;

						object obj = m_Property.GetValue(m_Object, null);

						if (obj == null)
							m_Mobile.SendMessage("The property is null and so you cannot view its properties.");
						else if (!Server.Commands.BaseCommand.IsAccessible(m_Mobile, obj))
							m_Mobile.SendMessage("You may not view their properties.");
						else
							viewProps = obj;

						break;
					}
				default:
					{
						toSet = null;
						shouldSet = false;

						break;
					}
			}

			if (shouldSet)
			{
				try
				{
					Server.Commands.CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, toSet == null ? "(null)" : toSet.ToString());
					m_Property.SetValue(m_Object, toSet, null);
				}
				catch
				{
					m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
				}
			}

			if (shouldSend)
				m_Mobile.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));

			if (viewProps != null)
				m_Mobile.SendGump(new PropertiesGump(m_Mobile, viewProps));
		}
	}
}