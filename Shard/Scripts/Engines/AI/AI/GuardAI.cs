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

/* Scripts\Engines\AI\AI\GuardAI.cs
 * ChangeLog
 *  6/30/10, Adam
 *		Initial creation
 *		Guards now do some basic heals and will cast the new NPC spell "InvisibleShield", "Kal Sanct Grav" on suicide bombers. (Kal (summon), Sanct (Protection), Grav (field))
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;

namespace Server.Mobiles
{
	public class GuardAI : HybridAI
	{
		public GuardAI(BaseCreature guard)
			: base(guard)
		{

		}

		public override Spell ChooseSpell(Mobile target)
		{
			if (m_Mobile.InRange(target, 5) && target.SuicideBomber && !target.InvisibleShield)
				return new InvisibleShieldSpell(m_Mobile, null);
			else
				return base.ChooseSpell(target);
		}

		public override bool DoActionCombat()
		{
			bool dac = base.DoActionCombat();

			if (m_Mobile.Spell == null && DateTime.Now >= m_Mobile.NextSpellTime)
			{
				if (m_Mobile.Spell == null || !(m_Mobile.Spell as Spell).Cast())
					EquipWeapon();
			}
			else if (m_Mobile.Spell is Spell && ((Spell)m_Mobile.Spell).State == SpellState.Sequencing)
				EquipWeapon();

			return dac;
		}

		public override bool DoActionWander()
		{
			bool daw = base.DoActionWander();

			if (m_Mobile.Spell == null && DateTime.Now >= m_Mobile.NextSpellTime)
			{
				if (m_Mobile.Spell == null || !(m_Mobile.Spell as Spell).Cast())
					EquipWeapon();
			}
			else if (m_Mobile.Spell is Spell && ((Spell)m_Mobile.Spell).State == SpellState.Sequencing)
				EquipWeapon();

			return daw;
		}

		public override bool PreferMagic()
		{
			DateTime m_FightMode = DateTime.Now;

			if (IsAllowed(FightStyle.Melee) && (double)m_Mobile.Mana < m_Mobile.ManaMax * .30)
				return false;	// low on mana use melee
			else if (m_Mobile.Combatant is BaseCreature)
				return false;	// destroy pets with our weapon
			if (m_Mobile.InRange(m_Mobile.Combatant, 5) && m_Mobile.Combatant.SuicideBomber && !m_Mobile.Combatant.InvisibleShield)
				return true;	// we need to cast InvisibleShield

			// - finally -
			else if ((m_FightMode.Second >= 0 && m_FightMode.Second <= 15) || (m_FightMode.Second >= 30 && m_FightMode.Second <= 45))
				return false;	// prefer weapon for the first 15 seconds and the 3rd 15 seconds out of each minute
			else
				return true;	// use magic
		}

		#region Serialize
		private SaveFlags m_flags;

		[Flags]
		private enum SaveFlags
		{	// 0x00 - 0x800 reserved for version
			unused = 0x1000
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			int version = 1;								// current version (up to 4095)
			m_flags = m_flags | (SaveFlags)version;			// save the version and flags
			writer.Write((int)m_flags);

			// add your version specific stuffs here.
			// Make sure to use the SaveFlags for conditional Serialization
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			m_flags = (SaveFlags)reader.ReadInt();				// grab the version an flags
			int version = (int)(m_flags & (SaveFlags)0xFFF);	// maskout the version

			// add your version specific stuffs here.
			// Make sure to use the SaveFlags for conditional Serialization
			switch (version)
			{
				default: break;
			}

		}
		#endregion Serialize
	}
}

