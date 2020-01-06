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
using Server.Mobiles;
using System.Text.RegularExpressions;
using Server.Commands;

namespace Server.Items
{
	public class PropertyCondition : RuleCondition
	{

		// Limitation on the playermobile property

		public override bool Guage(object o, ref ArrayList Fallthroughs)
		{
			if (!(o is Mobile) || o == null)
				return false;

			Mobile Player = (Mobile)o;

			if (Property.Length > 7)
			{
				if (Property.Substring(0, 6) == "skill ")
				{
					// Looking for a skill

					string SkillArg = Property.Substring(6);
					string strSkill = ((SkillArg.Substring(0, 1)).ToUpper() + SkillArg.Substring(1));

					for (int isk = 0; isk < 53; isk++)
					{
						// Fallthrough if the player matched a skill that allowed such
						if (Fallthroughs.Contains(Player))
							continue;

						// Limit :
						// -1	- Fall through : if the skill value matches or is less, passes all other conditions
						// 0	- Require the skill to be at least this
						// 1	- Limit the skill to this

						if (Player.Skills[isk].SkillName.ToString() == strSkill)
						{
							switch (Limit)
							{
								case -1:
									if (Player.Skills[isk].Base > Quantity)
									{
										Fallthroughs.Add(Player);
										return true;
									}
									break;

								case 1:
									if (Player.Skills[isk].Base > Quantity)
									{
										Rule.FailTextDyn = string.Format("Your skill in this is {0}.", Player.Skills[isk].Base);
										return false;
									}
									break;

								case 0:
									if (Player.Skills[isk].Base < Quantity)
									{
										Rule.FailTextDyn = string.Format("Your skill in this is {0}.", Player.Skills[isk].Base);
										return false;
									}
									break;
							}
						}
					}

					return true;
				}
			}

			// Regular player property


			if (PropertyVal == "")
			{
				// This is a quantity match
				PlayerMobile FakeGM = new PlayerMobile();
				FakeGM.AccessLevel = AccessLevel.GameMaster;

				string sVal = Properties.GetValue(FakeGM, Player, Property);

				FakeGM.Delete();

				int iStrPos = Property.Length + 3;

				// Ascertain numeric value
				string sNum = "";
				while (sVal[iStrPos] != ' ')
					sNum += sVal[iStrPos++];

				int iVal;

				try
				{
					iVal = Convert.ToInt32(sNum);
				}
				catch (Exception exc)
				{
					Console.WriteLine("TourneyStoneAddon: Exception - (trying to convert {1} to integer)", exc, sNum);
					return true;
				}

				// Compare

				switch (Limit)
				{
					case 1:
						if (Quantity >= iVal)
							return true;
						break;
					case 0:
						if (Quantity <= iVal)
							return true;
						break;
				}

				return false;
			}
			else
			{
				// This is a text match
				Regex PattMatch = new Regex("= \"*" + PropertyVal, RegexOptions.IgnoreCase);

				PlayerMobile FakeGM = new PlayerMobile();
				FakeGM.AccessLevel = AccessLevel.GameMaster;

				if (PattMatch.IsMatch(Properties.GetValue(FakeGM, Player, Property)))
				{
					FakeGM.Delete();
					return false;
				}

				FakeGM.Delete();
			}

			return true;
		}
	}

}