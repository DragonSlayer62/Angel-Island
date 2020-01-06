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

namespace Server.Items
{
	public abstract class BaseStaff : BaseMeleeWeapon
	{
		public override int DefHitSound { get { return 0x233; } }
		public override int DefMissSound { get { return 0x239; } }

		public override SkillName DefSkill { get { return SkillName.Macing; } }
		public override WeaponType DefType { get { return WeaponType.Staff; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Bash2H; } }

		public BaseStaff(int itemID)
			: base(itemID)
		{
		}

		public BaseStaff(Serial serial)
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

		public override void OnHit(Mobile attacker, Mobile defender)
		{
			base.OnHit(attacker, defender);

			defender.Stam -= Utility.Random(2, 4); // 3-5 points of stamina loss
		}
	}
}