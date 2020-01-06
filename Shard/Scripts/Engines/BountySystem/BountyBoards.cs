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

/* Engines/BountySystem/BountyBoards.cs
 * CHANGELOG:
 *	1/27/05, Pix
 *		Added optional Placer name to override Placer.Name on the bounty board.
 *	5/16/04, Pixie
 *		Initial Version.
 */

using System;
using System.Collections;
using System.Text;
using Server;
using Server.Network;
using Server.BountySystem;

namespace Server.Items
{
	[Flipable(0x1E5E, 0x1E5F)]
	public class BountyBoard : BaseBountyBoard
	{
		[Constructable]
		public BountyBoard()
			: base(0x1E5E)
		{
		}

		public BountyBoard(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int LBFundAmount
		{
			get { return BountyKeeper.LBFund; }
			set
			{
				if (value >= 0)
				{
					BountyKeeper.LBFund = value;
				}
			}
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

	public abstract class BaseBountyBoard : Item
	{
		private string m_BoardName;

		[CommandProperty(AccessLevel.GameMaster)]
		public string BoardName
		{
			get { return m_BoardName; }
			set { m_BoardName = value; }
		}

		public BaseBountyBoard(int itemID)
			: base(itemID)
		{
			m_BoardName = "Bounty board";
			Movable = false;
			Hue = 0x3FF;
		}


		public override void OnSingleClick(Mobile from)
		{
			this.LabelTo(from, "Bounty Board");
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (CheckRange(from))
			{
				//Cleanup();
				//from.Send( new BountyDisplayBoard( this ) );
				//from.Send( new ContainerContent( from, this ) );

				from.SendGump(new BountyDisplayGump(from, 0));
			}
			else
			{
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			}
		}

		public virtual bool CheckRange(Mobile from)
		{
			if (from.AccessLevel >= AccessLevel.GameMaster)
				return true;

			return (from.Map == this.Map && from.InRange(GetWorldLocation(), 2));
		}


		public BaseBountyBoard(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((string)m_BoardName);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_BoardName = reader.ReadString();
						break;
					}
			}
		}
	}

}