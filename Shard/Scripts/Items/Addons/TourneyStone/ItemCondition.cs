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


/* Scripts/Items/Addons/TourneyStone/ItemCondition.cs
 * ChangeLog :
 * 	02/25/07, weaver
 *		Initial creation (code moved out of TournamentStoneAddon.cs)
 *
 */

using System;
using Server;
using System.Collections;
using Server.Misc;


namespace Server.Items
{

	public class ItemCondition : RuleCondition
	{
		// Item type + amount kind of rule

		public override bool Guage(object o, ref ArrayList Fallthroughs)
		{
			if (o == null || o.GetType() != typeof(ArrayList))
				return false;

			// wea: 25/Feb/2007 Modified to receive CurrentHeld list as opposed
			// to individual HeldItem

			int RuleItemMatchQty = 0;

			foreach (HeldItem hi in ((ArrayList)o))
			{
				if (hi.m_Type == ItemType || hi.m_Type.IsSubclassOf(ItemType))
					RuleItemMatchQty += hi.m_Count;
			}


			// Limit
			// -1	- Fall through : if the item count matches, all conditions pass
			// 0	- Require the count to be at least this
			// 1	- Limit the count to a maximum of this

			switch (Limit)
			{
				case -1:
					if (RuleItemMatchQty >= Quantity)
					{
						foreach (HeldItem hi in ((ArrayList)o))
						{
							foreach (Item item in hi.Ref)
							{
								Fallthroughs.Add(item);
							}
						}
					}
					return true;
				case 1:
					if (RuleItemMatchQty > Quantity)
						break;
					return true;
				case 0:
					if (RuleItemMatchQty < Quantity)
						break;
					return true;
			}

			// FAILED!!

			ClassNameTranslator cnt = new ClassNameTranslator();
			Rule.FailTextDyn = string.Format("{0} x {1}", RuleItemMatchQty, cnt.TranslateClass(this.ItemType));

			return false;
		}
	}

}