/***************************************************************************
 *                               CREDITS
 *                         -------------------
 *                         : (C) 2004-2009 Luke Tomasello (AKA Adam Ant)
 *                         :   and the Angel Island Software Team
 *                         :   luke@tomasello.com
 *                         :   Official Documentation:
 *                         :   www.game-master.net, wiki.game-master.net
 *                         :   Official Source Code (SVN Repository):
 *                         :   http://game-master.net:8050/svn/angelisland
 *                         : 
 *                         : (C) May 1, 2002 The RunUO Software Team
 *                         :   info@runuo.com
 *
 *   Give credit where credit is due!
 *   Even though this is 'free software', you are encouraged to give
 *    credit to the individuals that spent many hundreds of hours
 *    developing this software.
 *   Many of the ideas you will find in this Angel Island version of 
 *   Ultima Online are unique and one-of-a-kind in the gaming industry! 
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

/* Scripts\Items\Misc\Pill.cs
 * ChangeLog
 *  5/21/11, Adam
 *		- first time checkin
 */

using System;
using Server;

namespace Server.Items
{
	public class Pill : Item
	{
		private string[] m_commands = new string[10];

		[Constructable]
		public Pill()
			: base(0xF8B)
		{
			Hue = 55;
			Stackable = true;
			Weight = 1.0;
			LootType = LootType.Regular;
			Name = "an unusual pill";
		}

		public Pill(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command1 { get { return m_commands[0]; } set { m_commands[0] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command2 { get { return m_commands[1]; } set { m_commands[1] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command3 { get { return m_commands[2]; } set { m_commands[2] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command4 { get { return m_commands[3]; } set { m_commands[3] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command5 { get { return m_commands[4]; } set { m_commands[4] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command6 { get { return m_commands[5]; } set { m_commands[5] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command7 { get { return m_commands[6]; } set { m_commands[6] = value; Validate(); } }
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Owner)]
		public string Command8 { get { return m_commands[7]; } set { m_commands[7] = value; Validate(); } }

		private void Validate()
		{
			string[] pack = new string[m_commands.Length];
			Array.Copy(m_commands, pack, pack.Length);
			for (int jx = 0; jx < m_commands.Length; jx++ )
				m_commands[jx] = null;
			int kj = 0;
			for (int ix = 0; ix < pack.Length; ix++)
			{
				if (pack[ix] != null)
					m_commands[kj++] = pack[ix];
			}
			InvalidateProperties();
			return;
		}

		private void Execute(Mobile from, string command)
		{
			if (Core.Debug || Core.UOTC || from.AccessLevel >= AccessLevel.Administrator)
			{	// we need a dummy mobile with elevated access to execute the commands.
				Mobile m = new Mobile();
				m.AccessLevel = AccessLevel.Administrator;

				from.SendMessage(String.Format("Executing command '{0}'.", command.Substring(1)));

				if (CommandSystem.Handle(m, from, command) == false)
					from.SendMessage(String.Format("The command '{0}' failed to execute.", command));
				else
					from.SendMessage(String.Format("'{0}' - ok.", command.Substring(1)));

				m.Delete();
			}
			return;
		}

		public override void OnDoubleClick(Mobile from)
		{
			base.OnDoubleClick(from);

			if (IsChildOf(from.Backpack))
			{
				from.LocalOverheadMessage(Server.Network.MessageType.Emote, Utility.RandomOrangeHue(), true, "*crunch*");
				this.Delete();

				for (int ix = 0; ix < m_commands.Length; ix++)
					if (m_commands[ix] != null)
						Execute(from, m_commands[ix]);
			}
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version

			// unsmart 
			writer.Write(m_commands.Length);
			for (int ix = 0; ix < m_commands.Length; ix++)
				writer.Write(m_commands[ix]);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			int count = reader.ReadInt();
			for (int ix = 0; ix < count; ix++)
				m_commands[ix] = reader.ReadString();
		}
	}
}
