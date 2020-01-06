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

/* Items/Traps/FireColumnTrap.cs
 * CHANGELOG:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	public class FireColumnTrap : BaseTrap
	{
		[Constructable]
		public FireColumnTrap()
			: base(0x1B71)
		{
		}

		public override bool PassivelyTriggered { get { return true; } }
		public override TimeSpan PassiveTriggerDelay { get { return TimeSpan.FromSeconds(2.0); } }
		public override int PassiveTriggerRange { get { return 3; } }
		public override TimeSpan ResetDelay { get { return TimeSpan.FromSeconds(0.5); } }

		public override void OnTrigger(Mobile from)
		{
			Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
			Effects.PlaySound(Location, Map, 0x225);

			if (from.Alive && CheckRange(from.Location, 0))
				Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.5), from, from, Utility.RandomMinMax(10, 40), 0, 100, 0, 0, 0);
		}

		public FireColumnTrap(Serial serial)
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
	}
}