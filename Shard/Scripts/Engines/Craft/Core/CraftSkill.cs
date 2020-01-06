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
	public class CraftSkill
	{
		private SkillName m_SkillToMake;
		private double m_MinSkill;
		private double m_MaxSkill;

		public CraftSkill(SkillName skillToMake, double minSkill, double maxSkill)
		{
			m_SkillToMake = skillToMake;
			m_MinSkill = minSkill;
			m_MaxSkill = maxSkill;
		}

		public SkillName SkillToMake
		{
			get { return m_SkillToMake; }
		}

		public double MinSkill
		{
			get { return m_MinSkill; }
		}

		public double MaxSkill
		{
			get { return m_MaxSkill; }
		}
	}
}