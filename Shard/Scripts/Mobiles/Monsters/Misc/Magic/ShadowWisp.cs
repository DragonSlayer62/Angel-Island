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

/* ./Scripts/Mobiles/Monsters/Misc/Magic/ShadowWisp.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
*/

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a wisp corpse")]
	public class ShadowWisp : BaseCreature
	{
		// An evil version of the normal Wisp. These dark wisps attack on sight and are slightly stronger than their neutral cousins. 
		//	They should not be mistaken for the Shadow Wisps, doing so could be very lethal.
		// http://web.archive.org/web/20080804183753/uo.stratics.com/database/view.php?db_content=hunters&id=364
		[Constructable]
		public ShadowWisp()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a shadow wisp";
			Body = 165;
			BaseSoundID = 466;

			SetStr(16, 40);
			SetDex(16, 45);
			SetInt(11, 25);

			SetHits(10, 24);

			SetDamage(5, 10);

			SetSkill(SkillName.EvalInt, 40.0);
			SetSkill(SkillName.Magery, 50.0);
			SetSkill(SkillName.Meditation, 40.0);
			SetSkill(SkillName.MagicResist, 10.0);
			SetSkill(SkillName.Tactics, 0.1, 15.0);
			SetSkill(SkillName.Wrestling, 25.1, 40.0);

			Fame = 500;
			Karma = -500;

			VirtualArmor = 18;

			AddItem(new LightSource());
		}

		public ShadowWisp(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new Bone());

				switch (Utility.Random(6))
				{
					case 0: PackItem(new Head()); break;
					case 1: PackItem(new Torso()); break;
					case 2: PackItem(new LeftArm()); break;
					case 3: PackItem(new LeftLeg()); break;
					case 4: PackItem(new RightArm()); break;
					case 5: PackItem(new RightLeg()); break;
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806170939/uo.stratics.com/hunters/shadowwisp.shtml
					// Body Parts, Bones
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
						switch (Utility.Random(10))
						{
							case 0: PackItem(new LeftArm()); break;
							case 1: PackItem(new RightArm()); break;
							case 2: PackItem(new Torso()); break;
							case 3: PackItem(new Bone()); break;
							case 4: PackItem(new RibCage()); break;
							case 5: PackItem(new RibCage()); break;
							case 6: PackItem(new BonePile()); break;
							case 7: PackItem(new BonePile()); break;
							case 8: PackItem(new BonePile()); break;
							case 9: PackItem(new BonePile()); break;
						}
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
					{
						switch (Utility.Random(10))
						{
							case 0: PackItem(new LeftArm()); break;
							case 1: PackItem(new RightArm()); break;
							case 2: PackItem(new Torso()); break;
							case 3: PackItem(new Bone()); break;
							case 4: PackItem(new RibCage()); break;
							case 5: PackItem(new RibCage()); break;
							case 6: PackItem(new BonePile()); break;
							case 7: PackItem(new BonePile()); break;
							case 8: PackItem(new BonePile()); break;
							case 9: PackItem(new BonePile()); break;
						}
					}
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
