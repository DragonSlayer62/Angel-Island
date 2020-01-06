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

/* Scripts\Spells\Fifth\SummonCreature.cs
 * ChangeLog:
 *	2/3/11, Adam
 *		Make ability to summon a Horde Minion based upon Core.AngelIsland
 *	7/16/10, adam
 *		Add HordeMinionFamiliar as a summonable creature
 *			o 3 control slots
 *			o requires SpiritSpeak to summon
 * 	6/5/04, Pix
 * 		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class SummonCreatureSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Creature", "Kal Xen",
				SpellCircle.Fifth,
				266,
				9040,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public SummonCreatureSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		// TODO: Get real list
		private static Type[] m_Types = new Type[]
			{
				typeof( PolarBear ),
				typeof( GrizzlyBear ),
				typeof( BlackBear ),
				typeof( BrownBear ),
				typeof( Horse ),
				typeof( Walrus ),
				typeof( GreatHart ),
				typeof( Hind ),
				typeof( Dog ),
				typeof( Boar ),
				typeof( Chicken ),
				typeof( Rabbit )
			};

		public override bool CheckCast()
		{
			if (!base.CheckCast())
				return false;


			if ((Caster.Followers + 2) > Caster.FollowersMax)
			{
				Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if (CheckSequence())
			{
				try
				{
					BaseCreature creature = null;
					if ((Core.UOAI || Core.UOAR) && Caster.Skills.SpiritSpeak.Value > 50 && (Caster.Skills.SpiritSpeak.Value / 100) >= Utility.RandomDouble())
					{
						creature = new HordeMinionFamiliar();
						creature.ControlSlots = 3;
					}
					else
					{
						creature = (BaseCreature)Activator.CreateInstance(m_Types[Utility.Random(m_Types.Length)]);
						creature.ControlSlots = 2;
					}

					TimeSpan duration;

					if (Core.AOS)
						duration = TimeSpan.FromSeconds((2 * Caster.Skills.Magery.Fixed) / 5);
					else
						duration = TimeSpan.FromSeconds(4.0 * Caster.Skills[SkillName.Magery].Value);

					SpellHelper.Summon(creature, Caster, 0x215, duration, false, false);
				}
				catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
			}

			FinishSequence();
		}

		public override TimeSpan GetCastDelay()
		{
			if (Core.AOS)
				return TimeSpan.FromTicks(base.GetCastDelay().Ticks * 5);

			return base.GetCastDelay() + TimeSpan.FromSeconds(6.0);
		}
	}
}