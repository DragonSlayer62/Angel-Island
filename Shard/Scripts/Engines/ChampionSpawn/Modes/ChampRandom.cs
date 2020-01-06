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

/* Scripts/Engines/ChampionSpawn/Modes/ChampRandom.cs
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
	public class ChampRandom : ChampEngine
	{
		[Constructable]
		public ChampRandom()
			: base()
		{
			// default constructor
			// assgin initial values
			PickChamp();
			//Switch graphics on
			Graphics = true;

			// Switch restart on, 5 min delay
			m_bRestart = true;
			m_RestartDelay = TimeSpan.FromMinutes(5);
		}

		public void PickChamp()
		{
			// Randomly pick one of the 5 main big champs
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

		public ChampRandom(Serial serial)
			: base(serial)
		{
		}

		// #region serialize

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0: break;
			}
		}
		// #endregion

		public override void Restart()
		{
			// When the champ re-starts, we want to choose a new champ first
			PickChamp();
			base.Restart();
		}

		public override void OnSingleClick(Mobile from)
		{
			if (from.AccessLevel >= AccessLevel.GameMaster)
			{
				// this is a gm, allow normal text from base and champ indicator
				LabelTo(from, "Random Champ");
				base.OnSingleClick(from);
			}
		}
	}
}
