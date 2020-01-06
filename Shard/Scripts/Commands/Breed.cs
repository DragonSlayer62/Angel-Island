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

/* Scripts/Commands/Breed.cs
 * Changelog
 *  4/4/07, Adam
 *      comment out the registration for [breed
 *	12/07/06 Taran Kain
 *		Initial version.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Commands
{
	class Breed
	{
		public static void Initialize()
		{
			//if (Server.Misc.TestCenter.Enabled)
			//Server.Commands.Register("Breed", AccessLevel.Player, new CommandEventHandler(Breed_OnCommand));
		}

		public static void Breed_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Target the female.");
			e.Mobile.Target = new BreedFemaleTarget();
		}

		public class BreedFemaleTarget : Target
		{
			public BreedFemaleTarget()
				: base(10, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				BaseCreature female = targeted as BaseCreature;

				if (female == null)
					from.SendMessage("You must select an animal to breed!");
				else if (!female.Female)
					from.SendMessage("That is not a female.");
				else if (!female.BreedingEnabled)
					from.SendMessage("That creature is not capable of being bred.");
				else if (!female.Controlled)
					from.SendMessage("The animal must be tamed before it can be bred.");
				else
				{
					from.SendMessage("Now select a male to breed {0} with.", female.Name);
					from.Target = new BreedMaleTarget(female);
				}
			}
		}

		public class BreedMaleTarget : Target
		{
			private BaseCreature m_Female;

			public BreedMaleTarget(BaseCreature female)
				: base(10, false, TargetFlags.None)
			{
				m_Female = female;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				BaseCreature male = targeted as BaseCreature;

				if (male == null)
					from.SendMessage("You must select an animal to breed with!");
				else if (male.Female)
					from.SendMessage("That is a female!");
				else if (!male.BreedingEnabled)
					from.SendMessage("That creature is not capable of being bred.");
				else if (male.GetType() != m_Female.GetType())
					from.SendMessage("Both the male and female must be the same species.");
				else if (!male.Controlled)
					from.SendMessage("The animal must be tamed before it can be bred.");
				else
				{
					from.SendMessage("Target where the child should be born.");
					from.Target = new ChildTarget(m_Female, male);
				}
			}
		}

		public class ChildTarget : Target
		{
			BaseCreature m_Female, m_Male;

			public ChildTarget(BaseCreature female, BaseCreature male)
				: base(10, true, TargetFlags.None)
			{
				m_Female = female;
				m_Male = male;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				IPoint3D loc = targeted as IPoint3D;

				if (loc == null)
					from.SendMessage("Invalid location.");
				else
				{
					BaseCreature child = m_Female.BreedWith(m_Male);

					if (child == null)
						from.SendMessage("An unknown error occurred and has been logged.");

					child.Controlled = true;
					child.ControlMaster = from;
					child.MoveToWorld(new Point3D(loc), from.Map);
				}
			}
		}
	}
}
