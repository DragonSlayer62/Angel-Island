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
using Server;

namespace Server.Spells
{
	public class SpellInfo
	{
		private string m_Name;
		private string m_Mantra;
		private SpellCircle m_Circle;
		private Type[] m_Reagents;
		private int[] m_Amounts;
		private int m_Action;
		private bool m_AllowTown;
		private int m_LeftHandEffect, m_RightHandEffect;

		public SpellInfo(string name, string mantra, SpellCircle circle, params Type[] regs)
			: this(name, mantra, circle, 16, 0, 0, true, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, bool allowTown, params Type[] regs)
			: this(name, mantra, circle, 16, 0, 0, allowTown, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, int action, params Type[] regs)
			: this(name, mantra, circle, action, 0, 0, true, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, int action, bool allowTown, params Type[] regs)
			: this(name, mantra, circle, action, 0, 0, allowTown, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, int action, int handEffect, params Type[] regs)
			: this(name, mantra, circle, action, handEffect, handEffect, true, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, int action, int handEffect, bool allowTown, params Type[] regs)
			: this(name, mantra, circle, action, handEffect, handEffect, allowTown, regs)
		{
		}

		public SpellInfo(string name, string mantra, SpellCircle circle, int action, int leftHandEffect, int rightHandEffect, bool allowTown, params Type[] regs)
		{
			m_Name = name;
			m_Mantra = mantra;
			m_Circle = circle;
			m_Action = action;
			m_Reagents = regs;
			m_AllowTown = allowTown;

			m_LeftHandEffect = leftHandEffect;
			m_RightHandEffect = rightHandEffect;

			m_Amounts = new int[regs.Length];

			for (int i = 0; i < regs.Length; ++i)
				m_Amounts[i] = 1;
		}

		public int Action { get { return m_Action; } set { m_Action = value; } }
		public bool AllowTown { get { return m_AllowTown; } set { m_AllowTown = value; } }
		public int[] Amounts { get { return m_Amounts; } set { m_Amounts = value; } }
		public SpellCircle Circle { get { return m_Circle; } set { m_Circle = value; } }
		public string Mantra { get { return m_Mantra; } set { m_Mantra = value; } }
		public string Name { get { return m_Name; } set { m_Name = value; } }
		public Type[] Reagents { get { return m_Reagents; } set { m_Reagents = value; } }
		public int LeftHandEffect { get { return m_LeftHandEffect; } set { m_LeftHandEffect = value; } }
		public int RightHandEffect { get { return m_RightHandEffect; } set { m_RightHandEffect = value; } }
	}
}