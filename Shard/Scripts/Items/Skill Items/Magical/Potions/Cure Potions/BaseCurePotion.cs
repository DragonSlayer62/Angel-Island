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

/* Scripts/Items/Skill Items/Magical/Potions/Cure Potions/BaseCurePotion.cs
 * ChangeLog
 *	7/23/05, Adam
 *		Remove all Necromancy, and Chivalry nonsense
 */

using System;
using Server;

namespace Server.Items
{
	public class CureLevelInfo
	{
		private Poison m_Poison;
		private double m_Chance;

		public Poison Poison
		{
			get { return m_Poison; }
		}

		public double Chance
		{
			get { return m_Chance; }
		}

		public CureLevelInfo(Poison poison, double chance)
		{
			m_Poison = poison;
			m_Chance = chance;
		}
	}

	public abstract class BaseCurePotion : BasePotion
	{
		public abstract CureLevelInfo[] LevelInfo { get; }

		public BaseCurePotion(PotionEffect effect)
			: base(0xF07, effect)
		{
		}

		public BaseCurePotion(Serial serial)
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

		public void DoCure(Mobile from)
		{
			bool cure = false;

			CureLevelInfo[] info = LevelInfo;

			for (int i = 0; i < info.Length; ++i)
			{
				CureLevelInfo li = info[i];

				if (li.Poison == from.Poison && Scale(from, li.Chance) > Utility.RandomDouble())
				{
					cure = true;
					break;
				}
			}

			if (cure && from.CurePoison(from))
			{
				from.SendLocalizedMessage(500231); // You feel cured of poison!

				from.FixedEffect(0x373A, 10, 15);
				from.PlaySound(0x1E0);
			}
			else if (!cure)
			{
				from.SendLocalizedMessage(500232); // That potion was not strong enough to cure your ailment!
			}
		}

		public override void Drink(Mobile from)
		{
			//if ( Spells.Necromancy.TransformationSpell.UnderTransformation( from, typeof( Spells.Necromancy.VampiricEmbraceSpell ) ) )
			//{
			//	from.SendLocalizedMessage( 1061652 ); // The garlic in the potion would surely kill you.
			//}
			//else 
			if (from.Poisoned)
			{
				DoCure(from);

				BasePotion.PlayDrinkEffect(from);

				from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
				from.PlaySound(0x1E0);

				this.Delete();
			}
			else
			{
				from.SendLocalizedMessage(1042000); // You are not poisoned.
			}
		}
	}
}