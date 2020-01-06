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

/* Engines/Township/TownshipRegion.cs
 * CHANGELOG:
 * 5-11-2010 - Pix
 *      Added IsPointInTownship helper function
 */

using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Network;
using System.Collections;
using System.Collections.Generic;

namespace Server.Regions
{
	public class TownshipRegion : Server.Regions.CustomRegion
	{
		public TownshipRegion(TownshipStone townstone, Map map)
			: base(townstone, map)
		{
			Setup();
		}
		public TownshipRegion(RegionControl rc, Map map)
			: base(rc, map)
		{
			Setup();
		}

		private void Setup()
		{
			this.IsGuarded = false;
		}

		#region spam management
		private class SpamEntry
		{
			private string m_text;
			public string Text { get { return m_text; } }
			private DateTime m_when;
			public DateTime When { get { return m_when; } }
			public SpamEntry(string text)
			{
				m_text = text;
				m_when = DateTime.Now + TimeSpan.FromMinutes(10.0);
			}
		}
		private Dictionary<Mobile, SpamEntry> m_SpamQueue = new Dictionary<Mobile, SpamEntry>();
		private void SpamClean()
		{	//expire old list entries
			List<Mobile> delete_list = new List<Mobile>();
			foreach (KeyValuePair<Mobile, SpamEntry> kvp in m_SpamQueue)
			{
				if (DateTime.Now > kvp.Value.When)
					delete_list.Add(kvp.Key);
			}
			foreach (Mobile m in delete_list)
			{
				m_SpamQueue.Remove(m);
			}
		}
		public bool IsSpam(Mobile m, string text)
		{	// always start by cleaning out old messages
			SpamClean();
			if (m_SpamQueue.ContainsKey(m))
				return true;
			return false;
		}
		public void QueueSpam(Mobile m, string text)
		{// always start by cleaning out old messages
			SpamClean();
			if (m_SpamQueue.ContainsKey(m) == false)
			{
				m_SpamQueue[m] = new SpamEntry(text);
			}
			else
			{	// update with latest spam from this player
				m_SpamQueue.Remove(m);
				m_SpamQueue[m] = new SpamEntry(text);
			}
		}

		// process township commands
		public override void OnSpeech(SpeechEventArgs e)
		{
			if (e.Handled == false && this.TStone != null && this.TStone.Guild != null && (e.Mobile.Guild as Server.Guilds.Guild != null))
			{
				if (e.Speech.ToLower() == ".status")
				{	// always start by cleaning out old messages
					SpamClean();
					if (this.TStone.Guild.IsMember(e.Mobile) || this.TStone.Guild.IsAlly(e.Mobile.Guild as Server.Guilds.Guild))
					{
						if (m_SpamQueue.Count > 0)
						{
							foreach (KeyValuePair<Mobile, SpamEntry> kvp in m_SpamQueue)
								e.Mobile.SendMessage(kvp.Value.Text);
						}
						else
							e.Mobile.SendMessage("You have no new messages.");
					}
					e.Handled = true;
				}
			}
			base.OnSpeech(e);
		}
		#endregion spam management

		private bool IsControllerGood()
		{
			return (this.m_Controller != null && this.m_Controller is TownshipStone);
		}

		public static TownshipRegion GetTownshipAt(Mobile m)
		{
			CustomRegion c = CustomRegion.FindDRDTRegion(m);
			if (c is TownshipRegion)
			{
				return (TownshipRegion)c;
			}

			return null;
		}

		public static TownshipRegion GetTownshipAt(Point3D point, Map map)
		{
			CustomRegion c = CustomRegion.FindDRDTRegion(map, point);
			if (c is TownshipRegion)
			{
				return (TownshipRegion)c;
			}
			return null;
		}

		public bool IsPointInTownship(Point3D point)
		{
			for (int i = 0; i < this.Coords.Count; i++)
			{
				if (this.Coords[i] is Rectangle2D)
				{
					Rectangle2D r = (Rectangle2D)this.Coords[i];
					if (r.Contains(new Point2D(point.X, point.Y)))
					{
						return true;
					}
				}
				else if (this.Coords[i] is Rectangle3D)
				{
					Rectangle3D r = (Rectangle3D)this.Coords[i];
					if (r.Contains(point))
					{
						return true;
					}
				}
			}

			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (IsControllerGood())
			{
				//forward to controller, which keeps track of everything
				((TownshipStone)this.m_Controller).OnEnter(m);
			}

			base.OnEnter(m);
		}

		public override void OnExit(Mobile m)
		{
			if (IsControllerGood())
			{
				//forward to controller, which keeps track of everything
				((TownshipStone)this.m_Controller).OnExit(m);
			}

			base.OnExit(m);
		}

		public bool CanBuildHouseInTownship(Mobile m)
		{
			PlayerMobile pm = m as PlayerMobile;
			if (pm != null)
			{
				if (IsControllerGood())
				{
					return ((TownshipStone)this.m_Controller).CanBuildHouseInTownship(pm);
				}
				else
				{
					//if bad controller, default to yes.
					return true;
				}
			}

			return false;
		}

		public override bool IsNoMurderZone
		{
			get
			{
				if (IsControllerGood())
				{
					return !((TownshipStone)this.m_Controller).MurderZone;
				}
				else
				{
					return base.IsNoMurderZone;
				}
			}
		}

		public override bool IsMobileCountable(Mobile aggressor)
		{
			if (IsControllerGood())
			{
				return ((TownshipStone)this.m_Controller).IsMobileCountable(aggressor);
			}
			else
			{
				return base.IsMobileCountable(aggressor);
			}
		}

		public TownshipStone TStone
		{
			get
			{
				if (IsControllerGood())
				{
					return (TownshipStone)this.m_Controller;
				}
				else
				{
					return null;
				}
			}
		}


	}
}
