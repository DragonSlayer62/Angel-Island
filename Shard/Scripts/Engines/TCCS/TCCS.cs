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

/* Scripts/Engines/TCCS/TCCS.cs
 * ChangeLog
 *	5/17/10, Adam
 *		o Add new m_EntryID for finding and deleting entries
 *		o Add new find and remove methods
 *  11/19/06 Plasma
 *      Fixed 2.0 warning
 *	01/13/06 Pix
 *		Moved.  Changed namespace.  Separated out PJUM aspects.
 *		Replaced BountyKeeper Xml function uses with new XmlUtility class.
 *	11/20/05 Pix
 *		Removed extraneous DecayDisableUpdateEntries() call on Save.
 *  10/06/05 Taran Kain
 *		Added sanity check initialization code
 *	3/29/05, Adam
 *		Change the text displayed from "DateTime" to "Expires"
 *	3/28/05 - Pix
 *		Added UpdateLocations to update the locations of the macroers.
 *	3/28/05 - Pix
 *		Added utility function to tell whether a mobile's already reported.
 *	3/27/05 - Pix
 *		Added macroers to be made criminal on the heartbeat.
 *	1/30/05 - Pix
 *		Added better [tccs support (and fixed problems)
 *	1/27/05 - Pix
 *		Initial Version.
 */

using System;
using System.Collections;
using System.IO;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Commands;

namespace Server.Engines
{
	/// <summary>
	/// Contains the types of List Entries for use in other systems.
	/// </summary>
	public enum ListEntryType { Unknown, Generic, PJUM, TownCrier };

	/// <summary>
	/// All Town Crier Messages are of type ListEntry.
	/// Entries only require the Lines, Enabled, and DateTime be set.
	/// </summary>
	public class ListEntry
	{
		private Mobile m_Mobile;
		private DateTime m_DateTime;
		private string[] m_Lines;
		private bool m_Enabled;
		private ListEntryType m_Type = ListEntryType.Unknown;
		private double m_EntryID = 0.0;							// used for searching and deleting across reboots

		public ListEntry()
		{
		}

		public ListEntry(string[] lines, Mobile m, DateTime dt, ListEntryType type)
		{
			m_Lines = lines;
			m_Mobile = m;
			m_DateTime = dt;
			m_Enabled = true;
			m_Type = type;
			m_EntryID = ((TimeSpan)(DateTime.Now - DateTime.MinValue)).TotalMilliseconds;
		}

		public bool Enabled
		{
			get { return m_Enabled; }
			set { m_Enabled = value; }
		}

		public string[] Lines
		{
			get { return m_Lines; }
		}

		public Mobile Mobile
		{
			get { return m_Mobile; }
		}

		public DateTime DateTime
		{
			get { return m_DateTime; }
		}

		public ListEntryType Type
		{
			get { return m_Type; }
		}

		public double EntryID
		{
			get { return m_EntryID; }
		}

		public void Save(XmlTextWriter xml)
		{
			xml.WriteStartElement("listentry");

			if (m_Mobile != null)
			{
				string strMobile = m_Mobile.Serial.Value.ToString();
				xml.WriteStartElement("mobile");
				xml.WriteString(strMobile);
				xml.WriteEndElement();
			}

			xml.WriteStartElement("datetime");
			xml.WriteString(XmlConvert.ToString(m_DateTime, XmlDateTimeSerializationMode.Unspecified));
			xml.WriteEndElement();

			xml.WriteStartElement("linecount");
			xml.WriteString(m_Lines.Length.ToString());
			xml.WriteEndElement();

			for (int i = 0; i < m_Lines.Length; i++)
			{
				xml.WriteStartElement("line" + i);
				xml.WriteString(m_Lines[i]);
				xml.WriteEndElement();
			}

			xml.WriteStartElement("enabled");
			if (m_Enabled)
				xml.WriteString("true");
			else
				xml.WriteString("false");
			xml.WriteEndElement();

			xml.WriteStartElement("type");
			xml.WriteString(((int)m_Type).ToString());
			xml.WriteEndElement();

			xml.WriteStartElement("id");
			xml.WriteString(m_EntryID.ToString());
			xml.WriteEndElement();

			xml.WriteEndElement();
		}

