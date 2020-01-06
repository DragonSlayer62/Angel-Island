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

/* Scripts/Misc/Mine-Ore Problem/DeadMiner.cs
 * CHANGELOG
 *  07/02/06, Kit
 *		Overrid InitOutFit/Body, new base mobile functions
 *  09/06/05 Taran Kain
 *		Set StaticCorpse property in OnDeath to prevent looting.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a dead miner")]
	public class DeadMiner : BaseCreature
	{
		[Constructable]
		public DeadMiner()
			: base(AIType.AI_Use_Default, FightMode.None, 0, 0, 0.0, 0.0)
		{
			InitBody();
			InitOutfit();

			this.Direction = (Direction)Utility.Random(8);

			Timer.DelayCall(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.0), new TimerCallback(Kill));
		}

		public DeadMiner(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
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
			AddItem((Utility.RandomBool() ? (Item)(new LongPants(Utility.RandomNeutralHue())) : (Item)(new ShortPants(Utility.RandomNeutralHue()))));
			AddItem(new Boots(Utility.RandomNeutralHue()));
			AddItem(new HalfApron(Utility.RandomNeutralHue()));

			switch (Utility.Random(4))
			{
				case 0: AddItem(new ShortHair(Utility.RandomHairHue())); break;
				case 1: AddItem(new PonyTail(Utility.RandomHairHue())); break;
				case 2: AddItem(new ReceedingHair(Utility.RandomHairHue())); break;
				case 3: AddItem(new KrisnaHair(Utility.RandomHairHue())); break;
			}

			AddItem(new Pickaxe());
		}

		public override void OnDeath(Server.Items.Container c)
		{
			base.OnDeath(c);

			Corpse corpse = c as Corpse;
			corpse.BeginDecay(TimeSpan.FromHours(24.0));
			corpse.StaticCorpse = true;
			for (int i = 0; i < 3; i++)
			{
				Point3D p = new Point3D(Location);
				p.X += Utility.RandomMinMax(-1, 1);
				p.Y += Utility.RandomMinMax(-1, 1);
				new Blood(Utility.Random(0x122A, 5), 86400.0).MoveToWorld(p, c.Map);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
