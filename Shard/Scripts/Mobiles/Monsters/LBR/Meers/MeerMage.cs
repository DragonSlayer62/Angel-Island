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

/* Scripts/Mobiles/Monsters/LBR/Meers/MeerMage.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName("a meer's corpse")]
	public class MeerMage : BaseCreature
	{
		[Constructable]
		public MeerMage()
			: base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a meer mage";
			Body = 770;

			SetStr(171, 200);
			SetDex(126, 145);
			SetInt(276, 305);

			SetHits(103, 120);

			SetDamage(24, 26);

			SetSkill(SkillName.EvalInt, 100.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.Meditation, 85.1, 95.0);
			SetSkill(SkillName.MagicResist, 80.1, 100.0);
			SetSkill(SkillName.Tactics, 70.1, 90.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 8000;
			Karma = 8000;

			VirtualArmor = 16;

			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5));
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 3 : 0; } }

		public override bool InitialInnocent { get { return true; } }

		public override int GetHurtSound()
		{
			return 0x14D;
		}

		public override int GetDeathSound()
		{
			return 0x314;
		}

		public override int GetAttackSound()
		{
			return 0x75;
		}

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if (DateTime.Now >= m_NextAbilityTime)
			{
				Mobile combatant = this.Combatant;

				if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12) && IsEnemy(combatant, RelationshipFilter.None) && !UnderEffect(combatant))
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

					// TODO: Forest summon ability

					this.Say(true, "I call a plague of insects to sting your flesh!");

					m_Table[combatant] = Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(7.0), new TimerStateCallback(DoEffect), new object[] { combatant, 0 });
				}
			}

			base.OnThink();
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect(Mobile m)
		{
			return m_Table.Contains(m);
		}

		public static void StopEffect(Mobile m, bool message)
		{
			Timer t = (Timer)m_Table[m];

			if (t != null)
			{
				if (message)
					m.PublicOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, "* The open flame begins to scatter the swarm of insects *");

				t.Stop();
				m_Table.Remove(m);
			}
		}

		public void DoEffect(object state)
		{
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int count = (int)states[1];

			if (!m.Alive)
			{
				StopEffect(m, false);
			}
			else
			{
				Torch torch = m.FindItemOnLayer(Layer.TwoHanded) as Torch;

				if (torch != null && torch.Burning)
				{
					StopEffect(m, true);
				}
				else
				{
					if ((count % 4) == 0)
					{
						m.LocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, "* The swarm of insects bites and stings your flesh! *");
						m.NonlocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, String.Format("* {0} is stung by a swarm of insects *", m.Name));
					}

					m.FixedParticles(0x91C, 10, 180, 9539, EffectLayer.Waist);
					m.PlaySound(0x00E);
					m.PlaySound(0x1BC);

					AOS.Damage(m, this, Utility.RandomMinMax(30, 40) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);

					states[1] = count + 1;

					if (!m.Alive)
						StopEffect(m, false);
				}
			}
		}

		public MeerMage(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(200, 400);

				PackScroll(3, 6);
				PackScroll(3, 6);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// no LBR
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					// TODO: standard runuo
				}
			}
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
		}
	}
}
