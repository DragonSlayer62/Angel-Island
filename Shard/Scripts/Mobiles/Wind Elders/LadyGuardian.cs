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

/* Scripts/Mobiles/Wind Elders/LadyGuardian.cs
 * ChangeLog
 *	1/3/09, Adam
 *		Update to new AI .. give potions, bandages and a pouch to trap
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/16/04, Froste
 *      Changed IOBAlignment to Council
 *  11/10/04, Froste
 *      Implemented new random IOB drop system and changed drop change to 12%
 *	11/05/04, Pigpen
 *		Made changes for Implementation of IOBSystem. Changes include:
 *		Removed IsEnemy and Aggressive Action Checks. These are now handled in BaseCreature.cs
 *		Set Creature IOBAlignment to Undead.
 *  9/26/04, Jade
 *      Increased gold drop from (100, 130) to (450, 600)
 *	9/23/04 smerX
 *		Enhanced speech
 *	8/4/04, Adam
 *		Remove Poison Immunity
 *	8/3/04, Adam
 *		Update Stats, Skills, and Damage/Resist values to be more consistent.
 *	7/21/04, mith
 *		IsEnemy() and AggressiveAction() code added to support Brethren property of BloodDrenchedBandana.
 *		OnMovement() modified to streamline the speech checking.
 * ChangeLog
 *	5/14/04, mith
 *		modified the way we do the warning speech.
 *	5/12/04, mith
 *		Fixed hue
 *		Fixed sandals to 7% drop
 *		Buffed skills and damage
 *	5/9/04, mith
 *		Added speech.
 *		Removed SetHits() from Mobile Initialization.
 * 	Created 5/5/04 by mith
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a corpse of the Lady Guardian")]
	public class LadyGuardian : BaseCreature
	{
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
		public DateTime m_NextSpeechTime;

		[Constructable]
		public LadyGuardian()
			: base(AIType.AI_BaseHybrid, FightMode.All | FightMode.Weakest, 10, 1, 0.15, 0.25) //0.2, 0.4
		{
			BardImmune = true;
			FightStyle = FightStyle.Magic | FightStyle.Smart | FightStyle.Bless | FightStyle.Curse;
			UsesHumanWeapons = false;
			UsesBandages = true;
			UsesPotions = true;
			CanRun = true;
			CanReveal = true; // magic and smart

			Name = "Lady Guardian";
			Body = 0x191;
			Hue = 0x83F4;
			Female = true;
			IOBAlignment = IOBAlignment.Council;
			ControlSlots = 6;

			BloodDrenchedBandana bandana = new BloodDrenchedBandana();
			bandana.LootType = LootType.Newbied;
			AddItem(bandana);

			AddItem(new Kilt(0x14C));
			AddItem(new LongHair(0x14F));

			Shirt shirt = new Shirt(0x1); //black shirt
			if (Utility.RandomDouble() <= 0.93)
				shirt.LootType = LootType.Newbied;
			AddItem(shirt);

			Sandals sandals = new Sandals(0x66C);
			if (Utility.RandomDouble() <= 0.93)
				sandals.LootType = LootType.Newbied;
			AddItem(sandals);

			SilverRing ring = new SilverRing();
			ring.Name = "For my lovely Jade";
			if (Utility.RandomDouble() < 0.95)
				ring.LootType = LootType.Newbied;
			AddItem(ring);

			SetStr(375, 400);
			SetDex(100, 125);
			SetInt(150, 175);

			SetHits(250, 303);
			SetDamage(11, 13);

			SetSkill(SkillName.EvalInt, 100.0, 110.0);
			SetSkill(SkillName.Magery, 100.0, 110.0);
			SetSkill(SkillName.MagicResist, 100.0, 110.0);
			SetSkill(SkillName.Meditation, 100.0, 110.0);
			SetSkill(SkillName.Tactics, 85.0, 100.0);
			SetSkill(SkillName.Wrestling, 100.0, 110.0);
			SetSkill(SkillName.Poisoning, 100.1, 101.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 16;

			m_NextSpeechTime = DateTime.Now;

			PackItem(new Bandage(Utility.RandomMinMax(VirtualArmor, VirtualArmor * 2)));
			PackStrongPotions(6, 12);
			PackItem(new Pouch());
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : false; } }
		public override Poison PoisonImmune { get { return null; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }

		public override bool Uncalmable
		{
			get
			{
				if (Hits > 1)
					Say("How about you learn how to play that thing?");

				return BardImmune;
			}
		}

		public LadyGuardian(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackReg(20);
				PackReg(20);
				PackGold(450, 600);

				PackItem(new TheGuardianBook());

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// ai special
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{	// Standard RunUO
					// ai special
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			this.Say(true, "Kal Vas Xen Corp");
			this.Say(true, "Bring forth the council!");

			return base.OnBeforeDeath();
		}

		public override void Damage(int amount, Mobile from)
		{
			Mobile combatant = this.Combatant;

			if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8) && combatant.InLOS(this))
			{
				if (this.Hits <= 200)
				{
					if (Utility.RandomBool())
					{
						this.Say(true, "Wretched Dog!");
						m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
					}
				}
				else if (this.Hits <= 100)
				{
					if (Utility.RandomBool())
					{
						this.Say(true, "Vile Heathen!");
						m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
					}
				}
			}

			base.Damage(amount, from);
		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{

			if (m.Player && m.Alive && m.InRange(this, 10) && m.AccessLevel == AccessLevel.Player && DateTime.Now >= m_NextSpeechTime && Combatant == null)
			{
				Item item = m.FindItemOnLayer(Layer.Helm);

				if (this.InLOS(m) && this.CanSee(m))
				{
					if (item is BloodDrenchedBandana)
					{
						this.Say("Leave these halls before it is too late!");
						m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
					}
					else
					{
						this.Say("Where is your bandana, friend?");
						m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
					}
				}

			}

			base.OnMovement(m, oldLocation);
		}

		public override void OnThink()
		{
			if (DateTime.Now >= m_NextSpeechTime)
			{
				Mobile combatant = this.Combatant;

				if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 7) && combatant.InLOS(this))
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
						case 0: this.Say(true, "Yet another knuckle dragging heathen to deal with!"); break;
						case 1: this.Say(true, "You must leave our sacred home vile heathen!"); break;
						case 2: this.Say(true, "You must leave now!"); break;
						case 3: this.Say(true, "Ah! You do bleed badly!"); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
				}

				base.OnThink();
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
