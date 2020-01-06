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
 *	restrictions set forth in subaragraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */

/* Misc\XmlUtility.cs
 * CHANGELOG:
 *	01/13/06, Pix
 *		Needed Utility class for XML.
 *		(TCCS and BountyKeeper systems should be changed to use this class)
 *		Any generic XML functions should be moved here.
 */

using System;
using System.Xml;

namespace Server
{
	/// <summary>
	/// Contains utility functions for working with Xml.
	/// </summary>
	public class XmlUtility
	{
		public XmlUtility()
		{
		}

		public static string GetText(XmlElement node, string defaultValue)
		{
			if (node == null)
				return defaultValue;

			return node.InnerText;
		}

		public static DateTime GetDateTime(string dateTimeString, DateTime defaultValue)
		{
			try
			{
				return XmlConvert.ToDateTime(dateTimeString);
			}
			catch
			{
				try
				{
					return DateTime.Parse(dateTimeString);
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public static int GetInt32(string intString, int defaultValue)
		{
			try
			{
				return XmlConvert.ToInt32(intString);
			}
			catch
			{
				try
				{
					return Convert.ToInt32(intString);
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public static double GetDouble(string dblString, double defaultValue)
		{
			if (dblString == null || dblString.Length == 0)
				return defaultValue;

			try
			{
				return XmlConvert.ToDouble(dblString);
			}
			catch
			{
				try
				{
					return Convert.ToDouble(dblString);
				}
				catch
				{
					return defaultValue;
				}
			}
		}

	}
}
