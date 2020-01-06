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
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Noble : BaseEscortable
	{
		[Constructable]
		public Noble()
		{
			Title = "the noble";

			SetSkill(SkillName.Parry, 80.0, 100.0);
			SetSkill(SkillName.Swords, 80.0, 100.0);
			SetSkill(SkillName.Tactics, 80.0, 100.0);
		}

		public override bool CanTeach { get { return Core.UOSP ? false : true; } }
		public override bool ClickTitle { get { return false; } } // Do not display 'the noble' when single-clicking

		public override void InitOutfit()
		{
			if (Female)
				AddItem(new FancyDress());
			else
				AddItem(new FancyShirt(GetRandomHue()));

			int lowHue = GetRandomHue();

			AddItem(new ShortPants(lowHue));

			if (Female)
				AddItem(new ThighBoots(lowHue));
			else
				AddItem(new Boots(lowHue));

			if (!Female)
				AddItem(new BodySash(lowHue));

			AddItem(new Cloak(GetRandomHue()));

			if (!Female)
				AddItem(new Longsword());

			switch (Utility.Random(4))
			{
				case 0: AddItem(new ShortHair(Utility.RandomHairHue())); break;
				case 1: AddItem(new TwoPigTails(Utility.RandomHairHue())); break;
				case 2: AddItem(new ReceedingHair(Utility.RandomHairHue())); break;
				case 3: AddItem(new KrisnaHair(Utility.RandomHairHue())); break;
			}

			PackGold(200, 250);
		}

		public Noble(Serial serial)
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