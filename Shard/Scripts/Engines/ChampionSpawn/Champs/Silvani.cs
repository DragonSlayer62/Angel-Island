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

/* Scripts\Engines\ChampionSpawn\Champs\Silvani.cs
 * CHANGELOG
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 * Last modified 3/23/04 by mith
 * Removed spawn of gold items in pack.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Silvani : BaseCreature
	{
		[Constructable]
		public Silvani()
			: base(AIType.AI_Mage, FightMode.Aggressor | FightMode.Evil, 18, 1, 0.2, 0.4)
		{
			Name = "Silvani";
			Body = 176;
			BaseSoundID = 0x467;

			SetStr(253, 400);
			SetDex(157, 850);
			SetInt(503, 800);

			SetHits(600);

			SetDamage(27, 38);

			SetSkill(SkillName.EvalInt, 100.0);
			SetSkill(SkillName.Magery, 97.6, 107.5);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.MagicResist, 100.5, 150.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 20000;
			Karma = 20000;

			VirtualArmor = 50;
		}

		public override void GenerateLoot()
		{
			if (!Core.UOAI && !Core.UOAR)
			{
				AddLoot(LootPack.UltraRich, 2);
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
		}

		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }

		public void SpawnPixies(Mobile target)
		{
			Map map = this.Map;

			if (map == null)
				return;

			int newPixies = Utility.RandomMinMax(3, 6);

			for (int i = 0; i < newPixies; ++i)
			{
				Pixie pixie = new Pixie();

				pixie.Team = this.Team;
				pixie.FightMode = FightMode.All | FightMode.Closest;

				bool validLocation = false;
				Point3D loc = this.Location;

				for (int j = 0; !validLocation && j < 10; ++j)
				{
					int x = X + Utility.Random(3) - 1;
					int y = Y + Utility.Random(3) - 1;
					int z = map.GetAverageZ(x, y);

					if (validLocation = map.CanFit(x, y, this.Z, 16, CanFitFlags.requireSurface))
						loc = new Point3D(x, y, Z);
					else if (validLocation = map.CanFit(x, y, z, 16, CanFitFlags.requireSurface))
						loc = new Point3D(x, y, z);
				}

				pixie.MoveToWorld(loc, map);
				pixie.Combatant = target;
			}
		}

		public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
		{
			if (0.1 >= Utility.RandomDouble())
				SpawnPixies(caster);
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			defender.Damage(Utility.Random(20, 10), this);
			defender.Stam -= Utility.Random(20, 10);
			defender.Mana -= Utility.Random(20, 10);
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			if (0.1 >= Utility.RandomDouble())
				SpawnPixies(attacker);
		}

		public Silvani(Serial serial)
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
