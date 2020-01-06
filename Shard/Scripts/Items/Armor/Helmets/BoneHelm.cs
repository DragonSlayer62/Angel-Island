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

/* Scripts/Items/Armor/Helmets/BoneHelm.cs
 * ChangeLog
 *  6/21/06, Kit
 *		Changed to material type bone not plate to allow correct Sdrop creation.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 15 lines removed.
 *  1/23/05, Froste
 *  Add Bone Magi variant of this Bone piece. Meddable, AR like leather, Exceptional.
 *	9/10/04, Pigpen
 *	Add Unholy Bone variant of this Helmet piece.
 */

using System;
using Server.Items;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0x1451, 0x1456)]
	public class BoneHelm : BaseArmor
	{

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 30; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 40; } }

		public override int ArmorBase { get { return 30; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Bone; } }

		[Constructable]
		public BoneHelm()
			: base(0x1451)
		{
			Weight = 3.0;
		}

		public BoneHelm(Serial serial)
			: base(serial)
		{
		}

// old name removed, see base class

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);

			if (Weight == 1.0)
				Weight = 3.0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class UnholyBoneHelm : BaseArmor
	{

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 30; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 40; } }

		public override int ArmorBase { get { return 30; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		[Constructable]
		public UnholyBoneHelm()
			: base(0x1451)
		{
			Weight = 3.0;
			Hue = 1109;
			ProtectionLevel = ArmorProtectionLevel.Guarding;
			Durability = ArmorDurabilityLevel.Massive;
			if (Utility.RandomDouble() < 0.20)
				Quality = ArmorQuality.Exceptional;
			Name = "Unholy Bone Helm";
		}

		public UnholyBoneHelm(Serial serial)
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
			writer.Write((int)0);

			if (Weight == 1.0)
				Weight = 3.0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class BoneMagiHelm : BaseArmor
	{

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 30; } }

		public override int AosStrReq { get { return 20; } }
		public override int OldStrReq { get { return 40; } }

		public override int ArmorBase { get { return 13; } }

		public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		[Constructable]
		public BoneMagiHelm()
			: base(0x1451)
		{
			Weight = 3.0;
			Quality = ArmorQuality.Exceptional;
			Name = "Helm of the Bone Magi";
		}

		public BoneMagiHelm(Serial serial)
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
			writer.Write((int)0);

			if (Weight == 1.0)
				Weight = 3.0;
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
