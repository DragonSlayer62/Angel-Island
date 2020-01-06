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
using Server.Items;

namespace Server.Targeting
{
	public class WandTarget : Target
	{
		private BaseWand m_Item;

		public WandTarget(BaseWand item)
			: base(6, false, TargetFlags.None)
		{
			m_Item = item;
		}

		private static int GetOffset(Mobile caster)
		{
			return 5 + (int)(caster.Skills[SkillName.Magery].Value * 0.02);
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			m_Item.DoWandTarget(from, targeted);
		}
	}
}