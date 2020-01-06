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

/* Scripts\Engines\ChampionSpawn\Champs\Seasonal\Fall\Items\MagicBox.cs
 * ChangeLog:
 *  10/1/07, Adam
 *      First time checkin
 */

using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Commands;

namespace Server.Items
{

	[DynamicFliping]
	[TinkerTrapable]
	[Flipable(0x9A8, 0xE80)]
	public class MagicBox : LockableContainer
	{
		public override int DefaultGumpID { get { return 0x4B; } }
		public override int DefaultDropSound { get { return 0x42; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public override int MaxWeight { get { return m_MaxWeight; } }

		private int m_MaxWeight;

		[Constructable]
		public MagicBox()
			: base(0x9A8)
		{
			Name = "magic box";
			m_MaxWeight = Utility.RandomMinMax(900, 1024);
			MaxItems = Utility.RandomMinMax(900, 1024);
			Weight = 25.0;
		}

		public MagicBox(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_MaxWeight);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_MaxWeight = reader.ReadInt();
		}
	}
}