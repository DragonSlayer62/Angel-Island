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

/* Scripts\Misc\Diagnostics.cs
 * ChangeLog
 *  8/11/07, Adam
 *      Have Assert return the result code so the Assert can be used within an IF
 *	8/2/07, Adam
 *		Add Assert() Diagnostic to raise an exception, record it an email the results to the team
 *      First time checkin
 */

using System;
using Server;
using Server.Commands;

namespace Server.Misc
{
	public class Diagnostics
	{
		public static bool Assert(bool assertion, string text)
		{
			if (assertion == true)
				return true;

			try { throw new ApplicationException(text); }
			catch (Exception ex) { LogHelper.LogException(ex); }

			return false;
		}
	}
}
