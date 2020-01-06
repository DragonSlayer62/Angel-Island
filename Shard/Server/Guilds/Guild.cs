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

/* Server/Guilds/Guild.cs
 * CHANGELOG:
 * 12/14/05 Kit,
 *		Added Case search for names/abbreviatons to Prevent Orc oRc from working.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Guilds
{
	public enum GuildType
	{
		Regular,
		Chaos,
		Order
	}

	public abstract class BaseGuild
	{
		private int m_Id;

		public BaseGuild(int Id)//serialization ctor
		{
			m_Id = Id;
			m_GuildList.Add(m_Id, this);
			if (m_Id + 1 > m_NextID)
				m_NextID = m_Id + 1;
		}

		public BaseGuild()
		{
			m_Id = m_NextID++;
			m_GuildList.Add(m_Id, this);
		}

		public int Id { get { return m_Id; } }

		public abstract void Deserialize(GenericReader reader);
		public abstract void Serialize(GenericWriter writer);

		public abstract string Abbreviation { get; set; }
		public abstract string Name { get; set; }
		public abstract GuildType Type { get; set; }
		public abstract bool Disbanded { get; }
		public abstract void OnDelete(Mobile mob);

		private static Hashtable m_GuildList = new Hashtable();
		private static int m_NextID = 1;

		public static Hashtable List
		{
			get
			{
				return m_GuildList;
			}
		}

		public static BaseGuild Find(int id)
		{
			return (BaseGuild)m_GuildList[id];
		}

		public static BaseGuild FindByName(string name)
		{
			foreach (BaseGuild g in m_GuildList.Values)
			{
				if (g.Name.ToLower() == name.ToLower())
					return g;
			}

			return null;
		}

		public static BaseGuild FindByAbbrev(string abbr)
		{
			foreach (BaseGuild g in m_GuildList.Values)
			{
				if (g.Abbreviation.ToLower() == abbr.ToLower())
					return g;
			}

			return null;
		}

		public static BaseGuild[] Search(string find)
		{
			string[] words = find.ToLower().Split(' ');
			ArrayList results = new ArrayList();

			foreach (BaseGuild g in m_GuildList.Values)
			{
				bool match = true;
				string name = g.Name.ToLower();
				for (int i = 0; i < words.Length; i++)
				{
					if (name.IndexOf(words[i]) == -1)
					{
						match = false;
						break;
					}
				}

				if (match)
					results.Add(g);
			}

			return (BaseGuild[])results.ToArray(typeof(BaseGuild));
		}
	}
}
