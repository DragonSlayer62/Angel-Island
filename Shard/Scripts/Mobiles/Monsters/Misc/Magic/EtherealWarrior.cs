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

/* Scripts/Mobiles/Monsters/Misc/Magic/EtherealWarrior.cs
 * ChangeLog
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("an ethereal warrior corpse")]
	public class EtherealWarrior : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public EtherealWarrior()
			: base(AIType.AI_Mage, FightMode.Aggressor | FightMode.Evil, 10, 1, 0.2, 0.4)
		{
			SetStr(586, 785);
			SetDex(177, 255);
			SetInt(351, 450);

			SetHits(352, 471);

			SetDamage(13, 19);

			SetSkill(SkillName.Anatomy, 50.1, 75.0);
			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 99.1, 100.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 7000;
			Karma = 7000;

			InitBody();

			VirtualArmor = 120;
		}

		public override int Feathers { get { return 100; } }

		public override void InitBody()
		{
			Name = NameList.RandomName("ethereal warrior");
			Body = 123;
		}

		public override int GetAngerSound()
		{
			return 0x2F8;
		}

		public override int GetIdleSound()
		{
			return 0x2F8;
		}

		public override int GetAttackSound()
		{
			return Utility.Random(0x2F5, 2);
		}

		public override int GetHurtSound()
		{
			return 0x2F9;
		}

		public override int GetDeathSound()
		{
			return 0x2F7;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			defender.Damage(Utility.Random(10, 10), this);
			defender.Stam -= Utility.Random(10, 10);
			defender.Mana -= Utility.Random(10, 10);
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			attacker.Damage(Utility.Random(10, 10), this);
			attacker.Stam -= Utility.Random(10, 10);
			attacker.Mana -= Utility.Random(10, 10);
		}

		public EtherealWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGem();
				PackMagicEquipment(1, 3, 0.50, 0.50);
				PackMagicEquipment(1, 3, 0.15, 0.15);
				PackGold(550, 650);
				PackItem(new Feather(100));
				// Category 4 MID
				PackMagicItem(2, 3, 0.10);
				PackMagicItem(2, 3, 0.05);
				PackMagicItem(2, 3, 0.02);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806193108/uo.stratics.com/hunters/etherealwarrior.shtml
					// 400-700 Gold, Gems, 100 Feathers
					if (Spawning)
					{
						PackGold(400, 700);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);
						PackItem(new Feather(100));
					}
				}
				else
				{	// standard RunUO
					// standard runuo
					AddLoot(LootPack.Rich, 3);
					AddLoot(LootPack.Gems);
				}
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
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
