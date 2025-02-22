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

/* Scripts/Items/Clothing/Shoes.cs
 * ChangeLog
 *	02/28/06, weaver
 *		Removed deserialisation check (golem controllers drop arcane stuff otherwise).
 *	10/07/05, erlein
 *		Added deserialization check for arcane charge max.. set to remove 
 *		newbification if it exists and max > 0.
 *	11/06/04,Pigpen
 *		Moved BrigandKinBoots to BrethrenClothing.cs
 *  9/16/04. Pigpen
 * 		Added BrigandKinBoots
 */

using System;

namespace Server.Items
{
	public abstract class BaseShoes : BaseClothing
	{
		private CraftResource m_Resource;

		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; Hue = CraftResources.GetHue(m_Resource); InvalidateProperties(); }
		}

		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public BaseShoes(int itemID)
			: this(itemID, 0)
		{
		}

		public BaseShoes(int itemID, int hue)
			: base(itemID, Layer.Shoes, hue)
		{
			m_Resource = DefaultResource;
		}

		public BaseShoes(Serial serial)
			: base(serial)
		{
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
						m_Resource = DefaultResource;
						break;
					}
			}
		}
	}

	[Flipable(0x2307, 0x2308)]
	public class FurBoots : BaseShoes
	{
		public override CraftResource DefaultResource { get { return CraftResource.None; } }

		[Constructable]
		public FurBoots()
			: this(0)
		{
		}

		[Constructable]
		public FurBoots(int hue)
			: base(0x2307, hue)
		{
			Weight = 3.0;
		}

		public FurBoots(Serial serial)
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

	[FlipableAttribute(0x170b, 0x170c)]
	public class Boots : BaseShoes
	{
		[Constructable]
		public Boots()
			: this(0)
		{
		}

		[Constructable]
		public Boots(int hue)
			: base(0x170B, hue)
		{
			Weight = 3.0;
		}

		public Boots(Serial serial)
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

	[Flipable]
	public class ThighBoots : BaseShoes, IArcaneEquip
	{
		#region Arcane Impl
		private int m_MaxArcaneCharges, m_CurArcaneCharges;

		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxArcaneCharges
		{
			get { return m_MaxArcaneCharges; }
			set { m_MaxArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int CurArcaneCharges
		{
			get { return m_CurArcaneCharges; }
			set { m_CurArcaneCharges = value; InvalidateProperties(); Update(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsArcane
		{
			get { return (m_MaxArcaneCharges > 0 && m_CurArcaneCharges >= 0); }
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);

			if (IsArcane)
				LabelTo(from, 1061837, String.Format("{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges));
		}

		public void Update()
		{
			if (IsArcane)
				ItemID = 0x26AF;
			else if (ItemID == 0x26AF)
				ItemID = 0x1711;

			if (IsArcane && CurArcaneCharges == 0)
				Hue = 0;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (IsArcane)
				list.Add(1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges); // arcane charges: ~1_val~ / ~2_val~
		}

		public void Flip()
		{
			if (ItemID == 0x1711)
				ItemID = 0x1712;
			else if (ItemID == 0x1712)
				ItemID = 0x1711;
		}
		#endregion

		[Constructable]
		public ThighBoots()
			: this(0)
		{
		}

		[Constructable]
		public ThighBoots(int hue)
			: base(0x1711, hue)
		{
			Weight = 4.0;
		}

		public ThighBoots(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			if (IsArcane)
			{
				writer.Write(true);
				writer.Write((int)m_CurArcaneCharges);
				writer.Write((int)m_MaxArcaneCharges);
			}
			else
			{
				writer.Write(false);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						if (reader.ReadBool())
						{
							m_CurArcaneCharges = reader.ReadInt();
							m_MaxArcaneCharges = reader.ReadInt();

							if (Hue == 2118)
								Hue = ArcaneGem.DefaultArcaneHue;
						}

						break;
					}
			}
		}
	}

	[FlipableAttribute(0x170f, 0x1710)]
	public class Shoes : BaseShoes
	{
		[Constructable]
		public Shoes()
			: this(0)
		{
		}

		[Constructable]
		public Shoes(int hue)
			: base(0x170F, hue)
		{
			Weight = 2.0;
		}

		public Shoes(Serial serial)
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

	[FlipableAttribute(0x170d, 0x170e)]
	public class Sandals : BaseShoes
	{
		[Constructable]
		public Sandals()
			: this(0)
		{
		}

		[Constructable]
		public Sandals(int hue)
			: base(0x170D, hue)
		{
			Weight = 1.0;
		}

		public Sandals(Serial serial)
			: base(serial)
		{
		}

		public override bool Dye(Mobile from, DyeTub sender)
		{
			from.SendLocalizedMessage(sender.FailMessage);
			return false;
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
