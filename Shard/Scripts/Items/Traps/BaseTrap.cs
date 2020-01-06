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

/* Items/Traps/BaseTrap.cs
 * CHANGELOG:
 *	2/21/06, Adam
 *		Make a check to see that something is a BaseCreature before
 *		casting it to a BaseCreature. This is *rarely* needed, but there is 
 *		at least one 'mobile' that runs around ans is not a BaseCreature.
 *		See: Wanderer.cs
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *  01/05/05, Darva
 *  		Caused traps to only attack players, tamed mobs, and controlled mobs.
 */

using System;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseTrap : Item
	{
		public virtual bool PassivelyTriggered { get { return false; } }
		public virtual TimeSpan PassiveTriggerDelay { get { return TimeSpan.Zero; } }
		public virtual int PassiveTriggerRange { get { return -1; } }
		public virtual TimeSpan ResetDelay { get { return TimeSpan.Zero; } }

		private DateTime m_NextPassiveTrigger, m_NextActiveTrigger;

		public virtual void OnTrigger(Mobile from)
		{
		}

		public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

		public virtual int GetEffectHue()
		{
			int hue = this.Hue & 0x3FFF;

			if (hue < 2)
				return 0;

			return hue - 1;
		}

		public bool CheckRange(Point3D loc, Point3D oldLoc, int range)
		{
			return CheckRange(loc, range) && !CheckRange(oldLoc, range);
		}

		public bool CheckRange(Point3D loc, int range)
		{
			return ((this.Z + 8) >= loc.Z && (loc.Z + 16) > this.Z)
				&& Utility.InRange(GetWorldLocation(), loc, range);
		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			base.OnMovement(m, oldLocation);

			if (m.Location == oldLocation)
				return;

			if (CheckRange(m.Location, oldLocation, 0) && DateTime.Now >= m_NextActiveTrigger)
			{
				m_NextActiveTrigger = m_NextPassiveTrigger = DateTime.Now + ResetDelay;
				if (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Controlled == true))
					OnTrigger(m);
			}
			else if (PassivelyTriggered && CheckRange(m.Location, oldLocation, PassiveTriggerRange) && DateTime.Now >= m_NextPassiveTrigger)
			{
				m_NextPassiveTrigger = DateTime.Now + PassiveTriggerDelay;
				if (m is PlayerMobile || (m is BaseCreature && ((BaseCreature)m).Controlled == true))
					OnTrigger(m);
			}
		}

		public BaseTrap(int itemID)
			: base(itemID)
		{
			Movable = false;
		}

		public BaseTrap(Serial serial)
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