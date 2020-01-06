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
	public abstract class BaseLeather : Item, ICommodity
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
				return String.Format(Amount == 1 ? "{0} piece of leather" : "{0} pieces of leather", Amount);
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

		public BaseLeather(CraftResource resource)
			: this(resource, 1)
		{
		}

		public BaseLeather(CraftResource resource, int amount)
			: base(0x1081)
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = CraftResources.GetHue(resource);

			m_Resource = resource;
		}

		public BaseLeather(Serial serial)
			: base(serial)
		{
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (Amount > 1)
				list.Add(1050039, "{0}\t#{1}", Amount, 1024199); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add(1024199); // cut leather
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
					return 1049684 + (int)(m_Resource - CraftResource.SpinedLeather);

				return 1047022;
			}
		}
	}

	[FlipableAttribute(0x1081, 0x1082)]
	public class Leather : BaseLeather
	{
		[Constructable]
		public Leather()
			: this(1)
		{
		}

		[Constructable]
		public Leather(int amount)
			: base(CraftResource.RegularLeather, amount)
		{
		}

		public Leather(Serial serial)
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
			return base.Dupe(new Leather(amount), amount);
		}
	}

	[FlipableAttribute(0x1081, 0x1082)]
	public class SpinedLeather : BaseLeather
	{
		[Constructable]
		public SpinedLeather()
			: this(1)
		{
		}

		[Constructable]
		public SpinedLeather(int amount)
			: base(CraftResource.SpinedLeather, amount)
		{
		}

		public SpinedLeather(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} piece of spined leather" : "{0} pieces of spined leather", Amount);
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
			return base.Dupe(new SpinedLeather(amount), amount);
		}
	}

	[FlipableAttribute(0x1081, 0x1082)]
	public class HornedLeather : BaseLeather
	{
		[Constructable]
		public HornedLeather()
			: this(1)
		{
		}

		[Constructable]
		public HornedLeather(int amount)
			: base(CraftResource.HornedLeather, amount)
		{
		}

		public HornedLeather(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} piece of horned leather" : "{0} pieces of horned leather", Amount);
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
			return base.Dupe(new HornedLeather(amount), amount);
		}
	}

	[FlipableAttribute(0x1081, 0x1082)]
	public class BarbedLeather : BaseLeather
	{
		[Constructable]
		public BarbedLeather()
			: this(1)
		{
		}

		[Constructable]
		public BarbedLeather(int amount)
			: base(CraftResource.BarbedLeather, amount)
		{
		}

		public BarbedLeather(Serial serial)
			: base(serial)
		{
		}

		public override string Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} piece of barbed leather" : "{0} pieces of barbed leather", Amount);
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
			return base.Dupe(new BarbedLeather(amount), amount);
		}
	}
}