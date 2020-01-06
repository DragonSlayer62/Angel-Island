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
	public class HarvestSoundTimer : Timer
	{
		private Mobile m_From;
		private Item m_Tool;
		private HarvestSystem m_System;
		private HarvestDefinition m_Definition;
		private object m_ToHarvest, m_Locked;
		private bool m_Last;

		public HarvestSoundTimer(Mobile from, Item tool, HarvestSystem system, HarvestDefinition def, object toHarvest, object locked, bool last)
			: base(def.EffectSoundDelay)
		{
			m_From = from;
			m_Tool = tool;
			m_System = system;
			m_Definition = def;
			m_ToHarvest = toHarvest;
			m_Locked = locked;
			m_Last = last;
		}

		protected override void OnTick()
		{
			m_System.DoHarvestingSound(m_From, m_Tool, m_Definition, m_ToHarvest);

			if (m_Last)
				m_System.FinishHarvesting(m_From, m_Tool, m_Definition, m_ToHarvest, m_Locked);
		}
	}
}