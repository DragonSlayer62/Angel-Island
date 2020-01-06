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

/* Items/Shields/ChaosShield.cs
 * CHANGELOG:
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Guilds;

namespace Server.Items
{
	public class ChaosShield : BaseShield
	{

		public override int InitMinHits { get { return 100; } }
		public override int InitMaxHits { get { return 125; } }

		public override int AosStrReq { get { return 95; } }

		public override int ArmorBase { get { return 32; } }

		[Constructable]
		public ChaosShield()
			: base(0x1BC3)
		{
			if (!Core.AOS)
				LootType = LootType.Newbied;

			Weight = 5.0;
		}

		public ChaosShield(Serial serial)
			: base(serial)
		{
		}

// old name removed, see base class

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);//version
		}

		public override bool OnEquip(Mobile from)
		{
			return Validate(from) && base.OnEquip(from);
		}

		public override void OnSingleClick(Mobile from)
		{
			if (Validate(Parent as Mobile))
				base.OnSingleClick(from);
		}

		public bool Validate(Mobile m)
		{
			if (m == null || !m.Player || m.AccessLevel != AccessLevel.Player || Core.AOS)
				return true;

			Guild g = m.Guild as Guild;

			if (g == null || g.Type != GuildType.Chaos)
			{
				m.FixedEffect(0x3728, 10, 13);
				Delete();

				return false;
			}

			return true;
		}
	}
}
