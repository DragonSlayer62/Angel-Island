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

/* Scripts\Engines\AI\Creature\OppositionGroup.cs
 * ChangeLog
 *	1/28/11, Adam
 *		Add back OppositionGroup for !Core.AngelIsland
 */

using System;
using Server;
using Server.Mobiles;

namespace Server
{
	public class OppositionGroup
	{
		private Type[][] m_Types;

		public OppositionGroup(Type[][] types)
		{
			m_Types = types;
		}

		public bool IsEnemy(object from, object target)
		{
			int fromGroup = IndexOf(from);
			int targGroup = IndexOf(target);

			return (fromGroup != -1 && targGroup != -1 && fromGroup != targGroup);
		}

		public int IndexOf(object obj)
		{
			if (obj == null)
				return -1;

			Type type = obj.GetType();

			for (int i = 0; i < m_Types.Length; ++i)
			{
				Type[] group = m_Types[i];

				bool contains = false;

				for (int j = 0; !contains && j < group.Length; ++j)
					contains = (type == group[j]);

				if (contains)
					return i;
			}

			return -1;
		}

		private static OppositionGroup m_TerathansAndOphidians = new OppositionGroup(new Type[][]
			{
				new Type[]
				{
					typeof( TerathanAvenger ),
					typeof( TerathanDrone ),
					typeof( TerathanMatriarch ),
					typeof( TerathanWarrior )
				},
				new Type[]
				{
					typeof( OphidianArchmage ),
					typeof( OphidianKnight ),
					typeof( OphidianMage ),
					typeof( OphidianMatriarch ),
					typeof( OphidianWarrior )
				}
			});

		public static OppositionGroup TerathansAndOphidians
		{
			get { return m_TerathansAndOphidians; }
		}

		private static OppositionGroup m_SavagesAndOrcs = new OppositionGroup(new Type[][]
			{
				new Type[]
				{
					typeof( Orc ),
					typeof( OrcBomber ),
					typeof( OrcBrute ),
					typeof( OrcCaptain ),
					typeof( OrcishLord ),
					typeof( OrcishMage ),
					typeof( SpawnedOrcishLord )
				},
				new Type[]
				{
					typeof( Savage ),
					typeof( SavageRider ),
					typeof( SavageRidgeback ),
					typeof( SavageShaman )
				}
			});

		public static OppositionGroup SavagesAndOrcs
		{
			get { return m_SavagesAndOrcs; }
		}

		private static OppositionGroup m_FeyAndUndead = new OppositionGroup(new Type[][]
			{
				new Type[]
				{
					typeof( Centaur ),
					typeof( EtherealWarrior ),
					typeof( Kirin ),
					typeof( LordOaks ),
					typeof( Pixie ),
					typeof( Silvani ),
					typeof( Unicorn ),
					typeof( Wisp ),
					typeof( Treefellow )
				},
				new Type[]
				{
					typeof( AncientLich ),
					typeof( Bogle ),
					typeof( LichLord ),
					typeof( Shade ),
					typeof( Spectre ),
					typeof( Wraith ),
					typeof( BoneKnight ),
					typeof( Ghoul ),
					typeof( Mummy ),
					typeof( SkeletalKnight ),
					typeof( Skeleton ),
					typeof( Zombie ),
					typeof( ShadowKnight ),
					typeof( DarknightCreeper ),
					//typeof( RevenantLion ),
					//typeof( LadyOfTheSnow ),
					typeof( RottingCorpse ),
					typeof( SkeletalDragon ),
					typeof( Lich )
				}
			});

		public static OppositionGroup FeyAndUndead
		{
			get { return m_FeyAndUndead; }
		}
	}
}