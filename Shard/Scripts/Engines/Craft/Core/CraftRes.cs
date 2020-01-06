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

namespace Server.Engines.Craft
{
	public class CraftRes
	{
		private Type m_Type;
		private int m_Amount;

		private string m_MessageString;
		private int m_MessageNumber;

		private string m_NameString;
		private int m_NameNumber;

		public CraftRes(Type type, int amount)
		{
			m_Type = type;
			m_Amount = amount;
		}

		public CraftRes(Type type, int name, int amount, int message)
			: this(type, amount)
		{
			m_NameNumber = name;
			m_MessageNumber = message;
		}

		public CraftRes(Type type, int name, int amount, string message)
			: this(type, amount)
		{
			m_NameNumber = name;
			m_MessageString = message;
		}

		public CraftRes(Type type, string name, int amount, int message)
			: this(type, amount)
		{
			m_NameString = name;
			m_MessageNumber = message;
		}

		public CraftRes(Type type, string name, int amount, string message)
			: this(type, amount)
		{
			m_NameString = name;
			m_MessageString = message;
		}

		public void SendMessage(Mobile from)
		{
			if (m_MessageNumber > 0)
				from.SendLocalizedMessage(m_MessageNumber);
			else if (m_MessageString != null && m_MessageString != String.Empty)
				from.SendMessage(m_MessageString);
			else
				from.SendLocalizedMessage(502925); // You don't have the resources required to make that item.
		}

		public Type ItemType
		{
			get { return m_Type; }
		}

		public string MessageString
		{
			get { return m_MessageString; }
		}

		public int MessageNumber
		{
			get { return m_MessageNumber; }
		}

		public string NameString
		{
			get { return m_NameString; }
		}

		public int NameNumber
		{
			get { return m_NameNumber; }
		}

		public int Amount
		{
			get { return m_Amount; }
		}
	}
}