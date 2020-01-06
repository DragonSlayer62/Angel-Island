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
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute(0x13E4, 0x13E3)]
	public class AncientSmithyHammer : BaseTool
	{
		private int m_Bonus;
		private SkillMod m_SkillMod;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Bonus
		{
			get
			{
				return m_Bonus;
			}
			set
			{
				m_Bonus = value;
				InvalidateProperties();

				if (m_Bonus == 0)
				{
					if (m_SkillMod != null)
						m_SkillMod.Remove();

					m_SkillMod = null;
				}
				else if (m_SkillMod == null && Parent is Mobile)
				{
					m_SkillMod = new DefaultSkillMod(SkillName.Blacksmith, true, m_Bonus);
					((Mobile)Parent).AddSkillMod(m_SkillMod);
				}
				else if (m_SkillMod != null)
				{
					m_SkillMod.Value = m_Bonus;
				}
			}
		}

		public override void OnAdded(object parent)
		{
			base.OnAdded(parent);

			if (m_Bonus != 0 && parent is Mobile)
			{
				if (m_SkillMod != null)
					m_SkillMod.Remove();

				m_SkillMod = new DefaultSkillMod(SkillName.Blacksmith, true, m_Bonus);
				((Mobile)parent).AddSkillMod(m_SkillMod);
			}
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved(parent);

			if (m_SkillMod != null)
				m_SkillMod.Remove();

			m_SkillMod = null;
		}

		public override CraftSystem CraftSystem { get { return DefBlacksmithy.CraftSystem; } }
		public override int LabelNumber { get { return 1045127; } } // ancient smithy hammer

		[Constructable]
		public AncientSmithyHammer(int bonus)
			: this(bonus, 600)
		{
		}

		[Constructable]
		public AncientSmithyHammer(int bonus, int uses)
			: base(uses, 0x13E4)
		{
			m_Bonus = bonus;
			Weight = 8.0;
			Layer = Layer.OneHanded;
			Hue = 0x482;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Bonus != 0)
				list.Add(1060451, "#1042354\t{0}", m_Bonus.ToString()); // ~1_skillname~ +~2_val~
		}

		public AncientSmithyHammer(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Bonus);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Bonus = reader.ReadInt();
						break;
					}
			}

			if (m_Bonus != 0 && Parent is Mobile)
			{
				if (m_SkillMod != null)
					m_SkillMod.Remove();

				m_SkillMod = new DefaultSkillMod(SkillName.Blacksmith, true, m_Bonus);
				((Mobile)Parent).AddSkillMod(m_SkillMod);
			}

			if (Hue == 0)
				Hue = 0x482;
		}
	}
}