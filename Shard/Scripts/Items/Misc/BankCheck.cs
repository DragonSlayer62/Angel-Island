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

/* Items/Misc/BankCheck.cs
 * ChangeLog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Quests;
using Necro = Server.Engines.Quests.Necro;
using Haven = Server.Engines.Quests.Haven;

namespace Server.Items
{
	public class BankCheck : Item
	{
		private int m_Worth;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Worth
		{
			get { return m_Worth; }
			set { m_Worth = value; InvalidateProperties(); }
		}

		public BankCheck(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Worth);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Worth = reader.ReadInt();
						break;
					}
			}
		}

		[Constructable]
		public BankCheck(int worth)
			: base(0x14F0)
		{
			Weight = 1.0;
			Hue = 0x34;
			LootType = LootType.Blessed;

			m_Worth = worth;
		}

		public override bool DisplayLootType { get { return false; } }

		public override int LabelNumber { get { return 1041361; } } // A bank check

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060738, m_Worth.ToString()); // value: ~1_val~
		}

		public override void OnSingleClick(Mobile from)
		{
			from.Send(new MessageLocalizedAffix(Serial, ItemID, MessageType.Label, 0x3B2, 3, 1041361, "", AffixType.Append, String.Concat(" ", m_Worth.ToString()), "")); // A bank check:
		}

		public override void OnDoubleClick(Mobile from)
		{
			BankBox box = from.BankBox;

			if (box != null && IsChildOf(box))
			{
				Delete();

				int deposited = 0;

				int toAdd = m_Worth;

				Gold gold;

				while (toAdd > 60000)
				{
					gold = new Gold(60000);

					if (box.TryDropItem(from, gold, false))
					{
						toAdd -= 60000;
						deposited += 60000;
					}
					else
					{
						gold.Delete();

						from.AddToBackpack(new BankCheck(toAdd));
						toAdd = 0;

						break;
					}
				}

				if (toAdd > 0)
				{
					gold = new Gold(toAdd);

					if (box.TryDropItem(from, gold, false))
					{
						deposited += toAdd;
					}
					else
					{
						gold.Delete();

						from.AddToBackpack(new BankCheck(toAdd));
					}
				}

				// Gold was deposited in your account:
				from.SendLocalizedMessage(1042672, true, " " + deposited.ToString());

				PlayerMobile pm = from as PlayerMobile;

				if (pm != null)
				{
					QuestSystem qs = pm.Quest;

					if (qs is Necro.DarkTidesQuest)
					{
						QuestObjective obj = qs.FindObjective(typeof(Necro.CashBankCheckObjective));

						if (obj != null && !obj.Completed)
							obj.Complete();
					}

					if (qs is Haven.UzeraanTurmoilQuest)
					{
						QuestObjective obj = qs.FindObjective(typeof(Haven.CashBankCheckObjective));

						if (obj != null && !obj.Completed)
							obj.Complete();
					}
				}
			}
			else
			{
				from.SendLocalizedMessage(1047026); // That must be in your bank box to use it.
			}
		}
	}
}