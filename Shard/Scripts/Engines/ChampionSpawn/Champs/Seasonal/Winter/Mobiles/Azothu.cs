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

/* Scripts\Engines\ChampionSpawn\Champs\Seasonal\Winter\Mobiles\Azothu.cs
 *	ChangeLog:
 *	1/1/09, Adam
 *		- Add potions and bandages
 *			Now uses real potions and real bandages
 *		- Cross heals is now turned off
 *		- Smart AI upgrade (adds healing with bandages)
 *  03/09/07, plasma    
 *      Removed cannedevil namespace reference
 *  1/12/07, Adam
 *      - Add Azothu's Locker to the loot drop
 *      - Add the skill EvalInt 
 *      - Increase damage per hit
 *      - change from AIType.AI_Mage to AIType.AI_GolemController
 *  1/11/07, Adam
 *      Select 'None' Skull type for this special champ
 *  01/05/07, plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	5/23/04 Created by smerX
 *
 */

using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Commands;
using Server.Engines.ChampionSpawn;

namespace Server.Mobiles
{
	public class Azothu : BaseChampion
	{
		private TimeSpan m_BreathDelay = TimeSpan.FromSeconds(10.0);
		private DateTime m_NextBreathTime = DateTime.Now;

		public override ChampionSkullType SkullType { get { return ChampionSkullType.None; } }
		public override AuraType MyAura { get { return AuraType.Ice; } }

		[Constructable]
		public Azothu()
			: base(AIType.AI_BaseHybrid)
		{
			BardImmune = true;
			FightStyle = FightStyle.Melee | FightStyle.Magic | FightStyle.Smart | FightStyle.Bless | FightStyle.Curse;
			UsesHumanWeapons = false;
			UsesBandages = true;
			UsesPotions = true;
			CanRun = false;
			CanReveal = true; // magic and smart

			Name = "Azothu";
			Body = 316;
			Hue = 0x556;

			SetStr(305, 425);
			SetDex(50, 90);
			SetInt(750, 850);

			SetHits(2000);
			SetStam(102, 300);

			SetDamage(28, 34);

			SetSkill(SkillName.MagicResist, 90.0, 97.6);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);
			SetSkill(SkillName.Anatomy, 97.6, 100.0);
			SetSkill(SkillName.Magery, 100.0);
			SetSkill(SkillName.Meditation, 97.6, 100.0);
			SetSkill(SkillName.EvalInt, 120.1, 130.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			PackItem(new Bandage(Utility.RandomMinMax(VirtualArmor, VirtualArmor * 2)));
			PackStrongPotions(6, 12);
			PackItem(new Pouch());
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		public override void OnThink()
		{
			Mobile m;

			try
			{
				Mobile combatant = this.Combatant;

				if (DateTime.Now >= m_NextBreathTime)
				{
					m = VerifyValidMobile(combatant, 8);

					if (m != null)
					{
						if (Utility.RandomBool()) // 50% chance of fire :D
						{
							DoSpecialAbility(m);
							DoHarmful(m);
						}

						m_NextBreathTime = DateTime.Now + m_BreathDelay;
					}
				}
			}
			catch (Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("Exception (non-fatal) caught in Azothu.OnThink: " + e.Message);
			}

			base.OnThink();
		}

		public void DoSpecialAbility(Mobile target)
		{
			try
			{
				Mobile m = VerifyValidMobile(target, 8);

				if (m != null)
				{
					Effects.SendMovingEffect(this, m, 0x36D4, 5, 0, false, false, 1151, 0);
					m.PlaySound(0x204);
					m.Paralyze(TimeSpan.FromSeconds(6.0));
					DoHarmful(m);
				}
			}
			catch (Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("Exception (non-fatal) caught in Azothu.OnDoSpecialAbility: " + e.Message);
			}
		}

		public override void Damage(int amount, Mobile from)
		{
			try
			{
				Mobile m = VerifyValidMobile(from, 10);

				if (m != null)
				{

					if (Utility.RandomDouble() >= 0.94) // 6% chance of para upon attack
					{
						m.SendMessage("Azothu looks to you, and you find yourself paralyzed.");
						m.Paralyze(TimeSpan.FromSeconds(4.0));
						m.PlaySound(0x204);
					}
				}
			}
			catch (Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("Exception (non-fatal) caught in Azothu.Damage: " + e.Message);
			}

			base.Damage(amount, from);
		}

		public Mobile VerifyValidMobile(Mobile m, int tileRange)
		{
			try
			{
				if (m != null && m is PlayerMobile && m.AccessLevel == AccessLevel.Player || m != null && m is BaseCreature && ((BaseCreature)m).Controlled)
				{
					if (m != null && m.Map == this.Map && m.InRange(this, tileRange) && m.Alive)
					{
						return m;
					}
				}
			}
			catch (Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("Exception (non-fatal) caught in Azothu.VerifyValidPlayer: " + e.Message);
			}

			return null;
		}

		public Azothu(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				Item item = null;
				switch (Utility.Random(8))
				{
					case 0:
						item = new SpecialLeggings();
						item.Name = "Azothu's Leggings";
						break;	// Leggings
					case 1:
						item = new SpecialArms();
						item.Name = "Azothu's Arms";
						break;	// arms
					case 2:
						item = new SpecialTunic();
						item.Name = "Azothu's Tunic";
						break;	// Chest
					case 3:
						item = new SpecialArmor();
						item.Name = "Azothu's Armor";
						break;	// Female Chest
					case 4:
						item = new SpecialGorget();
						item.Name = "Azothu's Gorget";
						break;	// gorget
					case 5:
						item = new SpecialGloves();
						item.Name = "Azothu's Gloves";
						break;	// gloves
					case 6:
						item = new SpecialHelm();
						item.Name = "Azothu's Helm";
						break;	// helm
					case 7:
						item = new MetalChest();
						item.Name = "Azothu's Locker";
						break;	// helm
				}

				if (item != null)
				{
					item.Hue = 0x84A;
					PackItem(item);
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
			return base.OnBeforeDeath();
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
