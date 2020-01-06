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
/* Scripts/Mobiles/Vendors/PresetMapBuy.cs
 * Changelog
 *  07/02/05 Taran Kain
 *		Made constructor correctly set type, was causing crashes
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class PresetMapBuyInfo : GenericBuyInfo
	{
		private PresetMapEntry m_Entry;

		public PresetMapBuyInfo(PresetMapEntry entry, int price, int amount)
			: base(entry.Name.ToString(), typeof(Server.Items.PresetMap), price, amount, 0x14EC, 0)
		{
			m_Entry = entry;
		}

		public override object GetObject()
		{
			return new PresetMap(m_Entry);
		}
	}
}