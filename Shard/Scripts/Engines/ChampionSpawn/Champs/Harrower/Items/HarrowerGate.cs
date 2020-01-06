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

/* Scripts\Engines\ChampionSpawn\Champs\Harrower\Items\HarrowerGate.cs
 * CHANGELOG
 *  01/05/07, Plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using Server;

namespace Server.Items
{
	public class HarrowerGate : Moongate
	{
		private Mobile m_Harrower;

		public override int LabelNumber { get { return 1049498; } } // dark moongate

		public HarrowerGate(Mobile harrower, Point3D loc, Map map, Point3D targLoc, Map targMap)
			: base(targLoc, targMap)
		{
			m_Harrower = harrower;

			Dispellable = false;
			ItemID = 0x1FD4;
			Light = LightType.Circle300;

			MoveToWorld(loc, map);
		}

		public HarrowerGate(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Harrower);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Harrower = reader.ReadMobile();

						if (m_Harrower == null)
							Delete();

						break;
					}
			}

			if (Light != LightType.Circle300)
				Light = LightType.Circle300;
		}
	}
}