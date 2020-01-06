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
using System.Collections;
using Server;

namespace Server.Gumps
{
	public class ParentNode
	{
		private ParentNode m_Parent;
		private object[] m_Children;

		private string m_Name;

		public ParentNode(XmlTextReader xml, ParentNode parent)
		{
			m_Parent = parent;

			Parse(xml);
		}

		private void Parse(XmlTextReader xml)
		{
			if (xml.MoveToAttribute("name"))
				m_Name = xml.Value;
			else
				m_Name = "empty";

			if (xml.IsEmptyElement)
			{
				m_Children = new object[0];
			}
			else
			{
				ArrayList children = new ArrayList();

				while (xml.Read() && xml.NodeType == XmlNodeType.Element)
				{
					if (xml.Name == "child")
					{
						ChildNode n = new ChildNode(xml, this);

						children.Add(n);
					}
					else
					{
						children.Add(new ParentNode(xml, this));
					}
				}

				m_Children = children.ToArray();
			}
		}

		public ParentNode Parent
		{
			get
			{
				return m_Parent;
			}
		}

		public object[] Children
		{
			get
			{
				return m_Children;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}
	}
}