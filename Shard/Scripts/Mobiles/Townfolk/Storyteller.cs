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

/* Scripts/Mobiles/Townfolk/Storyteller.cs
 * ChangeLog
 *  7/20/08, Adam
 *      move ParseSpeech() handling from BaseHuman to here
 *	7/10/08, Adam
 *		Initial version
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using EDI = Server.Mobiles;
using Server.BountySystem;
using Server.Commands;

namespace Server.Mobiles
{
	public class Storyteller : BaseHuman
	{

		[Constructable]
		public Storyteller()
			: base(AIType.AI_Melee, FightMode.Aggressor, 22, 1, 0.2, 1.0)
		{
			InitBody();
			InitOutfit();
			Title = "the storyteller";
		}

		public Storyteller(Serial serial)
			: base(serial)
		{
		}

		protected override void InitTriggers()
		{
			// register the keywords that trigger a conversation
			ConversationTriggers.Add("toyshop", "toyshop");						// okay sample (for beginners)
			ConversationTriggers.Add("zork1", "zork1");							// Converted to version 5
			//ConversationTriggers.Add("zork2", "zork2");						// ZLR does not currently run v3 files
			//ConversationTriggers.Add("zork3", "zork3");						// ZLR does not currently run v3 files
			ConversationTriggers.Add("dungeon", "zdungeon_r13");				// zdungeon_r13.z5 (original mainfram version of zork)
			ConversationTriggers.Add("the undiscovered underground", "ztuu");	// okay classic
			ConversationTriggers.Add("adventure", "advent");					// okay classic
		}

		public override void InitBody()
		{
			SetStr(90, 100);
			SetDex(90, 100);
			SetInt(15, 25);

			Hue = Utility.RandomSkinHue();

			if (Female = Utility.RandomBool())
			{
				Body = 401;
				Name = NameList.RandomName("female");
			}
			else
			{
				Body = 400;
				Name = NameList.RandomName("male");
			}
		}

		public override void InitOutfit()
		{
			WipeLayers();
			AddItem(new Shirt(Utility.RandomNeutralHue()));
			AddItem(new ShortPants(Utility.RandomNeutralHue()));
			AddItem(new Boots(Utility.RandomNeutralHue()));

			switch (Utility.Random(4))
			{
				case 0: AddItem(new ShortHair(Utility.RandomHairHue())); break;
				case 1: AddItem(new TwoPigTails(Utility.RandomHairHue())); break;
				case 2: AddItem(new ReceedingHair(Utility.RandomHairHue())); break;
				case 3: AddItem(new KrisnaHair(Utility.RandomHairHue())); break;
			}

			switch (Utility.Random(4))
			{
				case 0: AddItem(new Drums()); break;
				case 1: AddItem(new Harp()); break;
				case 2: AddItem(new Lute()); break;
				case 3: AddItem(new Tambourine()); break;
			}

			PackGold(26);
		}

		protected override bool ParseSpeech(Mobile from, string text)
		{
			string tx = text.ToLower();         // what was said
			string nm = this.Name.ToLower();    // my name
			string key = base.IsTrigger(tx);    // do we have a trigger word like "zork1"
			if (key == tx) return true;         // they know what they want, just play the game with them.

			bool story_pos = tx.IndexOf("story") != -1;
			bool game_pos = tx.IndexOf("game") != -1;
			bool name_pos = tx.IndexOf(nm) != -1;
			bool key_pos = (key != null) ? tx.IndexOf(key) != -1 : false;

			if (name_pos && (story_pos || game_pos) && key == null)
			{   // list the games we know about
				this.Say("I know how to play the following games:");
				string list = "";
				foreach (string key_name in ConversationTriggers.Keys)
					list += (key_name + "\n");
				this.Say(String.Format("{0} Which would you like to play?", list));
				return false;
			}
			else
			{
				return key != null;
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