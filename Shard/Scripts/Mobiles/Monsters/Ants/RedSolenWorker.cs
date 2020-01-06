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

/* ./Scripts/Mobiles/Monsters/Ants/RedSolenWorker.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a solen worker corpse")]
	public class RedSolenWorker : BaseCreature
	{
		[Constructable]
		public RedSolenWorker()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a red solen worker";
			Body = 781;
			BaseSoundID = 959;

			SetStr(96, 120);
			SetDex(81, 105);
			SetInt(36, 60);

			SetHits(58, 72);

			SetDamage(5, 7);

			SetSkill(SkillName.MagicResist, 60.0);
			SetSkill(SkillName.Tactics, 65.0);
			SetSkill(SkillName.Wrestling, 60.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;
		}

		public override int GetAngerSound()
		{
			return 0x269;
		}

		public override int GetIdleSound()
		{
			return 0x269;
		}

		public override int GetAttackSound()
		{
			return 0x186;
		}

		public override int GetHurtSound()
		{
			return 0x1BE;
		}

		public override int GetDeathSound()
		{
			return 0x8E;
		}

		public RedSolenWorker(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				SolenHelper.PackPicnicBasket(this);
				PackGold(50, 100);
				// TODO: 1-6 zoogi fungus
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{
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
					if (Spawning)
					{
						PackGold(Utility.Random(100, 180));

						SolenHelper.PackPicnicBasket(this);

						PackItem(new Server.Items.ZoogiFungus());
					}

					AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
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
