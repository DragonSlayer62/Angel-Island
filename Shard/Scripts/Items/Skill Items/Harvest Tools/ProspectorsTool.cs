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
using Server.Targeting;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class ProspectorsTool : BaseBashing, IUsesRemaining
	{
		private int m_UsesRemaining;

		[CommandProperty(AccessLevel.GameMaster)]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public bool ShowUsesRemaining { get { return true; } set { } }

		public override int LabelNumber { get { return 1049065; } } // prospector's tool

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ShadowStrike; } }

		//		public override int AosStrengthReq{ get{ return 40; } }
		//		public override int AosMinDamage{ get{ return 13; } }
		//		public override int AosMaxDamage{ get{ return 15; } }
		//		public override int AosSpeed{ get{ return 33; } }

		//		public override int OldMinDamage{ get{ return 6; } }
		//		public override int OldMaxDamage{ get{ return 8; } }
		public override int OldStrengthReq { get { return 10; } }
		public override int OldSpeed { get { return 33; } }

		public override int OldDieRolls { get { return 2; } }
		public override int OldDieMax { get { return 2; } }
		public override int OldAddConstant { get { return 4; } }

		[Constructable]
		public ProspectorsTool()
			: base(0xFB4)
		{
			Weight = 9.0;
			UsesRemaining = 50;
		}

		public ProspectorsTool(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack) || Parent == from)
				from.Target = new InternalTarget(this);
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public void Prospect(Mobile from, object toProspect)
		{
			if (!IsChildOf(from.Backpack) && Parent != from)
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			HarvestSystem system = Mining.System;

			int tileID;
			Map map;
			Point3D loc;

			if (!system.GetHarvestDetails(from, this, toProspect, out tileID, out map, out loc))
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestDefinition def = system.GetDefinition(tileID);

			if (def == null || def.Veins.Length <= 1)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestBank bank = def.GetBank(map, loc.X, loc.Y);

			if (bank == null)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}

			HarvestVein vein = bank.Vein, defaultVein = bank.DefaultVein;

			if (vein == null || defaultVein == null)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
				return;
			}
			else if (vein != defaultVein)
			{
				from.SendLocalizedMessage(1049049); // That ore looks to be prospected already.
				return;
			}

			int veinIndex = Array.IndexOf(def.Veins, vein);

			if (veinIndex < 0)
			{
				from.SendLocalizedMessage(1049048); // You cannot use your prospector tool on that.
			}
			else if (veinIndex >= (def.Veins.Length - 1))
			{
				from.SendLocalizedMessage(1049061); // You cannot improve valorite ore through prospecting.
			}
			else
			{
				bank.Vein = def.Veins[veinIndex + 1];
				from.SendLocalizedMessage(1049050 + veinIndex);

				--UsesRemaining;

				if (UsesRemaining <= 0)
				{
					from.SendLocalizedMessage(1049062); // You have used up your prospector's tool.
					Delete();
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version
			writer.Write((int)m_UsesRemaining);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_UsesRemaining = reader.ReadInt();
						break;
					}
				case 0:
					{
						m_UsesRemaining = 50;
						break;
					}
			}
		}

		private class InternalTarget : Target
		{
			private ProspectorsTool m_Tool;

			public InternalTarget(ProspectorsTool tool)
				: base(2, true, TargetFlags.None)
			{
				m_Tool = tool;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				m_Tool.Prospect(from, targeted);
			}
		}
	}
}