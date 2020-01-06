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

using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Vagabond : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public Vagabond()
			: base("the vagabond")
		{
			SetSkill(SkillName.ItemID, 60.0, 83.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBTinker());
			m_SBInfos.Add(new SBVagabond());
		}

		public override void InitOutfit()
		{
			AddItem(new FancyShirt(RandomBrightHue()));
			AddItem(new Shoes(GetShoeHue()));
			AddItem(new LongPants(GetRandomHue()));

			if (Utility.RandomBool())
				AddItem(new Cloak(RandomBrightHue()));

			switch (Utility.Random(2))
			{
				case 0: AddItem(new SkullCap(Utility.RandomNeutralHue())); break;
				case 1: AddItem(new Bandana(Utility.RandomNeutralHue())); break;
			}

			int hairHue = Utility.RandomHairHue();

			if (Female)
			{
				switch (Utility.Random(9))
				{
					case 0: AddItem(new Afro(hairHue)); break;
					case 1: AddItem(new KrisnaHair(hairHue)); break;
					case 2: AddItem(new PageboyHair(hairHue)); break;
					case 3: AddItem(new PonyTail(hairHue)); break;
					case 4: AddItem(new ReceedingHair(hairHue)); break;
					case 5: AddItem(new TwoPigTails(hairHue)); break;
					case 6: AddItem(new ShortHair(hairHue)); break;
					case 7: AddItem(new LongHair(hairHue)); break;
					case 8: AddItem(new BunsHair(hairHue)); break;
				}
			}
			else
			{
				switch (Utility.Random(8))
				{
					case 0: AddItem(new Afro(hairHue)); break;
					case 1: AddItem(new KrisnaHair(hairHue)); break;
					case 2: AddItem(new PageboyHair(hairHue)); break;
					case 3: AddItem(new PonyTail(hairHue)); break;
					case 4: AddItem(new ReceedingHair(hairHue)); break;
					case 5: AddItem(new TwoPigTails(hairHue)); break;
					case 6: AddItem(new ShortHair(hairHue)); break;
					case 7: AddItem(new LongHair(hairHue)); break;
				}

				switch (Utility.Random(5))
				{
					case 0: AddItem(new LongBeard(hairHue)); break;
					case 1: AddItem(new MediumLongBeard(hairHue)); break;
					case 2: AddItem(new Vandyke(hairHue)); break;
					case 3: AddItem(new Mustache(hairHue)); break;
					case 4: AddItem(new Goatee(hairHue)); break;
				}
			}

			PackGold(100, 200);
		}

		public Vagabond(Serial serial)
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
		}
	}
}