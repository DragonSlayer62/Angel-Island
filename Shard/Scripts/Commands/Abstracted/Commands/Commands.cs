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

/* Scripts/Commands/Abstracted/Commands/Commands.cs
 * CHANGELOG
 *	3/8/2016, Adam 
 *			Fix the argument parser in Execute to take a second argument of 1 OR MORE parts. 
 *			The old implementation assumed a value would only have one part to the argument. 
 *			But something like a time value can have two and maybe three. I.e., 3/6/2016 08:00:00
 *	5/7/10, Adam
 *		New [Dupe implementation that allows the area modifier, i.e., [area dupe
 *	7/8/08, Adam
 *		Have AddToPackCommand chjeck to see that there is at least one supplied argument (an item)
 *	6/7/2007, Pix
 *		Added possibility of using 1 token in the value param of [set prop value
 *	07/21/06, Rhiannon
 *		Set access level for [tele and [hide/unhide to Reporter.
 *	02/28/05, erlein
 *		Added check in delete command for ChestItemSpawner or Spawner
 *		Item types. Records mobile deleting in respective objects if matched.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;

namespace Server.Commands
{
	public class TargetCommands
	{
		public static void Initialize()
		{
			Register(new KillCommand(true));
			Register(new KillCommand(false));
			Register(new HideCommand(true));
			Register(new HideCommand(false));
			Register(new KickCommand(true));
			Register(new KickCommand(false));
			Register(new FirewallCommand());
			Register(new TeleCommand());
			Register(new SetCommand());
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "Immortal", "blessed", "true", ObjectTypes.Mobiles));
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "Invul", "blessed", "true", ObjectTypes.Mobiles));
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "Mortal", "blessed", "false", ObjectTypes.Mobiles));
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "NoInvul", "blessed", "false", ObjectTypes.Mobiles));
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "Squelch", "squelched", "true", ObjectTypes.Mobiles));
			Register(new AliasedSetCommand(AccessLevel.GameMaster, "Unsquelch", "squelched", "false", ObjectTypes.Mobiles));
			Register(new GetCommand());
			Register(new GetTypeCommand());
			Register(new DeleteCommand());
			Register(new RestockCommand());
			Register(new DismountCommand());
			Register(new AddCommand());
			Register(new AddToPackCommand());
			Register(new TellCommand());
			Register(new PrivSoundCommand());
			Register(new IncreaseCommand());
			Register(new OpenBrowserCommand());
			Register(new CountCommand());
			Register(new InterfaceCommand());
			Register(new DupeCommand());		// adam: convert to area aware
		}

		private static ArrayList m_AllCommands = new ArrayList();

		public static ArrayList AllCommands { get { return m_AllCommands; } }

		public static void Register(BaseCommand command)
		{
			m_AllCommands.Add(command);

			ArrayList impls = BaseCommandImplementor.Implementors;

			for (int i = 0; i < impls.Count; ++i)
			{
				BaseCommandImplementor impl = (BaseCommandImplementor)impls[i];

				if ((command.Supports & impl.SupportRequirement) != 0)
					impl.Register(command);
			}
		}
	}

	public class CountCommand : BaseCommand
	{
		public CountCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Complex;
			Commands = new string[] { "Count" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Count";
			Description = "Counts the number of objects that a command modifier would use. Generally used with condition arguments.";
			ListOptimized = true;
		}

		public override void ExecuteList(CommandEventArgs e, ArrayList list)
		{
			if (list.Count == 1)
				AddResponse("There is one matching object.");
			else
				AddResponse(String.Format("There are {0} matching objects.", list.Count));
		}
	}

	public class OpenBrowserCommand : BaseCommand
	{
		public OpenBrowserCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "OpenBrowser", "OB" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "OpenBrowser <url>";
			Description = "Opens the web browser of a targeted player to a specified url.";
		}

		public static void OpenBrowser_Callback(Mobile from, bool okay, object state)
		{
			object[] states = (object[])state;
			Mobile gm = (Mobile)states[0];
			string url = (string)states[1];

			if (okay)
			{
				gm.SendMessage("{0} : has opened their web browser to : {1}", from.Name, url);
				from.LaunchBrowser(url);
			}
			else
			{
				from.SendMessage("You have chosen not to open your web browser.");
				gm.SendMessage("{0} : has chosen not to open their web browser to : {1}", from.Name, url);
			}
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (e.Length == 1)
			{
				Mobile mob = (Mobile)obj;
				Mobile from = e.Mobile;

				if (mob.Player)
				{
					NetState ns = mob.NetState;

					if (ns == null)
					{
						LogFailure("That player is not online.");
					}
					else
					{
						string url = e.GetString(0);

						CommandLogging.WriteLine(from, "{0} {1} requesting to open web browser of {2} to {3}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob), url);
						AddResponse("Awaiting user confirmation...");
						mob.SendGump(new WarningGump(1060637, 30720, String.Format("A game master is requesting to open your web browser to the following URL:<br>{0}", url), 0xFFC000, 320, 240, new WarningGumpCallback(OpenBrowser_Callback), new object[] { from, url }));
					}
				}
				else
				{
					LogFailure("That is not a player.");
				}
			}
			else
			{
				LogFailure("Format: OpenBrowser <url>");
			}
		}
	}

	public class IncreaseCommand : BaseCommand
	{
		public IncreaseCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "Increase", "Inc" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Increase {<propertyName> <offset> ...}";
			Description = "Increases the value of a specified property by the specified offset.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (obj is BaseMulti)
			{
				LogFailure("This command does not work on multis.");
			}
			else if (e.Length >= 2)
			{
				string result = Properties.IncreaseValue(e.Mobile, obj, e.Arguments);

				if (result == "The property has been increased." || result == "The properties have been increased." || result == "The property has been decreased." || result == "The properties have been decreased." || result == "The properties have been changed.")
					AddResponse(result);
				else
					LogFailure(result);
			}
			else
			{
				LogFailure("Format: Increase {<propertyName> <offset> ...}");
			}
		}
	}

	public class PrivSoundCommand : BaseCommand
	{
		public PrivSoundCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "PrivSound" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "PrivSound <index>";
			Description = "Plays a sound to a given target.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile from = e.Mobile;

			if (e.Length == 1)
			{
				int index = e.GetInt32(0);
				Mobile mob = (Mobile)obj;

				CommandLogging.WriteLine(from, "{0} {1} playing sound {2} for {3}", from.AccessLevel, CommandLogging.Format(from), index, CommandLogging.Format(mob));
				mob.Send(new PlaySound(index, mob.Location));
			}
			else
			{
				from.SendMessage("Format: PrivSound <index>");
			}
		}
	}

	public class TellCommand : BaseCommand
	{
		public TellCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Tell" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Tell \"text\"";
			Description = "Sends a system message to a targeted player.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile mob = (Mobile)obj;
			Mobile from = e.Mobile;

			CommandLogging.WriteLine(from, "{0} {1} telling {2} \"{3}\"", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob), e.ArgString);

			mob.SendMessage(e.ArgString);
		}
	}

	public class AddToPackCommand : BaseCommand
	{
		public AddToPackCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.All;
			Commands = new string[] { "AddToPack", "AddToCont" };
			ObjectTypes = ObjectTypes.Both;
			ListOptimized = true;
			Usage = "AddToPack <name> [params] [set {<propertyName> <value> ...}]";
			Description = "Adds an item by name to the backpack of a targeted player or npc, or a targeted container. Optional constructor parameters. Optional set property list.";
		}

		public override void ExecuteList(CommandEventArgs e, ArrayList list)
		{
			if (e.Arguments.Length < 1)
			{	// make sure we have at least an item
				LogFailure(Usage);
				return;
			}

			ArrayList packs = new ArrayList(list.Count);

			for (int i = 0; i < list.Count; ++i)
			{
				object obj = list[i];
				Container cont = null;

				if (obj is Mobile)
					cont = ((Mobile)obj).Backpack;
				else if (obj is Container)
					cont = (Container)obj;

				if (cont != null)
					packs.Add(cont);
				else
					LogFailure("That is not a container.");
			}

			Add.Invoke(e.Mobile, e.Mobile.Location, e.Mobile.Location, e.Arguments, packs);
		}
	}

	public class AddCommand : BaseCommand
	{
		public AddCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple | CommandSupport.Self;
			Commands = new string[] { "Add" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Add [<name> [params] [set {<propertyName> <value> ...}]]";
			Description = "Adds an item or npc by name to a targeted location. Optional constructor parameters. Optional set property list. If no arguments are specified, this brings up a categorized add menu.";
		}

		public override bool ValidateArgs(BaseCommandImplementor impl, CommandEventArgs e)
		{
			if (e.Length >= 1)
			{
				Type t = ScriptCompiler.FindTypeByName(e.GetString(0));

				if (t == null)
				{
					e.Mobile.SendMessage("No type with that name was found.");

					string match = e.GetString(0).Trim();

					if (match.Length < 3)
					{
						e.Mobile.SendMessage("Invalid search string.");
						e.Mobile.SendGump(new Server.Gumps.AddGump(e.Mobile, match, 0, Type.EmptyTypes, false));
					}
					else
					{
						e.Mobile.SendGump(new Server.Gumps.AddGump(e.Mobile, match, 0, (Type[])Server.Gumps.AddGump.Match(match).ToArray(typeof(Type)), true));
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				e.Mobile.SendGump(new Server.Gumps.CategorizedAddGump(e.Mobile));
			}

			return false;
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			IPoint3D p = obj as IPoint3D;

			if (p == null)
				return;

			if (p is Item)
				p = ((Item)p).GetWorldTop();
			else if (p is Mobile)
				p = ((Mobile)p).Location;

			Add.Invoke(e.Mobile, new Point3D(p), new Point3D(p), e.Arguments);
		}
	}

	public class TeleCommand : BaseCommand
	{
		public TeleCommand()
		{
			AccessLevel = AccessLevel.Reporter;
			Supports = CommandSupport.Simple;
			Commands = new string[] { "Teleport", "Tele" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Teleport";
			Description = "Teleports your character to a targeted location.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			IPoint3D p = obj as IPoint3D;

			if (p == null)
				return;

			Mobile from = e.Mobile;

			SpellHelper.GetSurfaceTop(ref p);

			CommandLogging.WriteLine(from, "{0} {1} teleporting to {2}", from.AccessLevel, CommandLogging.Format(from), new Point3D(p));

			Point3D fromLoc = from.Location;
			Point3D toLoc = new Point3D(p);

			from.Location = toLoc;
			from.ProcessDelta();

			if (!from.Hidden)
			{
				Effects.SendLocationParticles(EffectItem.Create(fromLoc, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
				Effects.SendLocationParticles(EffectItem.Create(toLoc, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

				from.PlaySound(0x1FE);
			}
		}
	}

	public class DismountCommand : BaseCommand
	{
		public DismountCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Dismount" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Dismount";
			Description = "Forcefully dismounts a given target.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile from = e.Mobile;
			Mobile mob = (Mobile)obj;

			CommandLogging.WriteLine(from, "{0} {1} dismounting {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob));

			bool takenAction = false;

			for (int i = 0; i < mob.Items.Count; ++i)
			{
				Item item = (Item)mob.Items[i];

				if (item is IMountItem)
				{
					IMount mount = ((IMountItem)item).Mount;

					if (mount != null)
					{
						mount.Rider = null;
						takenAction = true;
					}

					if (mob.Items.IndexOf(item) == -1)
						--i;
				}
			}

			for (int i = 0; i < mob.Items.Count; ++i)
			{
				Item item = (Item)mob.Items[i];

				if (item.Layer == Layer.Mount)
				{
					takenAction = true;
					item.Delete();
					--i;
				}
			}

			if (takenAction)
				AddResponse("They have been dismounted.");
			else
				LogFailure("They were not mounted.");
		}
	}

	public class RestockCommand : BaseCommand
	{
		public RestockCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs;
			Commands = new string[] { "Restock" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Restock";
			Description = "Manually restocks a targeted vendor, refreshing the quantity of every item the vendor sells to the maximum. This also invokes the maximum quantity adjustment algorithms.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (obj is BaseVendor)
			{
				CommandLogging.WriteLine(e.Mobile, "{0} {1} restocking {2}", e.Mobile.AccessLevel, CommandLogging.Format(e.Mobile), CommandLogging.Format(obj));

				((BaseVendor)obj).Restock();
				AddResponse("The vendor has been restocked.");
			}
			else
			{
				AddResponse("That is not a vendor.");
			}
		}
	}

	public class GetTypeCommand : BaseCommand
	{
		public GetTypeCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "GetType" };
			ObjectTypes = ObjectTypes.All;
			Usage = "GetType";
			Description = "Gets the type name of a targeted object.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (obj == null)
			{
				AddResponse("The object is null.");
			}
			else
			{
				Type type = obj.GetType();

				if (type.DeclaringType == null)
					AddResponse(String.Format("The type of that object is {0}.", type.Name));
				else
					AddResponse(String.Format("The type of that object is {0}.", type.FullName));
			}
		}
	}

	public class GetCommand : BaseCommand
	{
		public GetCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "Get" };
			ObjectTypes = ObjectTypes.All;
			Usage = "Get <propertyName>";
			Description = "Gets a property value by name of a targeted object.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (e.Length == 1)
			{
				string result = Properties.GetValue(e.Mobile, obj, e.GetString(0));

				if (result == "Property not found." || result == "Property is write only." || result.StartsWith("Getting this property"))
					LogFailure(result);
				else
					AddResponse(result);
			}
			else
			{
				LogFailure("Format: Get <propertyName>");
			}
		}
	}

	public class AliasedSetCommand : BaseCommand
	{
		private string m_Name;
		private string m_Value;

		public AliasedSetCommand(AccessLevel level, string command, string name, string value, ObjectTypes objects)
		{
			m_Name = name;
			m_Value = value;

			AccessLevel = level;

			if (objects == ObjectTypes.Items)
				Supports = CommandSupport.AllItems;
			else if (objects == ObjectTypes.Mobiles)
				Supports = CommandSupport.AllMobiles;
			else
				Supports = CommandSupport.All;

			Commands = new string[] { command };
			ObjectTypes = objects;
			Usage = command;
			Description = String.Format("Sets the {0} property to {1}.", name, value);
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			string result = Properties.SetValue(e.Mobile, obj, m_Name, m_Value);

			if (result == "Property has been set.")
				AddResponse(result);
			else
				LogFailure(result);
		}
	}

	public class SetCommand : BaseCommand
	{
		public SetCommand()
		{
			AccessLevel = AccessLevel.Counselor;
			Supports = CommandSupport.All;
			Commands = new string[] { "Set" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Set <propertyName> <value>";
			Description = "Sets a property value by name of a targeted object.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (e.Length >= 2)
			{

				//3/8/2016, Adam Adam: This old implementation assumed a value would only have one part to the argument. 
				//	But something like a time value can have two and maybe three. I.e., 3/6/2016 08:00:00
				// string prop = e.GetString(0);
				// string val = e.GetString(1);

				string prop = e.GetString(0);
				string val=null;
				for (int ix=1; ix < e.Length; ix++)
					val += e.GetString(ix) + " ";
				val.TrimEnd();

				int p1 = val.IndexOf('%', 0);
				if (p1 >= 0)
				{
					int p2 = val.IndexOf('%', p1 + 1);
					if (p2 > 0)
					{
						//found a token, look it up.
						string token = val.Substring(p1 + 1, p2 - p1 - 1);
						string token_value = Properties.GetOnlyValue(e.Mobile, obj, token);
						if (token_value != null)
						{
							val = val.Replace("%" + token + "%", token_value);
						}
					}
				}

				string result = Properties.SetValue(e.Mobile, obj, prop, val);

				if (result == "Property has been set.")
					AddResponse(result);
				else
					LogFailure(result);
			}
			else
			{
				LogFailure("Format: Set <propertyName> <value>");
			}
		}
	}

	public class DeleteCommand : BaseCommand
	{
		public DeleteCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllNPCs | CommandSupport.AllItems;
			Commands = new string[] { "Delete", "Remove" };
			ObjectTypes = ObjectTypes.Both;
			Usage = "Delete";
			Description = "Deletes a targeted item or mobile. Does not delete players.";
		}

		private void OnConfirmCallback(Mobile from, bool okay, object state)
		{
			object[] states = (object[])state;
			CommandEventArgs e = (CommandEventArgs)states[0];
			ArrayList list = (ArrayList)states[1];

			bool flushToLog = false;

			if (okay)
			{
				AddResponse("Delete command confirmed.");

				if (list.Count > 20)
					CommandLogging.Enabled = false;

				base.ExecuteList(e, list);

				if (list.Count > 20)
				{
					flushToLog = true;
					CommandLogging.Enabled = true;
				}
			}
			else
			{
				AddResponse("Delete command aborted.");
			}

			Flush(from, flushToLog);
		}

		public override void ExecuteList(CommandEventArgs e, ArrayList list)
		{
			if (list.Count > 1)
			{
				e.Mobile.SendGump(new WarningGump(1060637, 30720, String.Format("You are about to delete {0} objects. This cannot be undone without a full server revert.<br><br>Continue?", list.Count), 0xFFC000, 420, 280, new WarningGumpCallback(OnConfirmCallback), new object[] { e, list }));
				AddResponse("Awaiting confirmation...");
			}
			else
			{
				base.ExecuteList(e, list);
			}
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			if (obj is Item)
			{
				// erl: added check for spawner or chestitemspawner
				// to log who is deleting it

				if (obj is Spawner)
					((Spawner)obj).LastProps = e.Mobile;
				else if (obj is ChestItemSpawner)
					((ChestItemSpawner)obj).LastProps = e.Mobile;

				CommandLogging.WriteLine(e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format(e.Mobile), CommandLogging.Format(obj));
				((Item)obj).Delete();
				AddResponse("The item has been deleted.");
			}
			else if (obj is Mobile && !((Mobile)obj).Player)
			{
				CommandLogging.WriteLine(e.Mobile, "{0} {1} deleting {2}", e.Mobile.AccessLevel, CommandLogging.Format(e.Mobile), CommandLogging.Format(obj));
				((Mobile)obj).Delete();
				AddResponse("The mobile has been deleted.");
			}
			else
			{
				LogFailure("That cannot be deleted.");
			}
		}
	}

	public class KillCommand : BaseCommand
	{
		private bool m_Value;

		public KillCommand(bool value)
		{
			m_Value = value;

			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			Commands = value ? new string[] { "Kill" } : new string[] { "Resurrect", "Res" };
			ObjectTypes = ObjectTypes.Mobiles;

			if (value)
			{
				Usage = "Kill";
				Description = "Kills a targeted player or npc.";
			}
			else
			{
				Usage = "Resurrect";
				Description = "Resurrects a targeted ghost.";
			}
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile mob = (Mobile)obj;
			Mobile from = e.Mobile;

			if (m_Value)
			{
				if (!mob.Alive)
				{
					LogFailure("They are already dead.");
				}
				else if (!mob.CanBeDamaged())
				{
					LogFailure("They cannot be harmed.");
				}
				else
				{
					CommandLogging.WriteLine(from, "{0} {1} killing {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob));
					mob.Kill();

					AddResponse("They have been killed.");
				}
			}
			else
			{
				if (mob.IsDeadBondedPet)
				{
					BaseCreature bc = mob as BaseCreature;

					if (bc != null)
					{
						CommandLogging.WriteLine(from, "{0} {1} resurrecting {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob));

						bc.PlaySound(0x214);
						bc.FixedEffect(0x376A, 10, 16);

						bc.ResurrectPet();

						AddResponse("It has been resurrected.");
					}
				}
				else if (!mob.Alive)
				{
					CommandLogging.WriteLine(from, "{0} {1} resurrecting {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(mob));

					mob.PlaySound(0x214);
					mob.FixedEffect(0x376A, 10, 16);

					mob.Resurrect();

					AddResponse("They have been resurrected.");
				}
				else
				{
					LogFailure("They are not dead.");
				}
			}
		}
	}

	public class HideCommand : BaseCommand
	{
		private bool m_Value;

		public HideCommand(bool value)
		{
			m_Value = value;

			AccessLevel = AccessLevel.Reporter;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { value ? "Hide" : "Unhide" };
			ObjectTypes = ObjectTypes.Mobiles;

			if (value)
			{
				Usage = "Hide";
				Description = "Makes a targeted mobile disappear in a puff of smoke.";
			}
			else
			{
				Usage = "Unhide";
				Description = "Makes a targeted mobile appear in a puff of smoke.";
			}
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile m = (Mobile)obj;

			CommandLogging.WriteLine(e.Mobile, "{0} {1} {2} {3}", e.Mobile.AccessLevel, CommandLogging.Format(e.Mobile), m_Value ? "hiding" : "unhiding", CommandLogging.Format(m));

			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y, m.Z - 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z + 4), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X, m.Y + 1, m.Z - 4), m.Map, 0x3728, 13);

			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 11), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 7), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z + 3), m.Map, 0x3728, 13);
			Effects.SendLocationEffect(new Point3D(m.X + 1, m.Y + 1, m.Z - 1), m.Map, 0x3728, 13);

			m.PlaySound(0x228);
			m.Hidden = m_Value;

			if (m_Value)
				AddResponse("They have been hidden.");
			else
				AddResponse("They have been revealed.");
		}
	}

	public class FirewallCommand : BaseCommand
	{
		public FirewallCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { "Firewall" };
			ObjectTypes = ObjectTypes.Mobiles;
			Usage = "Firewall";
			Description = "Adds a targeted player to the firewall (list of blocked IP addresses). This command does not ban or kick.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile from = e.Mobile;
			Mobile targ = (Mobile)obj;
			NetState state = targ.NetState;

			if (state != null)
			{
				CommandLogging.WriteLine(from, "{0} {1} firewalling {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(targ));

				try
				{
					Firewall.Add(((IPEndPoint)state.Socket.RemoteEndPoint).Address);
					AddResponse("They have been firewalled.");
				}
				catch (Exception ex)
				{
					LogHelper.LogException(ex);
					LogFailure(ex.Message);
				}
			}
			else
			{
				LogFailure("They are not online.");
			}
		}
	}

	public class KickCommand : BaseCommand
	{
		private bool m_Ban;

		public KickCommand(bool ban)
		{
			m_Ban = ban;

			AccessLevel = (ban ? AccessLevel.Administrator : AccessLevel.GameMaster);
			Supports = CommandSupport.AllMobiles;
			Commands = new string[] { ban ? "Ban" : "Kick" };
			ObjectTypes = ObjectTypes.Mobiles;

			if (ban)
			{
				Usage = "Ban";
				Description = "Bans the account of a targeted player.";
			}
			else
			{
				Usage = "Kick";
				Description = "Disconnects a targeted player.";
			}
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile from = e.Mobile;
			Mobile targ = (Mobile)obj;

			if (from.AccessLevel > targ.AccessLevel)
			{
				NetState fromState = from.NetState, targState = targ.NetState;

				if (fromState != null && targState != null)
				{
					Account fromAccount = fromState.Account as Account;
					Account targAccount = targState.Account as Account;

					if (fromAccount != null && targAccount != null)
					{
						CommandLogging.WriteLine(from, "{0} {1} {2} {3}", from.AccessLevel, CommandLogging.Format(from), m_Ban ? "banning" : "kicking", CommandLogging.Format(targ));

						targ.Say("I've been {0}!", m_Ban ? "banned" : "kicked");

						AddResponse(String.Format("They have been {0}.", m_Ban ? "banned" : "kicked"));

						targState.Dispose();

						if (m_Ban)
						{
							targAccount.Banned = true;
							targAccount.SetUnspecifiedBan(from);
							from.SendGump(new BanDurationGump(targAccount));
						}
					}
				}
				else if (targState == null)
				{
					LogFailure("They are not online.");
				}
			}
			else
			{
				LogFailure("You do not have the required access level to do this.");
			}
		}
	}

	public class DupeCommand : BaseCommand
	{
		private bool m_InBag = false;
		private int m_Amount = 1;

		public DupeCommand()
		{
			AccessLevel = AccessLevel.Administrator;
			Supports = CommandSupport.AllItems | CommandSupport.Area | CommandSupport.Single;
			Commands = new string[] { "Dupe" };
			ObjectTypes = ObjectTypes.Items;
			Usage = "Dupe [amount]";
			Description = "Dupes a targeted item.";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			/*if (e.Length == 1)
			{
				string result = Properties.GetValue(e.Mobile, obj, e.GetString(0));

				if (result == "Property not found." || result == "Property is write only." || result.StartsWith("Getting this property"))
					LogFailure(result);
				else
					AddResponse(result);
			}
			else
			{
				LogFailure("Format: Get <propertyName>");
			}*/
			m_Amount = 1;
			if (e.Length >= 1)
				m_Amount = e.GetInt32(0);
			PerItem(e.Mobile, obj);
		}

		private void PerItem(Mobile from, object targ)
		{
			bool done = false;
			if (!(targ is Item))
			{
				LogFailure("You can only dupe items.");
				return;
			}

			CommandLogging.WriteLine(from, "{0} {1} duping {2} (inBag={3}; amount={4})", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(targ), m_InBag, m_Amount);

			Item copy = (Item)targ;
			Container pack;

			if (m_InBag)
			{
				if (copy.Parent is Container)
					pack = (Container)copy.Parent;
				else if (copy.Parent is Mobile)
					pack = ((Mobile)copy.Parent).Backpack;
				else
					pack = null;
			}
			else
				pack = from.Backpack;

			Type t = copy.GetType();

			ConstructorInfo[] info = t.GetConstructors();

			foreach (ConstructorInfo c in info)
			{
				//if ( !c.IsDefined( typeof( ConstructableAttribute ), false ) ) continue;

				ParameterInfo[] paramInfo = c.GetParameters();

				if (paramInfo.Length == 0)
				{
					object[] objParams = new object[0];

					try
					{
						//from.SendMessage("Duping {0}...", m_Amount);
						for (int i = 0; i < m_Amount; i++)
						{
							object o = c.Invoke(objParams);

							if (o != null && o is Item)
							{
								Item newItem = (Item)o;
								CopyProperties(newItem, copy);//copy.Dupe( item, copy.Amount );
								newItem.Parent = null;

								if (pack != null)
									pack.DropItem(newItem);
								else
									newItem.MoveToWorld(from.Location, from.Map);
							}
						}
						//from.SendMessage("Done");
						done = true;
					}
					catch (Exception ex)
					{
						LogFailure(string.Format("{0}", ex.ToString()));
						return;
					}
				}
			}

			if (!done)
				LogFailure("Unable to dupe.  Item must have a 0 parameter constructor.");
			else
				AddResponse("Done.");
		}

		private static void CopyProperties(Item dest, Item src)
		{
			PropertyInfo[] props = src.GetType().GetProperties();

			for (int i = 0; i < props.Length; i++)
			{
				try
				{
					if (props[i].CanRead && props[i].CanWrite)
					{
						//Console.WriteLine( "Setting {0} = {1}", props[i].Name, props[i].GetValue( src, null ) );
						props[i].SetValue(dest, props[i].GetValue(src, null), null);
					}
				}
				catch
				{
					//Console.WriteLine( "Denied" );
				}
			}
		}
	}
}