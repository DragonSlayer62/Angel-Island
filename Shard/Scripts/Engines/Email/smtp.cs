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

/* Engines/Email/smtp.cs
 * CHANGELOG:
 *	9/8/08, Adam
 *		- normalize all weird newline combos
 *		- use GetEnvironmentVariable to get SMTP password
 *  12/24/06, Adam
 *      Add new static CheckEmailAddy() to validate an email address
 *  11/18/06, Adam
 *      Update to new .NET 2.0 email services
 *	11/8/06, Adam
 *		Add a bit of debug output
 *	11/7/06, Adam
 *		Copy attachments over to the email object
 *	11/3/06, Adam
 *		Go back to using Collaborative Data Object (CDO) for SMTP send in an attempt
 *			to resolve mail that is rejected by Yahoo and some other very picky servers.
 *			Also, stop relaying the email, and instead authenticate to a 'real' account (not an alias.)
 *	6/24/06, Adam
 *		Update header 'Date' field to us the RFC 1123 Pattern
 *	2/2/06, Adam
 *		Add a new constructor: 
 *			public bool Send(MailMessage message)
 *	1/31/06, Adam
 *		Initial Version
 *		From a modified version of this software by Dr. Peter Bromberg
 *		http://www.eggheadcafe.com/articles/20030316.asp
 */

using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Server.Misc;			// Test Center
using Server.Commands;

namespace Server.SMTP
{
	/// <summary>
	/// provides methods to send email via smtp direct to mail server
	/// </summary>
	public class SmtpDirect
	{
		/// <summary>
		/// Get / Set the name of the SMTP mail server
		/// </summary>

		public SmtpDirect()
		{
			// nada			
		}

		public static string FromEmailAddress
		{
			get { return "noreply@game-master.net"; }
		}

		public static string CCEmailAddress
		{
			get { return "aiaccounting@game-master.net"; }
		}

		public static string Server
		{
			get { return "mail.game-master.net"; }
		}

		public static Attachment MailAttachment(string file)
		{
			// Create  the file attachment for this e-mail message.
			Attachment data = new Attachment(file);
			// Add time stamp information for the file.
			ContentDisposition disposition = data.ContentDisposition;
			disposition.CreationDate = System.IO.File.GetCreationTime(file);
			disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
			disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
			return data;
		}

		// check the form of this address or list.
		// Under the message.to.Add is a call to System.Net.Mime.MailBnfHelper() which
		//  which validates the form
		public static bool CheckEmailAddy(string address, bool AllowList)
		{
			try
			{
				if (AllowList == false)
					if (address.IndexOf(',') >= 0 || address.IndexOf(';') >= 0)
						return false;

				MailMessage message = new MailMessage();
				message.To.Add(SmtpDirect.ClassicList(address));
			}
			catch
			{   // bad format
				return false;
			}
			// good form
			return true;
		}

		// convert a normal semicolon delimited list of emails addresses...
		public static string ClassicList(string list)
		{
			while (list.IndexOf(' ') > -1)
				list = list.Remove(list.IndexOf(' '), 1);
			list = list.Replace(';', ',');
			return list;
		}

		public bool SendEmail(string toAddress, string subject, string body, bool ccRegistration)
		{
			if (Server == null || FromEmailAddress == null)
				return false;

			MailMessage message = new MailMessage();
			try
			{
				message.To.Add(SmtpDirect.ClassicList(toAddress));
				message.From = new MailAddress(FromEmailAddress);

				if (ccRegistration && CCEmailAddress.Length != 0)
					message.CC.Add(SmtpDirect.ClassicList(CCEmailAddress));
			}
			catch (Exception ex)
			{   // you should be calling CheckEmailAddy() first to avoid this error
				LogHelper.LogException(ex);
				return false;
			}

			message.Subject = subject;
			message.Body = body;

			return Send(message);
		}

		public bool SendEmail(MailMessage message)
		{
			return Send(message);
		}

		public bool Send(MailMessage message)
		{
			bool result = false;
			try
			{
				Console.WriteLine("Email: To: {0} Subject: \"{1}\"",
					message.To.ToString() != null ? message.To.ToString() : "(null)",
					message.Subject.ToString() != null ? message.Subject.ToString() : "(null)");
				result = _Send(message);
			}
			// error has already been handled
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			if (result == true)
				Console.WriteLine("Email: Send ok.");
			else
				Console.WriteLine("Email: Send failed.");

			return result;
		}

		private bool _Send(MailMessage message)
		{
			try
			{
				// normalize all weird newline combos
				message.Body = message.Body.Replace("\r\n", "$n$");	// good => meta
				message.Body = message.Body.Replace("\r", "$n$");	// bad => meta
				message.Body = message.Body.Replace("\n", "$n$");	// bad => meta
				message.Body = message.Body.Replace("$n$", "\r\n");	// meta => good

				string password = Environment.GetEnvironmentVariable("NOREPLY_PASSWORD");
				if (password == null || password.Length == 0)
#if DEBUG
					return false;
#else
					throw new ApplicationException("the SMTP password is not set.");
#endif

				SmtpClient client = new SmtpClient(Server);
				client.Credentials = new System.Net.NetworkCredential(message.From.ToString(), password);
				client.Port = 25;
				client.Send(message);

				return true;
			}
			catch (System.Net.Sockets.SocketException se)
			{
				LogHelper.LogException(se);
				Console.WriteLine("Caught SocketException: {0}", se.Message);
				Console.WriteLine(se.StackTrace);
				return false;
			}
			catch (System.Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("Caught Exception: {0}", e.Message);
				Console.WriteLine(e.StackTrace);
				return false;
			}

		}
	}
}

