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

namespace Server
{
	public class UsageAttribute : Attribute
	{
		private string m_Usage;

		public string Usage { get { return m_Usage; } }

		public UsageAttribute(string usage)
		{
			m_Usage = usage;
		}
	}

	public class DescriptionAttribute : Attribute
	{
		private string m_Description;

		public string Description { get { return m_Description; } }

		public DescriptionAttribute(string description)
		{
			m_Description = description;
		}
	}

	public class AliasesAttribute : Attribute
	{
		private string[] m_Aliases;

		public string[] Aliases { get { return m_Aliases; } }

		public AliasesAttribute(params string[] aliases)
		{
			m_Aliases = aliases;
		}
	}
}