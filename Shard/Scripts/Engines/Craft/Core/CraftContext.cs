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
using System.Collections;

namespace Server.Engines.Craft
{
	public enum CraftMarkOption
	{
		MarkItem,
		DoNotMark,
		PromptForMark
	}

	public class CraftContext
	{
		private ArrayList m_Items;
		private int m_LastResourceIndex;
		private int m_LastResourceIndex2;
		private int m_LastGroupIndex;
		private bool m_DoNotColor;
		private CraftMarkOption m_MarkOption;

		public ArrayList Items { get { return m_Items; } }
		public int LastResourceIndex { get { return m_LastResourceIndex; } set { m_LastResourceIndex = value; } }
		public int LastResourceIndex2 { get { return m_LastResourceIndex2; } set { m_LastResourceIndex2 = value; } }
		public int LastGroupIndex { get { return m_LastGroupIndex; } set { m_LastGroupIndex = value; } }
		public bool DoNotColor { get { return m_DoNotColor; } set { m_DoNotColor = value; } }
		public CraftMarkOption MarkOption { get { return m_MarkOption; } set { m_MarkOption = value; } }

		public CraftContext()
		{
			m_Items = new ArrayList();
			m_LastResourceIndex = -1;
			m_LastResourceIndex2 = -1;
			m_LastGroupIndex = -1;
		}

		public CraftItem LastMade
		{
			get
			{
				if (m_Items.Count > 0)
					return (CraftItem)m_Items[0];

				return null;
			}
		}

		public void OnMade(CraftItem item)
		{
			m_Items.Remove(item);

			if (m_Items.Count == 10)
				m_Items.RemoveAt(9);

			m_Items.Insert(0, item);
		}
	}
}