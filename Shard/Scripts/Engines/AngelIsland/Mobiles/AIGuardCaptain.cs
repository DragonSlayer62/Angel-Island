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

/*	Scripts/Mobiles/Gaurds/AIGuardCaptain.cs
 * ChangeLog:
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 1 lines removed.
 *  7/21/04, Adam
 *		1. FightMode.Closest
 *		2. Redo the setting of skills and setting of Damage 
 *  7/17/04, Adam
 *		1. Add NightSightScroll to drop
 *	5/23/04 smerX
 *		Enabled healing
 *	4/29/04, mith
 *		Modified to use variables in CoreAI.
 *	4/12/04 mith
 *		Converted stats/skills to use dynamic values defined in CoreAI.	
 *	4/10/04 changes by mith
 *		Added bag of reagents and scrolls to loot.
 *		Changed name to "The Captain of the Guard".
 *	4/08/04 changes by smerX
 *		Added "YOU'LL NEVER GET OUT ALIVE" OnBeforeDeath
 *		Dropped 4 Lighthouse Passes OnBeforeDeath
 *	 4/1/04 Created by mith
 *		Changed starting skills to be from a range of 70-80 rather than flat 75.0.
 */

using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class AIGuardCaptain : BaseAIGuard
	{
		[Constructable]
		public AIGuardCaptain()
			: base()
		{
			Name = "The Captain of the Guard";
			Title = "";

			FightMode = FightMode.All | FightMode.Closest;

			InitStats(CoreAI.CaptainGuardStrength, 100, 100);

			// Set the BroadSword damage
			SetDamage(14, 25);

			SetSkill(SkillName.Anatomy, CoreAI.CaptainGuardSkillLevel);
			SetSkill(SkillName.Tactics, CoreAI.CaptainGuardSkillLevel);
			SetSkill(SkillName.Swords, CoreAI.CaptainGuardSkillLevel);
			SetSkill(SkillName.MagicResist, CoreAI.CaptainGuardSkillLevel);
		}

		public AIGuardCaptain(Serial serial)
			: base(serial)
		{
		}

		public override bool CanBandage { get { return true; } }
		public override TimeSpan BandageDelay { get { return TimeSpan.FromSeconds(12.0); } }
		public override int BandageMin { get { return 15; } }
		public override int BandageMax { get { return 30; } }

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				for (int i = 0; i <= CoreAI.CaptainGuardWeapDrop; ++i)
					DropWeapon(CoreAI.CaptainGuardWeapDrop, CoreAI.CaptainGuardWeapDrop);

				for (int i = 0; i <= CoreAI.CaptainGuardGHPotsDrop; ++i)
					DropItem(new GreaterHealPotion());

				for (int i = 1; i <= CoreAI.CaptainGuardNumLighthousePasses; ++i)
					DropItem(new AILHPass());

				DropItem(new BagOfReagents(CoreAI.CaptainGuardNumRegDrop));
				DropItem(new Bandage(CoreAI.CaptainGuardNumBandiesDrop));

				DropItem(new EnergyBoltScroll(CoreAI.CaptainGuardScrollDrop));
				DropItem(new PoisonScroll(CoreAI.CaptainGuardScrollDrop));
				DropItem(new HealScroll(CoreAI.CaptainGuardScrollDrop));
				DropItem(new GreaterHealScroll(CoreAI.CaptainGuardScrollDrop));
				DropItem(new CureScroll(CoreAI.CaptainGuardScrollDrop));
				DropItem(new NightSightScroll());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{
					if (Spawning)
					{	// ai special
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
			this.SpeechHue = 0;
			this.Say(true, "You'll never get out alive!");
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
