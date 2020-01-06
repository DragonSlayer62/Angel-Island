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
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseHides : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; InvalidateProperties(); }
		}

		public virtual string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} pile of hides" : "{0} piles of hides", Amount);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			writer.Write((int)m_Resource);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_Resource = (CraftResource)reader.ReadInt();
						break;
					}
				case 0:
					{
						OreInfo info = new OreInfo(reader.ReadInt(), reader.ReadInt(), reader.ReadString());

						m_Resource = CraftResources.GetFromOreInfo(info);
						break;
					}
			}
		}

		public BaseHides(CraftResource resource)
			: this(resource, 1)
		{
		}

		public BaseHides(CraftResource resource, int amount)
			: base(0x1079)
		{
			Stackable = true;
			Weight = 5.0;
			Amount = amount;
			Hue = CraftResources.GetHue(resource);

			m_Resource = resource;
		}

		public BaseHides(Serial serial)
			: base(serial)
		{
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (Amount > 1)
				list.Add(1050039, "{0}\t#{1}", Amount, 1024216); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add(1024216); // pile of hides
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (!CraftResources.IsStandard(m_Resource))
			{
				int num = CraftResources.GetLocalizationNumber(m_Resource);

				if (num > 0)
					list.Add(num);
				else
					list.Add(CraftResources.GetName(m_Resource));
			}
		}

		public override int LabelNumber
		{
			get
			{
				if (m_Resource >= CraftResource.SpinedLeather && m_Resource <= CraftResource.BarbedLeather)
					return 1049687 + (int)(m_Resource - CraftResource.SpinedLeather);

				return 1047023;
			}
		}
	}

	[FlipableAttribute(0x1079, 0x1078)]
	public class Hides : BaseHides, IScissorable
	{
		[Constructable]
		public Hides()
			: this(1)
		{
		}

		[Constructable]
		public Hides(int amount)
			: base(CraftResource.RegularLeather, amount)
		{
		}

		public Hides(Serial serial)
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

		public override Item Dupe(int amount)
		{
			return base.Dupe(new Hides(amount), amount);
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this)) return false;

			base.ScissorHelper(from, new Leather(), 1);

			return true;
		}
	}

	[FlipableAttribute(0x1079, 0x1078)]
	public class SpinedHides : BaseHides, IScissorable
	{
		[Constructable]
		public SpinedHides()
			: this(1)
		{
		}

		[Constructable]
		public SpinedHides(int amount)
			: base(CraftResource.SpinedLeather, amount)
		{
		}

		public SpinedHides(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} pile of spined hides" : "{0} piles of hides", Amount);
			}
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

		public override Item Dupe(int amount)
		{
			return base.Dupe(new SpinedHides(amount), amount);
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this)) return false;

			base.ScissorHelper(from, new SpinedLeather(), 1);

			return true;
		}
	}

	[FlipableAttribute(0x1079, 0x1078)]
	public class HornedHides : BaseHides, IScissorable
	{
		[Constructable]
		public HornedHides()
			: this(1)
		{
		}

		[Constructable]
		public HornedHides(int amount)
			: base(CraftResource.HornedLeather, amount)
		{
		}

		public HornedHides(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} pile of horned hides" : "{0} piles of hides", Amount);
			}
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

		public override Item Dupe(int amount)
		{
			return base.Dupe(new HornedHides(amount), amount);
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this)) return false;

			base.ScissorHelper(from, new HornedLeather(), 1);

			return true;
		}
	}

	[FlipableAttribute(0x1079, 0x1078)]
	public class BarbedHides : BaseHides, IScissorable
	{
		[Constructable]
		public BarbedHides()
			: this(1)
		{
		}

		[Constructable]
		public BarbedHides(int amount)
			: base(CraftResource.BarbedLeather, amount)
		{
		}

		public BarbedHides(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} pile of barbed hides" : "{0} piles of hides", Amount);
			}
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

		public override Item Dupe(int amount)
		{
			return base.Dupe(new BarbedHides(amount), amount);
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this)) return false;

			base.ScissorHelper(from, new BarbedLeather(), 1);

			return true;
		}
	}
}