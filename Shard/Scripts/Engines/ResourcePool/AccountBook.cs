/* *	This program is the CONFIDENTIAL and PROPRIETARY property 
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

/* Scripts/Engines/ResourcePool/AccountBook.cs
 * ChangeLog
 *  04/27/05 TK
 *		Made resources sorted by name so it's easier to find them in book.
 *  02/07/05 TK
 *		Made accountbooks un-copyable.
 *  06/02/05 TK
 *		Removed a few lingering debug WriteLine's
 *	03/02/05 Taran Kain
 *		Created.
 * 
 */

using System;
using System.Collections;
using Server;
using Server.Engines.ResourcePool;

namespace Server.Items
{
	class AccountBook : BaseBook
	{
		//		public override bool Writable { get { return false; } }

		[Constructable]
		public AccountBook()
			: base(0xFF1, "title", "author", 0, false)
		{
			Copyable = false;
		}

		public AccountBook(Serial ser)
			: base(ser)
		{
		}

		public override void OnSingleClick(Mobile from)
		{
			LabelTo(from, "an accounting book");
		}

		public override void OnDoubleClick(Mobile from)
		{
			Title = "Accounts";
			Author = from.Name;

			ClearPages();
			ArrayList al = new ArrayList(ResourcePool.Resources.Values);
			al.Sort();
			for (int i = 0; i < al.Count; i++)
			{
				if (al[i] is RDRedirect)
					continue;
				ResourceData rd = al[i] as ResourceData;

				string inv = rd.DescribeInvestment(from);

				AddLine(rd.Name + ":");
				AddLine(inv);

				if (Pages[Pages.Length - 1].Lines.Length < 8)
					AddLine("");
			}

			string ttemp = ResourceLogger.GetHistory(from);
			string[] history = ttemp.Split(new char[] { '\n' });

			string line;
			foreach (string trans in history)
			{
				if (trans == "")
					continue;
				string[] tlines = trans.Split(new char[] { ' ' });
				line = "";
				foreach (string t in tlines)
				{
					if ((line.Length + t.Length + 1) <= 20)
						line += t + " ";
					else
					{
						AddLine(line);
						line = t + " ";
					}
				}
				AddLine(line);
				AddLine("");
			}

			base.OnDoubleClick(from);
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

			Copyable = false;
		}
	}
}
