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

/*	Scripts\Items\Weapons\TemplateWeapon.cs
 *	ChangeLog:
 *	8/6/10, adam
 *		initial creation
 *		used for the dynamic creation of generic non-flippable weapons
 */

using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class TemplateWeapon : BaseMeleeWeapon
	{
		public override int OldStrengthReq { get { return 0; } }
		public override int OldSpeed { get { return 30; } }

		public override int OldDieRolls { get { return 1; } }
		public override int OldDieMax { get { return 8; } }
		public override int OldAddConstant { get { return 0; } }

		public override int DefHitSound { get { return -1; } }
		public override int DefMissSound { get { return -1; } }

		public override SkillName DefSkill { get { return SkillName.Wrestling; } }
		public override WeaponType DefType { get { return WeaponType.Fists; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Wrestle; } }

		public TemplateWeapon()
			: base(0)
		{
		}

		public TemplateWeapon(Serial serial)
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