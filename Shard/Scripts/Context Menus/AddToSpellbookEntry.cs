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

/* Scripts/Context Menus/AddToSpellbookEntry.cs
 * CHANGELOG
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using Server.Items;
using Server.Targeting;

namespace Server.ContextMenus
{
	public class AddToSpellbookEntry : ContextMenuEntry
	{
		public AddToSpellbookEntry()
			: base(6144, 3)
		{
		}

		public override void OnClick()
		{
			if (Owner.From.CheckAlive() && Owner.Target is SpellScroll)
				Owner.From.Target = new InternalTarget((SpellScroll)Owner.Target);
		}

		private class InternalTarget : Target
		{
			private SpellScroll m_Scroll;

			public InternalTarget(SpellScroll scroll)
				: base(3, false, TargetFlags.None)
			{
				m_Scroll = scroll;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Spellbook)
				{
					if (from.CheckAlive() && !m_Scroll.Deleted && m_Scroll.Movable && m_Scroll.Amount >= 1)
					{
						Spellbook book = (Spellbook)targeted;

						SpellbookType type = Spellbook.GetTypeForSpell(m_Scroll.SpellID);

						if (type != book.SpellbookType)
						{
						}
						else if (book.HasSpell(m_Scroll.SpellID))
						{
							from.SendLocalizedMessage(500179); // That spell is already present in that spellbook.
						}
						else
						{
							int val = m_Scroll.SpellID - book.BookOffset;

							if (val >= 0 && val < book.BookCount)
							{
								book.Content |= (ulong)1 << val;

								m_Scroll.Consume();

								from.Send(new Network.PlaySound(0x249, book.GetWorldLocation()));
							}
						}
					}
				}
			}
		}
	}
}