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

/* Scripts/Items/Misc/SpecialWalls.cs
 * CHANGELOG:
 *  02/28/07, Kit
 *      Added NoShoot flags to walls around fire temple.
 *  12/21/06, Kit
 *      Added NoShoot flags to tiles used in pirate ship
 *	11/08/06 - Pix
 *		Fixed dungeon arch LOS blocking.
 *	5/30/06 - Pix
 *		Overrode the wall tiles for the Hythloth L1->L2 area to be NoShoot.
 *	5/20/06 - Pix
 *		Overrode the floor/ceiling tiles for buildings in Wind to be NoShoot.
 *	9/12/05 - Pix
 *		Ort fort flap final fix (goes along with fix for exploit in Server/Map.cs)
 *	8/4/06 - Pix
 *		Orc fort walls changed for the last time.  No more paying attention to
 *		ever-changing orc requests.
 *	7/9/05 - Pix
 *		Orc/brigand down flaps are now impassible and block LOS.
 *	06/04/05 - Pix
 *		Orc fort walls - make sure you can't shoot/target through them
 *	01/05/05 - Pix
 *		Did it the right way now... basically just telling the server that those itemids
 *		don't block anything.
 *	12/27/04 - Pixie
 *		Initial Version - wanted brigand fort door flaps to be non-blocking, etc.
 */

using System;
using Server;

namespace Server.Items
{
	public class SpecialDoorFlags
	{
		public static void Initialize()
		{
			//firetemple outside walls
			TileData.ItemTable[0x3CF].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x3D0].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x3D1].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x3D2].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x3E0].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x3E1].Flags |= TileFlag.NoShoot;

			//pirate boat deck tiles(bridge tiles) and windows
			TileData.ItemTable[0xE].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0xF].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7C9].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CA].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CB].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CC].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CD].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CE].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7CF].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x7D0].Flags |= TileFlag.NoShoot;

			//dungeon arches - the N-S right and the E-W left need to not block LOS
			//TileData.ItemTable[0x24A].Flags = TileFlag.None; //N-S left
			TileData.ItemTable[0x246].Flags = TileFlag.None; //N-S right
			TileData.ItemTable[0x245].Flags = TileFlag.None; //E-W left
			//TileData.ItemTable[0x249].Flags = TileFlag.None; //E-W right

			//Orc Fort flap - left
			TileData.ItemTable[0x22C].Flags = TileFlag.Impassable | TileFlag.Door | TileFlag.NoShoot;
			//Orc Fort flap - right
			TileData.ItemTable[0x22D].Flags = TileFlag.Door | TileFlag.NoShoot;

			TileData.ItemTable[0x22E].Flags = TileFlag.None; //Brigand Fort flap - left
			TileData.ItemTable[0x22E].Flags |= TileFlag.Impassable;
			TileData.ItemTable[0x22E].Flags |= TileFlag.Door;
			TileData.ItemTable[0x22E].Flags |= TileFlag.NoShoot;

			TileData.ItemTable[0x22F].Flags = TileFlag.None; //Brigand Fort flap - right


			//NOTE NOTE NOTE by Pixie!
			//This was left in here as an example of what to do...
			//DO NOT CHANGE THIS BACK - the orcs have flip-flopped on this issue
			// and we should be done answering their requests.
			//Orc fort walls - make sure you can't shoot/target through them
			//TileData.ItemTable[0x22A].Flags |= TileFlag.NoShoot;
			//TileData.ItemTable[0x222].Flags |= TileFlag.NoShoot;
			//TileData.ItemTable[0x22B].Flags |= TileFlag.NoShoot;
			//TileData.ItemTable[0x422].Flags |= TileFlag.NoShoot;
			//TileData.ItemTable[0x423].Flags |= TileFlag.NoShoot;
			//TileData.ItemTable[0x424].Flags |= TileFlag.NoShoot;

			//Wind floor tiles:
			TileData.ItemTable[0x63B].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x63C].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x63D].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x63E].Flags |= TileFlag.NoShoot;

			//Hythloth level 1 -> level 2 walls:
			TileData.ItemTable[0x2FA].Flags |= TileFlag.NoShoot;
			TileData.ItemTable[0x2FB].Flags |= TileFlag.NoShoot;


		}
	}


	//NOTE!  The two classes below are useless... they're just here for historical sake.
	// I was doing the whole thing in a more-complicated-than-necessary way :-)  -- Pix.
	public class BFDoorLeft : Item
	{
		[Constructable]
		public BFDoorLeft()
			: base(0x22E)
		{
		}
		public BFDoorLeft(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}


	public class BFDoorRight : Item
	{
		[Constructable]
		public BFDoorRight()
			: base(0x22F)
		{
		}

		public BFDoorRight(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

}
