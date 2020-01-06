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

/* Scripts/Items/Construction/Doors/SecretDoors.cs
 * CHANGELOG
 *	
 *	9/01/06 Taran Kain
 *		Changed constructors to fit new BaseDoor constructor
 */

using System;

namespace Server.Items
{
	public class SecretStoneDoor1 : BaseDoor
	{
		[Constructable]
		public SecretStoneDoor1(DoorFacing facing)
			: base(0xE8, 0xE9, 0xED, 0xF4, facing)
		{
		}

		public SecretStoneDoor1(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SecretDungeonDoor : BaseDoor
	{
		[Constructable]
		public SecretDungeonDoor(DoorFacing facing)
			: base(0x314, 0x315, 0xED, 0xF4, facing)
		{
		}

		public SecretDungeonDoor(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SecretStoneDoor2 : BaseDoor
	{
		[Constructable]
		public SecretStoneDoor2(DoorFacing facing)
			: base(0x324, 0x325, 0xED, 0xF4, facing)
		{
		}

		public SecretStoneDoor2(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SecretWoodenDoor : BaseDoor
	{
		[Constructable]
		public SecretWoodenDoor(DoorFacing facing)
			: base(0x334, 0x335, 0xED, 0xF4, facing)
		{
		}

		public SecretWoodenDoor(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SecretLightWoodDoor : BaseDoor
	{
		[Constructable]
		public SecretLightWoodDoor(DoorFacing facing)
			: base(0x344, 0x345, 0xED, 0xF4, facing)
		{
		}

		public SecretLightWoodDoor(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SecretStoneDoor3 : BaseDoor
	{
		[Constructable]
		public SecretStoneDoor3(DoorFacing facing)
			: base(0x354, 0x355, 0xED, 0xF4, facing)
		{
		}

		public SecretStoneDoor3(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer) // Default Serialize method
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader) // Default Deserialize method
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}