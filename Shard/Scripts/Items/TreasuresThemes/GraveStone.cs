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

/* Scripts/Items/TreasureThemes/GraveStone.cs
 * CHANGELOG
 *	04/07/05, Kitaras	
 *		Initial Creation
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Misc;

namespace Server.Items
{

	public class BaseGraveStone : Item
	{
		private string m_Description;

		[CommandProperty(AccessLevel.GameMaster)]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
				InvalidateProperties();
			}
		}


		public BaseGraveStone(Serial serial)
			: base(serial)
		{
			Weight = 93.0;
			Name = "";
		}

		public BaseGraveStone(int itemID)
			: base(itemID)
		{
			Weight = 93.0;
			Name = "";
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_Description);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);


			int version = reader.ReadInt();
			m_Description = reader.ReadString();
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_Description != null && m_Description.Length > 0)
				LabelTo(from, m_Description);

			base.OnSingleClick(from);
		}

	}

	[Flipable(4465, 4466)]
	public class GraveStone1 : BaseGraveStone
	{


		[Constructable]
		public GraveStone1()
			: base(4466) // 4 differnt stone each haveing 2 directions
		{

		}

		public GraveStone1(Serial serial)
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

	[Flipable(4476, 4475)]
	public class GraveStone2 : BaseGraveStone
	{

		[Constructable]
		public GraveStone2()
			: base(4476) // 4 differnt stone each haveing 2 directions
		{
			Weight = 95.0;
		}

		public GraveStone2(Serial serial)
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

	[Flipable(4473, 4474)]
	public class GraveStone3 : BaseGraveStone
	{

		[Constructable]
		public GraveStone3()
			: base(4473)
		{
			Weight = 97.0;
		}

		public GraveStone3(Serial serial)
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

	[Flipable(4477, 4478)]
	public class GraveStone4 : BaseGraveStone
	{

		[Constructable]
		public GraveStone4()
			: base(4477) // 4 differnt stone each haveing 2 directions
		{
			Weight = 98.0;
		}

		public GraveStone4(Serial serial)
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
