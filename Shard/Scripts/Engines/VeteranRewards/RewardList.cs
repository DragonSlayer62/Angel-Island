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
	public class RewardList
	{
		private TimeSpan m_Age;
		private RewardEntry[] m_Entries;

		public TimeSpan Age { get { return m_Age; } }
		public RewardEntry[] Entries { get { return m_Entries; } }

		public RewardList(TimeSpan interval, int index, RewardEntry[] entries)
		{
			m_Age = TimeSpan.FromDays(interval.TotalDays * index);
			m_Entries = entries;

			for (int i = 0; i < entries.Length; ++i)
				entries[i].List = this;
		}
	}
}