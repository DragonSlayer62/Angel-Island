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

/* Scripts/Mobiles/Vendors/NPC/Importer.cs
 * ChangeLog
 *  12/03/06 Taran Kain
 *      Set Female = false. No trannies!
 *  1/4/05, Froste
 *      Changed the title from "the importer" to "the mystic importer"
 *  10/18/04, Froste
 *      Modified Restock to use OnRestock() because it's fixed now
 *  10/11/04, Froste
 *      Created this modified version of Mage.cs
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
	public class Importer : BaseVendor
	{
		public ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		//public override NpcGuild NpcGuild{ get{ return NpcGuild.MagesGuild; } }

		[Constructable]
		public Importer()
			: base("the mystic importer")
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
			m_SBInfos.Add(new SBImporter());
		}

		/*		public override VendorShoeType ShoeType
		 *		{
		 *			get{ return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
		 *		}
		 */
		public override void InitBody()
		{
			InitStats(100, 100, 25);

			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			NameHue = CalcInvulNameHue();

			Female = false;
			Body = 0x190;
			Name = NameList.RandomName("male");

		}

		public override void InitOutfit()
		{
			//			base.InitOutfit();

			//			AddItem( new Server.Items.Robe( Utility.RandomBlueHue() ) );
			AddItem(new Server.Items.GnarledStaff());

			if (Utility.RandomBool())
				AddItem(new Shoes(Utility.RandomBlueHue()));
			else
				AddItem(new Sandals(Utility.RandomBlueHue()));

			Item EvilMageRobe = new Robe();
			EvilMageRobe.Hue = 0x1;
			EvilMageRobe.LootType = LootType.Newbied;
			AddItem(EvilMageRobe);

			Item EvilWizHat = new WizardsHat();
			EvilWizHat.Hue = 0x1;
			EvilWizHat.LootType = LootType.Newbied;
			AddItem(EvilWizHat);

			Item Bracelet = new GoldBracelet();
			Bracelet.LootType = LootType.Newbied;
			AddItem(Bracelet);

			Item Ring = new GoldRing();
			Ring.LootType = LootType.Newbied;
			AddItem(Ring);

			Item hair = new LongHair();
			hair.Hue = 0x47E;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			if (!this.Female)
			{
				Item beard = new MediumLongBeard();
				beard.Hue = 0x47E;
				beard.Movable = false;
				beard.Layer = Layer.FacialHair;
				AddItem(beard);
			}

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

		public Importer(Serial serial)
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