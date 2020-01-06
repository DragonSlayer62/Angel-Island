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

/* ./Scripts/Items/Shields/BronzeShield.cs
 *	ChangeLog :
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
*/

using System;
using Server;

namespace Server.Items
{
	public class BronzeShield : BaseShield
	{

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 30; } }

		public override int AosStrReq { get { return 35; } }

		public override int ArmorBase { get { return 10; } }

		[Constructable]
		public BronzeShield()
			: base(0x1B72)
		{
			Weight = 6.0;
		}

		public BronzeShield(Serial serial)
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
	}
}

