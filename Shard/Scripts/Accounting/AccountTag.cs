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
using System.Xml;

namespace Server.Accounting
{
	public class AccountTag
	{
		private string m_Name, m_Value;

		/// <summary>
		/// Gets or sets the name of this tag.
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Gets or sets the value of this tag.
		/// </summary>
		public string Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		/// <summary>
		/// Constructs a new AccountTag instance with a specific name and value.
		/// </summary>
		/// <param name="name">Initial name.</param>
		/// <param name="value">Initial value.</param>
		public AccountTag(string name, string value)
		{
			m_Name = name;
			m_Value = value;
		}

		/// <summary>
		/// Deserializes an AccountTag instance from an xml element.
		/// </summary>
		/// <param name="node">The XmlElement instance from which to deserialize.</param>
		public AccountTag(XmlElement node)
		{
			m_Name = Accounts.GetAttribute(node, "name", "empty");
			m_Value = Accounts.GetText(node, "");
		}

		/// <summary>
		/// Serializes this AccountTag instance to an XmlTextWriter.
		/// </summary>
		/// <param name="xml">The XmlTextWriter instance from which to serialize.</param>
		public void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("tag");
			xml.WriteAttributeString("name", m_Name);
			xml.WriteString(m_Value);
			xml.WriteEndElement();
		}
	}
}