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


/* Scripts/Items/Addons/TourneyStone/RuleCondition.cs
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


	// RULE CONDITION =============================
	public abstract class RuleCondition
	{
		private Rule m_Rule;

		private int m_Quantity;
		private string m_Property;
		private string m_PropertyVal;
		private string m_ItemProperty;
		private Type m_ItemType;
		private bool m_Configurable;
		private int m_Limit;

		public Rule Rule
		{
			get { return m_Rule; }
			set { m_Rule = value; }
		}

		public int Quantity
		{
			get { return m_Quantity; }
			set { m_Quantity = value; }
		}
		public Type ItemType
		{
			get { return m_ItemType; }
			set { m_ItemType = value; }
		}
		public string Property
		{
			get { return m_Property; }
			set { m_Property = value; }
		}
		public string PropertyVal
		{
			get { return m_PropertyVal; }
			set { m_PropertyVal = value; }
		}
		public string ItemProperty
		{
			get { return m_ItemProperty; }
			set { m_ItemProperty = value; }
		}
		public bool Configurable
		{
			get { return m_Configurable; }
			set { m_Configurable = value; }
		}
		public int Limit
		{
			get { return m_Limit; }
			set { m_Limit = value; }
		}

		public virtual bool Guage(object o)
		{
			ArrayList empty = new ArrayList();
			return Guage(o, ref empty);
		}

		public abstract bool Guage(object o, ref ArrayList Fallthroughs);

		public RuleCondition()
		{
			m_ItemType = null;
			Quantity = 0;

			Property = "";
			PropertyVal = "";

			ItemProperty = "";
			Configurable = false;

			Limit = 1;
		}
	}
	// END CONDITION ==============================


}