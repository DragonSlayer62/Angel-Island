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
//  /Scripts/Mobiles/Vendors/NPC/Weaver.cs
//	CHANGE LOG
//  03/29/2004 - Pulse
//		Removed ability for this NPC vendor to support Bulk Order Deeds
//		Weavers will no longer issue or accept these deeds.

using System;
using System.Collections;
using Server;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
	public class Weaver : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override NpcGuild NpcGuild { get { return NpcGuild.TailorsGuild; } }

		[Constructable]
		public Weaver()
			: base("the weaver")
		{
			SetSkill(SkillName.Tailoring, 65.0, 88.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBWeaver());
		}

		public override VendorShoeType ShoeType
		{
			get { return VendorShoeType.Sandals; }
		}

		#region Bulk Orders
		public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
		{
			PlayerMobile pm = from as PlayerMobile;

			if (pm != null && pm.NextTailorBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()))
			{
				double theirSkill = pm.Skills[SkillName.Tailoring].Base;

				if (theirSkill >= 70.1)
					pm.NextTailorBulkOrder = TimeSpan.FromHours(6.0);
				else if (theirSkill >= 50.1)
					pm.NextTailorBulkOrder = TimeSpan.FromHours(2.0);
				else
					pm.NextTailorBulkOrder = TimeSpan.FromHours(1.0);

				if (theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble())
					return new LargeTailorBOD();

				return SmallTailorBOD.CreateRandomFor(from);
			}

			return null;
		}

		public override bool IsValidBulkOrder(Item item)
		{
			return (item is SmallTailorBOD || item is LargeTailorBOD);
		}

		public override bool SupportsBulkOrders(Mobile from)
		{
			// The following line allows this NPC to support the BOD system. 
			//return ( from is PlayerMobile && from.Skills[SkillName.Tailoring].Base > 0 );
			// return false from this function to disable BOD support.
			return false;
		}

		public override TimeSpan GetNextBulkOrder(Mobile from)
		{
			if (from is PlayerMobile)
				return ((PlayerMobile)from).NextTailorBulkOrder;

			return TimeSpan.Zero;
		}
		#endregion

		public Weaver(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}