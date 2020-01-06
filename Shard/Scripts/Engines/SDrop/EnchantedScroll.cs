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

/* Scripts/Engines/SDrop/EnchantedScroll.cs
 * ChangeLog:
 * 2/28/09, Adam
 *		Change EnchantedScroll to return a dud scroll 60% of the time (regular boots)
 *		This is a stopgap measure until we recast the SDrop system
 * 2/25/08, Pix
 *		Fixed OnsingleClick order so that the multiple lines of text aren't over eachother.
 *	8/11/05, erlein
 *		- Added two newline characters to name formatting to aid localization formatting
 *		- Moved the type check performed on enhancement attempt to SDrop.cs
 *		- Added OnSingleClick() call to display properties after successful enhancement
 *	7/13/05, erlein
 *		Changed text back to regular labelling display.
 *	7/13/05, erlein
 *		Initial creation.
 */

using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Engines;

namespace Server.Items
{
	public class EnchantedScroll : EnchantedItem
	{
		public override double SuccessAdjust
		{
			get
			{
				return CoreAI.EScrollSuccess;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public override bool Identified
		{
			get
			{
				return m_Identified;
			}
			set
			{
				m_Identified = value;

				if (value == true)
					this.Name = string.Format("an enchanted {0} scroll\n\n", sApproxLabel);
				else
					this.Name = "an enchanted scroll";
			}
		}

		// Construct from object reference of magical item passed us

		public EnchantedScroll(object miref, int baseimage)
			: base(baseimage)
		{
			Weight = 1.0;
			base.Name = "an enchanted scroll";
			Stackable = false;

			// only a 40% chance to suceed
			if (Server.Utility.RandomChance(40))
				// copy props
				base.GenerateiProps(miref);
			else
				base.GenerateiProps(new Rocks());

			((Item)miref).Delete();
		}

		public EnchantedScroll(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_Identified)
			{
				// Create target for use
				from.SendMessage("Choose the item you wish to enchant...");
				from.Target = new EnchantedScrollTarget(this);
			}
			else
				from.SendMessage("The scroll must be identified first.");
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);
			this.LabelTo(from, Name);
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


	public class EnchantedScrollTarget : Target
	{
		private EnchantedScroll m_EnchantedScroll;

		public EnchantedScrollTarget(EnchantedScroll escroll)
			: base(1, false, TargetFlags.None)
		{
			m_EnchantedScroll = escroll;
		}

		protected override void OnTarget(Mobile from, object target)
		{
			if (!(target is Item))
			{
				from.SendMessage("You must target the item you wish to enhance.");
				return;
			}

			// Create SDrop instance to handle enhancement operation

			SDrop SDropEI = new SDrop(m_EnchantedScroll, (Item)target, from);
			if (!SDropEI.CanEnhance())
				return;

			// Perform enhancement here

			if (Utility.RandomDouble() < SDropEI.EnhanceChance())
			{
				SDropEI.DoEnhance();
				((Item)target).OnSingleClick(from);

				from.SendMessage("You successfully enchant the item with the magic of the scroll!");
			}
			else
			{
				SDropEI.DoFailure();
				from.SendMessage("You fail to perform the enchantment! Both the scroll and item have been destroyed!");
			}
		}
	}
}