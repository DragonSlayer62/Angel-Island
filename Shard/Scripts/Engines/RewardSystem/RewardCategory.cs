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

/* Scripts/Engines/Reward System/RewardCategory.cs
 * Created 5/23/04 by mith
 * ChangeLog
 */

using System;
using System.Collections;

namespace Server.Engines.RewardSystem
{
	public class RewardCategory
	{
		private int m_Name;
		private string m_NameString;
		private string m_XmlString;
		private ArrayList m_Entries;

		public int Name { get { return m_Name; } }
		public string NameString { get { return m_NameString; } }
		public string XmlString { get { return m_XmlString; } }
		public ArrayList Entries { get { return m_Entries; } }

		public RewardCategory(int name, string xmlString)
		{
			m_Name = name;
			m_XmlString = xmlString;
			m_Entries = new ArrayList();
		}

		public RewardCategory(string name, string xmlString)
		{
			m_NameString = name;
			m_XmlString = xmlString;
			m_Entries = new ArrayList();
		}
	}
}