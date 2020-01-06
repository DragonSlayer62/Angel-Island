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


/* Scripts\Engines\CTFSystem\Items\CTFFlag.cs
 * CHANGELOG:
 * 4/10/10, adam
 *		initial framework.
 */

using System;
using Server;
using Server.Items;

namespace Server.Engines
{
	public class CTFFlag : BlackStaff
	{
		public override string OldName { get { return "ctf flag"; } }
		private CTFControl m_ctrl;

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		private CTFControl CTFControl { get { return m_ctrl; } set { m_ctrl = value; } }

		[Constructable]
		public CTFFlag()
			: base()
		{
			Weight = 6.0;
			Movable = false;
			Name = "ctf flag";
		}

		public CTFFlag(Serial serial)
			: base(serial)
		{
		}

		// force damage to FlagHPDamage
		public override int OldDieRolls { get { return 0; } }
		public override int OldDieMax { get { return 0; } }
		public override int OldAddConstant { get { return m_ctrl == null ? 70 : (m_ctrl.FlagHPDamage); } }

		public void Setup(CTFControl ctrl)
		{
			m_ctrl = ctrl;
			Hue = ctrl.FlagColor;
		}


		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (m_ctrl != null && this.ItemID == 0xDF1)
			{
				Point3D hot = this.Location;
				hot.Y--;
				if (m.Location == hot)
					m_ctrl.OnFlagMoveOver(m);
			}
			else if (m_ctrl != null && this.ItemID == 0xDF0)
			{
				Point3D hot = this.Location;
				hot.X--;
				if (m.Location == hot)
					m_ctrl.OnFlagMoveOver(m);
			}

		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m_ctrl != null)
				m_ctrl.OnFlagMoveOver(m);
			return true;
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved(parent);
			if (parent is Mobile)
				(parent as Mobile).SpeedRunFoot = TimeSpan.FromSeconds(0.2);	// speed up
		}

		public override void OnAdded(object parent)
		{
			base.OnAdded(parent);
			if (parent is Mobile)
				(parent as Mobile).SpeedRunFoot = TimeSpan.FromSeconds(0.3); // slow down
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version

			// version 1
			writer.Write(m_ctrl);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_ctrl = reader.ReadItem() as CTFControl;
						goto default;
					}

				default:
					break;
			}
		}
	}
}

