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

/* Items/SkillItems/Magical/Runebook.cs
 * CHANGELOG:
 *	8/1/05, erlein
 *		Added m_Crafter property + related stuff to display
 *		"Crafted by [crafter_name]"
 *		Added m_Quality property
 *	7/17/05: Pix
 *		Now closes all runebooks if you drop a recall rune on a runebook.
 *		This is to work around the bug about having a runebook open.
 *	3/6/05: Pix
 *		Made RunebookEntry.Location property be settable.
 *  02/15/05, Pixie
 *		CHANGED FOR RUNUO 1.0.0 MERGE.
 *	9/4/04, mith
 *		OnDragDrop(): Copied Else block from Spellbook, to prevent people dropping things on book to have it bounce back to original location.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Multis;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum RunebookQuality
	{
		Low,
		Regular,
		Exceptional
	}

	public class Runebook : Item, ISecurable, ICraftable
	{
		private ArrayList m_Entries;
		private string m_Description;
		private int m_CurCharges, m_MaxCharges;
		private int m_DefaultIndex;
		private SecureLevel m_Level;
		private Mobile m_Crafter;
		private RunebookQuality m_Quality;

		[CommandProperty(AccessLevel.GameMaster)]
		public SecureLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int CurCharges
		{
			get
			{
				return m_CurCharges;
			}
			set
			{
				m_CurCharges = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int MaxCharges
		{
			get
			{
				return m_MaxCharges;
			}
			set
			{
				m_MaxCharges = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Crafter
		{
			get
			{
				return m_Crafter;
			}
			set
			{
				m_Crafter = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public RunebookQuality Quality
		{
			get { return m_Quality; }
			set { m_Quality = value; InvalidateProperties(); }
		}

		public override int LabelNumber { get { return 1041267; } } // runebook

		[Constructable]
		public Runebook(int maxCharges)
			: base(Core.AOS ? 0x22C5 : 0xEFA)
		{
			Weight = 3.0;
			LootType = LootType.Blessed;
			Hue = 0x461;

			Layer = Layer.OneHanded;

			m_Entries = new ArrayList();

			m_MaxCharges = maxCharges;

			m_DefaultIndex = -1;

			m_Level = SecureLevel.CoOwners;
		}

		[Constructable]
		public Runebook()
			: this(6)
		{
		}

		public ArrayList Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public RunebookEntry Default
		{
			get
			{
				if (m_DefaultIndex >= 0 && m_DefaultIndex < m_Entries.Count)
					return (RunebookEntry)m_Entries[m_DefaultIndex];

				return null;
			}
			set
			{
				if (value == null)
					m_DefaultIndex = -1;
				else
					m_DefaultIndex = m_Entries.IndexOf(value);
			}
		}

		public Runebook(Serial serial)
			: base(serial)
		{
		}

		public override bool AllowEquipedCast(Mobile from)
		{
			return true;
		}

		public override void GetContextMenuEntries(Mobile from, ArrayList list)
		{
			base.GetContextMenuEntries(from, list);
			SetSecureLevelEntry.AddTo(from, this, list);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)2);

			writer.Write((Mobile)m_Crafter);
			writer.Write((short)m_Quality);
			writer.Write((int)m_Level);

			writer.Write(m_Entries.Count);

			for (int i = 0; i < m_Entries.Count; ++i)
				((RunebookEntry)m_Entries[i]).Serialize(writer);

			writer.Write(m_Description);
			writer.Write(m_CurCharges);
			writer.Write(m_MaxCharges);
			writer.Write(m_DefaultIndex);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			LootType = LootType.Blessed;

			if (Weight == 0.0)
				Weight = 3.0;

			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
					{
						m_Crafter = reader.ReadMobile();
						m_Quality = (RunebookQuality)reader.ReadShort();
						goto case 1;
					}
				case 1:
					{
						m_Level = (SecureLevel)reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						int count = reader.ReadInt();

						m_Entries = new ArrayList(count);

						for (int i = 0; i < count; ++i)
							m_Entries.Add(new RunebookEntry(reader));

						m_Description = reader.ReadString();
						m_CurCharges = reader.ReadInt();
						m_MaxCharges = reader.ReadInt();
						m_DefaultIndex = reader.ReadInt();

						break;
					}
			}
		}

		public void DropRune(Mobile from, RunebookEntry e, int index)
		{
			if (m_DefaultIndex == index)
				m_DefaultIndex = -1;

			m_Entries.RemoveAt(index);

			RecallRune rune = new RecallRune();

			rune.Target = e.Location;
			rune.TargetMap = e.Map;
			rune.Description = e.Description;
			rune.House = e.House;
			rune.Marked = true;

			from.AddToBackpack(rune);

			from.SendLocalizedMessage(502421); // You have removed the rune.
		}

		public bool IsOpen(Mobile toCheck)
		{
			NetState ns = toCheck.NetState;

			if (ns == null)
				return false;

			//GumpCollection gumps = ns.Gumps;
			List<Gump> gumps = new List<Gump>(ns.Gumps);

			for (int i = 0; i < gumps.Count; ++i)
			{
				if (gumps[i] is RunebookGump)
				{
					RunebookGump gump = (RunebookGump)gumps[i];

					if (gump.Book == this)
						return true;
				}
			}

			return false;
		}

		public override bool DisplayLootType { get { return false; } }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Crafter != null)
				list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~

			if (m_Quality == RunebookQuality.Exceptional)
				list.Add(1060636); // exceptional

			if (m_Description != null && m_Description.Length > 0)
				list.Add(m_Description);
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_Description != null && m_Description.Length > 0)
				LabelTo(from, m_Description);

			ArrayList attrs = new ArrayList();

			if (m_Quality == RunebookQuality.Exceptional)
				attrs.Add(new EquipInfoAttribute(1018305 - (int)m_Quality));

			int number = LabelNumber;

			if (Crafter == null && attrs.Count == 0)
			{
				base.OnSingleClick(from);
				return;
			}

			EquipmentInfo eqInfo = new EquipmentInfo(number, m_Crafter, false, (EquipInfoAttribute[])attrs.ToArray(typeof(EquipInfoAttribute)));

			from.Send(new DisplayEquipmentInfo(this, eqInfo));
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.InRange(GetWorldLocation(), 1))
			{
				from.CloseGump(typeof(RunebookGump));
				from.SendGump(new RunebookGump(from, this));
			}
		}

		public bool CheckAccess(Mobile m)
		{
			if (!IsLockedDown || m.AccessLevel >= AccessLevel.GameMaster)
				return true;

			BaseHouse house = BaseHouse.FindHouseAt(this);

			if (house != null && house.IsAosRules && (house.Public ? house.IsBanned(m) : !house.HasAccess(m)))
				return false;

			return (house != null && house.HasSecureAccess(m, m_Level));
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (dropped is RecallRune)
			{
				//Close all runebooks
				from.CloseGump(typeof(RunebookGump));

				if (!CheckAccess(from))
				{
					from.SendLocalizedMessage(502413); // That cannot be done while the book is locked down.
				}
				//else if ( IsOpen( from ) )
				//{
				//	from.SendLocalizedMessage( 1005571 ); // You cannot place objects in the book while viewing the contents.
				//}
				else if (m_Entries.Count < 16)
				{
					RecallRune rune = (RecallRune)dropped;

					if (rune.Marked && rune.TargetMap != null)
					{
						m_Entries.Add(new RunebookEntry(rune.Target, rune.TargetMap, rune.Description, rune.House));

						dropped.Delete();

						from.Send(new PlaySound(0x42, GetWorldLocation()));

						string desc = rune.Description;

						if (desc == null || (desc = desc.Trim()).Length == 0)
							desc = "(indescript)";

						from.SendMessage(desc);

						return true;
					}
					else
					{
						from.SendLocalizedMessage(502409); // This rune does not have a marked location.
					}
				}
				else
				{
					from.SendLocalizedMessage(502401); // This runebook is full.
				}
			}
			else if (dropped is RecallScroll)
			{
				if (m_CurCharges < m_MaxCharges)
				{
					from.Send(new PlaySound(0x249, GetWorldLocation()));

					int amount = dropped.Amount;

					if (amount > (m_MaxCharges - m_CurCharges))
					{
						dropped.Consume(m_MaxCharges - m_CurCharges);
						m_CurCharges = m_MaxCharges;
					}
					else
					{
						m_CurCharges += amount;
						dropped.Delete();

						return true;
					}
				}
				else
				{
					from.SendLocalizedMessage(502410); // This book already has the maximum amount of charges.
				}
			}
			else
			{
				// Adam: anything other than a scroll will get dropped into your backpack
				// (so your best sword doesn't get dropped on the ground.)
				from.AddToBackpack(dropped);
				//	For richness, we add the drop sound of the item dropped.
				from.PlaySound(dropped.GetDropSound());
				return true;
			}

			return false;
		}

		#region ICraftable Members

		public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
		{
			int charges = 5 + quality + (int)(from.Skills[SkillName.Inscribe].Value / 30);

			if (charges > 10)
				charges = 10;

			MaxCharges = (Core.SE ? charges * 2 : charges);

			if (makersMark)
				Crafter = from;

			return quality;
		}

#if old
		else if (item is Runebook)
					{
						int charges = 5 + quality + (int)(from.Skills[SkillName.Inscribe].Value / 30);
						endquality = quality;

						if (charges > 10)
							charges = 10;

						((Runebook)item).MaxCharges = charges;

						if (makersMark)
							((Runebook)item).Crafter = from;

						((Runebook)item).Quality = (RunebookQuality)quality;
					}
#endif

		#endregion
	}

	public class RunebookEntry
	{
		private Point3D m_Location;
		private Map m_Map;
		private string m_Description;
		private BaseHouse m_House;

		public Point3D Location
		{
			get { return m_Location; }
			set { m_Location = value; }
		}

		public Map Map
		{
			get { return m_Map; }
		}

		public string Description
		{
			get { return m_Description; }
		}

		public BaseHouse House
		{
			get { return m_House; }
		}

		public RunebookEntry(Point3D loc, Map map, string desc, BaseHouse house)
		{
			m_Location = loc;
			m_Map = map;
			m_Description = desc;
			m_House = house;
		}

		public RunebookEntry(GenericReader reader)
		{
			int version = reader.ReadByte();

			switch (version)
			{
				case 1:
					{
						m_House = reader.ReadItem() as BaseHouse;
						goto case 0;
					}
				case 0:
					{
						m_Location = reader.ReadPoint3D();
						m_Map = reader.ReadMap();
						m_Description = reader.ReadString();

						break;
					}
			}
		}

		public void Serialize(GenericWriter writer)
		{
			if (m_House != null && !m_House.Deleted)
			{
				writer.Write((byte)1); // version

				writer.Write(m_House);
			}
			else
			{
				writer.Write((byte)0); // version
			}

			writer.Write(m_Location);
			writer.Write(m_Map);
			writer.Write(m_Description);
		}
	}
}