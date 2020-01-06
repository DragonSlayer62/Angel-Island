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
	public abstract class BaseRefreshPotion : BasePotion
	{
		public abstract double Refresh { get; }

		public BaseRefreshPotion(PotionEffect effect)
			: base(0xF0B, effect)
		{
		}

		public BaseRefreshPotion(Serial serial)
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

		public override void Drink(Mobile from)
		{
			if (from.Stam < from.StamMax)
			{
				from.Stam += Scale(from, (int)(Refresh * from.StamMax));

				BasePotion.PlayDrinkEffect(from);

				this.Delete();
			}
			else
			{
				from.SendMessage("You decide against drinking this potion, as you are already at full stamina.");
			}
		}
	}
}