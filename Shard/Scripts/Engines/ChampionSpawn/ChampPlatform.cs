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

/* Scripts/Engines/ChampionSpawn/ChampPlatform.cs
 *	ChangeLog
 *	10/28/2006, plasma
 *		Initial creation 
 */

using System;
using System.Collections;
using Server;
using Server.Items;


namespace Server.Engines.ChampionSpawn
{
	public class ChampPlatform : BaseAddon
	{
		private ChampEngine m_Spawn;
		private bool m_quiet;

		public ChampPlatform(bool bQuiet, ChampEngine spawn)
		{
			m_Spawn = spawn;
			m_quiet = bQuiet;

			for (int x = -2; x <= 2; ++x)
				for (int y = -2; y <= 2; ++y)
					AddComponent(0x750, x, y, -5);

			for (int x = -1; x <= 1; ++x)
				for (int y = -1; y <= 1; ++y)
					AddComponent(0x750, x, y, 0);

			for (int i = -1; i <= 1; ++i)
			{
				AddComponent(0x751, i, 2, 0);
				AddComponent(0x752, 2, i, 0);

				AddComponent(0x753, i, -2, 0);
				AddComponent(0x754, -2, i, 0);
			}

			AddComponent(0x759, -2, -2, 0);
			AddComponent(0x75A, 2, 2, 0);
			AddComponent(0x75B, -2, 2, 0);
			AddComponent(0x75C, 2, -2, 0);
		}

		public void AddComponent(int id, int x, int y, int z)
		{
			AddonComponent ac = new AddonComponent(id);

			ac.Hue = 0x497;
			ac.Visible = (m_quiet == true) ? false : true;

			AddComponent(ac, x, y, z);
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

		}

		public ChampPlatform(Serial serial)
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