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

/* Scripts/Commands/Decorate.cs
 * CHANGELOG
 *	2/23/11, adam
 *		redesigned based on RunUO 2.0
 *		o) Make a 'wipe' pass first so as to clear any incorrect items. For instance, AI used LibraryBookcases instead of FullBookcases.
 *		o) now deletes N items from a destination as pervious Decorate commands would just shove the new item there on top of the old item
 *		o) explicitly 'skip' trammel so we can compare old (AI 1.0) and new deco
 *	3/18/07, Pix
 *		Commented out all Decorate_OnCommand references to Trammel, Malas, Ilshenar.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Quests.Haven;
using Server.Engines.Quests.Necro;
using System.Collections.Generic;

namespace Server.Commands
{
	public class Decorate
	{
		public enum DecoMode
		{
			add,
			delete,
			skip
		}

		public static void Initialize()
		{
			CommandSystem.Register("Decorate", AccessLevel.Administrator, new CommandEventHandler(Decorate_OnCommand));
		}

		[Usage("Decorate")]
		[Description("Generates world decoration.")]
		private static void Decorate_OnCommand(CommandEventArgs e)
		{
			m_Mobile = e.Mobile;
			m_Count = 0;

			m_Mobile.SendMessage("Generating world decoration, please wait.");

			// first wipe existing deco.
			//	we do this to update deco that may be wrong (librarybookcase vs fullbookcase)
			//	or has the wrong attributes (LightCircle for example.)
			Generate("Data/Decoration/Britannia", DecoMode.skip, Map.Trammel);					// skip trammel deco so we can compare to new felucca
			Generate("Data/Decoration/Britannia", DecoMode.delete, Map.Felucca);
			Generate("Data/Decoration/Trammel", DecoMode.skip, Map.Trammel);					// skip trammel deco so we can compare to new felucca
			Generate("Data/Decoration/Felucca", DecoMode.delete, Map.Felucca);
			Generate("Data/Decoration/Ilshenar", DecoMode.delete, Map.Ilshenar);
			Generate("Data/Decoration/Malas", DecoMode.delete, Map.Malas);
			Generate("Data/Decoration/Tokuno", DecoMode.delete, Map.Tokuno);

			m_Mobile.SendMessage("World generating complete. {0} items were wiped.", m_Count);
			m_Count = 0;

			Generate("Data/Decoration/Britannia", DecoMode.skip, Map.Trammel);					// skip trammel deco so we can compare to new felucca
			Generate("Data/Decoration/Britannia", DecoMode.add, Map.Felucca);
			Generate("Data/Decoration/Trammel", DecoMode.skip, Map.Trammel);					// skip trammel deco so we can compare to new felucca
			Generate("Data/Decoration/Felucca", DecoMode.add, Map.Felucca);
			Generate("Data/Decoration/Ilshenar", DecoMode.add, Map.Ilshenar);					// sure deco this, we sometimes use these areas
			Generate("Data/Decoration/Malas", DecoMode.skip, Map.Malas);						// not at this time (waste of items)
			Generate("Data/Decoration/Tokuno", DecoMode.skip, Map.Tokuno);						// not at this time (waste of items)

			m_Mobile.SendMessage("World generating complete. {0} items were generated.", m_Count);
		}

		public static void Generate(string folder, DecoMode mode, params Map[] maps)
		{
			if (!Directory.Exists(folder))
				return;

			string[] files = Directory.GetFiles(folder, "*.cfg");

			for (int i = 0; i < files.Length; ++i)
			{
				ArrayList list = DecorationList.ReadAll(files[i]);

				for (int j = 0; j < list.Count; ++j)
					m_Count += ((DecorationList)list[j]).Generate(mode, maps);
			}
		}

		private static Mobile m_Mobile;
		private static int m_Count;
	}

	public class DecorationList
	{
		private Type m_Type;
		private int m_ItemID;
		private string[] m_Params;
		private ArrayList m_Entries;

		public DecorationList()
		{
		}

		private static Type typeofStatic = typeof(Static);
		private static Type typeofLocalizedStatic = typeof(LocalizedStatic);
		private static Type typeofBaseDoor = typeof(BaseDoor);
		private static Type typeofAnkhWest = typeof(AnkhWest);
		private static Type typeofAnkhNorth = typeof(AnkhNorth);
		private static Type typeofBeverage = typeof(BaseBeverage);
		private static Type typeofLocalizedSign = typeof(LocalizedSign);
		private static Type typeofMarkContainer = typeof(MarkContainer);
		private static Type typeofWarningItem = typeof(WarningItem);
		private static Type typeofHintItem = typeof(HintItem);
		private static Type typeofCannon = typeof(Cannon);
		private static Type typeofSerpentPillar = typeof(SerpentPillar);

		public Item Construct()
		{
			Item item;

			try
			{
				if (m_Type == typeofStatic)
				{
					item = new Static(m_ItemID);
				}
				else if (m_Type == typeofLocalizedStatic)
				{
					int labelNumber = 0;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("LabelNumber"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
							{
								labelNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
								break;
							}
						}
					}

					item = new LocalizedStatic(m_ItemID, labelNumber);
				}
				else if (m_Type == typeofLocalizedSign)
				{
					int labelNumber = 0;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("LabelNumber"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
							{
								labelNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
								break;
							}
						}
					}

					item = new LocalizedSign(m_ItemID, labelNumber);
				}
				else if (m_Type == typeofAnkhWest || m_Type == typeofAnkhNorth)
				{
					bool bloodied = false;

					for (int i = 0; !bloodied && i < m_Params.Length; ++i)
						bloodied = (m_Params[i] == "Bloodied");

					if (m_Type == typeofAnkhWest)
						item = new AnkhWest(bloodied);
					else
						item = new AnkhNorth(bloodied);
				}
				else if (m_Type == typeofMarkContainer)
				{
					bool bone = false;
					bool locked = false;
					Map map = Map.Malas;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i] == "Bone")
						{
							bone = true;
						}
						else if (m_Params[i] == "Locked")
						{
							locked = true;
						}
						else if (m_Params[i].StartsWith("TargetMap"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								map = Map.Parse(m_Params[i].Substring(++indexOf));
						}
					}

					MarkContainer mc = new MarkContainer(bone, locked);

					mc.TargetMap = map;
					mc.Description = "strange location";

					item = mc;
				}
				else if (m_Type == typeofHintItem)
				{
					int range = 0;
					int messageNumber = 0;
					string messageString = null;
					int hintNumber = 0;
					string hintString = null;
					TimeSpan resetDelay = TimeSpan.Zero;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("Range"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("WarningString"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								messageString = m_Params[i].Substring(++indexOf);
						}
						else if (m_Params[i].StartsWith("WarningNumber"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								messageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("HintString"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								hintString = m_Params[i].Substring(++indexOf);
						}
						else if (m_Params[i].StartsWith("HintNumber"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								hintNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("ResetDelay"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								resetDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
						}
					}

					HintItem hi = new HintItem(m_ItemID, range, messageNumber, hintNumber);

					hi.WarningString = messageString;
					hi.HintString = hintString;
					hi.ResetDelay = resetDelay;

					item = hi;
				}
				else if (m_Type == typeofWarningItem)
				{
					int range = 0;
					int messageNumber = 0;
					string messageString = null;
					TimeSpan resetDelay = TimeSpan.Zero;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("Range"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("WarningString"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								messageString = m_Params[i].Substring(++indexOf);
						}
						else if (m_Params[i].StartsWith("WarningNumber"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								messageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("ResetDelay"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								resetDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
						}
					}

					WarningItem wi = new WarningItem(m_ItemID, range, messageNumber);

					wi.WarningString = messageString;
					wi.ResetDelay = resetDelay;

					item = wi;
				}
				else if (m_Type == typeofCannon)
				{
					CannonDirection direction = CannonDirection.North;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("CannonDirection"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								direction = (CannonDirection)Enum.Parse(typeof(CannonDirection), m_Params[i].Substring(++indexOf), true);
						}
					}

					item = new Cannon(direction);
				}
				else if (m_Type == typeofSerpentPillar)
				{
					string word = null;
					Rectangle2D destination = new Rectangle2D();

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("Word"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								word = m_Params[i].Substring(++indexOf);
						}
						else if (m_Params[i].StartsWith("DestStart"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								destination.Start = Point2D.Parse(m_Params[i].Substring(++indexOf));
						}
						else if (m_Params[i].StartsWith("DestEnd"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								destination.End = Point2D.Parse(m_Params[i].Substring(++indexOf));
						}
					}

					item = new SerpentPillar(word, destination);
				}
				else if (m_Type.IsSubclassOf(typeofBeverage))
				{
					BeverageType content = BeverageType.Liquor;
					bool fill = false;

					for (int i = 0; !fill && i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("Content"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
							{
								content = (BeverageType)Enum.Parse(typeof(BeverageType), m_Params[i].Substring(++indexOf), true);
								fill = true;
							}
						}
					}

					if (fill)
						item = (Item)Activator.CreateInstance(m_Type, new object[] { content });
					else
						item = (Item)Activator.CreateInstance(m_Type);
				}
				else if (m_Type.IsSubclassOf(typeofBaseDoor))
				{
					DoorFacing facing = DoorFacing.WestCW;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("Facing"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
							{
								facing = (DoorFacing)Enum.Parse(typeof(DoorFacing), m_Params[i].Substring(++indexOf), true);
								break;
							}
						}
					}

					item = (Item)Activator.CreateInstance(m_Type, new object[] { facing });
				}
				else
				{
					item = (Item)Activator.CreateInstance(m_Type);
				}
			}
			catch (Exception e)
			{
				throw new Exception(String.Format("Bad type: {0}", m_Type), e);
			}

			if (item is BaseAddon)
			{
				if (item is MaabusCoffin)
				{
					MaabusCoffin coffin = (MaabusCoffin)item;

					for (int i = 0; i < m_Params.Length; ++i)
					{
						if (m_Params[i].StartsWith("SpawnLocation"))
						{
							int indexOf = m_Params[i].IndexOf('=');

							if (indexOf >= 0)
								coffin.SpawnLocation = Point3D.Parse(m_Params[i].Substring(++indexOf));
						}
					}
				}
				else if (m_ItemID > 0)
				{
					ArrayList comps = ((BaseAddon)item).Components;

					for (int i = 0; i < comps.Count; ++i)
					{
						AddonComponent comp = comps[i] as AddonComponent;

						if (comp == null)
							continue;

						if (comp.Offset == Point3D.Zero)
							comp.ItemID = m_ItemID;
					}
				}
			}
			else if (item is BaseLight)
			{
				bool unlit = false, unprotected = false;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (!unlit && m_Params[i] == "Unlit")
						unlit = true;
					else if (!unprotected && m_Params[i] == "Unprotected")
						unprotected = true;

					if (unlit && unprotected)
						break;
				}

				if (!unlit)
					((BaseLight)item).Ignite();
				if (!unprotected)
					((BaseLight)item).Protected = true;

				if (m_ItemID > 0)
					item.ItemID = m_ItemID;
			}
			else if (item is Server.Mobiles.Spawner)
			{
				Server.Mobiles.Spawner sp = (Server.Mobiles.Spawner)item;

				sp.NextSpawn = TimeSpan.Zero;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("Spawn"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.ObjectNames.Add(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MinDelay"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.MinDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MaxDelay"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.MaxDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("NextSpawn"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.NextSpawn = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Count"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.Count = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Team"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.Team = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("HomeRange"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.HomeRange = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Running"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.Running = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Group"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							sp.Group = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
				}
			}
			else if (item is RecallRune)
			{
				RecallRune rune = (RecallRune)item;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("Description"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							rune.Description = m_Params[i].Substring(++indexOf);
					}
					else if (m_Params[i].StartsWith("Marked"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							rune.Marked = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("TargetMap"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							rune.TargetMap = Map.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Target"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							rune.Target = Point3D.Parse(m_Params[i].Substring(++indexOf));
					}
				}
			}
			else if (item is SkillTeleporter)
			{
				SkillTeleporter tp = (SkillTeleporter)item;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("Skill"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Skill = (SkillName)Enum.Parse(typeof(SkillName), m_Params[i].Substring(++indexOf), true);
					}
					else if (m_Params[i].StartsWith("RequiredFixedPoint"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Required = Utility.ToInt32(m_Params[i].Substring(++indexOf)) * 0.01;
					}
					else if (m_Params[i].StartsWith("Required"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Required = Utility.ToDouble(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MessageString"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.MessageString = m_Params[i].Substring(++indexOf);
					}
					else if (m_Params[i].StartsWith("MessageNumber"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.MessageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("PointDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MapDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Creatures"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SourceEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("DestEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SoundID"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Delay"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
				}

				if (m_ItemID > 0)
					item.ItemID = m_ItemID;
			}
			else if (item is KeywordTeleporter)
			{
				KeywordTeleporter tp = (KeywordTeleporter)item;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("Substring"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Substring = m_Params[i].Substring(++indexOf);
					}
					else if (m_Params[i].StartsWith("Keyword"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Keyword = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Range"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("PointDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MapDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Creatures"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SourceEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("DestEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SoundID"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Delay"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
				}

				if (m_ItemID > 0)
					item.ItemID = m_ItemID;
			}
			else if (item is Teleporter)
			{
				Teleporter tp = (Teleporter)item;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("PointDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("MapDest"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Creatures"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SourceEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("DestEffect"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("SoundID"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
					}
					else if (m_Params[i].StartsWith("Delay"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
					}
				}

				if (m_ItemID > 0)
					item.ItemID = m_ItemID;
			}
			else if (item is FillableContainer)
			{
				FillableContainer cont = (FillableContainer)item;

				for (int i = 0; i < m_Params.Length; ++i)
				{
					if (m_Params[i].StartsWith("ContentType"))
					{
						int indexOf = m_Params[i].IndexOf('=');

						if (indexOf >= 0)
							cont.ContentType = (FillableContentType)Enum.Parse(typeof(FillableContentType), m_Params[i].Substring(++indexOf), true);
					}
				}

				if (m_ItemID > 0)
					item.ItemID = m_ItemID;
			}
			else if (m_ItemID > 0)
			{
				item.ItemID = m_ItemID;
			}

			item.Movable = false;

			for (int i = 0; i < m_Params.Length; ++i)
			{
				if (m_Params[i].StartsWith("Light"))
				{
					int indexOf = m_Params[i].IndexOf('=');

					if (indexOf >= 0)
						item.Light = (LightType)Enum.Parse(typeof(LightType), m_Params[i].Substring(++indexOf), true);
				}
				else if (m_Params[i].StartsWith("Hue"))
				{
					int indexOf = m_Params[i].IndexOf('=');

					if (indexOf >= 0)
					{
						int hue = Utility.ToInt32(m_Params[i].Substring(++indexOf));

						if (item is DyeTub)
							((DyeTub)item).DyedHue = hue;
						else
							item.Hue = hue;
					}
				}
				else if (m_Params[i].StartsWith("Name"))
				{
					int indexOf = m_Params[i].IndexOf('=');

					if (indexOf >= 0)
						item.Name = m_Params[i].Substring(++indexOf);
				}
				else if (m_Params[i].StartsWith("Amount"))
				{
					int indexOf = m_Params[i].IndexOf('=');

					if (indexOf >= 0)
					{
						// Must supress stackable warnings

						bool wasStackable = item.Stackable;

						item.Stackable = true;
						item.Amount = Utility.ToInt32(m_Params[i].Substring(++indexOf));
						item.Stackable = wasStackable;
					}
				}
			}

			return item;
		}

		private static Queue m_DeleteQueue = new Queue();

		private static Item FindItem(int x, int y, int z, Map map)
		{
			IPooledEnumerable eable;
			eable = map.GetItemsInRange(new Point3D(x, y, z), 0);
			Item found = null;
			foreach (Item item in eable)
			{
				if (item == null || item.Deleted)
					continue;

				// yeah, GetItemsInRange() does not respect exact Z, not sure why
				if (item.Z != z)
					continue;

				found = item;
				break;
			}
			eable.Free();

			return found;
		}

		private static bool FindItem(int x, int y, int z, Map map, Item srcItem)
		{
			int itemID = srcItem.ItemID;

			bool res = false;

			IPooledEnumerable eable;

			if (srcItem is BaseDoor)
			{
				eable = map.GetItemsInRange(new Point3D(x, y, z), 1);

				foreach (Item item in eable)
				{
					if (!(item is BaseDoor))
						continue;

					BaseDoor bd = (BaseDoor)item;
					Point3D p;
					int bdItemID;

					if (bd.Open)
					{
						p = new Point3D(bd.X - bd.Offset.X, bd.Y - bd.Offset.Y, bd.Z - bd.Offset.Z);
						bdItemID = bd.ClosedID;
					}
					else
					{
						p = bd.Location;
						bdItemID = bd.ItemID;
					}

					if (p.X != x || p.Y != y)
						continue;

					if (item.Z == z && bdItemID == itemID)
						res = true;
					else if (Math.Abs(item.Z - z) < 8)
						m_DeleteQueue.Enqueue(item);
				}
			}
			else if ((TileData.ItemTable[itemID & 0x3FFF].Flags & TileFlag.LightSource) != 0)
			{
				eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

				LightType lt = srcItem.Light;
				string srcName = srcItem.ItemData.Name;

				foreach (Item item in eable)
				{
					if (item.Z == z)
					{
						if (item.ItemID == itemID)
						{
							if (item.Light != lt)
								m_DeleteQueue.Enqueue(item);
							else
								res = true;
						}
						else if ((item.ItemData.Flags & TileFlag.LightSource) != 0 && item.ItemData.Name == srcName)
						{
							m_DeleteQueue.Enqueue(item);
						}
					}
				}
			}
			else if (srcItem is Teleporter || srcItem is FillableContainer || srcItem is BaseBook)
			{
				eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

				Type type = srcItem.GetType();

				foreach (Item item in eable)
				{
					if (item.Z == z && item.ItemID == itemID)
					{
						if (item.GetType() != type)
							m_DeleteQueue.Enqueue(item);
						else
							res = true;
					}
				}
			}
			else
			{
				eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

				foreach (Item item in eable)
				{
					if (item.Z == z && item.ItemID == itemID)
					{
						eable.Free();
						return true;
					}
				}
			}

			eable.Free();

			while (m_DeleteQueue.Count > 0)
				((Item)m_DeleteQueue.Dequeue()).Delete();

			return res;
		}

		public int Generate(Decorate.DecoMode mode, Map[] maps)
		{
			int count = 0;
			Item item = null;
			LogHelper log = null;

			if (mode != Decorate.DecoMode.skip)
				log = new LogHelper(mode == Decorate.DecoMode.delete ? "deco_delete.txt" : "deco_add.txt", false, true);

			for (int i = 0; i < m_Entries.Count; ++i)
			{
				DecorationEntry entry = (DecorationEntry)m_Entries[i];
				Point3D loc = entry.Location;
				string extra = entry.Extra;

				for (int j = 0; j < maps.Length; ++j)
				{
					// some maps are not loaded - like tokuno
					if (maps[j] == null)
						continue;

					if (item == null)
						item = Construct();

					if (item == null)
						continue;

					if (mode == Decorate.DecoMode.delete)
					{
						Item temp;
						// smart wipe whatever is there:
						while ((temp = FindItem(loc.X, loc.Y, loc.Z, maps[j])) != null)
						{
							if (temp.ItemID != item.ItemID)
							{
								string sx = String.Format("Deleting a {0}: ID {3} at {1} in {2}", temp, temp.Location, maps[j], temp.ItemID);
								Console.WriteLine(sx);
								log.Log(sx);
								temp.Delete();
							}
							else
							{	// okay to replace items since they may have corrected lighting or other attributes
								string sx = String.Format("Replaceing a {0}: ID {3} at {1} in {2}", temp, temp.Location, maps[j], temp.ItemID);
								Console.WriteLine(sx);
								log.Log(sx);
								sx = String.Format("  With a {0}: ID {3} at {1} in {2}", item, temp.Location, maps[j], item.ItemID);
								Console.WriteLine(sx);
								log.Log(sx);
								temp.Delete();
							}
						}
					}
					else if (mode == Decorate.DecoMode.skip)
					{	// do nothing

						if (item != null)
							item.Delete();
						item = null;
					}
					else if (mode == Decorate.DecoMode.add)
					{
						if (FindItem(loc.X, loc.Y, loc.Z, maps[j], item))
						{
							Console.WriteLine("Should never happen since we wiped the items first!");
							string sx = String.Format("Collide: {0}: ID {3} already exists at {1} in {2}", item, loc, maps[j], item.ItemID);
							Console.WriteLine(sx);
							log.Log(sx);
						}
						else
						{

							string sx = String.Format("Adding: {0}: ID {3} at {1} in {2}", item, loc, maps[j], item.ItemID);
							Console.WriteLine(sx);
							log.Log(sx);

							item.MoveToWorld(loc, maps[j]);
							++count;

							if (item is BaseDoor)
							{
								IPooledEnumerable eable = maps[j].GetItemsInRange(loc, 1);

								Type itemType = item.GetType();

								foreach (Item link in eable)
								{
									if (link != item && link.Z == item.Z && link.GetType() == itemType)
									{
										((BaseDoor)item).Link = (BaseDoor)link;
										((BaseDoor)link).Link = (BaseDoor)item;
										break;
									}
								}

								eable.Free();
							}
							else if (item is MarkContainer)
							{
								try { ((MarkContainer)item).Target = Point3D.Parse(extra); }
								catch { }
							}

							item = null;
						}
					}
				}
			}

			if (item != null)
				item.Delete();

			if (log != null)
				log.Finish();

			return count;
		}

		public static ArrayList ReadAll(string path)
		{
			using (StreamReader ip = new StreamReader(path))
			{
				ArrayList list = new ArrayList();

				for (DecorationList v = Read(ip); v != null; v = Read(ip))
					list.Add(v);

				return list;
			}
		}

		private static string[] m_EmptyParams = new string[0];

		public static DecorationList Read(StreamReader ip)
		{
			string line;

			while ((line = ip.ReadLine()) != null)
			{
				line = line.Trim();

				if (line.Length > 0 && !line.StartsWith("#"))
					break;
			}

			if (string.IsNullOrEmpty(line))
				return null;

			DecorationList list = new DecorationList();

			int indexOf = line.IndexOf(' ');

			list.m_Type = ScriptCompiler.FindTypeByName(line.Substring(0, indexOf++), true);

			if (list.m_Type == null)
			{
				// no need to throw an exception here since we know certain tokuno and other objects won't be found.
				// just report the error and continue
				LogHelper log = new LogHelper("deco_error.txt", false, true);
				log.Log(String.Format("Type not found for header: '{0}'", line));
				log.Finish();
				return Read(ip);
				//throw new ArgumentException(String.Format("Type not found for header: '{0}'", line));
			}

			line = line.Substring(indexOf);
			indexOf = line.IndexOf('(');
			if (indexOf >= 0)
			{
				list.m_ItemID = Utility.ToInt32(line.Substring(0, indexOf - 1));

				string parms = line.Substring(++indexOf);

				if (line.EndsWith(")"))
					parms = parms.Substring(0, parms.Length - 1);

				list.m_Params = parms.Split(';');

				for (int i = 0; i < list.m_Params.Length; ++i)
					list.m_Params[i] = list.m_Params[i].Trim();
			}
			else
			{
				list.m_ItemID = Utility.ToInt32(line);
				list.m_Params = m_EmptyParams;
			}

			list.m_Entries = new ArrayList();

			while ((line = ip.ReadLine()) != null)
			{
				line = line.Trim();

				if (line.Length == 0)
					break;

				if (line.StartsWith("#"))
					continue;

				list.m_Entries.Add(new DecorationEntry(line));
			}

			return list;
		}
	}

	public class DecorationEntry
	{
		private Point3D m_Location;
		private string m_Extra;

		public Point3D Location { get { return m_Location; } }
		public string Extra { get { return m_Extra; } }

		public DecorationEntry(string line)
		{
			string x, y, z;

			Pop(out x, ref line);
			Pop(out y, ref line);
			Pop(out z, ref line);

			m_Location = new Point3D(Utility.ToInt32(x), Utility.ToInt32(y), Utility.ToInt32(z));
			m_Extra = line;
		}

		public void Pop(out string v, ref string line)
		{
			int space = line.IndexOf(' ');

			if (space >= 0)
			{
				v = line.Substring(0, space++);
				line = line.Substring(space);
			}
			else
			{
				v = line;
				line = "";
			}
		}
	}
}