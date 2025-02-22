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
using Server;

namespace Server.Items
{
	public class RedBook : BaseBook
	{
		public override double BookWeight { get { return 2.0; } }

		[Constructable]
		public RedBook()
			: base(0xFF1)
		{
		}

		[Constructable]
		public RedBook(int pageCount, bool writable)
			: base(0xFF1, pageCount, writable)
		{
		}

		[Constructable]
		public RedBook(string title, string author, int pageCount, bool writable)
			: base(0xFF1, title, author, pageCount, writable)
		{
		}

		// Intended for defined books only
		public RedBook(bool writable)
			: base(0xFF1, writable)
		{
		}

		public RedBook(Serial serial)
			: base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}
	}
}