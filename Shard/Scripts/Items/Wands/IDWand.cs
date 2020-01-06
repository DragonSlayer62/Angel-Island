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

/* Scripts/Items/Wands/IDWand.cs
 * CHANGE LOG
 *	04/14/07, weaver
 *		Centralised identifaction routine into ItemIdentification.cs.
 *	11/04/05, weaver
 *		Added check and report on whether item identified is player crafted.
 *	07/13/05, weaver
 *		Added EnchantedScroll for SDrop system.
 *  07/06/04, Pix
 *		Changed charges to 30-50
 *  06/05/04, Pix
 *		Merged in 1.0RC0 code.
 *	05/11/04, Pulse
 *		Added "is BaseJewel" and "is BaseClothing" conditions to the OnWandTarget method to
 *		implement identifying of magic jewelry and clothing
 */

using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class IDWand : BaseWand
	{
		public override TimeSpan GetUseDelay { get { return TimeSpan.Zero; } }

		[Constructable]
		public IDWand()
			: base(WandEffect.Identification, 30, 50)
		{
		}

		public IDWand(Serial serial)
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

		public override bool OnWandTarget(Mobile from, object o)
		{
			return ItemIdentification.IdentifyItem(from, o);
		}
	}
}