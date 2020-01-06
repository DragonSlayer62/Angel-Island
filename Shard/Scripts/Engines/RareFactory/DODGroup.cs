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

/* Scripts/Engines/RareFactory/DODGroup.cs
 * ChangeLog:
 *  6/8/07, Adam
 *      Add group versioning.
 *  6/7/07, Adam
 *      Rewrite I/O to correct a logic error that was corrupting the database.
 *          You MUST pass prefixstring=true if you are writing strings with BinaryFileWriter
 *	17/Mar/2007, weaver
 *		- Added rarity property
 *		- Removed unnecessary namespace includes.
 *	28/Feb/2007, weaver
 *		Initial creation.
 * 
 */

using System;
using System.IO;
using System.Collections;
using Server.Items;

namespace Server.Engines
{

	// DODGroup (Dynamic Object Definition Group)
	// 
	// Holds references to the DODinstances
	//

	public class DODGroup
	{
		// Rarity level (0= default, ordinary; 1= not very rare really; 10= omfg unique)
		private short m_Rarity;
		public short Rarity
		{
			get
			{
				return m_Rarity;
			}
			set
			{
				m_Rarity = value;
			}
		}

		// Name of the group
		private string m_Name;
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		// References to DOD instances belonging
		// to this DOD group
		private ArrayList m_DODInst;
		public ArrayList DODInst
		{
			get
			{
				return m_DODInst;
			}
			set
			{
				m_DODInst = value;
			}
		}


		public DODGroup(string text, short rarity)
		{
			m_DODInst = new ArrayList();

			m_Name = text;
			m_Rarity = rarity;
		}


		public DODGroup(GenericReader sr)
		{
			m_DODInst = new ArrayList();
			Load(sr);
		}


		public bool Load(GenericReader sr)
		{
			// Load instancing data

			// version
			short version = sr.ReadShort();

			switch (version)
			{

				case 0:
					{
						m_Name = sr.ReadString();       // get the name
						m_Rarity = sr.ReadShort();      // Now grab the rarity level!
						int numDODs = sr.ReadShort();   // How many DODs does this group link?
						if (numDODs == 0)              // are there any to process?
							return true;

						// Read them all in
						for (int ig = 0; ig < numDODs; ig++)
						{
							string sDOD = sr.ReadString();
							// Match the equivalent DOD instances and link
							for (int ii = 0; ii < RareFactory.DODInst.Count; ii++)
							{
								if (RareFactory.DODInst[ii] is DODInstance)
								{
									if (((DODInstance)RareFactory.DODInst[ii]).Name == sDOD)
									{
										m_DODInst.Add((DODInstance)RareFactory.DODInst[ii]);
										// adam: shouldn't we be breaking here?!!
									}
								}
							}
						}
					}
					break;
			}

			return true;
		}

		public void Save(GenericWriter /*BinaryFileWriter*/ bfw)
		{
			// Save instancing data

			// Version
			bfw.Write((short)0);

			bfw.Write((string)m_Name);
			bfw.Write((short)m_Rarity);
			bfw.Write((short)m_DODInst.Count); // How many DODs does this group link?

			if (m_DODInst.Count == 0)
				return;

			// Write them all
			for (int ig = 0; ig < m_DODInst.Count; ig++)
			{
				string sDOD = ((DODInstance)m_DODInst[ig]).Name;
				bfw.Write((string)sDOD);
			}

		}

	}

}
