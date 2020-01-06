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

/* Items/SkillItems/Magical/Scrolls/SpellScroll.cs
 * CHANGELOG:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Spells;

namespace Server.Items
{
	public class SpellScroll : Item
	{
		private int m_SpellID;

		public int SpellID
		{
			get
			{
				return m_SpellID;
			}
		}

		public SpellScroll(Serial serial)
			: base(serial)
		{
		}

		[Constructable]
		public SpellScroll(int spellID, int itemID)
			: this(spellID, itemID, 1)
		{
		}

		[Constructable]
		public SpellScroll(int spellID, int itemID, int amount)
			: base(itemID)
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;

			m_SpellID = spellID;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_SpellID);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_SpellID = reader.ReadInt();

						break;
					}
			}
		}

		public override void GetContextMenuEntries(Mobile from, ArrayList list)
		{
			base.GetContextMenuEntries(from, list);

			if (from.Alive && this.Movable)
				list.Add(new ContextMenus.AddToSpellbookEntry());
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new SpellScroll(m_SpellID, ItemID, amount), amount);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!Multis.DesignContext.Check(from))
				return; // They are customizing

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			Spell spell = SpellRegistry.NewSpell(m_SpellID, from, this);

			if (spell != null)
				spell.Cast();
			else
				from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
		}
	}
}