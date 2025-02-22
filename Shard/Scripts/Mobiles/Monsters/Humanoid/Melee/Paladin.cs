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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Paladin.cs
 * ChangeLog
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	9/20/05, Adam
 *		Add the Parry skill
 *  9/20/05, Adam
 *		a. Add speech-machine framework.
 *			add some sassy speech when players try to bard
 *		b. Make bard immune.
 *  9/19/05, Adam
 *		a. Change Karma loss to that for a 'good' aligned creature
 *		b. remove powder of transloacation
 *	9/19/05, Adam
 *		Change surcoat and cloak drop rate to 30% each from 50% each
 *  9/16/05, Adam
 *		spawned from BrigandLeader.cs
 */

using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	public class Paladin : BaseCreature
	{
		private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
		public DateTime m_NextSpeechTime;

		[Constructable]
		public Paladin()
			: base(AIType.AI_Melee, FightMode.Aggressor | FightMode.Criminal, 10, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the paladin";
			Hue = Utility.RandomSkinHue();
			IOBAlignment = IOBAlignment.Good;
			ControlSlots = 5;
			BardImmune = true;

			SetStr(386, 400);
			SetDex(70, 90);
			SetInt(161, 175);

			SetDamage(20, 30);

			SetSkill(SkillName.Anatomy, 125.0);
			SetSkill(SkillName.Fencing, 46.0, 77.5);
			SetSkill(SkillName.Macing, 35.0, 57.5);
			SetSkill(SkillName.Parry, 80.0, 98.5);
			SetSkill(SkillName.MagicResist, 83.5, 92.5);
			SetSkill(SkillName.Swords, 125.0);
			SetSkill(SkillName.Tactics, 125.0);
			SetSkill(SkillName.Lumberjacking, 125.0);

			Fame = 5000;
			Karma = 5000;

			InitBody();
			InitOutfit();

			VirtualArmor = 40;

			m_NextSpeechTime = DateTime.Now;

			PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

		}

		public override bool AlwaysMurderer { get { return false; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool CanRummageCorpses { get { return false; } }
		public override bool ClickTitle { get { return true; } }

		public override bool CanBandage { get { return true; } }
		public override TimeSpan BandageDelay { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(13, 15)); } }

		public override void InitBody()
		{
			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");

			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
			}
		}
		public override void InitOutfit()
		{
			WipeLayers();
			int hue = Utility.RandomSpecialVioletHue();
			int longhair = 0x203C;
			Item hair = new Item(longhair);
			hair.Hue = hue;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			double chance = 0.70; // 30% chance to drop
			AddItem(new PlateChest(), LootType.Regular);
			AddItem(new PlateArms(), LootType.Regular);
			AddItem(new PlateGorget(), LootType.Regular);
			AddItem(new PlateLegs(), LootType.Regular);
			AddItem(new PlateHelm(), LootType.Regular);
			AddItem(new Surcoat(hue), (Utility.RandomDouble() > chance) ? LootType.Regular : LootType.Newbied);
			AddItem(new Cloak(hue), (Utility.RandomDouble() > chance) ? LootType.Regular : LootType.Newbied);

			AddItem(new Boots(Utility.RandomNeutralHue()));
			AddItem(new FancyShirt());
			AddItem(new VikingSword());
			AddItem(new OrderShield(), LootType.Newbied);

		}

		public override bool Uncalmable
		{
			get
			{
				if (Hits > 1 && DateTime.Now >= m_NextSpeechTime)
				{
					int phrase = Utility.Random(4);

					switch (phrase)
					{
						case 0: this.Say(true, "My, what a lovely melody. Do you know the Minuet In G Major?"); break;
						case 1: this.Say(true, "*sings along*"); break;
						case 2: this.Say(true, "*tosses a shilling in the hat*"); break;
						case 3: this.Say(true, "Your music is wasted on me friend."); break;
					}

					m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
				}

				return BardImmune;
			}
		}
		/*
				public override void Damage( int amount, Mobile from )
				{
					Mobile combatant = this.Combatant;

					if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 8 ) && combatant.InLOS( this ) )
					{
						if ( this.Hits <= 200 )
						{
							if ( Utility.RandomBool() )
							{
								this.Say( true, "Wretched Dog!" );
								m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
							}
						}
						else if ( this.Hits <= 100 )
						{
							if ( Utility.RandomBool() )
							{
								this.Say( true, "Vile Heathen!" );
								m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
							}					
						}
					}
				
					base.Damage( amount, from );
				}

				public override void OnMovement( Mobile m, Point3D oldLocation )
				{

					if ( m.Player && m.Alive && m.InRange( this, 10 ) && m.AccessLevel == AccessLevel.Player && DateTime.Now >= m_NextSpeechTime && Combatant == null)
					{
						Item item = m.FindItemOnLayer( Layer.Helm );

						if ( this.InLOS( m ) && this.CanSee( m ) )
						{
							if ( item is BloodDrenchedBandana )
							{
								this.Say ( "Leave these halls before it is too late!" );
								m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
							} 
							else
							{
								this.Say ( "Where is your bandana, friend?" );
								m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
							}
						}
			
					}
		
					base.OnMovement( m, oldLocation );
				}

				public override void OnThink()
				{
					if ( DateTime.Now >= m_NextSpeechTime )
					{
						Mobile combatant = this.Combatant;

						if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 7 ) && combatant.InLOS( this ) )
						{
							int phrase = Utility.Random( 4 );

							switch ( phrase )
							{
								case 0: this.Say( true, "Yet another knuckle dragging heathen to deal with!" ); break;
								case 1: this.Say( true, "You must leave our sacred home vile heathen!" ); break;
								case 2: this.Say( true, "You must leave now!" ); break;
								case 3: this.Say( true, "Ah! You do bleed badly!" ); break;
							}
					
							m_NextSpeechTime = DateTime.Now + m_SpeechDelay;				
						}

						base.OnThink();
					}			
				}
		*/
		public Paladin(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(300, 450);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// ai special
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					// ai special
				}
			}

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
