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

/* Items/Deeds/VendorRenegotiationContract.cs
 * ChangeLog:
 *  1/15/00, Adam
 *		Initial Creation
 *		Convert a Player Vendor from (modified)OSI fees to a commission model
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Text.RegularExpressions;
using Server.Misc;

namespace Server.Items
{
	public class VendorRenegotiationContractTarget : Target // Create our targeting class (which we derive from the base target class)
	{
		private VendorRenegotiationContract m_Deed;

		public VendorRenegotiationContractTarget(VendorRenegotiationContract deed)
			: base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target)
		{

			if (target is PlayerVendor)
			{
				PlayerVendor vendor = (PlayerVendor)target;
				if (vendor.IsOwner(from))
				{
					if (vendor.PricingModel == PricingModel.Commission)
					{
						from.SendMessage("This vendor is already working on commission.");
					}
					else
					{
						vendor.PricingModel = PricingModel.Commission;
						vendor.SayTo(from, String.Format("I shall now work for a minimum wage plus a {0}% comission.", ((int)(vendor.Commission * 100)).ToString()));
						m_Deed.Delete();
					}
				}

				else
				{
					vendor.SayTo(from, "I do not work for thee! Only my master may renegotiate my contract.");
				}
			}
			else
			{
				from.SendMessage("Thou canst only renegotiate the contracts of thy own servants.");
			}
		}

	}


	public class VendorRenegotiationContract : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public VendorRenegotiationContract()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "a vendor renegotiation contract";
		}

		public VendorRenegotiationContract(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			// Make sure deed is in pack
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			// Create target and call it
			from.SendMessage("Whose contract dost thou wish to renegotiate?");
			from.Target = new VendorRenegotiationContractTarget(this);
		}

	}

}

