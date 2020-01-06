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

/* 
 * ChangeLog
 *	5/26/05, Kit
 *		Added check to only create empty bottles in backpack for Players
 */
using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum PotionEffect
	{
		Nightsight,
		CureLesser,
		Cure,
		CureGreater,
		Agility,
		AgilityGreater,
		Strength,
		StrengthGreater,
		PoisonLesser,
		Poison,
		PoisonGreater,
		PoisonDeadly,
		Refresh,
		RefreshTotal,
		HealLesser,
		Heal,
		HealGreater,
		ExplosionLesser,
		Explosion,
		ExplosionGreater
	}

	public abstract class BasePotion : Item, ICraftable
	{
		private PotionEffect m_PotionEffect;

		public PotionEffect PotionEffect
		{
			get
			{
				return m_PotionEffect;
			}
			set
			{
				m_PotionEffect = value;
				InvalidateProperties();
			}
		}

		public override int LabelNumber { get { return 1041314 + (int)m_PotionEffect; } }

		public BasePotion(int itemID, PotionEffect effect)
			: base(itemID)
		{
			m_PotionEffect = effect;

			Stackable = false;
			Weight = 1.0;
		}

		public BasePotion(Serial serial)
			: base(serial)
		{
		}

		public virtual bool RequireFreeHand { get { return true; } }

		public static bool HasFreeHand(Mobile m)
		{
			Item handOne = m.FindItemOnLayer(Layer.OneHanded);
			Item handTwo = m.FindItemOnLayer(Layer.TwoHanded);

			if (handTwo is BaseWeapon)
				handOne = handTwo;

			return (handOne == null || handTwo == null);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!Movable)
				return;

			if (from.InRange(this.GetWorldLocation(), 1))
			{
				if (!RequireFreeHand || HasFreeHand(from))
					Drink(from);
				else
					from.SendLocalizedMessage(502172); // You must have a free hand to drink a potion.
			}
			else
			{
				from.SendLocalizedMessage(502138); // That is too far away for you to use
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_PotionEffect);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_PotionEffect = (PotionEffect)reader.ReadInt();
						break;
					}
			}
		}

		public abstract void Drink(Mobile from);

		public static void PlayDrinkEffect(Mobile m)
		{
			m.RevealingAction();

			m.PlaySound(0x2D6);

			if (m is PlayerMobile)
				m.AddToBackpack(new Bottle());

			if (m.Body.IsHuman /*&& !m.Mounted*/ )
				m.Animate(34, 5, 1, true, false, 0);
		}

		public static TimeSpan Scale(Mobile m, TimeSpan v)
		{
			if (!Core.AOS)
				return v;

			double scalar = 1.0 + (0.01 * AosAttributes.GetValue(m, AosAttribute.EnhancePotions));

			return TimeSpan.FromSeconds(v.TotalSeconds * scalar);
		}

		public static double Scale(Mobile m, double v)
		{
			if (!Core.AOS)
				return v;

			double scalar = 1.0 + (0.01 * AosAttributes.GetValue(m, AosAttribute.EnhancePotions));

			return v * scalar;
		}

		public static int Scale(Mobile m, int v)
		{
			if (!Core.AOS)
				return v;

			return AOS.Scale(v, 100 + AosAttributes.GetValue(m, AosAttribute.EnhancePotions));
		}

		#region ICraftable Members

		public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
		{
			if (craftSystem is DefAlchemy)
			{
				Container pack = from.Backpack;

				// Publish 15
				// You will now be able to make potions directly into a potion keg.
				if (pack != null && (Core.UOAI || Core.UOAR || Core.UOMO || Core.Publish >= 15))
				{
					List<PotionKeg> kegs = pack.FindItemsByType<PotionKeg>();

					for (int i = 0; i < kegs.Count; ++i)
					{
						PotionKeg keg = kegs[i];

						if (keg == null)
							continue;

						if (keg.Held <= 0 || keg.Held >= 100)
							continue;

						if (keg.Type != PotionEffect)
							continue;

						++keg.Held;

						Consume();
						from.AddToBackpack(new Bottle());

						return -1; // signal placed in keg
					}
				}
			}

			return 1;
		}

#if old
		if (craftSystem is DefAlchemy && item is BasePotion)
					{
						BasePotion pot = (BasePotion)item;

						Container pack = from.Backpack;

						if (pack != null)
						{
							Item[] kegs = pack.FindItemsByType(typeof(PotionKeg), true);

							for (int i = 0; i < kegs.Length; ++i)
							{
								PotionKeg keg = kegs[i] as PotionKeg;

								if (keg == null)
									continue;

								if (keg.Held <= 0 || keg.Held >= 100)
									continue;

								if (keg.Type != pot.PotionEffect)
									continue;

								++keg.Held;
								item.Delete();
								item = new Bottle();

								endquality = -1; // signal placed in keg

								break;
							}
						}
					}
#endif

		#endregion
	}
}