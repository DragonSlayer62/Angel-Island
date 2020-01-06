using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Ethics
{
	public class PowerDefinition
	{
		private int m_Power;	// lifeforce in old ethics system
		private int m_Sphere;	// basically the circle to cast

		private TextDefinition m_Name;
		private TextDefinition m_Phrase;
		private TextDefinition m_Description;

		public int Power { get { return m_Power; } }	// lifeforce in old ethics system
		public int Sphere { get { return m_Sphere; } }	

		public TextDefinition Name { get { return m_Name; } }
		public TextDefinition Phrase { get { return m_Phrase; } }
		public TextDefinition Description { get { return m_Description; } }

		public PowerDefinition(int power, int sphere, TextDefinition name, TextDefinition phrase, TextDefinition description)
		{
			m_Power = power;
			m_Sphere = sphere;
			m_Name = name;
			m_Phrase = phrase;
			m_Description = description;
		}
	}
}
