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

/* Scripts/Engines/ChampionSpawn/Modes/ChampInvasion.cs
 *	ChangeLog:
 *	10/28/2006, plasma
 *		Initial creation
 * 
 **/
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.ChampionSpawn
{
	// This is the town ivasion champion spawn, automated by the AES
	public class ChampInvasion : ChampEngine
	{
		// Members
		private TownInvasionAES m_Monitor;				// external AES spawn monitor

		// props
		public TownInvasionAES Monitor					// and a prop for it
		{
			get { return m_Monitor; }
			set { m_Monitor = value; }
		}

		[Constructable]
		public ChampInvasion()
			: base()
		{
			// pick a random champ
			PickChamp();
			// switch off  gfx and restart timer
			Graphics = false;
			m_bRestart = false;
		}

		protected override void AdvanceLevel()
		{
			// has champ just been completed?
			if (IsFinalLevel)
			{
				// tell AES that the champ is over
				if (m_Monitor != null && m_Monitor.Deleted == false)
					m_Monitor.ChampComplete = true;
			}
			base.AdvanceLevel();
		}
		public ChampInvasion(Serial serial)
			: base(serial)
		{
		}

		// #region serialize

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
			writer.Write(m_Monitor);  //AES 			
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Monitor = reader.ReadItem() as TownInvasionAES;
						break;
					}
			}
		}
		// #endregion

		public void PickChamp()
		{
			// Currently the invasions randomly pick one of the 5 main big skull giving champs
			switch (Utility.Random(5))
			{
				case 0:
					{
						SpawnType = ChampLevelData.SpawnTypes.Abyss;
						break;
					}
				case 1:
					{
						SpawnType = ChampLevelData.SpawnTypes.Arachnid;
						break;
					}
				case 2:
					{
						SpawnType = ChampLevelData.SpawnTypes.ColdBlood;
						break;
					}
				case 3:
					{
						SpawnType = ChampLevelData.SpawnTypes.UnholyTerror;
						break;
					}
				case 4:
					{
						SpawnType = ChampLevelData.SpawnTypes.VerminHorde;
						break;
					}
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			if (from.AccessLevel >= AccessLevel.GameMaster)
			{
				// this is a gm, allow normal text from base and champ indicator
				LabelTo(from, "Invasion Champ");
				base.OnSingleClick(from);
			}
		}
	}
}
