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

/* Scripts/Items/Armor/Leather/LeatherGloves.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 15 lines removed.
 *	9/11/04, Adam
 *		 Return Arcane Properties from Corpse Skin variant because it was breaking serialization for 
 *			existing items.
 *  9/11/04, Pigpen
 *		Add Hellish variant of this leather piece.
 *		Removed Arcane Properties from Corpse Skin variant.
 *	7/16/04, Adam
 *		Add Corpse Skin variant of this leather piece.
 */

using System;
using Server.Items;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	[Flipable]
	public class LeatherGloves : BaseArmor, IArcaneEquip
	{

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 10; } }

		public override int ArmorBase { get { return 13; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		[Constructable]
		public LeatherGloves()
			: base(0x13C6)
		{
			Weight = 1.0;
		}

		public LeatherGloves(Serial serial)
			: base(serial)
		{
		}

// old name removed, see base class

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

		public void Update()
		{
			if (IsArcane)
				ItemID = 0x26B0;
			else if (ItemID == 0x26B0)
				ItemID = 0x13C6;

			if (IsArcane && CurArcaneCharges == 0)
				Hue = 0;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (IsArcane)
				list.Add(1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges); // arcane charges: ~1_val~ / ~2_val~
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);

			if (IsArcane)
				LabelTo(from, 1061837, String.Format("{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges));
		}

		public void Flip()
		{
			if (ItemID == 0x13C6)
				ItemID = 0x13CE;
			else if (ItemID == 0x13CE)
				ItemID = 0x13C6;
		}
		#endregion
	}

	public class CorpseSkinGloves : BaseArmor, IArcaneEquip
	{

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 10; } }

		public override int ArmorBase { get { return 13; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		[Constructable]
		public CorpseSkinGloves()
			: base(0x13C6)
		{
			Weight = 1.0;
			Hue = 2101;
			ProtectionLevel = ArmorProtectionLevel.Guarding;
			Durability = ArmorDurabilityLevel.Massive;
			if (Utility.RandomDouble() < 0.20)
				Quality = ArmorQuality.Exceptional;
			Name = "Corpse Skin Gloves";
		}

		public CorpseSkinGloves(Serial serial)
			: base(serial)
		{
		}

		// Special version that DOES NOT show armor attributes and tags
		public override void OnSingleClick(Mobile from)
		{
			ArrayList attrs = new ArrayList();

			int number;

			if (Name == null)
			{
				number = LabelNumber;
			}
			else
			{
				this.LabelTo(from, Name);
				number = 1041000;
			}

			if (attrs.Count == 0 && Crafter == null && Name != null)
				return;

			EquipmentInfo eqInfo = new EquipmentInfo(number, Crafter, false, (EquipInfoAttribute[])attrs.ToArray(typeof(EquipInfoAttribute)));

			from.Send(new DisplayEquipmentInfo(this, eqInfo));

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

		public void Update()
		{
			if (IsArcane)
				ItemID = 0x26B0;
			else if (ItemID == 0x26B0)
				ItemID = 0x13C6;

			if (IsArcane && CurArcaneCharges == 0)
				Hue = 0;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (IsArcane)
				list.Add(1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges); // arcane charges: ~1_val~ / ~2_val~
		}
		/*
				public override void OnSingleClick( Mobile from )
				{
					base.OnSingleClick( from );

					if ( IsArcane )
						LabelTo( from, 1061837, String.Format( "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ) );
				}
		*/
		public void Flip()
		{
			if (ItemID == 0x13C6)
				ItemID = 0x13CE;
			else if (ItemID == 0x13CE)
				ItemID = 0x13C6;
		}
		#endregion
	}

	public class HellishGloves : BaseArmor
	{

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 40; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 10; } }

		public override int ArmorBase { get { return 13; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
		public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		[Constructable]
		public HellishGloves()
			: base(0x13C6)
		{
			Weight = 1.0;
			Hue = 1645;
			ProtectionLevel = ArmorProtectionLevel.Guarding;
			Durability = ArmorDurabilityLevel.Massive;
			if (Utility.RandomDouble() < 0.20)
				Quality = ArmorQuality.Exceptional;
			Name = "Hellish Gloves";
		}

		public HellishGloves(Serial serial)
			: base(serial)
		{
		}

		// Special version that DOES NOT show armor attributes and tags
		public override void OnSingleClick(Mobile from)
		{
			ArrayList attrs = new ArrayList();

			int number;

			if (Name == null)
			{
				number = LabelNumber;
			}
			else
			{
				this.LabelTo(from, Name);
				number = 1041000;
			}

			if (attrs.Count == 0 && Crafter == null && Name != null)
				return;

			EquipmentInfo eqInfo = new EquipmentInfo(number, Crafter, false, (EquipInfoAttribute[])attrs.ToArray(typeof(EquipInfoAttribute)));

			from.Send(new DisplayEquipmentInfo(this, eqInfo));

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
