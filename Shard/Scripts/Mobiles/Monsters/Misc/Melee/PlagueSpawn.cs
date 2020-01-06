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

/* Scripts/Mobiles/Monsters/Misc/Melee/PlagueSpawn.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a plague spawn corpse")]
	public class PlagueSpawn : BaseCreature
	{
		private Mobile m_Owner;
		private DateTime m_ExpireTime;

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime ExpireTime
		{
			get { return m_ExpireTime; }
			set { m_ExpireTime = value; }
		}

		[Constructable]
		public PlagueSpawn()
			: this(null)
		{
		}

		public override bool AlwaysMurderer { get { return true; } }

		public override void DisplayPaperdollTo(Mobile to)
		{
		}

		public override void GetContextMenuEntries(Mobile from, ArrayList list)
		{
			base.GetContextMenuEntries(from, list);

			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i] is ContextMenus.PaperdollEntry)
					list.RemoveAt(i--);
			}
		}

		public override void OnThink()
		{
			bool expired;

			expired = (DateTime.Now >= m_ExpireTime);

			if (!expired && m_Owner != null)
				expired = m_Owner.Deleted || Map != m_Owner.Map || !InRange(m_Owner, 16);

			if (expired)
			{
				PlaySound(GetIdleSound());
				Delete();
			}
			else
			{
				base.OnThink();
			}
		}

		public PlagueSpawn(Mobile owner)
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			m_Owner = owner;
			m_ExpireTime = DateTime.Now + TimeSpan.FromMinutes(1.0);

			Name = "a plague spawn";
			Hue = Utility.Random(0x11, 15);

			switch (Utility.Random(12))
			{
				case 0: // earth elemental
					Body = 14;
					BaseSoundID = 268;
					break;
				case 1: // headless one
					Body = 31;
					BaseSoundID = 0x39D;
					break;
				case 2: // person
					Body = Utility.RandomList(400, 401);
					break;
				case 3: // gorilla
					Body = 0x1D;
					BaseSoundID = 0x9E;
					break;
				case 4: // serpent
					Body = 0x15;
					BaseSoundID = 0xDB;
					break;
				default:
				case 5: // slime
					Body = 51;
					BaseSoundID = 456;
					break;
			}

			SetStr(201, 300);
			SetDex(80);
			SetInt(16, 20);

			SetHits(121, 180);

			SetDamage(11, 17);

			SetSkill(SkillName.MagicResist, 25.0);
			SetSkill(SkillName.Tactics, 25.0);
			SetSkill(SkillName.Wrestling, 50.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 20;
		}

		public PlagueSpawn(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(0, 25);
			else
			{
				AddLoot(LootPack.Poor);
				AddLoot(LootPack.Gems);
			}
		}

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
