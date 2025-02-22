/***************************************************************************
 *                                 Prompt.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Prompt.cs,v 1.7 2011/02/24 18:32:56 luket Exp $
 *   $Author: luket $
 *   $Date: 2011/02/24 18:32:56 $
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

namespace Server.Prompts
{
	public abstract class Prompt
	{
		private int m_Serial;
		private static int m_Serials;

		public int Serial
		{
			get
			{
				return m_Serial;
			}
		}

		public Prompt()
		{
			do
			{
				m_Serial = ++m_Serials;
			} while (m_Serial == 0);
		}

		public virtual void OnCancel(Mobile from)
		{
		}

		public virtual void OnResponse(Mobile from, string text)
		{
		}
	}
}