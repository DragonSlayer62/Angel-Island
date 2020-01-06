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

/* Scripts/Mobiles/Gaurds/AICellGuard.cs
 * Created 4/1/04 by mith
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 1 lines removed.
 *  7/21/04, Adam
 *		1. Redo the setting of skills and setting of Damage 
 *  7/17/04, Adam
 *		1. Add NightSightScroll to drop
 *		2. Replace MindBlastScroll with FireballScroll
 * 4/12/04 mith
 *	Converted stats/skills to use dynamic values defined in CoreAI.
 * 4/10/04 changes by mith
 *	Added bag of reagents and scrolls to loot.
 * 4/1/04 changes by mith
 *	Changed starting skills to be from a range of 70-80 rather than flat 75.0.
 */
using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class AICellGuard : BaseAIGuard
	{
		[Constructable]
		public AICellGuard()
			: base()
		{
			InitStats(CoreAI.CellGuardStrength, 100, 100);

			// Set the BroadSword damage
			SetDamage(14, 25);

			SetSkill(SkillName.Anatomy, CoreAI.CellGuardSkillLevel);
			SetSkill(SkillName.Tactics, CoreAI.CellGuardSkillLevel);
			SetSkill(SkillName.Swords, CoreAI.CellGuardSkillLevel);
			SetSkill(SkillName.MagicResist, CoreAI.CellGuardSkillLevel);
		}

		public AICellGuard(Serial serial)
			: base(serial)
		{
		}
		
		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				DropWeapon(1, 1);
				DropWeapon(1, 1);

				DropItem(new BagOfReagents(CoreAI.CellGuardNumRegDrop));
				DropItem(new ParalyzeScroll());
				DropItem(new FireballScroll());
				DropItem(new NightSightScroll());
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
