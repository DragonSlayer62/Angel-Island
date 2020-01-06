/*	This program is the CONFIDENTIAL and PROPRIETARY property
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

/* Scripts/Mobiles/Guards/DeadGuard.cs
 * CHANGELOG
 *  09/06/05 Taran Kain
 *		Set StaticCorpse to true in OnDeath to prevent looting.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a dead guard")]
	public class DeadGuard : BaseGuard
	{
		[Constructable]
		public DeadGuard()
			: base(null)
		{
			this.Direction = (Direction)Utility.Random(8);

			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i] is Halberd)
					((Item)Items[i]).Movable = true;
			}

			//AddItem(new Halberd());

			Timer.DelayCall(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.0), new TimerCallback(Kill));
		}

		public DeadGuard(Serial serial)
			: base(serial)
		{
		}

		public override Mobile Focus
		{
			get { return null; }
			set { ; }
		}

		public override bool OnBeforeDeath()
		{
			return true;
		}

		public override void OnDeath(Server.Items.Container c)
		{
			base.OnDeath(c);

			Corpse corpse = c as Corpse;
			corpse.BeginDecay(TimeSpan.FromHours(24.0));
			corpse.StaticCorpse = true;
			for (int i = 0; i < 3; i++)
			{
				Point3D p = new Point3D(Location);
				p.X += Utility.RandomMinMax(-1, 1);
				p.Y += Utility.RandomMinMax(-1, 1);
				new Blood(Utility.Random(0x122A, 5), 86400.0).MoveToWorld(p, c.Map);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
