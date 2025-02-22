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

/* Scripts/Commands/Abstracted/Implementors/BaseCommandImplementor.cs
 * CHANGELOG
 *	1/3/09, Adam
 *		Move Begin() and End() calls from around ExecuteList() in BaseCommand and move them here to
 *			RunCommand in so they get called for Target as well as Area
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Text;
using System.Collections;
using Server;

namespace Server.Commands
{
	[Flags]
	public enum CommandSupport
	{
		Single = 0x0001,
		Global = 0x0002,
		Online = 0x0004,
		Multi = 0x0008,
		Area = 0x0010,
		Self = 0x0020,
		Region = 0x0040,
		Contained = 0x0080,

		All = Single | Global | Online | Multi | Area | Self | Region | Contained,
		AllMobiles = All & ~Contained,
		AllNPCs = All & ~(Online | Self | Contained),
		AllItems = All & ~(Online | Self | Region),

		Simple = Single | Multi,
		Complex = Global | Online | Area | Region | Contained
	}

	public abstract class BaseCommandImplementor
	{
		public static void RegisterImplementors()
		{
			Register(new RegionCommandImplementor());
			Register(new GlobalCommandImplementor());
			Register(new OnlineCommandImplementor());
			Register(new SingleCommandImplementor());
			Register(new MultiCommandImplementor());
			Register(new AreaCommandImplementor());
			Register(new SelfCommandImplementor());
			Register(new ContainedCommandImplementor());
		}

		private string[] m_Accessors;
		private AccessLevel m_AccessLevel;
		private CommandSupport m_SupportRequirement;
		private Hashtable m_Commands;
		private string m_Usage;
		private string m_Description;
		private bool m_SupportsConditionals;

		public bool SupportsConditionals
		{
			get { return m_SupportsConditionals; }
			set { m_SupportsConditionals = value; }
		}

		public string[] Accessors
		{
			get { return m_Accessors; }
			set { m_Accessors = value; }
		}

		public string Usage
		{
			get { return m_Usage; }
			set { m_Usage = value; }
		}

		public string Description
		{
			get { return m_Description; }
			set { m_Description = value; }
		}

		public AccessLevel AccessLevel
		{
			get { return m_AccessLevel; }
			set { m_AccessLevel = value; }
		}

		public CommandSupport SupportRequirement
		{
			get { return m_SupportRequirement; }
			set { m_SupportRequirement = value; }
		}

		public Hashtable Commands
		{
			get { return m_Commands; }
		}

		public BaseCommandImplementor()
		{
			m_Commands = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		}

		public virtual void Compile(Mobile from, BaseCommand command, ref string[] args, ref object obj)
		{
			obj = null;
		}

		public virtual void Register(BaseCommand command)
		{
			for (int i = 0; i < command.Commands.Length; ++i)
				m_Commands[command.Commands[i]] = command;
		}

		public bool CheckObjectTypes(BaseCommand command, ObjectConditional cond, out bool items, out bool mobiles)
		{
			items = mobiles = false;

			bool condIsItem = cond.IsItem;
			bool condIsMobile = cond.IsMobile;

			switch (command.ObjectTypes)
			{
				case ObjectTypes.All:
				case ObjectTypes.Both:
					{
						if (condIsItem)
							items = true;

						if (condIsMobile)
							mobiles = true;

						break;
					}
				case ObjectTypes.Items:
					{
						if (condIsItem)
						{
							items = true;
						}
						else if (condIsMobile)
						{
							command.LogFailure("You may not use an mobile type condition for this command.");
							return false;
						}

						break;
					}
				case ObjectTypes.Mobiles:
					{
						if (condIsMobile)
						{
							mobiles = true;
						}
						else if (condIsItem)
						{
							command.LogFailure("You may not use an item type condition for this command.");
							return false;
						}

						break;
					}
			}

			return true;
		}

		public void RunCommand(Mobile from, BaseCommand command, string[] args)
		{
			try
			{
				object obj = null;

				Compile(from, command, ref args, ref obj);

				RunCommand(from, obj, command, args);
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
				from.SendMessage(ex.Message);
			}
		}

		public string GenerateArgString(string[] args)
		{
			if (args.Length == 0)
				return "";

			// NOTE: this does not preserve the case where quotation marks are used on a single word

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < args.Length; ++i)
			{
				if (i > 0)
					sb.Append(' ');

				if (args[i].IndexOf(' ') >= 0)
				{
					sb.Append('"');
					sb.Append(args[i]);
					sb.Append('"');
				}
				else
				{
					sb.Append(args[i]);
				}
			}

			return sb.ToString();
		}

		public void RunCommand(Mobile from, object obj, BaseCommand command, string[] args)
		{
			try
			{
				CommandEventArgs e = new CommandEventArgs(from, command.Commands[0], GenerateArgString(args), args);

				if (!command.ValidateArgs(this, e))
					return;

				bool flushToLog = false;

				if (obj is ArrayList)
				{
					ArrayList list = (ArrayList)obj;

					if (list.Count > 20)
						CommandLogging.Enabled = false;
					else if (list.Count == 0)
						command.LogFailure("Nothing was found to use this command on.");

					command.Begin(e);
					command.ExecuteList(e, list);
					command.End(e);

					if (list.Count > 20)
					{
						flushToLog = true;
						CommandLogging.Enabled = true;
					}
				}
				else if (obj != null)
				{
					if (command.ListOptimized)
					{
						ArrayList list = new ArrayList();
						list.Add(obj);
						command.Begin(e);
						command.ExecuteList(e, list);
						command.End(e);
					}
					else
					{
						command.Begin(e);
						command.Execute(e, obj);
						command.End(e);
					}
				}

				command.Flush(from, flushToLog);
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
				from.SendMessage(ex.Message);
			}
		}

		public virtual void Process(Mobile from, object target, BaseCommand command, string[] args)
		{
			RunCommand(from, command, args);
		}

		public virtual void Execute(CommandEventArgs e)
		{
			if (e.Length >= 1)
			{
				BaseCommand command = (BaseCommand)m_Commands[e.GetString(0)];

				if (command == null)
				{
					e.Mobile.SendMessage("That is either an invalid command name or one that does not support this modifier.");
				}
				else if (e.Mobile.AccessLevel < command.AccessLevel)
				{
					e.Mobile.SendMessage("You do not have access to that command.");
				}
				else
				{
					string[] oldArgs = e.Arguments;
					string[] args = new string[oldArgs.Length - 1];

					for (int i = 0; i < args.Length; ++i)
						args[i] = oldArgs[i + 1];

					Process(e.Mobile, null, command, args);
				}
			}
			else
			{
				e.Mobile.SendMessage("You must supply a command name.");
			}
		}

		public void Register()
		{
			if (m_Accessors == null)
				return;

			for (int i = 0; i < m_Accessors.Length; ++i)
				Server.CommandSystem.Register(m_Accessors[i], m_AccessLevel, new CommandEventHandler(Execute));
		}

		public static void Register(BaseCommandImplementor impl)
		{
			m_Implementors.Add(impl);
			impl.Register();
		}

		private static ArrayList m_Implementors;

		public static ArrayList Implementors
		{
			get
			{
				if (m_Implementors == null)
				{
					m_Implementors = new ArrayList();
					RegisterImplementors();
				}

				return m_Implementors;
			}
		}
	}
}