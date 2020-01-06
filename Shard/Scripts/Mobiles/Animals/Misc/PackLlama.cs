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

/* ./Scripts/Mobiles/Animals/Misc/PackLlama.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a llama corpse")]
	public class PackLlama : BaseCreature
	{
		[Constructable]
		public PackLlama()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a pack llama";
			Body = 292;
			BaseSoundID = 0x3F3;

			SetStr(52, 80);
			SetDex(36, 55);
			SetInt(16, 30);

			SetHits(50);
			SetStam(86, 105);
			SetMana(0);

			SetDamage(2, 6);

			SetSkill(SkillName.MagicResist, 15.1, 20.0);
			SetSkill(SkillName.Tactics, 19.2, 29.0);
			SetSkill(SkillName.Wrestling, 19.2, 29.0);

			Fame = 0;
			Karma = 200;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;

			Container pack = Backpack;

			if (pack != null)
				pack.Delete();

			pack = new StrongBackpack();
			pack.Movable = false;

			AddItem(pack);
		}

		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public PackLlama(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			PackAnimal.TryPackOpen(this, from);
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

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
