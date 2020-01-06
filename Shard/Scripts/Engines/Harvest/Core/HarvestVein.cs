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

namespace Server.Engines.Harvest
{
	public class HarvestVein
	{
		private double m_VeinChance;
		private double m_ChanceToFallback;
		private HarvestResource m_PrimaryResource;
		private HarvestResource m_FallbackResource;

		public double VeinChance { get { return m_VeinChance; } set { m_VeinChance = value; } }
		public double ChanceToFallback { get { return m_ChanceToFallback; } set { m_ChanceToFallback = value; } }
		public HarvestResource PrimaryResource { get { return m_PrimaryResource; } set { m_PrimaryResource = value; } }
		public HarvestResource FallbackResource { get { return m_FallbackResource; } set { m_FallbackResource = value; } }

		public HarvestVein(double veinChance, double chanceToFallback, HarvestResource primaryResource, HarvestResource fallbackResource)
		{
			m_VeinChance = veinChance;
			m_ChanceToFallback = chanceToFallback;
			m_PrimaryResource = primaryResource;
			m_FallbackResource = fallbackResource;
		}
	}
}