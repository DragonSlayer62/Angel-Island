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

/* Scripts\Engines\CTFSystem\Items\CTFScorePlate.cs
 * CHANGELOG:
 * 4/10/10, adam
 *		initial framework.
 */

using System;
using Server;
using Server.Items;

namespace Server.Engines
{
	public class CTFScorePlate : Item
	{
		private CTFControl.Team m_team;
		private CTFControl m_ctrl;

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public CTFControl.Team Team { get { return m_team; } set { m_team = value; } }

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		private CTFControl CTFControl { get { return m_ctrl; } set { m_ctrl = value; } }

		[Constructable]
		public CTFScorePlate()
			: base(0x1BC3)
		{
			Movable = false;
			Visible = true;
		}

		public CTFScorePlate(Serial serial)
			: base(serial)
		{
		}

		public void Setup(CTFControl ctrl, CTFControl.Team team)
		{
			m_ctrl = ctrl;
			m_team = team;
			Hue = ctrl.TeamColor(team);
		}

		public override bool OnMoveOver(Mobile m)
		{
			if (m_ctrl != null && m_ctrl.Deleted == false)
			{
				m_ctrl.OnPlateMoveOver(m, m_team);
			}
			return true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version

			// version 1
			writer.Write(m_ctrl);
			writer.Write((int)m_team);
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
						m_team = (CTFControl.Team)reader.ReadInt();
						goto default;
					}

				default:
					break;
			}
		}
	}
}

