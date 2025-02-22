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

/* Scripts/Multis/SurveyTool.cs
 * ChangeLog:
 *	2/28/10. Adam
 *		the survey tool is only for looking, it can't actually move anything out of the way
 *		I was fooled into making the previous change since the code was moving already placed Items and Mobiles for the preview.
 *		I commented out ALL of that logic since the Survey tool is only a preview tool. (Unlike the placement tool which is available only on test)
 *	2/27/10, Adam
 *		When placing a house over a tent, we need to Annex the tent
 *		Annex = deed it all and place the proceeds into the owners bank
 *  07/03/07, plasma
 *      Allowed custom plot preview on production once more
 *  5/1/07, Adam
 *      Make custom house placement based on TestCenter.Enabled == true
 * 10/19/06, Adam
 *		Tower preview: not necessarily an exploit, but lets track it
 * 10/16/06, Adam
 *		Add flag to disable tower placement
 * 12/11/05, Kit
 *		Corrected secure/lockdown/lockbox limits.
 * 9/23/04, Lego Eater
 *		made survey tool from houseplacementtool.cs
 * 9/24/04, Lego Eater
 *		changed so gump isnt sent on failer to place.
 *		added send msg on succesfull place
 *		changed so gump isnt sent on succesfull place.
 *		changed display to show Lock Dowsn /Lock Boxes /Secures
 *		changed lock downs display to correct displaying.
 */

using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;
using Server.Misc;                      // test center

namespace Server.Items
{
	public class SurveyTool : Item
	{


		[Constructable]
		public SurveyTool()
			: base(0x14F6)
		{
			Name = "Survey Tool";
			Weight = 3.0;

		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
				from.SendGump(new SurveyToolCategoryGump(from));
			else
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
		}

		public SurveyTool(Serial serial)
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

