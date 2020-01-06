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

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class MagicWand : BaseBashing
	{
		//		public override int AosStrengthReq{ get{ return 5; } }
		//		public override int AosMinDamage{ get{ return 9; } }
		//		public override int AosMaxDamage{ get{ return 11; } }
		//		public override int AosSpeed{ get{ return 40; } }
		//
		//		public override int OldMinDamage{ get{ return 2; } }
		//		public override int OldMaxDamage{ get{ return 6; } }
		public override int OldStrengthReq { get { return 0; } }
		public override int OldSpeed { get { return 35; } }

		public override int OldDieRolls { get { return 3; } }
		public override int OldDieMax { get { return 3; } }
		public override int OldAddConstant { get { return 0; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		[Constructable]
		public MagicWand()
			: base(0xDF2)
		{
			Weight = 1.0;
		}

		public MagicWand(Serial serial)
			: base(serial)
		{
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
	}
}