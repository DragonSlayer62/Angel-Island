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
using Server;

namespace Server.Gumps
{
	public class ChildNode
	{
		private ParentNode m_Parent;

		private string m_Name;
		private Point3D m_Location;

		public ChildNode(XmlTextReader xml, ParentNode parent)
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

			int x = 0, y = 0, z = 0;

			if (xml.MoveToAttribute("x"))
				x = Utility.ToInt32(xml.Value);

			if (xml.MoveToAttribute("y"))
				y = Utility.ToInt32(xml.Value);

			if (xml.MoveToAttribute("z"))
				z = Utility.ToInt32(xml.Value);

			m_Location = new Point3D(x, y, z);
		}

		public ParentNode Parent
		{
			get
			{
				return m_Parent;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}

		public Point3D Location
		{
			get
			{
				return m_Location;
			}
		}
	}
}