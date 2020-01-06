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

/* Scripts/Mobiles/Healers/PricedHealer.cs 
 * Changelog
 *	06/28/06, Adam
 *		Logic cleanup
 */

using System;
using Server;
using Server.Gumps;

namespace Server.Mobiles
{
	public class PricedHealer : BaseHealer
	{
		private int m_Price;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Price
		{
			get { return m_Price; }
			set { m_Price = value; }
		}

		[Constructable]
		public PricedHealer()
			: this(5000)
		{
		}

		[Constructable]
		public PricedHealer(int price)
		{
			m_Price = price;

			NameHue = CalcInvulNameHue();
		}


		public override void InitSBInfo()
		{
		}

		public override void OfferResurrection(Mobile m)
		{
			Direction = GetDirectionTo(m);

			m.PlaySound(0x214);
			m.FixedEffect(0x376A, 10, 16);

			m.CloseGump(typeof(ResurrectGump));
			m.SendGump(new ResurrectGump(m, this, m_Price));
		}

		public override bool CheckResurrect(Mobile m)
		{
			return true;
		}

		public PricedHealer(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Price);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Price = reader.ReadInt();
						break;
					}
			}

			NameHue = CalcInvulNameHue();
		}
	}
}