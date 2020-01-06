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

namespace Server.Engines.Craft
{
	public class CraftSubResCol : System.Collections.CollectionBase
	{
		private Type m_Type;
		private string m_NameString;
		private int m_NameNumber;
		private bool m_Init;

		public bool Init
		{
			get { return m_Init; }
			set { m_Init = value; }
		}

		public Type ResType
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public string NameString
		{
			get { return m_NameString; }
			set { m_NameString = value; }
		}

		public int NameNumber
		{
			get { return m_NameNumber; }
			set { m_NameNumber = value; }
		}

		public CraftSubResCol()
		{
			m_Init = false;
		}

		public void Add(CraftSubRes craftSubRes)
		{
			List.Add(craftSubRes);
		}

		public void Remove(int index)
		{
			if (index > Count - 1 || index < 0)
			{
			}
			else
			{
				List.RemoveAt(index);
			}
		}

		public CraftSubRes GetAt(int index)
		{
			return (CraftSubRes)List[index];
		}

		public CraftSubRes SearchFor(Type type)
		{
			for (int i = 0; i < List.Count; i++)
			{
				CraftSubRes craftSubRes = (CraftSubRes)List[i];
				if (craftSubRes.ItemType == type)
				{
					return craftSubRes;
				}
			}
			return null;
		}
		public int IndexOf(Type type)
		{
			for (int i = 0; i < List.Count; i++)
			{
				CraftSubRes craftSubRes = (CraftSubRes)List[i];
				if (craftSubRes.ItemType == type)
				{
					return i;
				}
			}
			return -1;
		}


	}
}