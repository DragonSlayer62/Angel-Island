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
/* Changelog
 * 1/27/05 Darva
 *		Allow gates to be used by people other than owner.
 * 1/26/05 Darva,
 *		Prevent use of gate if in combat.
 * 1/25/05 Darva,
 *		Changes to constructor, for use with moonstone.cs
 */

using System;
using Server;
using Server.Network;
using Server.Engines.PartySystem;
using Server.Spells;

namespace Server.Items
{
	public class MoonstoneGate : Moongate
	{
		private Mobile m_Caster;
		private Point3D m_Destination;

		public MoonstoneGate(Point3D loc, Map map, Mobile caster, int hue, Point3D Destination)
			: base(loc, map)
		{
			MoveToWorld(loc, map);
			Dispellable = false;
			Hue = hue;

			m_Caster = caster;
			m_Destination = Destination;
			base.Target = m_Destination;
			new InternalTimer(this).Start();

			Effects.PlaySound(loc, map, 0x20E);
		}

		public MoonstoneGate(Serial serial)
			: base(serial)
		{
		}

		public override void UseGate(Mobile m)
		{
			if (SpellHelper.CheckCombat(m))
				m.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??

			else
				base.UseGate(m);

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

			Delete();
		}

		private class InternalTimer : Timer
		{
			private Item m_Item;

			public InternalTimer(Item item)
				: base(TimeSpan.FromSeconds(30.0))
			{
				m_Item = item;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				m_Item.Delete();
			}
		}
	}
}