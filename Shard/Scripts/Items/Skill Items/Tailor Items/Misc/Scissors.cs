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
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public interface IScissorable
	{
		bool Scissor(Mobile from, Scissors scissors);
	}

	[FlipableAttribute(0xf9f, 0xf9e)]
	public class Scissors : Item
	{
		[Constructable]
		public Scissors()
			: base(0xF9F)
		{
			Weight = 1.0;
		}

		public Scissors(Serial serial)
			: base(serial)
		{
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

		public override void OnDoubleClick(Mobile from)
		{
			from.SendLocalizedMessage(502434); // What should I use these scissors on?

			from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private Scissors m_Item;

			public InternalTarget(Scissors item)
				: base(1, false, TargetFlags.None)
			{
				m_Item = item;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Item.Deleted) return;

				/*if ( targeted is Item && !((Item)targeted).IsStandardLoot() )
				{
					from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
				}
				else */
				if (targeted is IScissorable)
				{
					IScissorable obj = (IScissorable)targeted;

					if (obj.Scissor(from, m_Item))
						from.PlaySound(0x248);
				}
				else
				{
					from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
				}
			}
		}
	}
}