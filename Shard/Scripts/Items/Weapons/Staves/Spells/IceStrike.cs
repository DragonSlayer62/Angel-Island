using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.First
{
	public class IceStrikeSpell : Spell
	{
	public override bool ClearHandsOnCast{ get{ return false; } }
		private static SpellInfo m_Info = new SpellInfo(
				"Ice Strike", "The Staff Summons the Power for the incantation",
				SpellCircle.First,
				245,
				9042,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public IceStrikeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return true; } }

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				int damage;
				
				if ( Core.AOS )
				{
					//damage = GetNewAosDamage( 50, 1, 5 );
					
					damage = (Caster.Mana / 2) + 10;
					Caster.Mana -= damage;
				}
				else
				{
					damage = Utility.Random( 27, 22 );

					if ( CheckResisted( m ) )
					{
						//damage *= 0.6;

						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					}

					//damage *= GetDamageScalar( m );
				}

				//m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
				m.FixedParticles( 0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head );
				m.FixedParticles( 0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head );
				m.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );
				m.PlaySound( 0x208 );

				SpellHelper.Damage( this, m, damage, 0, 0, 100, 0, 0 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private IceStrikeSpell m_Owner;

			public InternalTarget( IceStrikeSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}