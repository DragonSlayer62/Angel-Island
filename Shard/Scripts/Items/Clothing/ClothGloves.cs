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

/* Engines/Crafting/DefTailoring.cs
 * CHANGELOG:
 *  7/18/06 Fixed name bug
 *  7/18/06 Created by Rhiannon.
 */

using System;
using Server.Items;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseGloves : BaseClothing
	{
		public BaseGloves(int itemID)
			: this(itemID, 1001)
		{
		}

		public BaseGloves(int itemID, int hue)
			: base(itemID, Layer.Gloves, hue)
		{
		}

		public BaseGloves(Serial serial)
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
	public class ClothGloves : BaseGloves
	{
		[Constructable]
		public ClothGloves()
			: this(1001)
		{
		}

		[Constructable]
		public ClothGloves(int hue)
			: base(0x13C6, hue)
		{
			Weight = 1.0;
		}

		public ClothGloves(Serial serial)
			: base(serial)
		{
		}

		public override void OnSingleClick(Mobile from)
		{
			ArrayList attrs = new ArrayList();

			if (Quality == ClothingQuality.Exceptional)
				attrs.Add(new EquipInfoAttribute(1018305 - (int)Quality));

			int number;

			if (Name == null)
			{
				this.LabelTo(from, "cloth gloves");
				number = 1041000;
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