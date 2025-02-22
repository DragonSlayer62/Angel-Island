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

/* Scripts/Items/TreasureThemes/GenieBottle.cs
 * CHANGELOG
 *  12/26/07, Pix
 *      Cleanup up code (mostly spelling).
 *      Added OnSingleClick override so we can show that a lamp has been used.
 *  3/5/07, Adam,
 *      - Add check to prevent genie spawn during server war
 *      - Rearrange logic to create genie AFTER the check to see if it has already been spawned.
 *  04/03/05, Kit
 *		Fixed bug causeing lamps to become bugged and act as if already used.
 *	04/01/05, Kitaras	
 *		Initial Creation
 */
using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class GenieBottle : Item
	{

		private bool m_summoned;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool HasBeenSummonded
		{
			get
			{
				return m_summoned;
			}
			set
			{
				m_summoned = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public GenieBottle()
			: this(false) //for testing purpose disable spawning of genie currently
		{

		}

		[Constructable]
		public GenieBottle(bool charged)
			: base(0xF00)
		{
			Name = "a brass lamp";
			Hue = 1706;
			m_summoned = charged;

		}

		public GenieBottle(Serial serial)
			: base(serial)
		{
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_summoned)
			{
				this.LabelTo(from, "[used]");
			}
			base.OnSingleClick(from);
		}

		public override void OnDoubleClick(Mobile m)
		{
			Map map = m.Map;
			if (!IsChildOf(m.Backpack))
			{
				m.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			if (Server.Misc.AutoRestart.ServerWars == true && m.AccessLevel < AccessLevel.GameMaster)
			{   // no genie during server wars
				m.SendMessage("The genie refuses to be awoken at this time.");
				return;
			}

			if (m_summoned == false)
			{
				BaseCreature Genie = new Genie(m);
				m.SendMessage("You bring forth the genie of the lamp.");
				bool spawned = false;
				for (int i = 0; !spawned && i < 10; ++i)
				{
					int x = m.X - 3 + Utility.Random(7);
					int y = m.Y - 3 + Utility.Random(7);

					if (map.CanSpawnMobile(x, y, m.Z))
					{
						m.PlaySound(0x208);
						Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z + 4), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z - 4), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z + 4), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z - 4), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 11), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 7), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 3), m.Map, 0x3728, 13);
						Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z - 1), m.Map, 0x3728, 13);
						m_summoned = true;
						Genie.MoveToWorld(new Point3D(x, y, m.Z), m.Map);
						this.Delete();
						spawned = true;
						Genie.Say("How dare a mortal such as you disturb my slumber!");
					}
					else
					{
						int z = map.GetAverageZ(x, y);

						if (map.CanSpawnMobile(x, y, z))
						{
							m.PlaySound(0x208);
							Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z + 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x, y + 1, m.Z - 4), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 11), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 7), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z + 3), m.Map, 0x3728, 13);
							Effects.SendLocationEffect(new Point3D(x + 1, y + 1, m.Z - 1), m.Map, 0x3728, 13);
							m_summoned = true;
							this.Delete();
							Genie.MoveToWorld(new Point3D(x, y, z), m.Map);
							spawned = true;
							Genie.Say(true, "How dare a mortal such as you disturb my slumber!");
						}
					}
				}

				if (!spawned)
				{
					m.SendMessage("You fail to release anything.");
					Genie.Delete();
				}

			}
			else
			{
				m.SendMessage("You rub the lamp, but nothing exciting happens.");
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
			writer.Write(m_summoned);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_summoned = reader.ReadBool();
		}
	}
}