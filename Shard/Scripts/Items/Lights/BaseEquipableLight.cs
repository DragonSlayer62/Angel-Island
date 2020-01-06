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

namespace Server.Items
{
	public abstract class BaseEquipableLight : BaseLight
	{
		[Constructable]
		public BaseEquipableLight(int itemID)
			: base(itemID)
		{
			Layer = Layer.TwoHanded;
		}

		public BaseEquipableLight(Serial serial)
			: base(serial)
		{
		}

		public override void Ignite()
		{
			if (!(Parent is Mobile) && RootParent is Mobile)
			{
				Mobile holder = (Mobile)RootParent;

				if (holder.EquipItem(this))
				{
					if (this is Candle)
						holder.SendLocalizedMessage(502969); // You put the candle in your left hand.
					else if (this is Torch)
						holder.SendLocalizedMessage(502971); // You put the torch in your left hand.

					base.Ignite();
				}
				else
				{
					holder.SendLocalizedMessage(502449); // You cannot hold this item.
				}
			}
			else
			{
				base.Ignite();
			}
		}

		public override void OnAdded(object parent)
		{
			if (Burning && parent is Container)
				Douse();

			base.OnAdded(parent);
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