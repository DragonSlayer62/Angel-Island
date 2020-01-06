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

using System;
using Server;
using Server.Accounting;

namespace Server.Misc
{
	public class AccountPrompt
	{
		// This script prompts the console for a username and password when 0 accounts have been loaded
		public static void Initialize()
		{
			if (Accounts.Table.Count == 0 && !Core.Service)
			{
				Console.WriteLine("This server has no accounts.");
				Console.WriteLine("Do you want to create an administrator account now? (y/n)");

				if (Console.ReadLine().StartsWith("y"))
				{
					Console.Write("Username: ");
					string username = Console.ReadLine();

					Console.Write("Password: ");
					string password = Console.ReadLine();

					Account a = Accounts.AddAccount(username, password);

					a.AccessLevel = AccessLevel.Administrator;

					Console.WriteLine("Account created, continuing");
				}
				else
				{
					Console.WriteLine("Account not created.");
				}
			}
		}
	}
}