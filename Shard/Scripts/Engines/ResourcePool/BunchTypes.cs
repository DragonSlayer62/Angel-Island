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

/* Scripts/Engines/ResourcePool/BunchTypes.cs
 * CHANGELOG:
 *	4/23/05 - Pix
 *		Initial Version.
 */

using System;

namespace Server.Items
{
	//Server.Items.Board
	public class BunchBoard : Item
	{
		public BunchBoard()
			: base(0x1BD8)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Board));
		}

		public BunchBoard(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Arrow
	public class BunchArrow : Item
	{
		public BunchArrow()
			: base(0x0F41)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Arrow));
		}

		public BunchArrow(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Bolt
	public class BunchBolt : Item
	{
		public BunchBolt()
			: base(0x1BFD)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Bolt));
		}

		public BunchBolt(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Shaft
	public class BunchShaft : Item
	{
		public BunchShaft()
			: base(0x1BD6)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Shaft));
		}

		public BunchShaft(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Feather
	public class BunchFeather : Item
	{
		public BunchFeather()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Feather));
			Hue = 0x0DFA;
		}

		public BunchFeather(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Cloth
	public class BunchCloth : Item
	{
		public BunchCloth()
			: base(0x0F9B)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Cloth));
		}

		public BunchCloth(Serial serial)
			: base(serial)
		{
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


	//Server.Items.Leather
	public class BunchLeather : Item
	{
		public BunchLeather()
			: base(0x1067)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(Leather));
		}

		public BunchLeather(Serial serial)
			: base(serial)
		{
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


	//Server.Items.SpinedLeather
	public class BunchSpinedLeather : Item
	{
		public BunchSpinedLeather()
			: base(0x1067)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(SpinedLeather));
			Hue = 0x0406;
		}

		public BunchSpinedLeather(Serial serial)
			: base(serial)
		{
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


	//Server.Items.HornedLeather
	public class BunchHornedLeather : Item
	{
		public BunchHornedLeather()
			: base(0x1067)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(HornedLeather));
			Hue = 0x071D;
		}

		public BunchHornedLeather(Serial serial)
			: base(serial)
		{
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


	//Server.Items.BarbedLeather
	public class BunchBarbedLeather : Item
	{
		public BunchBarbedLeather()
			: base(0x1067)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(BarbedLeather));
			Hue = 0x0715;
		}

		public BunchBarbedLeather(Serial serial)
			: base(serial)
		{
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


	//Server.Items.IronIngot
	public class BunchIronIngot : Item
	{
		public BunchIronIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(IronIngot));
		}

		public BunchIronIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.DullCopperIngot
	public class BunchDullCopperIngot : Item
	{
		public BunchDullCopperIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(DullCopperIngot));
			Hue = 0x0973;
		}

		public BunchDullCopperIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.ShadowIronIngot
	public class BunchShadowIronIngot : Item
	{
		public BunchShadowIronIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(ShadowIronIngot));
			Hue = 0x0966;
		}

		public BunchShadowIronIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.CopperIngot
	public class BunchCopperIngot : Item
	{
		public BunchCopperIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(CopperIngot));
			Hue = 0x096D;
		}

		public BunchCopperIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.BronzeIngot
	public class BunchBronzeIngot : Item
	{
		public BunchBronzeIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(BronzeIngot));
			Hue = 0x0972;
		}

		public BunchBronzeIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.GoldIngot
	public class BunchGoldIngot : Item
	{
		public BunchGoldIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(GoldIngot));
			Hue = 0x08A5;
		}

		public BunchGoldIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.AgapiteIngot
	public class BunchAgapiteIngot : Item
	{
		public BunchAgapiteIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(AgapiteIngot));
			Hue = 0x0979;
		}

		public BunchAgapiteIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.VeriteIngot
	public class BunchVeriteIngot : Item
	{
		public BunchVeriteIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(VeriteIngot));
			Hue = 0x089F;
		}

		public BunchVeriteIngot(Serial serial)
			: base(serial)
		{
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


	//Server.Items.ValoriteIngot
	public class BunchValoriteIngot : Item
	{
		public BunchValoriteIngot()
			: base(0x1BF3)
		{
			Name = Server.Engines.ResourcePool.ResourcePool.GetBunchName(typeof(ValoriteIngot));
			Hue = 0x08AB;
		}

		public BunchValoriteIngot(Serial serial)
			: base(serial)
		{
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