		public ListEntry(XmlElement node)
		{
			string strValue = XmlUtility.GetText(node["enabled"], "true");
			if (strValue.Equals("true"))
				m_Enabled = true;
			else
				m_Enabled = false;

			m_DateTime = XmlUtility.GetDateTime(XmlUtility.GetText(node["datetime"], null), DateTime.Now);

			int linecount = XmlUtility.GetInt32(XmlUtility.GetText(node["linecount"], "0"), 0);
			m_Lines = new string[linecount];
			for (int i = 0; i < linecount; i++)
			{
				m_Lines[i] = XmlUtility.GetText(node["line" + i], "????");
			}

			int serial = XmlUtility.GetInt32(XmlUtility.GetText(node["mobile"], "0"), 0);
			if (serial != 0)
			{
				m_Mobile = (PlayerMobile)World.FindMobile(serial);
			}

			m_Type = (ListEntryType)XmlUtility.GetInt32(XmlUtility.GetText(node["type"], "0"), 0);

			m_EntryID = XmlUtility.GetDouble(XmlUtility.GetText(node["id"], "0"), m_EntryID);
		}

	}

	/// <summary>
	/// Summary description for TCCS.
	/// </summary>
	public class TCCS
	{
		private static ArrayList m_List;

		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler(OnLoad);
			EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
		}

		public static void Initialize()
		{
			CommandSystem.Register("TCCS", AccessLevel.Administrator, new CommandEventHandler(TCCS_OnCommand));
			if (m_List == null)
			{
				m_List = new ArrayList();
			}
		}

		public static void TCCS_OnCommand(CommandEventArgs e)
		{
			try
			{
				Mobile m = e.Mobile;

				bool showformat = true;

				if (e.Arguments.Length == 1 && e.Arguments[0].ToLower() == "list")
				{
					e.Mobile.SendMessage("TCCS Info:");
					e.Mobile.SendMessage("   There are {0} entries.", TCCS.TheList.Count);
					for (int i = 0; i < TCCS.TheList.Count; i++)
					{
						ListEntry x = (ListEntry)TCCS.TheList[i];
						e.Mobile.SendMessage(" [{0}]::Mob: {1}, Enabled: {2}, Expires: {3}",
							i, x.Mobile, x.Enabled, x.DateTime);
					}
					showformat = false;
				}
				else if (e.Arguments.Length > 1)
				{
					int number = 0;
					bool invalid = false;
					try
					{
						number = Int32.Parse(e.Arguments[1]);
					}
					catch
					{
						invalid = true;
					}

					if (number >= TCCS.TheList.Count || TCCS.TheList.Count == 0 || number < 0)
					{
						m.SendMessage("{0} is out of range.  Max is currently {1}", number, TCCS.TheList.Count - 1);
						invalid = true;
						showformat = false;
					}

					if (!invalid)
					{
						showformat = false;
						if (e.Arguments[0].ToLower() == "show")
						{
							ListEntry x = (ListEntry)TCCS.TheList[number];
							m.SendMessage("TCCS at {0}:", number);
							m.SendMessage("   Mob: {0}, Enabled: {1}, DateTime: {2}",
								x.Mobile, x.Enabled, x.DateTime);
						}
						else if (e.Arguments[0].ToLower() == "delete")
						{
							TCCS.TheList.RemoveAt(number);
							m.SendMessage("ListEntry deleted");
						}
					}
				}

				if (showformat)
				{
					m.SendMessage("format is:");
					m.SendMessage("[TCCS <list|show|delete> <number>");
					m.SendMessage("Always do TCCS show before TCCS delete.");
				}
			}
			catch (Exception exc)
			{
				LogHelper.LogException(exc);
				System.Console.WriteLine("Exception caught in TCCS_OnCommand: {0}", exc.Message);
				System.Console.WriteLine("Command was: {0}", e.Command);
				System.Console.WriteLine(exc.StackTrace);
			}
		}

		public static ArrayList TheList
		{
			get
			{
				if (m_List == null)
				{
					m_List = new ArrayList();
				}

				return m_List;
			}
		}

		public static void AddEntry(ListEntry entry)
		{
			TheList.Add(entry);

			ArrayList instances = TownCrier.Instances;

			for (int i = 0; i < instances.Count; ++i)
				((TownCrier)instances[i]).ForceBeginAutoShout();
		}

		public static bool RemoveEntry(ListEntry entry)
		{
			if (TheList != null && TheList.Count > 0 && TheList.Contains(entry))
			{
				TheList.Remove(entry);
				return true;
			}

			return false;
		}

