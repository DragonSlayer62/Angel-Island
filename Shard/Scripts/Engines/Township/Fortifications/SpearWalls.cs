using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Multis;
using Server.Targeting;

namespace Server.Township
{
	public class SpearFortificationWall : BaseFortificationWall
	{
		protected static int[] m_ids = new int[] 
			{
				//full walls:
				0x221, //spear wall
				0x222, //spear wall other direction
				0x223, //spear wall corner
				0x227, //log wall
				0x228, //log wall other direction
				0x226, //log wall corner
				//half walls:
				0x423, //spear half-wall
				0x424, //spear half-wall other direction
				0x425 //spear half-wall corner
			};

		public SpearFortificationWall()
			: base(0x221)
		{
			this.RepairSkill = SkillName.Carpentry;
			this.Weight = 150;
		}

		public SpearFortificationWall(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		public override void Change(Mobile from)
		{
			if (CanChange(from))
			{
				int index = -1;
				for (int i = 0; i < m_ids.Length; i++)
				{
					if (m_ids[i] == this.ItemID)
					{
						index = i;
						break;
					}
				}

				if (index == -1)
				{
					this.ItemID = m_ids[0];
				}
				else
				{
					index++;
					if (index >= m_ids.Length)
					{
						index = 0;
					}
					this.ItemID = m_ids[index];
				}
			}
			else
			{
				from.SendMessage("You can't change this wall.");
			}
		}

	}
}
