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

/* Scripts/Items/Skill Items/Tailor Items/Misc/ColorSwatch.cs
 * ChangeLog:
 *	10/16/05, erlein
 *		Added Dupe() function override so swatch stacks work correctly.
 *	10/15/05, erlein
 *		Moved most of functional code to SpecialDyeTub for new dye tub based craft model.
 *	10/15/05, erlein
 *		Initial creation.
 */

using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class ColorSwatch : Item
	{
		private string m_StoredColorName;
		public string StoredColorName
		{
			get
			{
				return m_StoredColorName;
			}
			set
			{
				m_StoredColorName = value;
			}
		}

		[Constructable]
		public ColorSwatch()
			: this(1)
		{
		}

		[Constructable]
		public ColorSwatch(int amount)
			: base(0x175D)
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
			Name = "a color swatch";
			m_StoredColorName = "";
		}

		public ColorSwatch(Serial serial)
			: base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new ColorSwatch(), amount);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_StoredColorName);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_StoredColorName = reader.ReadString();
		}

		public override void OnSingleClick(Mobile from)
		{
			// Say what colour it is

			if (m_StoredColorName == "")
				from.SendMessage("This swatch has not yet been soaked in any dye.");
			else
				from.SendMessage("You examine your swatch and note it is " + m_StoredColorName.ToLower() + ".");

			base.OnSingleClick(from);
		}
	}
}