			if (Weight == 0.0)
				Weight = 3.0;
		}
	}

	public class SurveyToolCategoryGump : Gump
	{
		private Mobile m_From;

		private const int LabelColor = 0x7FFF;
		private const int LabelColorDisabled = 0x4210;

		public SurveyToolCategoryGump(Mobile from)
			: base(50, 50)
		{
			m_From = from;

			from.CloseGump(typeof(SurveyToolCategoryGump));
			from.CloseGump(typeof(SurveyToolListGump));

			AddPage(0);

			AddBackground(0, 0, 270, 145, 5054);

			AddImageTiled(10, 10, 250, 125, 2624);
			AddAlphaRegion(10, 10, 250, 125);


			AddLabel(50, 10, 1153, "Survey Tool");

			AddButton(10, 110, 4017, 4019, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(45, 110, 150, 20, 3000363, LabelColor, false, false); // Close

			AddButton(10, 40, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(45, 40, 200, 20, 1060390, LabelColor, false, false); // Classic Houses

			AddButton(10, 60, 4005, 4007, 2, GumpButtonType.Reply, 0);
			AddHtmlLocalized(45, 60, 200, 20, 1060391, LabelColor, false, false); // 2-Story Customizable Houses

			AddButton(10, 80, 4005, 4007, 3, GumpButtonType.Reply, 0);
			AddHtmlLocalized(45, 80, 200, 20, 1060392, LabelColor, false, false); // 3-Story Customizable Houses

		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (!m_From.CheckAlive())
				return;

			switch (info.ButtonID)
			{
				case 1: // Classic Houses
					{
						m_From.SendGump(new SurveyToolListGump(m_From, SurveyToolEntry.ClassicHouses));
						break;
					}
				case 2: // 2-Story Customizable Houses
					{
						m_From.SendGump(new SurveyToolListGump(m_From, SurveyToolEntry.TwoStoryFoundations));
						break;
					}
				case 3: // 3-Story Customizable Houses
					{
						m_From.SendGump(new SurveyToolListGump(m_From, SurveyToolEntry.ThreeStoryFoundations));
						break;
					}
			}
		}
	}

	public class SurveyToolListGump : Gump
	{
		private Mobile m_From;
		private SurveyToolEntry[] m_Entries;

		private const int LabelColor = 0x7FFF;
		private const int LabelHue = 0x480;

		public SurveyToolListGump(Mobile from, SurveyToolEntry[] entries)
			: base(50, 50)
		{
			m_From = from;
			m_Entries = entries;

			from.CloseGump(typeof(SurveyToolCategoryGump));
			from.CloseGump(typeof(SurveyToolListGump));

			AddPage(0);

			AddBackground(0, 0, 520, 420, 5054);

			AddImageTiled(10, 10, 500, 20, 2624);
			AddAlphaRegion(10, 10, 500, 20);

			AddLabel(50, 10, 1153, "Survey Tool");

			AddImageTiled(10, 40, 500, 20, 2624);
			AddAlphaRegion(10, 40, 500, 20);

			AddHtmlLocalized(50, 40, 225, 20, 1060235, LabelColor, false, false); // House Description
			AddLabel(275, 40, 1153, "Lock Downs");
			AddLabel(350, 40, 1153, "Lock Boxes");
			AddLabel(425, 40, 1153, "Secures");
			//AddHtmlLocalized( 275, 40, 75, 20, 1060236, LabelColor, false, false ); // Storage
			//AddHtmlLocalized( 350, 40, 75, 20, 1060237, LabelColor, false, false ); // Lockdowns
			//AddHtmlLocalized( 425, 40, 75, 20, 1060034, LabelColor, false, false ); // Cost

			AddImageTiled(10, 70, 500, 280, 2624);
			AddAlphaRegion(10, 70, 500, 280);

			AddImageTiled(10, 360, 500, 20, 2624);
			AddAlphaRegion(10, 360, 500, 20);

			//AddHtmlLocalized( 10, 360, 250, 20, 1060645, LabelColor, false, false ); // Bank Balance:
			//AddLabel( 250, 360, LabelHue, Banker.GetBalance( from ).ToString() );

			AddImageTiled(10, 390, 500, 20, 2624);
			AddAlphaRegion(10, 390, 500, 20);

			AddButton(10, 390, 4017, 4019, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(50, 390, 100, 20, 3000363, LabelColor, false, false); // Close

			for (int i = 0; i < entries.Length; ++i)
			{
				int page = 1 + (i / 14);
				int index = i % 14;

				if (index == 0)
				{
					if (page > 1)
					{
						AddButton(450, 390, 4005, 4007, 0, GumpButtonType.Page, page);
						AddHtmlLocalized(400, 390, 100, 20, 3000406, LabelColor, false, false); // Next
					}

					AddPage(page);

					if (page > 1)
					{
						AddButton(200, 390, 4014, 4016, 0, GumpButtonType.Page, page - 1);
						AddHtmlLocalized(250, 390, 100, 20, 3000405, LabelColor, false, false); // Previous
					}
				}

				SurveyToolEntry entry = entries[i];

				int y = 70 + (index * 20);

				AddButton(10, y, 4005, 4007, 1 + i, GumpButtonType.Reply, 0);
				AddHtmlLocalized(50, y, 225, 20, entry.Description, LabelColor, false, false);
				AddLabel(275, y, LabelHue, entry.Storage.ToString());
				AddLabel(350, y, LabelHue, entry.Lockdowns.ToString());
				AddLabel(425, y, LabelHue, entry.Cost.ToString());
			}
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (!m_From.CheckAlive())
				return;

			int index = info.ButtonID - 1;

			if (index >= 0 && index < m_Entries.Length)
			{
				if (m_From.AccessLevel < AccessLevel.Player && BaseHouse.HasAccountHouse(m_From))
					m_From.SendLocalizedMessage(501271); // You already own a house, you may not place another!
				else
				{
					if (m_Entries[index].Type == typeof(Tower) && CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.TowerAllowed) == false)
					{
						m_From.SendMessage("Tower preview temporarily disabled");
					}
					else
					{	// not necessarily an exploit, but lets track it
						if (m_Entries[index].Type == typeof(Tower))
							LogHelper.TrackIt(m_From, "Note: Tower preview.", true);
						m_From.Target = new NewSurveyToolTarget(m_Entries, m_Entries[index]);
					}
				}
			}
			//else
			//{
			//m_From.SendGump( new SurveyToolCategoryGump( m_From ) );
			//}
		}
	}

	public class NewSurveyToolTarget : MultiTarget
	{
		private SurveyToolEntry m_Entry;
		private SurveyToolEntry[] m_Entries;

		private bool m_Placed;

		public NewSurveyToolTarget(SurveyToolEntry[] entries, SurveyToolEntry entry)
			: base(entry.MultiID, entry.Offset)
		{
			Range = 14;

			m_Entries = entries;
			m_Entry = entry;
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (!from.CheckAlive())
				return;

			IPoint3D ip = o as IPoint3D;

			if (ip != null)
			{
				if (ip is Item)
					ip = ((Item)ip).GetWorldTop();

				Point3D p = new Point3D(ip);

				Region reg = Region.Find(new Point3D(p), from.Map);

				if (from.AccessLevel >= AccessLevel.GameMaster || reg.AllowHousing(p))
					m_Placed = m_Entry.OnPlacement(from, p);
				else if (reg is TreasureRegion)
					from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
				else
					from.SendLocalizedMessage(501265); // Housing can not be created in this area.
			}
		}

		protected override void OnTargetFinish(Mobile from)
		{
			if (!from.CheckAlive())
				return;

			//if ( !m_Placed )
			//from.SendGump( new SurveyToolListGump( from, m_Entries ) );
		}
	}

	public class SurveyToolEntry
	{
		private Type m_Type;
		private int m_Description;
		private int m_Storage;
		private int m_Lockdowns;
		private int m_Cost;
		private int m_MultiID;
		private Point3D m_Offset;

		public Type Type { get { return m_Type; } }

		public int Description { get { return m_Description; } }
		public int Storage { get { return m_Storage; } }
		public int Lockdowns { get { return m_Lockdowns; } }
		public int Cost { get { return m_Cost; } }

		public int MultiID { get { return m_MultiID; } }
		public Point3D Offset { get { return m_Offset; } }

		public SurveyToolEntry(Type type, int description, int storage, int lockdowns, int cost, int xOffset, int yOffset, int zOffset, int multiID)
		{
			m_Type = type;
			m_Description = description;
			m_Storage = storage;
			m_Lockdowns = lockdowns;
			m_Cost = cost;

			m_Offset = new Point3D(xOffset, yOffset, zOffset);

			m_MultiID = multiID;
		}

		public BaseHouse ConstructHouse(Mobile from)
		{
			try
			{
				object[] args;

				if (m_Type == typeof(HouseFoundation))
					args = new object[4] { from, m_MultiID, m_Storage, m_Lockdowns };
				else if (m_Type == typeof(SmallOldHouse) || m_Type == typeof(SmallShop) || m_Type == typeof(TwoStoryHouse))
					args = new object[2] { from, m_MultiID };
				else
					args = new object[1] { from };

				return Activator.CreateInstance(m_Type, args) as BaseHouse;
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			return null;
		}

		public void PlacementWarn_Callback(Mobile from, bool okay, object state)
		{
			if (!from.CheckAlive())
				return;

			PreviewHouse prevHouse = (PreviewHouse)state;

			if (!okay)
			{
				prevHouse.Delete();
				return;
			}

			if (prevHouse.Deleted)
			{
				/* Too much time has passed and the test house you created has been deleted.
				 * Please try again!
				 */
				from.SendGump(new NoticeGump(1060637, 30720, 1060647, 32512, 320, 180, null, null));

				return;
			}

			Point3D center = prevHouse.Location;
			Map map = prevHouse.Map;

			prevHouse.Delete();

			ArrayList toMove;
			//Point3D center = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
			HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

			switch (res)
			{
				case HousePlacementResult.Valid:
					{
						if (from.AccessLevel < AccessLevel.Player && BaseHouse.HasAccountHouse(from))
						{
							from.SendLocalizedMessage(501271); // You already own a house, you may not place another!
						}
						else
						{
							BaseHouse house = ConstructHouse(from);

							if (house == null)
								return;

							//house.Price = m_Cost;

							//if ( Banker.Withdraw( from, m_Cost ) )
							//{
							//from.SendLocalizedMessage( 1060398, m_Cost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
							//}
							//else
							//{
							//house.RemoveKeys( from );
							//house.Delete();
							//from.SendLocalizedMessage( 1060646 ); // You do not have the funds available in your bank box to purchase this house.  Try placing a smaller house, or adding gold or checks to your bank box.
							//return;
							//}

							house.MoveToWorld(center, from.Map);

							/* Adam: the survey tool is only for looking, it can't actually move anything out of the way
							for ( int i = 0; i < toMove.Count; ++i )
							{
								object o = toMove[i];

								if (o is Tent)
									(o as Tent).Annex();
								else if (o is Mobile)
									((Mobile)o).Location = banLoc;
								else if ( o is Item )
									((Item)o).Location = banLoc;
							}*/
						}

						break;
					}
				case HousePlacementResult.BadItem:
				case HousePlacementResult.BadLand:
				case HousePlacementResult.BadStatic:
				case HousePlacementResult.BadRegionHidden:
				case HousePlacementResult.NoSurface:
					{
						from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
						break;
					}
				case HousePlacementResult.BadRegion:
					{
						from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
						break;
					}
				case HousePlacementResult.BadRegionTownship:
					{
						from.SendMessage("You are not authorized to build in this township.");
						break;
					}
			}
		}

		public bool OnPlacement(Mobile from, Point3D p)
		{
			if (!from.CheckAlive())
				return false;

			ArrayList toMove;
			Point3D center = new Point3D(p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z);
			HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

			switch (res)
			{
				case HousePlacementResult.Valid:
					{
						if (from.AccessLevel < AccessLevel.Player && BaseHouse.HasAccountHouse(from))
						{
							from.SendLocalizedMessage(501271); // You already own a house, you may not place another!
						}
						else
						{
							from.SendLocalizedMessage(1011576); // This is a valid location.

							PreviewHouse prev = new PreviewHouse(m_MultiID);

							MultiComponentList mcl = prev.Components;

							Point3D banLoc = new Point3D(center.X + mcl.Min.X, center.Y + mcl.Max.Y + 1, center.Z);

							for (int i = 0; i < mcl.List.Length; ++i)
							{
								MultiTileEntry entry = mcl.List[i];

								int itemID = entry.m_ItemID & 0x3FFF;

								if (itemID >= 0xBA3 && itemID <= 0xC0E)
								{
									banLoc = new Point3D(center.X + entry.m_OffsetX, center.Y + entry.m_OffsetY, center.Z);
									break;
								}
							}

							/* Adam: the survey tool is only for looking, it can't actually move anything out of the way
							for ( int i = 0; i < toMove.Count; ++i )
							{
								object o = toMove[i];

								if (o is Tent)
									(o as Tent).Annex();
								else if (o is Mobile)
									((Mobile)o).Location = banLoc;
								else if ( o is Item )
									((Item)o).Location = banLoc;
							}*/

							prev.MoveToWorld(center, from.Map);

							/* You are about to place a new house.
							 * Placing this house will condemn any and all of your other houses that you may have.
							 * All of your houses on all shards will be affected.
							 * 
							 * In addition, you will not be able to place another house or have one transferred to you for one (1) real-life week.
							 * 
							 * Once you accept these terms, these effects cannot be reversed.
							 * Re-deeding or transferring your new house will not uncondemn your other house(s) nor will the one week timer be removed.
							 * 
							 * If you are absolutely certain you wish to proceed, click the button next to OKAY below.
							 * If you do not wish to trade for this house, click CANCEL.
							 */
							from.SendMessage("This House Seems To fit Here");

							return true;
						}

						break;
					}
				case HousePlacementResult.BadItem:
				case HousePlacementResult.BadLand:
				case HousePlacementResult.BadStatic:
				case HousePlacementResult.BadRegionHidden:
				case HousePlacementResult.NoSurface:
					{
						from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
						break;
					}
				case HousePlacementResult.BadRegion:
					{
						from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
						break;
					}
				case HousePlacementResult.BadRegionTownship:
					{
						from.SendMessage("You are not authorized to build in this township.");
						break;
					}
			}

			return false;
		}

		private static Hashtable m_Table;

		static SurveyToolEntry()
		{
			m_Table = new Hashtable();

			FillTable(m_ClassicHouses);
			FillTable(m_TwoStoryFoundations);
			FillTable(m_ThreeStoryFoundations);
		}

		public static SurveyToolEntry Find(BaseHouse house)
		{
			object obj = m_Table[house.GetType()];

			if (obj is SurveyToolEntry)
			{
				return ((SurveyToolEntry)obj);
			}
			else if (obj is ArrayList)
			{
				ArrayList list = (ArrayList)obj;

				for (int i = 0; i < list.Count; ++i)
				{
					SurveyToolEntry e = (SurveyToolEntry)list[i];

					if (e.m_MultiID == (house.ItemID & 0x3FFF))
						return e;
				}
			}
			else if (obj is Hashtable)
			{
				Hashtable table = (Hashtable)obj;

				obj = table[house.ItemID & 0x3FFF];

				if (obj is SurveyToolEntry)
					return (SurveyToolEntry)obj;
			}

			return null;
		}

		private static void FillTable(SurveyToolEntry[] entries)
		{
			for (int i = 0; i < entries.Length; ++i)
			{
				SurveyToolEntry e = (SurveyToolEntry)entries[i];

				object obj = m_Table[e.m_Type];

				if (obj == null)
				{
					m_Table[e.m_Type] = e;
				}
				else if (obj is SurveyToolEntry)
				{
					ArrayList list = new ArrayList();

					list.Add(obj);
					list.Add(e);

					m_Table[e.m_Type] = list;
				}
				else if (obj is ArrayList)
				{
					ArrayList list = (ArrayList)obj;

					if (list.Count == 8)
					{
						Hashtable table = new Hashtable();

						for (int j = 0; j < list.Count; ++j)
							table[((SurveyToolEntry)list[j]).m_MultiID] = list[j];

						table[e.m_MultiID] = e;

						m_Table[e.m_Type] = table;
					}
					else
					{
						list.Add(e);
					}
				}
				else if (obj is Hashtable)
				{
					((Hashtable)obj)[e.m_MultiID] = e;
				}
			}
		}

		private static SurveyToolEntry[] m_ClassicHouses = new SurveyToolEntry[]
			{
																						
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011303,	270,	2,	2,		0,	4,	0,	0x0064	),
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011304,	270,	2,	2,		0,	4,	0,	0x0066	),
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011305,	270,	2,	2,		0,	4,	0,	0x0068	),
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011306,	270,	2,	2,		0,	4,	0,	0x006A	),
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011307,	270,	2,	2,		0,	4,	0,	0x006C	),
				new SurveyToolEntry( typeof( SmallOldHouse ),		1011308,	270,	2,	2,		0,	4,	0,	0x006E	),
				new SurveyToolEntry( typeof( SmallShop ),			1011321,	275,	2,	2,	   -1,	4,	0,	0x00A0	),
				new SurveyToolEntry( typeof( SmallShop ),			1011322,	275,	2,	2,		0,	4,	0,	0x00A2	),
				new SurveyToolEntry( typeof( SmallTower ),			1011317,	300,	2,	2,		3,	4,	0,	0x0098	),
				new SurveyToolEntry( typeof( TwoStoryVilla ),		1011319,	600,	2,	4,		3,	6,	0,	0x009E	),
				new SurveyToolEntry( typeof( SandStonePatio ),		1011320,	450,	2,	3,	   -1,	4,	0,	0x009C	),
				new SurveyToolEntry( typeof( LogCabin ),			1011318,	600,	2,	4,		1,	6,	0,	0x009A	),
				new SurveyToolEntry( typeof( GuildHouse ),			1011309,	600,	2,	4,	   -1,	7,	0,	0x0074	),
				new SurveyToolEntry( typeof( TwoStoryHouse ),		1011310,	750,	3,	5,	   -3,	7,	0,	0x0076	),
				new SurveyToolEntry( typeof( TwoStoryHouse ),		1011311,	750,	3,	5,	   -3,	7,	0,	0x0078	),
				new SurveyToolEntry( typeof( LargePatioHouse ),		1011315,	600,	2,	4,	   -4,	7,	0,	0x008C	),
				new SurveyToolEntry( typeof( LargeMarbleHouse ),	1011316,	750,	3,	5,	   -4,	7,	0,	0x0096	),
				new SurveyToolEntry( typeof( Tower ),				1011312,	1150,	4,	8,		0,	7,	0,	0x007A	),
				new SurveyToolEntry( typeof( Keep ),				1011313,	1300,	5,	9,		0, 11,	0,	0x007C	),
				new SurveyToolEntry( typeof( Castle ),				1011314,	1950,	7,	14,		0, 16,	0,	0x007E	)
			};

		public static SurveyToolEntry[] ClassicHouses { get { return m_ClassicHouses; } }



		private static SurveyToolEntry[] m_TwoStoryFoundations = new SurveyToolEntry[]
			{
				new SurveyToolEntry( typeof( HouseFoundation ),		1060241,	425,	212,	30500,		0,	4,	0,	0x13EC	), // 7x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060242,	580,	290,	34500,		0,	5,	0,	0x13ED	), // 7x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060243,	650,	325,	38500,		0,	5,	0,	0x13EE	), // 7x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060244,	700,	350,	42500,		0,	6,	0,	0x13EF	), // 7x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060245,	750,	375,	46500,		0,	6,	0,	0x13F0	), // 7x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060246,	800,	400,	50500,		0,	7,	0,	0x13F1	), // 7x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060253,	580,	290,	34500,		0,	4,	0,	0x13F8	), // 8x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060254,	650,	325,	39000,		0,	5,	0,	0x13F9	), // 8x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060255,	700,	350,	43500,		0,	5,	0,	0x13FA	), // 8x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060256,	750,	375,	48000,		0,	6,	0,	0x13FB	), // 8x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060257,	800,	400,	52500,		0,	6,	0,	0x13FC	), // 8x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060258,	850,	425,	57000,		0,	7,	0,	0x13FD	), // 8x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060259,	1100,	550,	61500,		0,	7,	0,	0x13FE	), // 8x13 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060265,	650,	325,	38500,		0,	4,	0,	0x1404	), // 9x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060266,	700,	350,	43500,		0,	5,	0,	0x1405	), // 9x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060267,	750,	375,	48500,		0,	5,	0,	0x1406	), // 9x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060268,	800,	400,	53500,		0,	6,	0,	0x1407	), // 9x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060269,	850,	425,	58500,		0,	6,	0,	0x1408	), // 9x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060270,	1100,	550,	63500,		0,	7,	0,	0x1409	), // 9x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060271,	1100,	550,	68500,		0,	7,	0,	0x140A	), // 9x13 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060277,	700,	350,	42500,		0,	4,	0,	0x1410	), // 10x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060278,	750,	375,	48000,		0,	5,	0,	0x1411	), // 10x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060279,	800,	400,	53500,		0,	5,	0,	0x1412	), // 10x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060280,	850,	425,	59000,		0,	6,	0,	0x1413	), // 10x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060281,	1100,	550,	64500,		0,	6,	0,	0x1414	), // 10x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060282,	1100,	550,	70000,		0,	7,	0,	0x1415	), // 10x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060283,	1150,	575,	75500,		0,	7,	0,	0x1416	), // 10x13 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060289,	750,	375,	46500,		0,	4,	0,	0x141C	), // 11x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060290,	800,	400,	52500,		0,	5,	0,	0x141D	), // 11x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060291,	850,	425,	58500,		0,	5,	0,	0x141E	), // 11x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060292,	1100,	550,	64500,		0,	6,	0,	0x141F	), // 11x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060293,	1100,	550,	70500,		0,	6,	0,	0x1420	), // 11x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060294,	1150,	575,	76500,		0,	7,	0,	0x1421	), // 11x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060295,	1200,	600,	82500,		0,	7,	0,	0x1422	), // 11x13 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060301,	800,	400,	50500,		0,	4,	0,	0x1428	), // 12x7 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060302,	850,	425,	57000,		0,	5,	0,	0x1429	), // 12x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060303,	1100,	550,	63500,		0,	5,	0,	0x142A	), // 12x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060304,	1100,	550,	70000,		0,	6,	0,	0x142B	), // 12x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060305,	1150,	575,	76500,		0,	6,	0,	0x142C	), // 12x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060306,	1200,	600,	83000,		0,	7,	0,	0x142D	), // 12x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060307,	1250,	625,	89500,		0,	7,	0,	0x142E	), // 12x13 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060314,	1100,	550,	61500,		0,	5,	0,	0x1435	), // 13x8 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060315,	1100,	550,	68500,		0,	5,	0,	0x1436	), // 13x9 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060316,	1150,	575,	75500,		0,	6,	0,	0x1437	), // 13x10 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060317,	1200,	600,	82500,		0,	6,	0,	0x1438	), // 13x11 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060318,	1250,	625,	89500,		0,	7,	0,	0x1439	), // 13x12 2-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060319,	1300,	650,	96500,		0,	7,	0,	0x143A	)  // 13x13 2-Story Customizable House
			};

		public static SurveyToolEntry[] TwoStoryFoundations { get { return m_TwoStoryFoundations; } }



		private static SurveyToolEntry[] m_ThreeStoryFoundations = new SurveyToolEntry[]
			{
				new SurveyToolEntry( typeof( HouseFoundation ),		1060272,	1150,	575,	73500,		0,	8,	0,	0x140B	), // 9x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060284,	1200,	600,	81000,		0,	8,	0,	0x1417	), // 10x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060285,	1250,	625,	86500,		0,	8,	0,	0x1418	), // 10x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060296,	1250,	625,	88500,		0,	8,	0,	0x1423	), // 11x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060297,	1300,	650,	94500,		0,	8,	0,	0x1424	), // 11x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060298,	1350,	675,	100500,		0,	9,	0,	0x1425	), // 11x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060308,	1300,	650,	96000,		0,	8,	0,	0x142F	), // 12x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060309,	1350,	675,	102500,		0,	8,	0,	0x1430	), // 12x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060310,	1370,	685,	109000,		0,	9,	0,	0x1431	), // 12x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060311,	1370,	685,	115500,		0,	9,	0,	0x1432	), // 12x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060320,	1350,	675,	103500,		0,	8,	0,	0x143B	), // 13x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060321,	1370,	685,	110500,		0,	8,	0,	0x143C	), // 13x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060322,	1370,	685,	117500,		0,	9,	0,	0x143D	), // 13x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060323,	2119,	1059,	124500,		0,	9,	0,	0x143E	), // 13x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060324,	2119,	1059,	131500,		0,	10,	0,	0x143F	), // 13x18 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060327,	1150,	575,	73500,		0,	5,	0,	0x1442	), // 14x9 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060328,	1200,	600,	81000,		0,	6,	0,	0x1443	), // 14x10 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060329,	1250,	625,	88500,		0,	6,	0,	0x1444	), // 14x11 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060330,	1300,	650,	96000,		0,	7,	0,	0x1445	), // 14x12 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060331,	1350,	675,	103500,		0,	7,	0,	0x1446	), // 14x13 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060332,	1370,	685,	111000,		0,	8,	0,	0x1447	), // 14x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060333,	1370,	685,	118500,		0,	8,	0,	0x1448	), // 14x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060334,	2119,	1059,	126000,		0,	9,	0,	0x1449	), // 14x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060335,	2119,	1059,	133500,		0,	9,	0,	0x144A	), // 14x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060336,	2119,	1059,	141000,		0,	10,	0,	0x144B	), // 14x18 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060340,	1250,	625,	86500,		0,	6,	0,	0x144F	), // 15x10 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060341,	1300,	650,	94500,		0,	6,	0,	0x1450	), // 15x11 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060342,	1350,	675,	102500,		0,	7,	0,	0x1451	), // 15x12 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060343,	1370,	685,	110500,		0,	7,	0,	0x1452	), // 15x13 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060344,	1370,	685,	118500,		0,	8,	0,	0x1453	), // 15x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060345,	2119,	1059,	126500,		0,	8,	0,	0x1454	), // 15x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060346,	2119,	1059,	134500,		0,	9,	0,	0x1455	), // 15x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060347,	2119,	1059,	142500,		0,	9,	0,	0x1456	), // 15x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060348,	2119,	1059,	150500,		0,	10,	0,	0x1457	), // 15x18 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060353,	1350,	675,	100500,		0,	6,	0,	0x145C	), // 16x11 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060354,	1370,	685,	109000,		0,	7,	0,	0x145D	), // 16x12 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060355,	1370,	685,	117500,		0,	7,	0,	0x145E	), // 16x13 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060356,	2119,	1059,	126000,		0,	8,	0,	0x145F	), // 16x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060357,	2119,	1059,	134500,		0,	8,	0,	0x1460	), // 16x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060358,	2119,	1059,	143000,		0,	9,	0,	0x1461	), // 16x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060359,	2119,	1059,	151500,		0,	9,	0,	0x1462	), // 16x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060360,	2119,	1059,	160000,		0,	10,	0,	0x1463	), // 16x18 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060366,	1370,	685,	115500,		0,	7,	0,	0x1469	), // 17x12 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060367,	2119,	1059,	124500,		0,	7,	0,	0x146A	), // 17x13 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060368,	2119,	1059,	133500,		0,	8,	0,	0x146B	), // 17x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060369,	2119,	1059,	142500,		0,	8,	0,	0x146C	), // 17x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060370,	2119,	1059,	151500,		0,	9,	0,	0x146D	), // 17x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060371,	2119,	1059,	160500,		0,	9,	0,	0x146E	), // 17x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060372,	2119,	1059,	169500,		0,	10,	0,	0x146F	), // 17x18 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060379,	2119,	1059,	131500,		0,	7,	0,	0x1476	), // 18x13 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060380,	2119,	1059,	141000,		0,	8,	0,	0x1477	), // 18x14 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060381,	2119,	1059,	150500,		0,	8,	0,	0x1478	), // 18x15 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060382,	2119,	1059,	160000,		0,	9,	0,	0x1479	), // 18x16 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060383,	2119,	1059,	169500,		0,	9,	0,	0x147A	), // 18x17 3-Story Customizable House
				new SurveyToolEntry( typeof( HouseFoundation ),		1060384,	2119,	1059,	179000,		0,	10,	0,	0x147B	)  // 18x18 3-Story Customizable House
			};

		public static SurveyToolEntry[] ThreeStoryFoundations { get { return m_ThreeStoryFoundations; } }
	}
}