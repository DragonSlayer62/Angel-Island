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
 ********************************************************
 * What this script does: 
 *    Makes a Champion-like gold splash of [amount] in
 * a 7x7 square around targeted location.  This creates
 * 49 piles of [amount/49] gold.  A small amount of gold
 * is added to each pile to make the amount seem random.
 * Only AcessLevel.Administrator can use the command, and
 * the amount and location is logged.
 ********************************************************
 * 
 * Changelog:
 * 04/23/05, erlein
 *  Changed to Seer level access.
 * 7/20/04, Old Salty
 * 	Command Created.
 * 
 */


using System;
using Server;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.Commands
{
	public class SplashGold
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("SplashGold", AccessLevel.Seer, new CommandEventHandler(SplashGold_OnCommand));
		}

		[Usage("SplashGold [amount]")]
		[Description("Creates champion-type gold splash.")]
		private static void SplashGold_OnCommand(CommandEventArgs e)
		{
			int amount = 1;
			if (e.Length >= 1)
				amount = e.GetInt32(0);

			if (amount < 100)
			{
				e.Mobile.SendMessage("Splash at least 100 gold.");
			}
			else if (amount > 2800000)
			{
				e.Mobile.SendMessage("Amount exceeded.  Use an amount less than 2800000.");
			}
			else
			{
				e.Mobile.Target = new SplashTarget(amount > 0 ? amount : 1);
				e.Mobile.SendMessage("Where do you want the center of the gold splash to be?");
			}
		}

		private class SplashTarget : Target
		{
			private int m_Amount;
			private string m_Location;

			public SplashTarget(int amount)
				: base(15, true, TargetFlags.None)
			{
				m_Amount = amount;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				IPoint3D center = targ as IPoint3D;
				if (center != null)
				{
					Point3D p = new Point3D(center);
					m_Location = (p.X.ToString() + ", " + p.Y.ToString());

					Map map = from.Map;

					if (map != null)
					{
						for (int x = -3; x <= 3; ++x)
						{
							for (int y = -3; y <= 3; ++y)
							{
								double dist = Math.Sqrt(x * x + y * y);

								if (dist <= 12)
									new GoodiesTimer(map, p.X + x, p.Y + y, m_Amount).Start();
							}
						}
					}
				}
				CommandLogging.WriteLine(from, "{0} {1} splashed {2} gold at {3} )", from.AccessLevel, CommandLogging.Format(from), m_Amount, m_Location);

			}
		}

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;
			private int m_Amount;

			public GoodiesTimer(Map map, int x, int y, int amount)
				: base(TimeSpan.FromSeconds(Utility.RandomDouble() * 10.0))
			{
				m_Amount = amount;
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ(m_X, m_Y);
				bool canFit = m_Map.CanFit(m_X, m_Y, z, 6, CanFitFlags.requireSurface);

				for (int i = -3; !canFit && i <= 3; ++i)
				{
					canFit = m_Map.CanFit(m_X, m_Y, z + i, 6, CanFitFlags.requireSurface);

					if (canFit)
						z += i;
				}

				if (!canFit)
					return;

				Gold g = new Gold(m_Amount / 49 + Utility.Random(m_Amount / 10000, m_Amount / 1000 - m_Amount / 10000));

				g.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);

				if (0.5 >= Utility.RandomDouble())
				{
					switch (Utility.Random(3))
					{
						case 0: // Fire column
							{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
								Effects.PlaySound(g, g.Map, 0x208);

								break;
							}
						case 1: // Explosion
							{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
								Effects.PlaySound(g, g.Map, 0x307);

								break;
							}
						case 2: // Ball of fire
							{
								Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);

								break;
							}
					}
				}
			}
		}
	}
}
