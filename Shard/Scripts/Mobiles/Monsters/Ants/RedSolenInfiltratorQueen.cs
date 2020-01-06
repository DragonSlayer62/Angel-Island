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

/* ./Scripts/Mobiles/Monsters/Ants/RedSolenInfiltratorQueen.cs
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
	[CorpseName("a solen infiltrator corpse")] // TODO: Corpse name?
	public class RedSolenInfiltratorQueen : BaseCreature
	{
		[Constructable]
		public RedSolenInfiltratorQueen()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a red solen infiltrator";
			Body = 783;
			BaseSoundID = 959;

			SetStr(326, 350);
			SetDex(141, 165);
			SetInt(96, 120);

			SetHits(151, 162);

			SetDamage(10, 15);

			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.Wrestling, 90.0);

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 50;
		}

		public override int GetAngerSound()
		{
			return 0x259;
		}

		public override int GetIdleSound()
		{
			return 0x259;
		}

		public override int GetAttackSound()
		{
			return 0x195;
		}

		public override int GetHurtSound()
		{
			return 0x250;
		}

		public override int GetDeathSound()
		{
			return 0x25B;
		}

		public RedSolenInfiltratorQueen(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				SolenHelper.PackPicnicBasket(this);
				PackGold(150, 200);
				// TODO: 4-16 zoogi fungus
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

						PackItem(new ZoogiFungus((0.05 < Utility.RandomDouble()) ? 4 : 16));
					}

					AddLoot(LootPack.Rich);
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
