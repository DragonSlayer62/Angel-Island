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

/* Scripts/Mobiles/Monsters/Misc/Magic/Wisp.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  9/26/04, Jade
 *      Decreased gold drop from (250, 300) to (150, 200)
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName("a wisp corpse")]
	public class Wisp : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Wisp; } }

		public override Faction FactionAllegiance { get { return CouncilOfMages.Instance; } }
		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Hero; } }

		[Constructable]
		public Wisp()
			: base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "a wisp";
			Body = 58;
			BaseSoundID = 466;

			SetStr(196, 225);
			SetDex(196, 225);
			SetInt(196, 225);

			SetHits(118, 135);

			SetDamage(17, 18);

			SetSkill(SkillName.EvalInt, 80.0);
			SetSkill(SkillName.Magery, 80.0);
			SetSkill(SkillName.MagicResist, 80.0);
			SetSkill(SkillName.Tactics, 80.0);
			SetSkill(SkillName.Wrestling, 80.0);

			Fame = 4000;
			Karma = 0;

			VirtualArmor = 40;
			AddItem(new LightSource());
		}

		public Wisp(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(150, 200);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020414125938/uo.stratics.com/hunters/wisp.shtml
					// loot: None
				}
				else
				{
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average);
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
