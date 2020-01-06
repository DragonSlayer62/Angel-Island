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


/* Scripts/Items/Addons/TourneyStone/ItemPropertyCondition.cs
 * ChangeLog :
 *  02/28/07, weaver
 *		Added safety check to ensure object being passed is correct.
 * 	02/25/07, weaver
 *		Initial creation (code moved out of TournamentStoneAddon.cs)
 *
 */

using System;
using Server;
using System.Collections;
using Server.Mobiles;
using System.Text.RegularExpressions;
using Server.Misc;
using Server.Commands;

namespace Server.Items
{

	public class ItemPropertyCondition : RuleCondition
	{
		// Limitation on the item property

		public override bool Guage(object o, ref ArrayList Fallthroughs)
		{
			if (o == null || !(o is HeldItem))          // wea: 28/Feb/2007 Added safety check
				return false;

			// We'll be dealing with a helditem object then
			HeldItem ipi = (HeldItem)o;

			// Limit
			// -1	- Fall through : if the item matches, passes all checks
			// 0	- Require the value to be at least this / require this property
			// 1	- Limit the value to a max of this / only this

			if (ipi.m_Type == ItemType || ipi.m_Type.IsSubclassOf(ItemType))
			{
				PlayerMobile FakeGM = new PlayerMobile();
				FakeGM.AccessLevel = AccessLevel.GameMaster;

				int FailCount = 0;

				foreach (Item item in ipi.Ref)
				{
					if (Fallthroughs.Contains(item))
						continue;		// Fallthrough

					string PropTest = Properties.GetValue(FakeGM, item, Property);

					if (PropertyVal != "")
					{
						Regex IPMatch = new Regex("= \"*" + PropertyVal, RegexOptions.IgnoreCase);
						if (IPMatch.IsMatch(PropTest))
						{
							switch (Limit)
							{
								case -1:
									// Not required, but has fallthrough and matches so skip to next item reference
									Fallthroughs.Add(item);
									continue;
								case 1:
									// It's limited to this and has matched, so that's fine
									continue;
								case 0:
									// Required, matched, so fine
									continue;
							}
						}
						else
						{
							switch (Limit)
							{
								case -1:
									// Not required, so don't worry but don't fallthrough either
									continue;
								case 1:
									// It's limited to this and doesn't match, so it fails
									break;
								case 0:
									// Required, not matched so not cool
									break;
							}
						}
					}
					else
					{
						// Ascertain numeric value
						string sNum = "";
						int iStrPos = Property.Length + 3;

						while (PropTest[iStrPos] != ' ')
							sNum += PropTest[iStrPos++];

						int iCompareTo;

						try
						{
							iCompareTo = Convert.ToInt32(sNum);
						}
						catch (Exception exc)
						{
							Console.WriteLine("TourneyStoneAddon: Exception - (trying to convert {1} to integer)", exc, sNum);
							continue;
						}

						switch (Limit)
						{
							case 0:
								if (iCompareTo >= Quantity)
									continue;
								break;
							case 1:
								if (iCompareTo <= Quantity)
									continue;
								break;
							case -1:
								if (iCompareTo <= Quantity)
								{
									Fallthroughs.Add(item);
									continue;
								}
								break;
						}
					}

					// FAILED!!! Otherwise we would have continued
					FailCount++;

				} // Loop Item


				if (FailCount > 0)
				{
					ClassNameTranslator cnt = new ClassNameTranslator();
					Rule.FailTextDyn = string.Format("{0} x {1}", FailCount, cnt.TranslateClass(ipi.m_Type));

					return false;

				}
				else
					return true;
			}

			return true;
		}
	}

}