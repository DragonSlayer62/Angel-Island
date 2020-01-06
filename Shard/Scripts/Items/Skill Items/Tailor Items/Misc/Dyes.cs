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
using Server.Targeting;
using Server.HuePickers;

namespace Server.Items
{
	public class Dyes : Item
	{
		[Constructable]
		public Dyes()
			: base(0xFA9)
		{
			Weight = 3.0;
		}

		public Dyes(Serial serial)
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

			if (Weight == 0.0)
				Weight = 3.0;
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendLocalizedMessage(500856); // Select the dye tub to use the dyes on.
			from.Target = new InternalTarget();
		}

		private class InternalTarget : Target
		{
			public InternalTarget()
				: base(1, false, TargetFlags.None)
			{
			}

			private class InternalPicker : HuePicker
			{
				private DyeTub m_Tub;

				public InternalPicker(DyeTub tub)
					: base(tub.ItemID)
				{
					m_Tub = tub;
				}

				public override void OnResponse(int hue)
				{
					m_Tub.DyedHue = hue;
				}
			}

			private static void SetTubHue(Mobile from, object state, int hue)
			{
				((DyeTub)state).DyedHue = hue;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is DyeTub)
				{
					DyeTub tub = (DyeTub)targeted;

					if (tub.Redyable)
					{
						if (tub.CustomHuePicker == null)
							from.SendHuePicker(new InternalPicker(tub));
						else
							from.SendGump(new CustomHuePickerGump(from, tub.CustomHuePicker, new CustomHuePickerCallback(SetTubHue), tub));
					}
					else if (tub is BlackDyeTub)
					{
						from.SendLocalizedMessage(1010092); // You can not use this on a black dye tub.
					}
					else
					{
						from.SendMessage("That dye tub may not be redyed.");
					}
				}
				else
				{
					from.SendLocalizedMessage(500857); // Use this on a dye tub.
				}
			}
		}
	}
}