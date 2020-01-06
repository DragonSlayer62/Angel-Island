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

/* Scripts\Engines\CTFSystem\Regions\CTFRegion.cs
 * CHANGELOG:
 * 4/10/10, Adam
 *		Added new region overrides (for new region overridables)
 *		KeepsItemsOnDeath
 *			Yep, players keep all their loot
 *		OnAfterDeath
 *			We want to freeze the ghost after death so they can't ghost spy etc.
 *			We can't use OnDeath() because the frozen flag is cleared after we get called.
 * 4/10/10, adam
 *		initial framework.
 */

using System;
using Server;
using Server.Regions;
using Server.Items;
using Server.Engines;

namespace Server.Engines
{
	public class CTFRegion : Server.Regions.CustomRegion
	{
		public CTFRegion(CTFControl ctfcontrol, Map map)
			: base(ctfcontrol, map)
		{
			Setup();
		}

		public CTFRegion(RegionControl rc, Map map)
			: base(rc, map)
		{
			Setup();
		}

		private void Setup()
		{	// new regions default to guarded, turn it off here
			IsGuarded = false;
		}

		// process CTF commands
		public override void OnSpeech(SpeechEventArgs e)
		{
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc != null)
			{
				if (ctfc.OnRegionSpeech(e))
					e.Handled = true;
			}
			base.OnSpeech(e);
		}

		public override bool OnDeath(Mobile m)
		{
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc == null) return base.OnDeath(m);
			ctfc.OnDeath(m);
			return base.OnDeath(m);
		}

		public override void OnAfterDeath(Mobile m)
		{
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc == null) return;
			ctfc.OnRegionAfterDeath(m);
		}

		public override bool KeepsItemsOnDeath()
		{	// everyone keeps their loot
			return true;
		}

		public override void OnPlayerAdd(Mobile m)
		{	// player is logging into the region
			base.OnPlayerAdd(m);
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc != null)
				ctfc.OnPlayerAdd(m);
		}

		public override bool CheckAccessibility(Item i, Mobile m)
		{
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc != null && m.AccessLevel == AccessLevel.Player)
				return ctfc.OnRegionCheckAccessibility(i, m);
			else
				return base.CheckAccessibility(i, m);
		}

		public override bool EquipItem(Mobile m, Item item)
		{
			bool result = true;
			CTFControl ctfc = this.m_Controller as CTFControl;
			if (ctfc != null)
				result = ctfc.OnRegionEquipItem(m, item);
			return base.EquipItem(m, item) && result;
		}
	}
}