		public static bool RemoveEntry(double id)
		{
			if (TheList != null && TheList.Count > 0)
			{
				for (int ix = 0; ix < TheList.Count; ix++)
				{
					ListEntry le = TheList[ix] as ListEntry;
					if (le != null)
					{
						if (le.EntryID == id)
						{	// okay to change the list here since we are exiting the enumeration
							RemoveEntry(le);
							return true;
						}
					}
				}
			}

			return false;
		}

		public static ListEntry FindEntry(double id)
		{
			if (TheList != null && TheList.Count > 0)
			{
				for (int ix = 0; ix < TheList.Count; ix++)
				{
					ListEntry le = TheList[ix] as ListEntry;
					if (le != null)
					{
						if (le.EntryID == id)
							return le;
					}
				}
			}

			return null;
		}

		public static ListEntry FindEntry(Mobile m)
		{
			if (TheList != null && TheList.Count > 0)
			{
				for (int ix = 0; ix < TheList.Count; ix++)
				{
					ListEntry le = TheList[ix] as ListEntry;
					if (le != null)
					{
						if (le.Mobile == m)
							return le;
					}
				}
			}

			return null;
		}

		public static void DecayDisableUpdateEntries()
		{
			ArrayList toDelete = new ArrayList();

			foreach (ListEntry le in TheList)
			{
				if (le.DateTime < DateTime.Now)
				{
					toDelete.Add(le);
					le.Enabled = false;
				}

				if (le.Mobile != null)
				{
					if (!le.Mobile.Alive || le.Mobile.Map == Map.Internal)
					{
						le.Enabled = false;
					}
					else
					{
						le.Enabled = true;
					}
				}
			}

			for (int i = 0; i < toDelete.Count; i++)
			{
				TheList.Remove(toDelete[i]);
			}
		}


		public static ListEntry GetRandomEntry()
		{
			if (m_List != null)
			{
				if (m_List.Count != 0)
				{
					DecayDisableUpdateEntries();

					bool bAtLeastOne = false;
					//make sure there's at least one enabled
					for (int i = 0; i < m_List.Count; i++)
					{
						if (((ListEntry)m_List[i]).Enabled)
						{
							bAtLeastOne = true;
						}
					}

					if (bAtLeastOne)
					{
						int c = 0;
						c = Utility.Random(m_List.Count);
						ListEntry ret = (ListEntry)m_List[c];
						while (ret.Enabled == false)
						{
							c = Utility.Random(m_List.Count);
							ret = (ListEntry)m_List[c];
						}
						return ret;
					}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}


		public static void OnSave(WorldSaveEventArgs e)
		{
			System.Console.WriteLine("TCCS Saving...");

			try
			{
				if (!Directory.Exists("Saves/AngelIsland"))
					Directory.CreateDirectory("Saves/AngelIsland");

				string filePath = Path.Combine("Saves/AngelIsland", "TCCS.xml");

				using (StreamWriter op = new StreamWriter(filePath))
				{
					XmlTextWriter xml = new XmlTextWriter(op);

					xml.Formatting = Formatting.Indented;
					xml.IndentChar = '\t';
					xml.Indentation = 1;

					xml.WriteStartDocument(true);

					xml.WriteStartElement("TCCS");

					xml.WriteAttributeString("count", TCCS.TheList.Count.ToString());

					foreach (ListEntry le in TCCS.TheList)
					{
						le.Save(xml);
					}

					xml.WriteEndElement();

					xml.Close();
				}
			}
			catch (Exception exc)
			{
				LogHelper.LogException(exc);
				System.Console.WriteLine("Error in TCCS.OnSave(): " + exc.Message);
				System.Console.WriteLine(exc.StackTrace);
			}
		}

		public static void OnLoad()
		{
			System.Console.WriteLine("TCCS Loading...");

			string filePath = Path.Combine("Saves/AngelIsland", "TCCS.xml");

			if (!File.Exists(filePath))
			{
				return;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(filePath);

				XmlElement root = doc["TCCS"];

				foreach (XmlElement entry in root.GetElementsByTagName("listentry"))
				{
					try
					{
						ListEntry le = new ListEntry(entry);

						TCCS.TheList.Add(le);
					}
					catch
					{
						Console.WriteLine("Warning: A TCCS ListEntry instance load failed");
					}
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}

	}
}
