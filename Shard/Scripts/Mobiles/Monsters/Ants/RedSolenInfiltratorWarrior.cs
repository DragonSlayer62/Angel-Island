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

/* ./Scripts/Mobiles/Monsters/Ants/RedSolenInfiltratorWarrior.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
*/

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a solen infiltrator corpse")]
	public class RedSolenInfiltratorWarrior : BaseCreature
	{
		[Constructable]
		public RedSolenInfiltratorWarrior()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a red solen infiltrator";
			Body = 782;
			BaseSoundID = 959;

			SetStr(206, 230);
			SetDex(121, 145);
			SetInt(66, 90);

			SetHits(96, 107);

			SetDamage(5, 15);

			SetSkill(SkillName.MagicResist, 80.0);
			SetSkill(SkillName.Tactics, 80.0);
			SetSkill(SkillName.Wrestling, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;
		}

		public override int GetAngerSound()
		{
			return 0xB5;
		}

		public override int GetIdleSound()
		{
			return 0xB5;
		}

		public override int GetAttackSound()
		{
			return 0x289;
		}

		public override int GetHurtSound()
		{
			return 0xBC;
		}

		public override int GetDeathSound()
		{
			return 0xE4;
		}

		public RedSolenInfiltratorWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				int gems = Utility.RandomMinMax(1, 4);

				for (int i = 0; i < gems; ++i)
					PackGem();

				SolenHelper.PackPicnicBasket(this);
				PackGold(250, 350);
				// TODO: 3-13 zoogi fungus
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
						SolenHelper.PackPicnicBasket(this);

						PackItem(new ZoogiFungus((0.05 < Utility.RandomDouble()) ? 3 : 13));
					}

					AddLoot(LootPack.Average, 2);
					AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
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
