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
using System.IO;
using Microsoft.Win32;
using Server;

namespace Server.Misc
{
	public class DataPath
	{
		/* If you have not installed Ultima Online,
		 * or wish the server to use a seperate set of datafiles,
		 * change the 'CustomPath' value, example:
		 * 
		 * private const string CustomPath = @"C:\Program Files\Ultima Online";
		 */
		private static string CustomPath = null;

		/* The following is a list of files which a required for proper execution:
		 * 
		 * Multi.idx
		 * Multi.mul
		 * VerData.mul
		 * TileData.mul
		 * Map*.mul
		 * StaIdx*.mul
		 * Statics*.mul
		 * MapDif*.mul
		 * MapDifL*.mul
		 * StaDif*.mul
		 * StaDifL*.mul
		 * StaDifI*.mul
		 */

		public static void Configure()
		{
			string pathReg = GetExePath("Ultima Online");
			string pathTD = GetExePath("Ultima Online Third Dawn");

			if (CustomPath != null)
				Core.DataDirectories.Add(CustomPath);

			if (pathReg != null)
				Core.DataDirectories.Add(pathReg);

			if (pathTD != null)
				Core.DataDirectories.Add(pathTD);

			if (Core.DataDirectories.Count == 0)
			{
				Console.WriteLine("Enter the Ultima Online directory:");
				Console.Write("> ");

				Core.DataDirectories.Add(Console.ReadLine());
			}
		}

		private static string GetExePath(string subName)
		{
			try
			{
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\Origin Worlds Online\{0}\1.0", subName)))
				{
					if (key == null)
						return null;

					string v = key.GetValue("ExePath") as string;

					if (v == null || v.Length <= 0)
						return null;

					if (!File.Exists(v))
						return null;

					v = Path.GetDirectoryName(v);

					if (v == null)
						return null;

					return v;
				}
			}
			catch
			{
				return null;
			}
		}
	}
}