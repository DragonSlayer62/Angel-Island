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
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class RunicSewingKit : BaseRunicTool
	{
		public override CraftSystem CraftSystem { get { return DefTailoring.CraftSystem; } }

		public override void AddNameProperty(ObjectPropertyList list)
		{
			string v = " ";

			if (!CraftResources.IsStandard(Resource))
			{
				int num = CraftResources.GetLocalizationNumber(Resource);

				if (num > 0)
					v = String.Format("#{0}", num);
				else
					v = CraftResources.GetName(Resource);
			}

			list.Add(1061119, v); // ~1_LEATHER_TYPE~ runic sewing kit
		}

		public override void OnSingleClick(Mobile from)
		{
			string v = " ";

			if (!CraftResources.IsStandard(Resource))
			{
				int num = CraftResources.GetLocalizationNumber(Resource);

				if (num > 0)
					v = String.Format("#{0}", num);
				else
					v = CraftResources.GetName(Resource);
			}

			LabelTo(from, 1061119, v); // ~1_LEATHER_TYPE~ runic sewing kit
		}

		[Constructable]
		public RunicSewingKit(CraftResource resource)
			: base(resource, 0xF9D)
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue(resource);
		}

		[Constructable]
		public RunicSewingKit(CraftResource resource, int uses)
			: base(resource, uses, 0xF9D)
		{
			Weight = 2.0;
			Hue = CraftResources.GetHue(resource);
		}

		public RunicSewingKit(Serial serial)
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

			if (ItemID == 0x13E4 || ItemID == 0x13E3)
				ItemID = 0xF9D;
		}
	}
}