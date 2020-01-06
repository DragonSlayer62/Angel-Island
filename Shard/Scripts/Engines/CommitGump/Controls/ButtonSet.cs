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
 *			March 27, 2007
 */

/* Scripts/Engines/CommitGump/Controls/ButtonSet.cs
 * CHANGELOG:								 
 *	01/26/09 - Plasma,
 *		Initial creation
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server.Gumps;
using Server.Mobiles;
using Server.Engines.CommitGump;
using Server.Engines.IOBSystem;

namespace Server.Engines.CommitGump.Controls
{

	/// <summary>
	/// Creates a set of buttons and labels, providing hooks to control their response, text colour and button status.
	/// The index will always come back 0-based from the start of you data. 
	/// You can simply use the index straight in a collection indexer, cast to a enum, etc.
	/// </summary>
	public class ButtonSet : ICommitGumpEntity
	{
		#region delegates

		//Invoked to obtain the button press status for each button
		public delegate bool QueryButtonStatusDelegate(int buttonID);
		//Invoked to obtain the label colouur for each button
		public delegate int QueryLabelColourDelegate(int buttonID);
		//Invoked when a button is pressed
		public delegate void ButtonPressedDelegate(int buttonID);

		#endregion

		#region fields

		int m_Columns = 0;			//How many columns to draw with the data
		int m_XSpacer = 0;			//Width between columns
		int m_YSpacer = 0;			//Height between rows
		int m_OnImage = 0;
		int m_OffImage = 0;
		int m_StartX = 0;
		int m_StartY = 0;
		int m_ButtonIdOffset = 0;
		CommitGumpBase m_Gump = null;
		Dictionary<int, string> _setData = new Dictionary<int, string>();

		//delegates
		private ButtonPressedDelegate m_ButtonPress;
		private QueryButtonStatusDelegate m_QueryButtonStatus;
		private QueryLabelColourDelegate m_QueryButtonTextColour;

		#endregion

		public ButtonSet()
		{

		}

		//epic
		public ButtonSet(CommitGumpBase gump, Type enumType,
				 int Columns, int XSpacer,
				 int YSpacer, int OnImage,
				 int OffImage, int StartX,
				 int StartY, int ButtonIdOffset,
				 QueryButtonStatusDelegate queryStatusMethod, ButtonPressedDelegate pressMethod, QueryLabelColourDelegate queryColourMethod
			)
		{
			if (!enumType.IsEnum) return;
			if (Enum.GetUnderlyingType(enumType) != typeof(int)) return;
			m_Gump = gump;
			m_Columns = Columns;
			m_XSpacer = XSpacer;
			m_YSpacer = YSpacer;
			m_OnImage = OnImage;
			m_OffImage = OffImage;
			m_StartX = StartX;
			m_StartY = StartY;
			m_ButtonIdOffset = ButtonIdOffset;
			m_QueryButtonStatus = queryStatusMethod;
			m_ButtonPress = pressMethod;
			m_QueryButtonTextColour = queryColourMethod;

			foreach (int val in Enum.GetValues(enumType))
				_setData.Add(val, Enum.GetName(enumType, val));

			((ICommitGumpEntity)this).Create();


		}

		//epic
		public ButtonSet(CommitGumpBase gump, List<String> items,
				 int Columns, int XSpacer,
				 int YSpacer, int OnImage,
				 int OffImage, int StartX,
				 int StartY, int ButtonIdOffset,
				 QueryButtonStatusDelegate queryStatusMethod, ButtonPressedDelegate pressMethod, QueryLabelColourDelegate queryColourMethod
			)
		{
			m_Gump = gump;
			m_Columns = Columns;
			m_XSpacer = XSpacer;
			m_YSpacer = YSpacer;
			m_OnImage = OnImage;
			m_OffImage = OffImage;
			m_StartX = StartX;
			m_StartY = StartY;
			m_ButtonIdOffset = ButtonIdOffset;
			m_QueryButtonStatus = queryStatusMethod;
			m_ButtonPress = pressMethod;
			m_QueryButtonTextColour = queryColourMethod;
			int id = 0;
			foreach (string s in items)
			{
				_setData.Add(id, s);
				++id;					 //LOCAL id - this is used with the offset for the gump.  The point here is so the calee doesn't have to care about IDs.
			}
			items.Clear();

			((ICommitGumpEntity)this).Create();
		}

		private int GetHighestID()
		{
			int highest = 0;
			foreach (KeyValuePair<int, string> kvp in _setData)
				if (kvp.Key > highest) highest = kvp.Key;
			return highest;
		}

		#region ICommitGumpEntity Members

		string ICommitGumpEntity.ID
		{
			get { return string.Empty; }
		}

		void ICommitGumpEntity.CommitChanges()
		{
		}

		void ICommitGumpEntity.Create()
		{
			int x = m_StartX;
			int y = m_StartY;
			int col = 0;
			foreach (KeyValuePair<int, string> kvp in _setData)
			{
				int on = m_OnImage, off = m_OffImage;
				if (m_QueryButtonStatus != null)
				{
					if (m_QueryButtonStatus(kvp.Key))
					{
						on = m_OffImage;
						off = m_OnImage;
					}
				}
				m_Gump.AddButton(x + (col * m_XSpacer), y, off, on, m_ButtonIdOffset + kvp.Key, GumpButtonType.Reply, 0);
				m_Gump.AddLabel(x + (col * m_XSpacer) + 38, y, m_QueryButtonTextColour(kvp.Key), kvp.Value);
				col++;
				if (col == m_Columns)
				{
					col = 0;
					y += m_YSpacer;
				}
			}

		}

		void ICommitGumpEntity.LoadStateInfo()
		{
		}

		CommitGumpBase.GumpReturnType ICommitGumpEntity.OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (info.ButtonID >= m_ButtonIdOffset && info.ButtonID <= (m_ButtonIdOffset + GetHighestID()))
			{
				if (m_ButtonPress != null)
				{
					m_ButtonPress(info.ButtonID - m_ButtonIdOffset);
				}
			}
			return CommitGumpBase.GumpReturnType.None;
		}

		void ICommitGumpEntity.SaveStateInfo()
		{

		}

		bool ICommitGumpEntity.Validate()
		{
			return true;
		}

		#endregion
	}

}
