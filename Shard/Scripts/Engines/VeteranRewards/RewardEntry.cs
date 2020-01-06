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

namespace Server.Engines.VeteranRewards
{
	public class RewardEntry
	{
		private RewardList m_List;
		private RewardCategory m_Category;
		private Type m_ItemType;
		private int m_Name;
		private string m_NameString;
		private object[] m_Args;

		public RewardList List { get { return m_List; } set { m_List = value; } }
		public RewardCategory Category { get { return m_Category; } }
		public Type ItemType { get { return m_ItemType; } }
		public int Name { get { return m_Name; } }
		public string NameString { get { return m_NameString; } }
		public object[] Args { get { return m_Args; } }

		public Item Construct()
		{
			try
			{
				Item item = Activator.CreateInstance(m_ItemType, m_Args) as Item;

				if (item is IRewardItem)
					((IRewardItem)item).IsRewardItem = true;

				return item;
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			return null;
		}

		public RewardEntry(RewardCategory category, int name, Type itemType, params object[] args)
		{
			m_Category = category;
			m_ItemType = itemType;
			m_Name = name;
			m_Args = args;
			category.Entries.Add(this);
		}

		public RewardEntry(RewardCategory category, string name, Type itemType, params object[] args)
		{
			m_Category = category;
			m_ItemType = itemType;
			m_NameString = name;
			m_Args = args;
			category.Entries.Add(this);
		}
	}
}