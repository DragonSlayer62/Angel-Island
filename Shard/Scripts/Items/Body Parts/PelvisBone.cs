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

/* Scripts\Items\Body Parts\PelvisBone.cs
 * ChangeLog
 *	12/28/10, adam
 *		first time checkin - updated from:
 *		http://www.google.com/codesearch/p?hl=en#biPgqLK3B_w/trunk/Scripts/Items/Body%20Parts/PelvisBone.cs&q=BoneShards&exact_package=http://runuomondains.googlecode.com/svn&d=4
 */

using System;
using Server;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0x1B1B, 0x1B1C)]
	public class PelvisBone : Item, IScissorable
	{
		[Constructable]
		public PelvisBone()
			: base(0x1B1B + Utility.Random(2))
		{
			Stackable = false;
			Weight = 1.0;
		}

		public PelvisBone(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this))
				return false;

			base.ScissorHelper(from, new Bone(), 1);
			from.PlaySound(0x21B);

			return false;
		}
	}
}