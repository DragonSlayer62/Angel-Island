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

/* Scripts/Mobiles/Vendors/NPC/Bootlegger.cs
 * ChangeLog
 *  10/18/04, Froste
 *      Modified Restock to use OnRestock() because it's fixed now
 *  10/17/04, Froste
 *      Created this modified version of Importer.cs
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	4/29/04, mith
 *		Modified Restock to use OnRestockReagents() to restock 100 of each item instead of only 20.
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	public class Bootlegger : BaseVendor
	{
		public ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		//public override NpcGuild NpcGuild{ get{ return NpcGuild.MagesGuild; } }

		[Constructable]
		public Bootlegger()
			: base("the bootlegger")
		{
			/*			SetSkill( SkillName.EvalInt, 65.0, 88.0 );
			 *			SetSkill( SkillName.Inscribe, 60.0, 83.0 );
			 *			SetSkill( SkillName.Magery, 64.0, 100.0 );
			 *			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			 *			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			 *			SetSkill( SkillName.Wrestling, 36.0, 68.0 );
			 */
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBBootlegger());
		}

		/*		public override VendorShoeType ShoeType
		 *		{
		 *			get{ return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
		 *		}
		 */
		public override void InitBody()
		{
			SetStr(126, 145);
			SetDex(91, 110);
			SetInt(161, 185);

			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			NameHue = CalcInvulNameHue();

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
				AddItem(new Skirt(Utility.RandomNeutralHue()));
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
				AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}

		}

		public override void InitOutfit()
		{
			Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			AddItem(new Boots(Utility.RandomNeutralHue()));
			AddItem(new FancyShirt());

			// all brigands wear a bandana, but won't drop this one
			//	see OnBeforeDeath for the bandana they do drop
			Bandana bandana = new Bandana();
			bandana.LootType = LootType.Newbied;
			AddItem(bandana);

		}

		public override void Restock()
		{
			base.LastRestock = DateTime.Now;

			IBuyItemInfo[] buyInfo = this.GetBuyInfo();

			foreach (IBuyItemInfo bii in buyInfo)
				bii.OnRestock(); // change bii.OnRestockReagents() to OnRestock()
		}

		public override bool HandlesOnSpeech(Mobile from)
		{
			if (from.InRange(this.Location, 2))
				return true;

			return base.HandlesOnSpeech(from);
		}

		/*      public override void OnSpeech(SpeechEventArgs e)
		 *    {
		 *        base.OnSpeech( e );
		 *         this.Say("Leave these halls before it is too late!");
		 *    }
		 */

		public override void GetContextMenuEntries(Mobile from, ArrayList list)
		{
			base.GetContextMenuEntries(from, list);

			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i] is ContextMenus.VendorBuyEntry)
					list.RemoveAt(i--);
				else if (list[i] is ContextMenus.VendorSellEntry)
					list.RemoveAt(i--);
			}
		}

		public Bootlegger(Serial serial)
			: base(serial)
		{
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

			NameHue = CalcInvulNameHue();
		}

	}
}