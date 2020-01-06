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
	public class AccountComment
	{
		private string m_AddedBy;
		private string m_Content;
		private DateTime m_LastModified;

		/// <summary>
		/// A string representing who added this comment.
		/// </summary>
		public string AddedBy
		{
			get { return m_AddedBy; }
		}

		/// <summary>
		/// Gets or sets the body of this comment. Setting this value will reset LastModified.
		/// </summary>
		public string Content
		{
			get { return m_Content; }
			set { m_Content = value; m_LastModified = DateTime.Now; }
		}

		/// <summary>
		/// The date and time when this account was last modified -or- the comment creation time, if never modified.
		/// </summary>
		public DateTime LastModified
		{
			get { return m_LastModified; }
		}

		/// <summary>
		/// Constructs a new AccountComment instance.
		/// </summary>
		/// <param name="addedBy">Initial AddedBy value.</param>
		/// <param name="content">Initial Content value.</param>
		public AccountComment(string addedBy, string content)
		{
			m_AddedBy = addedBy;
			m_Content = content;
			m_LastModified = DateTime.Now;
		}

		/// <summary>
		/// Deserializes an AccountComment instance from an xml element.
		/// </summary>
		/// <param name="node">The XmlElement instance from which to deserialize.</param>
		public AccountComment(XmlElement node)
		{
			m_AddedBy = Accounts.GetAttribute(node, "addedBy", "empty");
			m_LastModified = Accounts.GetDateTime(Accounts.GetAttribute(node, "lastModified"), DateTime.Now);
			m_Content = Accounts.GetText(node, "");
		}

		/// <summary>
		/// Serializes this AccountComment instance to an XmlTextWriter.
		/// </summary>
		/// <param name="xml">The XmlTextWriter instance from which to serialize.</param>
		public void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("comment");

			xml.WriteAttributeString("addedBy", m_AddedBy);
			xml.WriteAttributeString("lastModified", XmlConvert.ToString(m_LastModified));

			xml.WriteString(m_Content);

			xml.WriteEndElement();
		}
	}
}