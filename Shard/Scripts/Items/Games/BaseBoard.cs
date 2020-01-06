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

/*	/Scripts/Items/Games/BaseBoard.cs
 *	CHANGELOG:
 *	07/23/08, weaver
 *		Automated IPooledEnumerable optimizations. 1 loops updated.
 *	2/17/05, mith
 *		Changed inheritance from Container to BaseContainer to fix ownership bugs in 1.0.0
 *	11/2/04, Adam
 *		Revert the bounce back fix below as we've fixed the bounce-back exploit with vendors directly
 *	9/4/04, mith
 *		OnDragDrop(): Copied Else block from Spellbook, to prevent people dropping things on book to have it bounce back to original location.
 *		OnDragDropInto(): Same as OnDragDrop()
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using Server.ContextMenus;

namespace Server.Items
{
	public abstract class BaseBoard : BaseContainer
	{
		public override int DefaultDropSound { get { return -1; } }

		public BaseBoard(int itemID)
			: base(itemID)
		{
			CreatePieces();

			Weight = 5.0;
		}

		public abstract void CreatePieces();

		public void Reset()
		{
			for (int i = Items.Count - 1; i >= 0; --i)
			{
				if (i < Items.Count)
					((Item)Items[i]).Delete();
			}

			CreatePieces();
		}

		public void CreatePiece(BasePiece piece, int x, int y)
		{
			AddItem(piece);
			piece.Location = new Point3D(x, y, 0);
		}

		public override bool DisplaysContent { get { return false; } } // Do not display (x items, y stones)

		public BaseBoard(Serial serial)
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

			if (Weight == 1.0)
				Weight = 5.0;
		}

		public override TimeSpan DecayTime { get { return TimeSpan.FromDays(1.0); } }

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (!(dropped is BasePiece))
			{
				// Adam: anything other than a scroll will get dropped into your backpack
				// (so your best sword doesn't get dropped on the ground.)
				from.AddToBackpack(dropped);
				//	For richness, we add the drop sound of the item dropped.
				from.PlaySound(dropped.GetDropSound());
				return true;
			}

			BasePiece piece = dropped as BasePiece;

			return (piece != null && piece.Board == this && base.OnDragDrop(from, dropped));
		}

		public override bool OnDragDropInto(Mobile from, Item dropped, Point3D point)
		{
			BasePiece piece = dropped as BasePiece;

			if (piece != null && piece.Board == this && base.OnDragDropInto(from, dropped, point))
			{
				Packet p = new PlaySound(0x127, GetWorldLocation());

				p.Acquire();

				if (RootParent == from)
				{
					from.Send(p);
				}
				else
				{
					IPooledEnumerable eable = this.GetClientsInRange(2);
					foreach (NetState state in eable)
						state.Send(p);
					eable.Free();
				}

				p.Release();

				return true;
			}
			/*
			else
			{
				// Adam: anything other than a scroll will get dropped into your backpack
				// (so your best sword doesn't get dropped on the ground.)
				from.AddToBackpack( dropped );
				//	For richness, we add the drop sound of the item dropped.
				from.PlaySound( dropped.GetDropSound() );
				return true;
			}
			*/

			return false;
		}

		public override void GetContextMenuEntries(Mobile from, ArrayList list)
		{
			base.GetContextMenuEntries(from, list);

			if (ValidateDefault(from, this))
				list.Add(new DefaultEntry(from, this));
		}

		public static bool ValidateDefault(Mobile from, BaseBoard board)
		{
			if (from.AccessLevel >= AccessLevel.GameMaster)
				return true;

			if (!from.Alive)
				return false;

			if (board.IsChildOf(from.Backpack))
				return true;

			object root = board.RootParent;

			if (root is Mobile && root != from)
				return false;

			if (board.Deleted || board.Map != from.Map || !from.InRange(board.GetWorldLocation(), 1))
				return false;

			BaseHouse house = BaseHouse.FindHouseAt(board);

			return (house != null && house.IsOwner(from));
		}

		public class DefaultEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseBoard m_Board;

			public DefaultEntry(Mobile from, BaseBoard board)
				: base(6162, from.AccessLevel >= AccessLevel.GameMaster ? -1 : 1)
			{
				m_From = from;
				m_Board = board;
			}

			public override void OnClick()
			{
				if (BaseBoard.ValidateDefault(m_From, m_Board))
					m_Board.Reset();
			}
		}
	}
}
