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

/* Scripts/Mobiles/Monsters/AOS/Revenant.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 9 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class Revenant : BaseCreature
	{
		private Mobile m_Target;
		private DateTime m_ExpireTime;

		public override void DisplayPaperdollTo(Mobile to)
		{
			// Do nothing
		}

		public override Mobile ConstantFocus { get { return m_Target; } }
		public override bool NoHouseRestrictions { get { return true; } }

		public override double DispelDifficulty { get { return 80.0; } }
		public override double DispelFocus { get { return 20.0; } }

		public Revenant(Mobile caster, Mobile target, TimeSpan duration)
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a revenant";
			Body = 400;
			Hue = 1;
			BardImmune = true;
			// TODO: Sound values?

			double scalar = caster.Skills[SkillName.SpiritSpeak].Value * 0.01;

			m_Target = target;
			m_ExpireTime = DateTime.Now + duration;

			SetStr(200);
			SetDex(150);
			SetInt(150);

			SetDamage(16, 17);

			SetSkill(SkillName.MagicResist, 100.0 * scalar); // magic resist is absolute value of spiritspeak
			SetSkill(SkillName.Tactics, 100.0); // always 100
			SetSkill(SkillName.Swords, 100.0 * scalar); // not displayed in animal lore but tests clearly show this is influenced
			SetSkill(SkillName.DetectHidden, 75.0 * scalar);

			scalar /= 1.2;


			Fame = 0;
			Karma = 0;

			ControlSlots = 3;

			VirtualArmor = 32;

			Item shroud = new DeathShroud();

			shroud.Hue = 0x455;

			shroud.Movable = false;

			AddItem(shroud);

			Halberd weapon = new Halberd();

			weapon.Hue = 1;
			weapon.Movable = false;

			AddItem(weapon);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void OnThink()
		{
			if (!m_Target.Alive || DateTime.Now > m_ExpireTime)
			{
				Kill();
				return;
			}
			else if (Map != m_Target.Map || !InRange(m_Target, 15))
			{
				Map fromMap = Map;
				Point3D from = Location;

				Map toMap = m_Target.Map;
				Point3D to = m_Target.Location;

				if (toMap != null)
				{
					for (int i = 0; i < 5; ++i)
					{
						Point3D loc = new Point3D(to.X - 4 + Utility.Random(9), to.Y - 4 + Utility.Random(9), to.Z);

						if (toMap.CanSpawnMobile(loc))
						{
							to = loc;
							break;
						}
						else
						{
							loc.Z = toMap.GetAverageZ(loc.X, loc.Y);

							if (toMap.CanSpawnMobile(loc))
							{
								to = loc;
								break;
							}
						}
					}
				}

				Map = toMap;
				Location = to;

				ProcessDelta();

				Effects.SendLocationParticles(EffectItem.Create(from, fromMap, EffectItem.DefaultDuration), 0x3728, 1, 13, 37, 7, 5023, 0);
				FixedParticles(0x3728, 1, 13, 5023, 37, 7, EffectLayer.Waist);

				PlaySound(0x37D);
			}

			if (m_Target.Hidden && InRange(m_Target, 3) && DateTime.Now >= this.NextSkillTime && UseSkill(SkillName.DetectHidden))
			{
				Target targ = this.Target;

				if (targ != null)
					targ.Invoke(this, this);
			}

			Combatant = m_Target;
			FocusMob = m_Target;

			if (AIObject != null)
				AIObject.Action = ActionType.Combat;

			base.OnThink();
		}

		public override bool OnBeforeDeath()
		{
			Effects.PlaySound(Location, Map, 0x10B);
			Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(10.0)), 0x37CC, 1, 50, 2101, 7, 9909, 0);

			Delete();
			return false;
		}

		public Revenant(Serial serial)
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

			Delete();
		}
	}
}
