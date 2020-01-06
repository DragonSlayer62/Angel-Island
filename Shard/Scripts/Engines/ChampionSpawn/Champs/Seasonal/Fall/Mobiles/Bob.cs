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

/* Scripts\Engines\ChampionSpawn\Champs\Seasonal\Fall\Mobiles\Bob.cs
 * ChangeLog
 *  9/29/07, Adam
 *		Create from Brigand.
 */

using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	public class Bob : BaseCreature
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public Bob()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Weakest, 10, 1, 0.25, 0.5)
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = 33770;
			BardImmune = true;
			CanRun = true;

			SetStr(96, 115);
			SetDex(86, 105);
			SetInt(51, 65);

			SetDamage(23, 27);

			SetSkill(SkillName.Macing, 100);
			SetSkill(SkillName.MagicResist, 100);
			SetSkill(SkillName.Tactics, 100);

			InitBody();
			InitOutfit();

			Fame = 1000;
			Karma = -1000;

			PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : false; } }

		public override bool CanBandage { get { return true; } }
		public override TimeSpan BandageDelay { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(10, 13)); } }

		public Bob(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
			this.Female = false;
			Body = 0x190;
			Name = NameList.RandomName("Bob");
		}
		public override void InitOutfit()
		{
			WipeLayers();

			Robe robe = new Robe(23);
			AddItem(robe);

			AddItem(new Club());
		}
		public override void GenerateLoot()
		{
			PackGold(100, 150);
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
