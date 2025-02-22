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

/* Scripts/Commands/Properties.cs
 * CHANGELOG
 *	3/8/2016, Adam
 *		Add typeofDateTime to IsParsable() so that dateTime values may be correctly parsed when 
 *		they have two parts like: 3/8/2016 08:00:00
 *	7/17/11, Adam
 *		allow enum flags to be passed and parsed in [set flags flag1|flag2|flag3...
 *	6/7/2007, Pix
 *		Added InternalGetOnlyValue() and GetOnlyValue()
 *  07/26/06, Rhiannon
 *		Changed InternalSetValue() to prevent changing access level to Owner or ReadOnly.
 *	02/28/05, erlein
 *		Added check + set of LastProps() for ChestItemSpawner type.
 *	02/27/05, erlein
 *		Added the same Spawner type check in the setvalue routine
 *		to cover [set command usage.
 *	02/27/05, erlein
 *		Added check for Spawner type in props target to store mobile
 *		using command on it (for logging purposes).
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;

using CPA = Server.CommandPropertyAttribute;

namespace Server.Commands
{
	public class Properties
	{
		public static void Register()
		{
			Server.CommandSystem.Register("Props", AccessLevel.Counselor, new CommandEventHandler(Props_OnCommand));
			Server.CommandSystem.Register("GuildProps", AccessLevel.Counselor, new CommandEventHandler(GuildProps_OnCommand));
		}

		private class PropsTarget : Target
		{
			private bool m_Normal;

			public PropsTarget(bool normal)
				: base(-1, true, TargetFlags.None)
			{
				m_Normal = normal;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (!BaseCommand.IsAccessible(from, o))
					from.SendMessage("That is not accessible.");
				else if (m_Normal)
					from.SendGump(new PropertiesGump(from, o));
				else if (o is Guildstone)
					from.SendGump(new PropertiesGump(from, ((Guildstone)o).Guild));

				//erl: record in spawner mobile calling props on it
				if (o.GetType().Name == "Spawner")
				{
					((Spawner)o).LastProps = from;
				}
				else if (o.GetType().Name == "ChestItemSpawner")
				{
					((ChestItemSpawner)o).LastProps = from;
				}

			}
		}

		[Usage("Props [serial]")]
		[Description("Opens a menu where you can view and edit all properties of a targeted (or specified) object.")]
		private static void Props_OnCommand(CommandEventArgs e)
		{
			if (e.Length == 1)
			{
				IEntity ent = World.FindEntity(e.GetInt32(0));

				if (ent == null)
					e.Mobile.SendMessage("No object with that serial was found.");
				else if (!BaseCommand.IsAccessible(e.Mobile, ent))
					e.Mobile.SendMessage("That is not accessible.");
				else
					e.Mobile.SendGump(new PropertiesGump(e.Mobile, ent));
			}
			else
			{
				e.Mobile.Target = new PropsTarget(true);
			}
		}

		[Usage("GuildProps")]
		[Description("Opens a menu where you can view and edit guild properties of a targeted guild stone.")]
		private static void GuildProps_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new PropsTarget(false);
		}

		private static bool CIEqual(string l, string r)
		{
			return Insensitive.Equals(l, r);
		}

		private static Type typeofCPA = typeof(CPA);

		public static CPA GetCPA(PropertyInfo p)
		{
			object[] attrs = p.GetCustomAttributes(typeofCPA, false);

			if (attrs.Length == 0)
				return null;

			return attrs[0] as CPA;
		}

		public static PropertyInfo[] GetPropertyInfoChain(Mobile from, Type type, string propertyString, bool isReading, ref string failReason)
		{
			string[] split = propertyString.Split('.');

			if (split.Length == 0)
				return null;

			PropertyInfo[] info = new PropertyInfo[split.Length];

			for (int i = 0; i < info.Length; ++i)
			{
				string propertyName = split[i];

				if (CIEqual(propertyName, "current"))
					continue;

				PropertyInfo[] props = type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

				bool reading = true;

				if (i == info.Length - 1)
					reading = isReading;

				for (int j = 0; j < props.Length; ++j)
				{
					PropertyInfo p = props[j];

					if (CIEqual(p.Name, propertyName))
					{
						CPA attr = GetCPA(p);

						if (attr == null)
						{
							failReason = String.Format("Property '{0}' not found.", propertyName);
							return null;
						}

						if (from.AccessLevel < (reading ? attr.ReadLevel : attr.WriteLevel))
						{
							failReason = String.Format("You must be at least {0} to {1} the property '{2}'.",
								Mobile.GetAccessLevelName((reading ? attr.ReadLevel : attr.WriteLevel)), reading ? "get" : "set", propertyName);

							return null;
						}

						if (from.AccessLevel < (reading ? attr.ReadLevel : attr.WriteLevel))
						{
							failReason = String.Format("You must be at least {0} to {1} the property '{2}'.",
								Mobile.GetAccessLevelName(attr.ReadLevel), reading ? "get" : "set", propertyName);

							return null;
						}
						if (reading && !p.CanRead)
						{
							failReason = String.Format("Property '{0}' is write only.", propertyName);
							return null;
						}
						else if (!reading && !p.CanWrite)
						{
							failReason = String.Format("Property '{0}' is read only.", propertyName);
							return null;
						}

						info[i] = p;
						type = p.PropertyType;
						break;
					}
				}

				if (info[i] == null)
				{
					failReason = String.Format("Property '{0}' not found.", propertyName);
					return null;
				}
			}

			return info;
		}

		public static PropertyInfo GetPropertyInfo(Mobile from, ref object obj, string propertyName, bool reading, ref string failReason)
		{
			PropertyInfo[] chain = GetPropertyInfoChain(from, obj.GetType(), propertyName, reading, ref failReason);

			if (chain == null)
				return null;

			return GetPropertyInfo(ref obj, chain, ref failReason);
		}

		public static PropertyInfo GetPropertyInfo(ref object obj, PropertyInfo[] chain, ref string failReason)
		{
			if (chain == null || chain.Length == 0)
			{
				failReason = "Property chain is empty.";
				return null;
			}

			for (int i = 0; i < chain.Length - 1; ++i)
			{
				if (chain[i] == null)
					continue;

				obj = chain[i].GetValue(obj, null);

				if (obj == null)
				{
					failReason = String.Format("Property '{0}' is null.", chain[i]);
					return null;
				}
			}

			return chain[chain.Length - 1];
		}

		public static string GetValue(Mobile from, object o, string name)
		{
			string failReason = "";
			PropertyInfo p = GetPropertyInfo(from, ref o, name, true, ref failReason);

			if (p == null)
				return failReason;

			return InternalGetValue(o, p);
		}
		public static string GetOnlyValue(Mobile from, object o, string name)
		{
			string failReason = "";
			PropertyInfo p = GetPropertyInfo(from, ref o, name, true, ref failReason);

			if (p == null)
			{
				from.SendMessage("Property \"{0}\" does not exist on that object.", name);
				return null;
			}
			return InternalGetOnlyValue(o, p);
		}

		public static string IncreaseValue(Mobile from, object o, string[] args)
		{
			Type type = o.GetType();

			PropertyInfo[] props = type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] realProps = new PropertyInfo[args.Length / 2];
			int[] realValues = new int[args.Length / 2];

			bool positive = false, negative = false;

			for (int i = 0; i < realProps.Length; ++i)
			{
				string name = args[i * 2];

				try
				{
					string valueString = args[1 + (i * 2)];

					if (valueString.StartsWith("0x"))
					{
						realValues[i] = Convert.ToInt32(valueString.Substring(2), 16);
					}
					else
					{
						realValues[i] = Convert.ToInt32(valueString);
					}
				}
				catch
				{
					return "Offset value could not be parsed.";
				}

				if (realValues[i] > 0)
					positive = true;
				else if (realValues[i] < 0)
					negative = true;
				else
					return "Zero is not a valid value to offset.";

				foreach (PropertyInfo p in props)
				{
					if (CIEqual(p.Name, name))
					{
						CPA attr = GetCPA(p);

						if (attr == null)
							return "Property not found.";

						if (from.AccessLevel < attr.ReadLevel)
							return String.Format("Getting this property requires at least {0} access level.", Mobile.GetAccessLevelName(attr.ReadLevel));

						if (!p.CanRead)
							return "Property is write only.";

						if (from.AccessLevel < attr.WriteLevel)
							return String.Format("Setting this property requires at least {0} access level.", Mobile.GetAccessLevelName(attr.WriteLevel));

						if (!p.CanWrite)
							return "Property is read only.";

						realProps[i] = p;
					}
				}

				if (realProps[i] == null)
					return "Property not found.";
			}

			for (int i = 0; i < realProps.Length; ++i)
			{
				object obj = realProps[i].GetValue(o, null);
				long v = (long)Convert.ChangeType(obj, TypeCode.Int64);
				v += realValues[i];

				realProps[i].SetValue(o, Convert.ChangeType(v, realProps[i].PropertyType), null);
			}

			if (realProps.Length == 1)
			{
				if (positive)
					return "The property has been increased.";

				return "The property has been decreased.";
			}

			if (positive && negative)
				return "The properties have been changed.";

			if (positive)
				return "The properties have been increased.";

			return "The properties have been decreased.";
		}

		private static string InternalGetValue(object o, PropertyInfo p)
		{
			Type type = p.PropertyType;

			object value = p.GetValue(o, null);
			string toString;

			if (value == null)
				toString = "(-null-)";
			else if (value is AccessLevel)
			{
				if ((AccessLevel)value == AccessLevel.Ignore)
					return AccessLevel.Player.ToString();
				else
					return value.ToString();
			}
			else if (IsNumeric(type))
				toString = String.Format("{0} (0x{0:X})", value);
			else if (IsChar(type))
				toString = String.Format("'{0}' ({1} [0x{1:X}])", value, (int)value);
			else if (IsString(type))
				toString = String.Format("\"{0}\"", value);
			else
				toString = value.ToString();

			return String.Format("{0} = {1}", p.Name, toString);
		}
		//Pix: stupid fucking code, I want JUST the value!
		private static string InternalGetOnlyValue(object o, PropertyInfo p)
		{
			Type type = p.PropertyType;

			object value = p.GetValue(o, null);
			string toString;

			if (value == null)
				toString = "(-null-)";
			//else if (IsNumeric(type))
			//    toString = String.Format("{0} (0x{0:X})", value);
			//else if (IsChar(type))
			//    toString = String.Format("'{0}' ({1} [0x{1:X}])", value, (int)value);
			//else if (IsString(type))
			//    toString = String.Format("\"{0}\"", value);
			else
				toString = value.ToString();

			//return String.Format("{0} = {1}", p.Name, toString);
			return toString;
		}

		public static string SetValue(Mobile from, object o, string name, string value)
		{
			//erl: record in spawner mobile calling props on it
			if (o is Spawner)
			{
				((Spawner)o).LastProps = from;
			}
			else if (o is ChestItemSpawner)
			{
				((ChestItemSpawner)o).LastProps = from;
			}

			object logObject = o;

			string failReason = "";
			PropertyInfo p = GetPropertyInfo(from, ref o, name, false, ref failReason);

			if (p == null)
				return failReason;

			return InternalSetValue(from, logObject, o, p, name, value, true);
		}

		private static Type typeofType = typeof(Type);

		private static bool IsType(Type t)
		{
			return (t == typeofType);
		}

		private static Type typeofChar = typeof(Char);

		private static bool IsChar(Type t)
		{
			return (t == typeofChar);
		}

		private static Type typeofString = typeof(String);

		private static bool IsString(Type t)
		{
			return (t == typeofString);
		}

		private static bool IsEnum(Type t)
		{
			return t.IsEnum;
		}

		private static Type typeofTimeSpan = typeof(TimeSpan);
		private static Type typeofDateTime = typeof(DateTime);
		private static Type typeofParsable = typeof(ParsableAttribute);

		private static bool IsParsable(Type t)
		{
			return (t == typeofTimeSpan || t == typeofDateTime || t.IsDefined(typeofParsable, false));
		}

		private static Type[] m_ParseTypes = new Type[] { typeof(string) };
		private static object[] m_ParseParams = new object[1];

		private static object Parse(object o, Type t, string value)
		{
			MethodInfo method = t.GetMethod("Parse", m_ParseTypes);

			m_ParseParams[0] = value;

			return method.Invoke(o, m_ParseParams);
		}

		private static Type[] m_NumericTypes = new Type[]
			{
				typeof( Byte ), typeof( SByte ),
				typeof( Int16 ), typeof( UInt16 ),
				typeof( Int32 ), typeof( UInt32 ),
				typeof( Int64 ), typeof( UInt64 )
			};

		private static bool IsNumeric(Type t)
		{
			return (Array.IndexOf(m_NumericTypes, t) >= 0);
		}

		public static string ConstructFromString(Type type, object obj, string value, ref object constructed)
		{
			object toSet;

			if (value == "(-null-)" && !type.IsValueType)
				value = null;

			if (IsEnum(type))
			{
				try
				{
					int total=0;
					if (value.Contains("|"))
					{	// allow enum flags to be passed. I.e., flag1|flag2|flag3...
						toSet = null;
						string[] bits = value.Split(new Char[] { '|' });
						foreach (string s in bits)
							total += (int)Enum.Parse(type, s, true);

						toSet = Enum.Parse(type, total.ToString(), true);
					}
					else
						toSet = Enum.Parse(type, value, true);
				}
				catch
				{
					return "That is not a valid enumeration member.";
				}
			}
			else if (IsType(type))
			{
				try
				{
					toSet = ScriptCompiler.FindTypeByName(value);

					if (toSet == null)
						return "No type with that name was found.";
				}
				catch
				{
					return "No type with that name was found.";
				}
			}
			else if (IsParsable(type))
			{
				try
				{
					toSet = Parse(obj, type, value);
				}
				catch
				{
					return "That is not properly formatted.";
				}
			}
			else if (value == null)
			{
				toSet = null;
			}
			else if (value.StartsWith("0x") && IsNumeric(type))
			{
				try
				{
					toSet = Convert.ChangeType(Convert.ToUInt64(value.Substring(2), 16), type);
				}
				catch
				{
					return "That is not properly formatted.";
				}
			}
			else
			{
				try
				{
					toSet = Convert.ChangeType(value, type);
				}
				catch
				{
					return "That is not properly formatted.";
				}
			}

			constructed = toSet;
			return null;
		}

		public static string InternalSetValue(Mobile from, object logobj, object o, PropertyInfo p, string pname, string value, bool shouldLog)
		{
			object toSet = null;
			string result = ConstructFromString(p.PropertyType, o, value, ref toSet);

			if (result != null)
				return result;

			if ((from.AccessLevel < AccessLevel.Owner) && (value == "owner"))
			{
				return "You do not have access to change AccessLevel to Owner";
			}

			if (value == "readonly")
			{
				return "You cannot change AccessLevel to ReadOnly.";
			}

			try
			{
				if (shouldLog)
					CommandLogging.LogChangeProperty(from, logobj, pname, value);

				p.SetValue(o, toSet, null);
				return "Property has been set.";
			}
			catch
			{
				return "An exception was caught, the property may not be set.";
			}
		}
	}
}