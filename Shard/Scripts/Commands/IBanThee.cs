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
 *	restrictions set forth in subaragraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */

/* Scripts/Commands/IBanThee.cs
 * ChangeLog
 *	3/1/10, Adam
 *		was .FightBroker access
 *		DO NOT USE until we find a better place to get the gold from. generating it from thin air would allow
 *		a FightBroker to generate gold. Maybe acceptable, but we must do so knowingly
 * 07/21/06, Rhiannon
 *		Changed access level to FightBroker
 * 04/13/06, Kit
 *		Initial Version.
 */

using System;
using Server;
using Server.Accounting;
using Server.BountySystem;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Engines;
using Server.Commands;

namespace Server.PJUM
{
	public class IBanThee
	{
		public static void Initialize()
		{
			// Adam: was .FightBroker access
			// DO NOT USE until we find a better place to get the gold from. generating it from thin air would allow
			//	a FightBroker to generate gold. Maybe acceptable, but we must do so knowingly
			CommandSystem.Register("ibanthee", AccessLevel.Administrator, new CommandEventHandler(IBanThee_OnCommand));
		}

		public IBanThee()//Mobile m, DateTime dt, Location loc)
		{
		}

		public static void IBanThee_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new IBanTheeTarget();
			e.Mobile.SendMessage("Who do you wish to ban?");
			e.Mobile.Say("I ban thee");
		}

		private class IBanTheeTarget : Target
		{
			public IBanTheeTarget()
				: base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (targ is PlayerMobile)
				{
					try
					{
						PlayerMobile pm = (PlayerMobile)targ;

						string location;
						int xLong = 0, yLat = 0, xMins = 0, yMins = 0;
						bool xEast = false, ySouth = false;
						Map map = pm.Map;
						bool valid = Sextant.Format(pm.Location, map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth);

						if (valid)
							location = Sextant.Format(xLong, yLat, xMins, yMins, xEast, ySouth);
						else
							location = "????";

						if (!valid)
							location = string.Format("{0} {1}", pm.X, pm.Y);

						if (map != null)
						{
							Region reg = pm.Region;

							if (reg != map.DefaultRegion)
							{
								location += (" in " + reg);
							}
						}

						//Output command log.
						Server.Commands.CommandLogging.WriteLine(from, "{0} used [ibanthee command on {1}({2}) - at {3}",
							from.Name, pm.Name, pm.Serial, location);

						from.SendMessage("Reporting {0} as Banned!", pm.Name);

						string[] lns = new string[2];
						lns[0] = String.Format("A bounty has been placed on the head of {0} for disrupting a royal tournament. .", pm.Name);
						lns[1] = String.Format("{0} was last seen at {1}.", pm.Name, location);


						if (PJUM.HasBeenReported(pm))
						{
							from.SendMessage("{0} has already been reported.", pm.Name);
						}
						else
						{

							//move player to outside arena
							pm.MoveToWorld(new Point3D(353, 905, 0), Map.Felucca);

							PJUM.AddMacroer(lns, pm, DateTime.Now + TimeSpan.FromHours(2));

							//Add bounty to player
							string name = String.Format("Officer {0}", Utility.RandomBool() ? NameList.RandomName("male") : NameList.RandomName("female"));

							int bountyAmount = 0;
							Container cont = pm.BankBox;
							if (cont != null)
							{
								int iAmountInBank = 0;

								Item[] golds = cont.FindItemsByType(typeof(Gold), true);
								foreach (Item g in golds)
								{
									iAmountInBank += g.Amount;
								}

								int min = Math.Min(iAmountInBank, 1000);
								int max = Math.Min(iAmountInBank, 3000);

								int randomAmount = Utility.RandomMinMax(min, max);

								if (cont.ConsumeTotal(typeof(Gold), randomAmount))
								{
									bountyAmount = randomAmount;
								}
							}
							// Adam: this is broken logic. Needs fixing
							if (bountyAmount == 0)
							{
								bountyAmount = 100;
							}

							Bounty bounty = new Bounty((PlayerMobile)from, pm, bountyAmount, name);
							BountyKeeper.Add(bounty);

							//Add comment to account
							Account acc = pm.Account as Account;
							string comment = String.Format("On {0}, {1} caught {2} disturbing event at {3} : removed using the [ibanthee command",
								DateTime.Now,
								from.Name,
								pm.Name,
								location);
							acc.Comments.Add(new AccountComment(from.Name, comment));
						}
					}
					catch (Exception except)
					{
						LogHelper.LogException(except);
						System.Console.WriteLine("Caught exception in [ibanthee command: {0}", except.Message);
						System.Console.WriteLine(except.StackTrace);
					}
				}
				else
				{
					from.SendMessage("Only players can be banned.");
				}
			}
		}

	}
}
