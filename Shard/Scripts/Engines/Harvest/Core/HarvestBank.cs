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
	public class HarvestBank
	{
		private int m_Current;
		private int m_Maximum;
		private DateTime m_NextRespawn;
		private HarvestVein m_Vein, m_DefaultVein;

		public int Current
		{
			get
			{
				CheckRespawn();
				return m_Current;
			}
		}

		public HarvestVein Vein
		{
			get
			{
				CheckRespawn();
				return m_Vein;
			}
			set
			{
				m_Vein = value;
			}
		}

		public HarvestVein DefaultVein
		{
			get
			{
				CheckRespawn();
				return m_DefaultVein;
			}
		}

		public void CheckRespawn()
		{
			if (m_Current == m_Maximum || m_NextRespawn > DateTime.Now)
				return;

			m_Current = m_Maximum;
			m_Vein = m_DefaultVein;
		}

		public void Consume(HarvestDefinition def, int amount)
		{
			CheckRespawn();

			if (m_Current == m_Maximum)
			{
				double min = def.MinRespawn.TotalMinutes;
				double max = def.MaxRespawn.TotalMinutes;
				double rnd = Utility.RandomDouble();

				m_Current = m_Maximum - amount;
				m_NextRespawn = DateTime.Now + TimeSpan.FromMinutes(min + (rnd * (max - min)));
			}
			else
			{
				m_Current -= amount;
			}

			if (m_Current < 0)
				m_Current = 0;
		}

		public HarvestBank(HarvestDefinition def, HarvestVein defaultVein)
		{
			m_Maximum = Utility.RandomMinMax(def.MinTotal, def.MaxTotal);
			m_Current = m_Maximum;
			m_DefaultVein = defaultVein;
			m_Vein = m_DefaultVein;
		}
	}
}