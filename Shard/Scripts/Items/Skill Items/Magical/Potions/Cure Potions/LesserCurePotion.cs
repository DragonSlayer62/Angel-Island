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

/* Items/SkillItems/Magical/Potions/Cure Potions/LesserCurePotion.cs
 * CHANGELOG:
 *	10/16/05, Pix
 *		Removed AOS cure levels.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;

namespace Server.Items
{
	public class LesserCurePotion : BaseCurePotion
	{
		private static CureLevelInfo[] m_OldLevelInfo = new CureLevelInfo[]
		{
			new CureLevelInfo( Poison.Lesser,  0.75 ), // 75% chance to cure lesser poison
			new CureLevelInfo( Poison.Regular, 0.50 ), // 50% chance to cure regular poison
			new CureLevelInfo( Poison.Greater, 0.15 )  // 15% chance to cure greater poison
		};

		public override CureLevelInfo[] LevelInfo { get { return m_OldLevelInfo; } }

		[Constructable]
		public LesserCurePotion()
			: base(PotionEffect.CureLesser)
		{
		}

		public LesserCurePotion(Serial serial)
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