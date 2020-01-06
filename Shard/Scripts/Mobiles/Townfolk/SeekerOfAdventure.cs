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

/* Scripts/Mobiles/Townfolk/SeekerOfAdventure.cs
 * ChangeLog
 *	05/18/06, Adam
 *		- rewrite to elimnate named locations and replace with Point locations.
 *		- Add many new and dangerous places to go
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class SeekerOfAdventure : BaseEscortable
	{
		private static string[] m_Dungeons = new string[]
			{
				new Point3D(5456,1862,0).ToString(),	// "Covetous"
				new Point3D(5202,599,0).ToString(),		// "Deceit" 
				new Point3D(5501,570,59).ToString(),	// "Despise"
				new Point3D(5243,1004,0).ToString(),	// "Destard" 
				new Point3D(5905,22,44).ToString(),		// "Hythloth"
				new Point3D(5395,126,0).ToString(),		// "Shame"
				new Point3D(5825,599,0).ToString(),		// "Wrong"

				// new places!!
				new Point3D(633,1486,0).ToString(),		// "Yew Orc fort"
				new Point3D(4619,1334,0).ToString(),	// "the pirate stronghold on Moonglow island"
				new Point3D(635,860,0).ToString(),		// "the Militia stronghold in Yew"
				new Point3D(964,722,0).ToString(),		// "the savage stronghold"
				new Point3D(1380,1487,10).ToString(),	// "Britain Graveyard"
				new Point3D(2667,2084,5).ToString(),	// "the pirate stronghold at Buc's Den"
				new Point3D(3011,3526,15).ToString(),	// "the brigand stronghold"
				new Point3D(5166,244,15).ToString(),	// "the Council stronghold"
			};

		public override string[] GetPossibleDestinations()
		{
			return m_Dungeons;
		}

		[Constructable]
		public SeekerOfAdventure()
		{
			Title = "the seeker of adventure";
		}

		public override bool ClickTitle { get { return false; } } // Do not display 'the seeker of adventure' when single-clicking

		public override void InitOutfit()
		{
			if (Female)
				AddItem(new FancyDress(GetRandomHue()));
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

			AddItem(new Longsword());

			switch (Utility.Random(4))
			{
				case 0: AddItem(new ShortHair(Utility.RandomHairHue())); break;
				case 1: AddItem(new TwoPigTails(Utility.RandomHairHue())); break;
				case 2: AddItem(new ReceedingHair(Utility.RandomHairHue())); break;
				case 3: AddItem(new KrisnaHair(Utility.RandomHairHue())); break;
			}

			PackGold(100, 150);
		}

		public SeekerOfAdventure(Serial serial)
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