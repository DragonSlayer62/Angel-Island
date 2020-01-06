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

#if false
using System;
using Server.Network;

namespace Server.Items
{
	public class GMStone : Item
	{
		[Constructable]
		public GMStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 0x489;
			Name = "a GM stone";
		}

		public GMStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel < AccessLevel.GameMaster )
			{
				from.AccessLevel = AccessLevel.GameMaster;

				from.SendAsciiMessage( 0x482, "The command prefix is \"{0}\"", Server.Commands.CommandPrefix );
				Server.Commands.CommandHandlers.Help_OnCommand( new CommandEventArgs( from, "help", "", new string[0] ) );
			}
			else
			{
				from.SendMessage( "The stone has no effect." );
			}
		}
	}
}
#endif