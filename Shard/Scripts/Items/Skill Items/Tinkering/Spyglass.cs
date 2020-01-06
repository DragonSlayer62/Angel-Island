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

/* Items/SkillItems/Tinkering/Spyglass.cs
 * CHANGELOG:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;

namespace Server.Items
{
	[Flipable(0x14F5, 0x14F6)]
	public class Spyglass : Item
	{
		[Constructable]
		public Spyglass()
			: base(0x14F5)
		{
			Weight = 3.0;
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1008155); // You peer into the heavens, seeking the moons...

			from.Send(new MessageLocalizedAffix(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 1008146 + (int)Clock.GetMoonPhase(Map.Trammel, from.X, from.Y), "", AffixType.Prepend, "Trammel : ", ""));
			from.Send(new MessageLocalizedAffix(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 1008146 + (int)Clock.GetMoonPhase(Map.Felucca, from.X, from.Y), "", AffixType.Prepend, "Felucca : ", ""));

			PlayerMobile player = from as PlayerMobile;

			if (player != null)
			{
				QuestSystem qs = player.Quest;

				if (qs is WitchApprenticeQuest)
				{
					FindIngredientObjective obj = qs.FindObjective(typeof(FindIngredientObjective)) as FindIngredientObjective;

					if (obj != null && !obj.Completed && obj.Ingredient == Ingredient.StarChart)
					{
						int hours, minutes;
						Clock.GetTime(from.Map, from.X, from.Y, out hours, out minutes);

						if (hours < 5 || hours > 17)
						{
							player.SendLocalizedMessage(1055040); // You gaze up into the glittering night sky.  With great care, you compose a chart of the most prominent star patterns.

							obj.Complete();
						}
						else
						{
							player.SendLocalizedMessage(1055039); // You gaze up into the sky, but it is not dark enough to see any stars.
						}
					}
				}
			}
		}

		public Spyglass(Serial serial)
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