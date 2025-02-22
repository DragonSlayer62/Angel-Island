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
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Doom
{
	public enum GauntletSpawnerState
	{
		InSequence,
		InProgress,
		Completed
	}

	public class GauntletSpawner : Item
	{
		public const int PlayersPerSpawn = 5;

		public const int InSequenceItemHue = 0x000;
		public const int InProgressItemHue = 0x676;
		public const int CompletedItemHue = 0x455;

		private GauntletSpawnerState m_State;

		private string m_TypeName;
		private BaseDoor m_Door;
		private BaseAddon m_Addon;
		private GauntletSpawner m_Sequence;
		private ArrayList m_Creatures;

		private Rectangle2D m_RegionBounds;
		private ArrayList m_Traps;

		private Region m_Region;

		[CommandProperty(AccessLevel.GameMaster)]
		public string TypeName
		{
			get { return m_TypeName; }
			set { m_TypeName = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseDoor Door
		{
			get { return m_Door; }
			set { m_Door = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseAddon Addon
		{
			get { return m_Addon; }
			set { m_Addon = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public GauntletSpawner Sequence
		{
			get { return m_Sequence; }
			set { m_Sequence = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool HasCompleted
		{
			get
			{
				if (m_Creatures.Count == 0)
					return false;

				for (int i = 0; i < m_Creatures.Count; ++i)
				{
					Mobile mob = (Mobile)m_Creatures[i];

					if (!mob.Deleted)
						return false;
				}

				return true;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Rectangle2D RegionBounds
		{
			get { return m_RegionBounds; }
			set { m_RegionBounds = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public GauntletSpawnerState State
		{
			get { return m_State; }
			set
			{
				if (m_State == value)
					return;

				m_State = value;

				int hue = 0;
				bool lockDoors = (m_State == GauntletSpawnerState.InProgress);

				switch (m_State)
				{
					case GauntletSpawnerState.InSequence: hue = InSequenceItemHue; break;
					case GauntletSpawnerState.InProgress: hue = InProgressItemHue; break;
					case GauntletSpawnerState.Completed: hue = CompletedItemHue; break;
				}

				if (m_Door != null)
				{
					m_Door.Hue = hue;
					m_Door.Locked = lockDoors;

					if (lockDoors)
					{
						m_Door.KeyValue = Key.RandomValue();
						m_Door.Open = false;
					}

					if (m_Door.Link != null)
					{
						m_Door.Link.Hue = hue;
						m_Door.Link.Locked = lockDoors;

						if (lockDoors)
						{
							m_Door.Link.KeyValue = Key.RandomValue();
							m_Door.Open = false;
						}
					}
				}

				if (m_Addon != null)
					m_Addon.Hue = hue;

				if (m_State == GauntletSpawnerState.InProgress)
				{
					CreateRegion();
					FullSpawn();

					m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), new TimerCallback(Slice));
				}
				else
				{
					ClearCreatures();
					ClearTraps();
					DestroyRegion();

					if (m_Timer != null)
						m_Timer.Stop();

					m_Timer = null;
				}
			}
		}

		private Timer m_Timer;

		public ArrayList Creatures
		{
			get { return m_Creatures; }
			set { m_Creatures = value; }
		}

		public ArrayList Traps
		{
			get { return m_Traps; }
			set { m_Traps = value; }
		}

		public Region Region
		{
			get { return m_Region; }
			set { m_Region = value; }
		}

		public virtual void CreateRegion()
		{
			if (m_Region != null)
				return;

			Map map = this.Map;

			if (map == null || map == Map.Internal)
				return;

			m_Region = new GauntletRegion(this, map);
		}

		public virtual void DestroyRegion()
		{
			if (m_Region != null)
				Region.RemoveRegion(m_Region);

			m_Region = null;
		}

		public virtual int ComputeTrapCount()
		{
			int area = m_RegionBounds.Width * m_RegionBounds.Height;

			return area / 100;
		}

		public virtual void ClearTraps()
		{
			for (int i = 0; i < m_Traps.Count; ++i)
				((Item)m_Traps[i]).Delete();

			m_Traps.Clear();
		}

		public virtual void SpawnTrap()
		{
			Map map = this.Map;

			if (map == null)
				return;

			Item trap = null;

			int random = Utility.Random(100);

			if (22 > random)
				trap = new SawTrap(Utility.RandomBool() ? SawTrapType.WestFloor : SawTrapType.NorthFloor);
			else if (44 > random)
				trap = new SpikeTrap(Utility.RandomBool() ? SpikeTrapType.WestFloor : SpikeTrapType.NorthFloor);
			else if (66 > random)
				trap = new GasTrap(Utility.RandomBool() ? GasTrapType.NorthWall : GasTrapType.WestWall);
			else if (88 > random)
				trap = new FireColumnTrap();
			else
				trap = new MushroomTrap();

			if (trap == null)
				return;

			if (trap is FireColumnTrap || trap is MushroomTrap)
				trap.Hue = 0x451;

			// try 10 times to find a valid location
			for (int i = 0; i < 10; ++i)
			{
				int x = Utility.Random(m_RegionBounds.X, m_RegionBounds.Width);
				int y = Utility.Random(m_RegionBounds.Y, m_RegionBounds.Height);
				int z = this.Z;

				if (!map.CanFit(x, y, z, 16, CanFitFlags.requireSurface))
					z = map.GetAverageZ(x, y);

				if (!map.CanFit(x, y, z, 16, CanFitFlags.requireSurface))
					continue;

				trap.MoveToWorld(new Point3D(x, y, z), map);
				m_Traps.Add(trap);

				return;
			}

			trap.Delete();
		}

		public virtual int ComputeSpawnCount()
		{
			int playerCount = 0;

			Map map = this.Map;

			if (map != null)
			{
				Point3D loc = GetWorldLocation();

				Sector sec = map.GetSector(loc);
				ArrayList regions = sec.Regions;

				for (int i = 0; playerCount == 0 && i < regions.Count; ++i)
				{
					Region reg = (Region)regions[i];

					if (reg != null && reg != m_Region && reg.Contains(loc))
						playerCount = reg.Players.Count;
				}
			}

			if (playerCount == 0 && m_Region != null)
				playerCount = m_Region.Players.Count;

			int count = (playerCount + PlayersPerSpawn - 1) / PlayersPerSpawn;

			if (count < 1)
				count = 1;

			return count;
		}

		public virtual void ClearCreatures()
		{
			for (int i = 0; i < m_Creatures.Count; ++i)
				((Mobile)m_Creatures[i]).Delete();

			m_Creatures.Clear();
		}

		public virtual void FullSpawn()
		{
			ClearCreatures();

			int count = ComputeSpawnCount();

			for (int i = 0; i < count; ++i)
				Spawn();

			ClearTraps();

			count = ComputeTrapCount();

			for (int i = 0; i < count; ++i)
				SpawnTrap();
		}

		public virtual void Spawn()
		{
			try
			{
				if (m_TypeName == null)
					return;

				Type type = ScriptCompiler.FindTypeByName(m_TypeName, true);

				if (type == null)
					return;

				object obj = Activator.CreateInstance(type);

				if (obj == null)
					return;

				if (obj is Item)
				{
					((Item)obj).Delete();
				}
				else if (obj is Mobile)
				{
					Mobile mob = (Mobile)obj;

					mob.MoveToWorld(GetWorldLocation(), this.Map);

					m_Creatures.Add(mob);
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}

		public virtual void RecurseReset()
		{
			if (m_State != GauntletSpawnerState.InSequence)
			{
				State = GauntletSpawnerState.InSequence;

				if (m_Sequence != null && !m_Sequence.Deleted)
					m_Sequence.RecurseReset();
			}
		}

		public virtual void Slice()
		{
			if (m_State != GauntletSpawnerState.InProgress)
				return;

			int count = ComputeSpawnCount();

			for (int i = m_Creatures.Count; i < count; ++i)
				Spawn();

			if (HasCompleted)
			{
				State = GauntletSpawnerState.Completed;

				if (m_Sequence != null && !m_Sequence.Deleted)
				{
					if (m_Sequence.State == GauntletSpawnerState.Completed)
						RecurseReset();

					m_Sequence.State = GauntletSpawnerState.InProgress;
				}
			}
		}

		[Constructable]
		public GauntletSpawner()
			: this(null)
		{
		}

		[Constructable]
		public GauntletSpawner(string typeName)
			: base(0x36FE)
		{
			Visible = false;
			Movable = false;
			Name = "doom spawner";

			m_TypeName = typeName;
			m_Creatures = new ArrayList();
			m_Traps = new ArrayList();
		}

		public GauntletSpawner(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			writer.Write(m_RegionBounds);

			writer.WriteItemList(m_Traps, false);

			writer.WriteMobileList(m_Creatures, false);

			writer.Write(m_TypeName);
			writer.Write(m_Door);
			writer.Write(m_Addon);
			writer.Write(m_Sequence);

			writer.Write((int)m_State);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_RegionBounds = reader.ReadRect2D();
						m_Traps = reader.ReadItemList();

						goto case 0;
					}
				case 0:
					{
						if (version < 1)
						{
							m_Traps = new ArrayList();
							m_RegionBounds = new Rectangle2D(X - 40, Y - 40, 80, 80);
						}

						m_Creatures = reader.ReadMobileList();

						m_TypeName = reader.ReadString();
						m_Door = reader.ReadItem() as BaseDoor;
						m_Addon = reader.ReadItem() as BaseAddon;
						m_Sequence = reader.ReadItem() as GauntletSpawner;

						State = (GauntletSpawnerState)reader.ReadInt();

						break;
					}
			}
		}

		public static void Initialize()
		{
			CommandSystem.Register("GenGauntlet", AccessLevel.Administrator, new CommandEventHandler(GenGauntlet_OnCommand));
		}

		public static void CreateTeleporter(int xFrom, int yFrom, int xTo, int yTo)
		{
			Static telePad = new Static(0x1822);
			Teleporter teleItem = new Teleporter(new Point3D(xTo, yTo, -1), Map.Malas, false);

			telePad.Hue = 0x482;
			telePad.MoveToWorld(new Point3D(xFrom, yFrom, -1), Map.Malas);

			teleItem.MoveToWorld(new Point3D(xFrom, yFrom, -1), Map.Malas);

			teleItem.SourceEffect = true;
			teleItem.DestEffect = true;
			teleItem.SoundID = 0x1FE;
		}

		public static BaseDoor CreateDoorSet(int xDoor, int yDoor, bool doorEastToWest, int hue)
		{
			BaseDoor hiDoor = new MetalDoor(doorEastToWest ? DoorFacing.NorthCCW : DoorFacing.WestCW);
			BaseDoor loDoor = new MetalDoor(doorEastToWest ? DoorFacing.SouthCW : DoorFacing.EastCCW);

			hiDoor.MoveToWorld(new Point3D(xDoor, yDoor, -1), Map.Malas);
			loDoor.MoveToWorld(new Point3D(xDoor + (doorEastToWest ? 0 : 1), yDoor + (doorEastToWest ? 1 : 0), -1), Map.Malas);

			hiDoor.Link = loDoor;
			loDoor.Link = hiDoor;

			hiDoor.Hue = hue;
			loDoor.Hue = hue;

			return hiDoor;
		}

		public static GauntletSpawner CreateSpawner(string typeName, int xSpawner, int ySpawner, int xDoor, int yDoor, int xPentagram, int yPentagram, bool doorEastToWest, int xStart, int yStart, int xWidth, int yHeight)
		{
			GauntletSpawner spawner = new GauntletSpawner(typeName);

			spawner.MoveToWorld(new Point3D(xSpawner, ySpawner, -1), Map.Malas);

			if (xDoor > 0 && yDoor > 0)
				spawner.Door = CreateDoorSet(xDoor, yDoor, doorEastToWest, 0);

			spawner.RegionBounds = new Rectangle2D(xStart, yStart, xWidth, yHeight);

			if (xPentagram > 0 && yPentagram > 0)
			{
				PentagramAddon pentagram = new PentagramAddon();

				pentagram.MoveToWorld(new Point3D(xPentagram, yPentagram, -1), Map.Malas);

				spawner.Addon = pentagram;
			}

			return spawner;
		}

		public static void CreatePricedHealer(int price, int x, int y)
		{
			PricedHealer healer = new PricedHealer(price);

			healer.MoveToWorld(new Point3D(x, y, -1), Map.Malas);

			healer.Home = healer.Location;
			healer.RangeHome = 5;
		}

		public static void CreateMorphItem(int x, int y, int inactiveItemID, int activeItemID, int range, int hue)
		{
			MorphItem item = new MorphItem(inactiveItemID, activeItemID, range);

			item.Hue = hue;
			item.MoveToWorld(new Point3D(x, y, -1), Map.Malas);
		}

		public static void CreateVarietyDealer(int x, int y)
		{
			VarietyDealer dealer = new VarietyDealer();

			/* Begin outfit */
			dealer.Name = "Nix";
			dealer.Title = "the Variety Dealer";

			dealer.Body = 400;
			dealer.Female = false;
			dealer.Hue = 0x8835;

			ArrayList items = new ArrayList(dealer.Items);

			for (int i = 0; i < items.Count; ++i)
			{
				Item item = (Item)items[i];

				if (item.Layer != Layer.ShopBuy && item.Layer != Layer.ShopResale && item.Layer != Layer.ShopSell)
					item.Delete();
			}

			dealer.AddItem(new LongBeard(0x482));
			dealer.AddItem(new TwoPigTails(0x482));

			dealer.AddItem(new FloppyHat(1));
			dealer.AddItem(new Robe(1));

			dealer.AddItem(new LanternOfSouls());

			dealer.AddItem(new Sandals(0x482));
			/* End outfit */

			dealer.MoveToWorld(new Point3D(x, y, -1), Map.Malas);

			dealer.Home = dealer.Location;
			dealer.RangeHome = 2;
		}

		public static void GenGauntlet_OnCommand(CommandEventArgs e)
		{
			/* Begin healer room */
			CreatePricedHealer(5000, 387, 400);
			CreateTeleporter(390, 407, 394, 405);

			BaseDoor healerDoor = CreateDoorSet(393, 404, true, 0x44E);

			healerDoor.Locked = true;
			healerDoor.KeyValue = Key.RandomValue();

			if (healerDoor.Link != null)
			{
				healerDoor.Link.Locked = true;
				healerDoor.Link.KeyValue = Key.RandomValue();
			}
			/* End healer room */

			/* Begin supply room */
			CreateMorphItem(433, 371, 0x29F, 0x116, 3, 0x44E);
			CreateMorphItem(433, 372, 0x29F, 0x115, 3, 0x44E);

			CreateVarietyDealer(492, 369);

			for (int x = 434; x <= 478; ++x)
			{
				for (int y = 371; y <= 372; ++y)
				{
					Static item = new Static(0x524);

					item.Hue = 1;
					item.MoveToWorld(new Point3D(x, y, -1), Map.Malas);
				}
			}
			/* End supply room */

			/* Begin gauntlet cycle */
			CreateTeleporter(471, 428, 474, 428);
			CreateTeleporter(462, 494, 462, 498);
			CreateTeleporter(403, 502, 399, 506);
			CreateTeleporter(357, 476, 356, 480);
			CreateTeleporter(361, 433, 357, 434);

			GauntletSpawner sp1 = CreateSpawner("DarknightCreeper", 491, 456, 473, 432, 417, 426, true, 473, 412, 39, 60);
			GauntletSpawner sp2 = CreateSpawner("FleshRenderer", 482, 520, 468, 496, 426, 422, false, 448, 496, 56, 48);
			GauntletSpawner sp3 = CreateSpawner("Impaler", 406, 538, 408, 504, 432, 430, false, 376, 504, 64, 48);
			GauntletSpawner sp4 = CreateSpawner("ShadowKnight", 335, 512, 360, 478, 424, 439, false, 300, 478, 72, 64);
			GauntletSpawner sp5 = CreateSpawner("AbysmalHorror", 326, 433, 360, 429, 416, 435, true, 300, 408, 60, 56);
			GauntletSpawner sp6 = CreateSpawner("DemonKnight", 423, 430, 0, 0, 423, 430, true, 392, 392, 72, 96);

			sp1.Sequence = sp2;
			sp2.Sequence = sp3;
			sp3.Sequence = sp4;
			sp4.Sequence = sp5;
			sp5.Sequence = sp6;
			sp6.Sequence = sp1;

			sp1.State = GauntletSpawnerState.InProgress;
			/* End gauntlet cycle */

			/* Begin exit gate */
			ConfirmationMoongate gate = new ConfirmationMoongate();

			gate.Dispellable = false;

			gate.Target = new Point3D(2350, 1270, -85);
			gate.TargetMap = Map.Malas;

			gate.GumpWidth = 420;
			gate.GumpHeight = 280;

			gate.MessageColor = 0x7F00;
			gate.MessageNumber = 1062109; // You are about to exit Dungeon Doom.  Do you wish to continue?

			gate.TitleColor = 0x7800;
			gate.TitleNumber = 1062108; // Please verify...

			gate.Hue = 0x44E;

			gate.MoveToWorld(new Point3D(433, 326, 4), Map.Malas);
			/* End exit gate */
		}
	}


	public class GauntletRegion : Region
	{
		private GauntletSpawner m_Spawner;

		public GauntletRegion(GauntletSpawner spawner, Map map)
			: base("", spawner.TypeName, map)
		{
			m_Spawner = spawner;

			Priority = Region.TownPriority + 1;
			LoadFromXml = false;

			Coords = new ArrayList();
			Coords.Add(spawner.RegionBounds);

			GoLocation = spawner.Location;

			Region.AddRegion(this);
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			global = 12;
		}

		public override void OnEnter(Mobile m)
		{
		}

		public override void OnExit(Mobile m)
		{
		}
	}
}