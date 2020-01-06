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


/* Scripts/Items/Addons/TourneyStone/Rule.cs
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


	public class Rule
	{
		private string m_Desc;
		private string m_FailText;
		private string m_FailTextDyn;

		private bool m_Active;
		private ArrayList m_Conditions;

		public ArrayList Conditions
		{
			get { return m_Conditions; }
			set { m_Conditions = value; }
		}

		public string Desc
		{
			get { return m_Desc; }
			set { m_Desc = value; }
		}

		public string FailText
		{
			get { return m_FailText; }
			set { m_FailText = value; }
		}

		public string FailTextDyn
		{
			get { return m_FailTextDyn; }
			set { m_FailTextDyn = value; }
		}

		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; }
		}

		public Rule()
		{
			Desc = "";
			FailText = "";
			FailTextDyn = "";
			Active = false;
		}

		public string DynFill(string sFilltp)
		{
			string sFilled = sFilltp;
			for (int cpos = 0; cpos < Conditions.Count; cpos++)
			{
				RuleCondition RuleCon = (RuleCondition)Conditions[cpos];

				sFilled = sFilled.Replace("%Condition" + (cpos + 1) + "_Quantity%", RuleCon.Quantity.ToString());
				sFilled = sFilled.Replace("%Condition" + (cpos + 1) + "_Property%", RuleCon.Property);
				sFilled = sFilled.Replace("%Condition" + (cpos + 1) + "_PropertyVal%", RuleCon.PropertyVal);
			}

			return sFilled;
		}

	}

}