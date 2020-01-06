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

/* Scripts\Spells\UnsummonTimer.cs
 * ChangeLog
 *	7/16/10, adam
 *		call the new virtual function OnBeforeDispel() to allow the summon to cleanup
 */

using System;
using Server.Mobiles;

namespace Server.Spells
{
    class UnsummonTimer : Timer
    {
        private BaseCreature m_Creature;
        private Mobile m_Caster;

        public UnsummonTimer(Mobile caster, BaseCreature creature, TimeSpan delay)
            : base(delay)
        {
            m_Caster = caster;
            m_Creature = creature;
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            if (!m_Creature.Deleted)
            {
                m_Creature.OnBeforeDispel(m_Caster);
                m_Creature.Delete();
            }
        }
    }
}