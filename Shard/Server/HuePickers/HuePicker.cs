/***************************************************************************
 *                               HuePicker.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: HuePicker.cs,v 1.8 2011/02/24 18:32:46 luket Exp $
 *   $Author: luket $
 *   $Date: 2011/02/24 18:32:46 $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Server.Network;

namespace Server.HuePickers
{
	public class HuePicker
	{
		private static int m_NextSerial = 1;

		private int m_Serial;
		private int m_ItemID;

		public int Serial
		{
			get
			{
				return m_Serial;
			}
		}

		public int ItemID
		{
			get
			{
				return m_ItemID;
			}
		}

		public HuePicker(int itemID)
		{
			do
			{
				m_Serial = m_NextSerial++;
			} while (m_Serial == 0);

			m_ItemID = itemID;
		}

		public virtual void OnResponse(int hue)
		{
		}

		public void SendTo(NetState state)
		{
			state.Send(new DisplayHuePicker(this));
			state.AddHuePicker(this);
		}
	}
}