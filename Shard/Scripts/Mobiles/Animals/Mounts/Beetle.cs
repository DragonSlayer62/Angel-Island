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

/* ./Scripts/Mobiles/Animals/Mounts/Beetle.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a giant beetle corpse")]
	public class Beetle : BaseMount
	{
		[Constructable]
		public Beetle()
			: this("a giant beetle")
		{
		}

		public override bool SubdueBeforeTame { get { return true; } } // Must be beaten into submission

		[Constructable]
		public Beetle(string name)
			: base(name, 0x317, 0x3EBC, AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.175, 0.350)
		{
			SetStr(300);
			SetDex(100);
			SetInt(500);

			SetHits(200);

			SetDamage(7, 20);

			SetSkill(SkillName.MagicResist, 80.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 4000;
			Karma = -4000;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 29.1;

			Container pack = Backpack;

			if (pack != null)
				pack.Delete();

			pack = new StrongBackpack();
			pack.Movable = false;

			AddItem(pack);
		}

		public override int GetAngerSound()
		{
			return 0x21D;
		}

		public override int GetIdleSound()
		{
			return 0x21D;
		}

		public override int GetAttackSound()
		{
			return 0x162;
		}

		public override int GetHurtSound()
		{
			return 0x163;
		}

		public override int GetDeathSound()
		{
			return 0x21D;
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public Beetle(Serial serial)
			: base(serial)
		{
		}

		#region Pack Animal Methods
		public override bool OnBeforeDeath()
		{
			if (!base.OnBeforeDeath())
				return false;

			PackAnimal.CombineBackpacks(this);

			return true;
		}

		public override bool IsSnoop(Mobile from)
		{
			if (PackAnimal.CheckAccess(this, from))
				return false;

			return base.IsSnoop(from);
		}

		public override bool OnDragDrop(Mobile from, Item item)
		{
			if (CheckFeed(from, item))
				return true;

			if (PackAnimal.CheckAccess(this, from))
			{
				AddToBackpack(item);
				return true;
			}

			return base.OnDragDrop(from, item);
		}

		public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
		{
			return PackAnimal.CheckAccess(this, from);
		}

		public override bool CheckNonlocalLift(Mobile from, Item item)
		{
			return PackAnimal.CheckAccess(this, from);
		}

		public override void GetContextMenuEntries(Mobile from, System.Collections.ArrayList list)
		{
			base.GetContextMenuEntries(from, list);

			PackAnimal.GetContextMenuEntries(this, from, list);
		}
		#endregion

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
