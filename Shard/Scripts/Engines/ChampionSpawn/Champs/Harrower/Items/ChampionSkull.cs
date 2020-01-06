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
 *  
 */

/* Scripts\Engines\ChampionSpawn\Champs\Harrower\Items\ChampionSkull.cs
 * ChangeLog        
 *  03/09/07, plasma    
 *      Removed cannedevil namespace reference
 *  01/05/07, plasma!
 *     Changed CannedEvil namespace to ChampionSpawn for cleanup!
*/


using System;
using Server;
using Server.Engines.ChampionSpawn;

namespace Server.Items
{
	public class ChampionSkull : Item
	{
		private ChampionSkullType m_Type;

		[CommandProperty(AccessLevel.GameMaster)]
		public ChampionSkullType Type { get { return m_Type; } set { m_Type = value; InvalidateProperties(); } }

		public override int LabelNumber { get { return 1049479 + (int)m_Type; } }

		[Constructable]
		public ChampionSkull(ChampionSkullType type)
			: base(0x1AE1)
		{
			m_Type = type;

			// TODO: All hue values
			switch (type)
			{
				case ChampionSkullType.Power: Hue = 0x159; break;
				case ChampionSkullType.Venom: Hue = 0x172; break;
				case ChampionSkullType.Greed: Hue = 0x1EE; break;
				case ChampionSkullType.Death: Hue = 0x025; break;
				case ChampionSkullType.Pain: Hue = 0x035; break;
			}
		}

		public ChampionSkull(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Type);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Type = (ChampionSkullType)reader.ReadInt();

						break;
					}
			}
		}
	}
}