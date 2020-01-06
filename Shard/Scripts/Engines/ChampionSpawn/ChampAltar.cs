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

/* Scripts/Engines/ChampionSpawn/ChampAltar.cs
 * ChangeLog
 *	10/28/2006, plasma
 *		Initial creation
 */

using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Engines.ChampionSpawn
{

	public class ChampAltar : PentagramAddon
	{
		private ChampEngine m_Spawn;
		private bool m_quiet;

		public ChampAltar(bool bQuiet, ChampEngine spawn)
			: base(bQuiet)
		{
			m_Spawn = spawn;
			m_quiet = bQuiet;
			Visible = true;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();
		}

		public ChampAltar(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Spawn);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Spawn = reader.ReadItem() as ChampEngine;

						if (m_Spawn == null)
							Delete();

						break;
					}
			}
		}
	}
}