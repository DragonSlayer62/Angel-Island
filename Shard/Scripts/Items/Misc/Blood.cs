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

/* CHANGELOG
 * 08/27/05, Taran Kain
 *		Added new constructor to specify the lifespan of the blood object.
 */

using System;
using Server;

namespace Server.Items
{
	public class Blood : Item
	{
		[Constructable]
		public Blood()
			: this(0x1645)
		{
		}

		[Constructable]
		public Blood(int itemID)
			: this(itemID, 3.0 + (Utility.RandomDouble() * 3.0))
		{
		}

		[Constructable]
		public Blood(int itemID, double lifespan)
			: base(itemID)
		{
			Movable = false;

			new InternalTimer(this, TimeSpan.FromSeconds(lifespan)).Start();
		}

		public Blood(Serial serial)
			: base(serial)
		{
			new InternalTimer(this, TimeSpan.FromSeconds(3.0 + (Utility.RandomDouble() * 3.0))).Start();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		private class InternalTimer : Timer
		{
			private Item m_Blood;

			public InternalTimer(Item blood, TimeSpan lifespan)
				: base(lifespan)
			{
				Priority = TimerPriority.FiftyMS;

				m_Blood = blood;
			}

			protected override void OnTick()
			{
				m_Blood.Delete();
			}
		}
	}
}