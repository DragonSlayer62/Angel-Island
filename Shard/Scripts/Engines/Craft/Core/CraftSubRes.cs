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

namespace Server.Engines.Craft
{
	public class CraftSubRes
	{
		private Type m_Type;
		private double m_ReqSkill;
		private string m_NameString;
		private int m_NameNumber;
		private int m_GenericNameNumber;
		private object m_Message;

		public CraftSubRes(Type type, string name, double reqSkill, object message)
		{
			m_Type = type;
			m_NameString = name;
			m_ReqSkill = reqSkill;
			m_Message = message;
		}

		public CraftSubRes(Type type, int name, double reqSkill, object message)
		{
			m_Type = type;
			m_NameNumber = name;
			m_ReqSkill = reqSkill;
			m_Message = message;
		}

		public CraftSubRes(Type type, int name, double reqSkill, int genericNameNumber, object message)
		{
			m_Type = type;
			m_NameNumber = name;
			m_ReqSkill = reqSkill;
			m_GenericNameNumber = genericNameNumber;
			m_Message = message;
		}

		public Type ItemType
		{
			get { return m_Type; }
		}

		public string NameString
		{
			get { return m_NameString; }
		}

		public int NameNumber
		{
			get { return m_NameNumber; }
		}

		public int GenericNameNumber
		{
			get { return m_GenericNameNumber; }
		}

		public object Message
		{
			get { return m_Message; }
		}

		public double RequiredSkill
		{
			get { return m_ReqSkill; }
		}
	}
}