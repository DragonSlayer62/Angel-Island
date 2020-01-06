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
using System.IO;
using System.Xml;
using System.Collections;
using Server;

namespace Server.Gumps
{
	public class LocationTree
	{
		private Map m_Map;
		private ParentNode m_Root;
		private Hashtable m_LastBranch;

		public LocationTree(string fileName, Map map)
		{
			m_LastBranch = new Hashtable();
			m_Map = map;

			string path = Path.Combine("Data/Locations/", fileName);

			if (File.Exists(path))
			{
				XmlTextReader xml = new XmlTextReader(new StreamReader(path));

				xml.WhitespaceHandling = WhitespaceHandling.None;

				m_Root = Parse(xml);

				xml.Close();
			}
		}

		public Hashtable LastBranch
		{
			get
			{
				return m_LastBranch;
			}
		}

		public Map Map
		{
			get
			{
				return m_Map;
			}
		}

		public ParentNode Root
		{
			get
			{
				return m_Root;
			}
		}

		private ParentNode Parse(XmlTextReader xml)
		{
			xml.Read();
			xml.Read();
			xml.Read();

			return new ParentNode(xml, null);
		}
	}
}