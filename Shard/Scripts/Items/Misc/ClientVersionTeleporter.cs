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

/* Items/Misc/ClientVersionTeleporter.cs
 * CHANGELOG:
 *	3/18/2008 - Pix
 *		Initial Version.
 */

using System;
using Server;
using Server.Network;
using System.Collections;

namespace Server.Items
{
	class ClientVersionTeleporter : Teleporter
	{
		private ClientVersion m_MinVersion = new ClientVersion(0, 0, 0, 0);
		private ClientVersion m_MaxVersion = new ClientVersion(99, 0, 0, 0);

		[Constructable]
		public ClientVersionTeleporter()
			: base()
		{
			Active = false; //initially set to false :O
			Name = "client version teleporter";
		}

		public ClientVersionTeleporter(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string MinVersion
		{
			get { return m_MinVersion.ToString(); }
			set { m_MinVersion = new ClientVersion(value); }
		}
		[CommandProperty(AccessLevel.GameMaster)]
		public string MaxVersion
		{
			get { return m_MaxVersion.ToString(); }
			set { m_MaxVersion = new ClientVersion(value); }
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m.NetState != null &&
				m.NetState.Version >= m_MinVersion &&
				m.NetState.Version <= m_MaxVersion)
			{
				return base.OnMoveOver(m);
			}

			return true;
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);
			LabelTo(from, "Min: " + this.m_MinVersion.ToString() + " - Max: " + this.m_MaxVersion.ToString());
		}


		#region Serialize/Deserialize
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			if (m_MinVersion == null)
			{
				writer.Write((int)0);//major
				writer.Write((int)0);//minor
				writer.Write((int)0);//revision
				writer.Write((int)0);//patch
			}
			else
			{
				writer.Write(m_MinVersion.Major);//major
				writer.Write(m_MinVersion.Minor);//minor
				writer.Write(m_MinVersion.Revision);//revision
				writer.Write(m_MinVersion.Patch);//patch
			}

			if (m_MaxVersion == null)
			{
				writer.Write((int)0);//major
				writer.Write((int)0);//minor
				writer.Write((int)0);//revision
				writer.Write((int)0);//patch
			}
			else
			{
				writer.Write(m_MaxVersion.Major);//major
				writer.Write(m_MaxVersion.Minor);//minor
				writer.Write(m_MaxVersion.Revision);//revision
				writer.Write(m_MaxVersion.Patch);//patch
			}

		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					int major = reader.ReadInt();
					int minor = reader.ReadInt();
					int revision = reader.ReadInt();
					int patch = reader.ReadInt();
					m_MinVersion = new ClientVersion(major, minor, revision, patch);

					major = reader.ReadInt();
					minor = reader.ReadInt();
					revision = reader.ReadInt();
					patch = reader.ReadInt();
					m_MaxVersion = new ClientVersion(major, minor, revision, patch);
					break;
			}
		}
		#endregion

	}
}
