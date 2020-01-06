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

/* Scripts/Engines/ChampionSpawn/CoreChamp.cs
 * created 5/6/04 by mith: variables for champ spawns tweakable by ChampGump.cs
 *  -Moved by plasma from /Engines/CannedEvil/
 * ChangeLog
 *  03/09/07, plasma    
 *      Removed cannedevil namespace reference
 *  01/05/07, plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!

 */

using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Server;
using Server.Engines.ChampionSpawn;

namespace Server
{

	public class CoreChamp
	{
		// Amount of monsters spawned per each level. 
		// The ChampionSpawn code divides this evenly across all of the sub-levels.
		public static int Level1SpawnAmount = 200;
		public static int Level2SpawnAmount = 150;
		public static int Level3SpawnAmount = 100;
		public static int Level4SpawnAmount = 50;

		// Scale from 1-5, 1 = Ruin, 2 = Might, 3 = Force, 4 = Power, 5 = Vanq
		// Anything higher than 5 will seed the randomness with more chance for 
		public static int MinMagicDropLevel = 3;
		public static int MaxMagicDropLevel = 5;
		public static int AmountOfMagicLoot = 6;

		public void ResetToDefaults()
		{
			Level1SpawnAmount = 1320;
			Level2SpawnAmount = 640;
			Level3SpawnAmount = 448;
			Level4SpawnAmount = 152;

			MinMagicDropLevel = 1;
			MaxMagicDropLevel = 3;
			AmountOfMagicLoot = 6;
		}

		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler(OnLoad);
			EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
		}

		public static void OnSave(WorldSaveEventArgs e)
		{
			Console.WriteLine("CoreChamp Saving...");
			if (!Directory.Exists("Saves/Champs"))
				Directory.CreateDirectory("Saves/Champs");

			string filePath = Path.Combine("Saves/Champs", "CoreChamp.xml");

			using (StreamWriter op = new StreamWriter(filePath))
			{
				XmlTextWriter xml = new XmlTextWriter(op);

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument(true);

				xml.WriteStartElement("CoreChamp");

				xml.WriteStartElement("Level1SpawnAmount");
				xml.WriteString(Level1SpawnAmount.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("Level2SpawnAmount");
				xml.WriteString(Level2SpawnAmount.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("Level3SpawnAmount");
				xml.WriteString(Level3SpawnAmount.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("Level4SpawnAmount");
				xml.WriteString(Level4SpawnAmount.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("MinMagicDropLevel");
				xml.WriteString(MinMagicDropLevel.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("MaxMagicDropLevel");
				xml.WriteString(MaxMagicDropLevel.ToString());
				xml.WriteEndElement();

				xml.WriteStartElement("AmountOfMagicLoot");
				xml.WriteString(AmountOfMagicLoot.ToString());
				xml.WriteEndElement();

				xml.WriteEndElement();

				xml.Close();
			}
		}

		public static void OnLoad()
		{
			Console.WriteLine("CoreChamp Loading...");
			string filePath = Path.Combine("Saves/Champs", "CoreChamp.xml");

			if (!File.Exists(filePath))
				return;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlElement root = doc["CoreChamp"];

			Level1SpawnAmount = GetValue(root["Level1SpawnAmount"], Level1SpawnAmount);
			Level2SpawnAmount = GetValue(root["Level2SpawnAmount"], Level2SpawnAmount);
			Level3SpawnAmount = GetValue(root["Level3SpawnAmount"], Level3SpawnAmount);
			Level4SpawnAmount = GetValue(root["Level4SpawnAmount"], Level4SpawnAmount);

			MinMagicDropLevel = GetValue(root["MinMagicDropLevel"], MinMagicDropLevel);
			MaxMagicDropLevel = GetValue(root["MaxMagicDropLevel"], MaxMagicDropLevel);
			AmountOfMagicLoot = GetValue(root["AmountOfMagicLoot"], AmountOfMagicLoot);
		}

		public static int GetValue(XmlElement node, int defaultValue)
		{
			try
			{
				return XmlConvert.ToInt32(node.InnerText);
			}
			catch
			{
				try
				{
					return Convert.ToInt32(node.InnerText);
				}
				catch
				{
					return defaultValue;
				}
			}
		}
	}
}