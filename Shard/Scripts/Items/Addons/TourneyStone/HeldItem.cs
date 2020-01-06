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


/* Scripts/Items/Addons/TourneyStone/HeldItem.cs
 * ChangeLog :
 * 	02/25/07, weaver
 *		Initial creation (code moved out of TournamentStoneAddon.cs)
 *
 */


using System;
using Server;
using System.Collections;

namespace Server.Items
{
	public class HeldItem
	{
		// What is this type?
		public Type m_Type;

		// How many items of this type?
		public int m_Count;

		// Reference to the objects themselves
		private ArrayList m_Ref;
		public ArrayList Ref { get { return m_Ref; } set { m_Ref = value; } }

		public HeldItem(Type type)
		{
			m_Count = 1;
			m_Type = type;
			m_Ref = new ArrayList();
		}
	}
}
