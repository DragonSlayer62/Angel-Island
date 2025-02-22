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

/* Accounting/PasswordGump.cs, created by Pixie
 * CHANGELOG:
 *  2/26/07, Adam
 *      Better address checking ala SmtpDirect.CheckEmailAddy()
 *	11/06/06, Pix
 *		Change as a result of function change in ProfileGump
 *	7/05/05, Pix
 *		Now sends audit emails instead of CCs.
 *	6/28/05, Pix
 *		Changed to 'reset password' functionality.
 *	5/17/04, Pixie
 *		Initial Version
 */

using System;
using System.Text;
using Server.Accounting;		// Emailer
using Server.SMTP;					    // core SMTP engine

namespace Server.Gumps
{
	public class PasswordGump : Gump
	{
		private Mobile m_From;
		private const int LabelHue = 0x480;
		private const int LabelColor32 = 0xFFFFFF;

		public PasswordGump(Mobile from)
			: base(50, 40)
		{
			m_From = from;

			from.CloseGump(typeof(PasswordGump));

			AddPage(0);
			AddBackground(0, 0, 480, 220, 5054);
			AddImageTiled(10, 10, 460, 200, 2624);
			AddAlphaRegion(10, 10, 460, 200);

			//x,y,width,height
			StringBuilder sb = new StringBuilder();
			sb.Append("Enter the account name for which you want to send a reset password.");
			sb.Append("  If this account has been activated (with a verified email address),");
			sb.Append(" a new password will be sent to the email address.");

			AddHtml(20, 20, 440, 100, sb.ToString(), true, true);

			AddLabelCropped(20, 140, 150, 20, LabelHue, "Account Name");
			AddTextField(160, 140, 100, 20, 0, "");

			AddButtonLabeled(160, 170, 1, "OK");

		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			int val = info.ButtonID;

			if (val <= 0)
				return;

			Mobile from = m_From;

			switch (val)
			{
				case 1:
					string accountname = info.GetTextEntry(0).Text;
					m_From.SendMessage("You entered [{0}]", accountname);

					foreach (Accounting.Account a in Accounting.Accounts.Table.Values)
					{
						if (a != null)
						{
							if (a.Username.ToLower() == accountname.ToLower())
							{
								if (a.AccountActivated)
								{
									if ((DateTime.Now - a.ResetPasswordRequestedTime) < TimeSpan.FromDays(1.0))
									{
										m_From.SendMessage("Reset password already requested.");
									}
									else
									{
										string newResetPassword = Server.Gumps.ProfileGump.CreateActivationKey(8);
										if (SmtpDirect.CheckEmailAddy(a.EmailAddress, false) == true)
										{
											string subject = "Angel Island Account password reset request";
											string body = "\nSomeone has requested a password reset for your account.\n";
											body += "A new password has been generated for your account.\n";
											body += "It is: " + newResetPassword;
											body += "\n\nIf you did not request this reset password, log onto Angel Island with your normal ";
											body += "password and the reset request will be cancelled.  Also, please report this ";
											body += "to the staff of Angel Island.";
											body += "\n\nYou can change your password using the [profile command.\n\n";
											body += "Regards,\n  The Angel Island Team\n\n";

											Emailer mail = new Emailer();
											if (mail.SendEmail(a.EmailAddress, subject, body, false))
											{
												string regSubject = "Password reset request";
												string regBody = "Password reset reqest made.\n";
												regBody += "Info of from: \n";
												regBody += "\n";
												Accounting.Account from_account = m_From.Account as Accounting.Account;
												regBody += "Account: " + (from_account == null ? "<unknown>" : from_account.Username) + "\n";
												regBody += "Character: " + from.Name + "\n";
												regBody += "IP: " + sender.Address.ToString() + "\n";
												regBody += "\n";
												regBody += "Account requested for: " + a.Username + "\n";
												regBody += "Email: " + a.EmailAddress + "\n";
												regBody += "Reset password: " + newResetPassword + "\n";
												regBody += "\n";
												mail.SendEmail("aiaccounting@game-master.net", regSubject, regBody, false);

												a.ResetPassword = newResetPassword;
												m_From.SendMessage("Password reset request generated.");
												m_From.SendMessage("Email sent to account's email address.");
											}
											else
											{
												m_From.SendMessage("Error sending email to account's email.");
											}
										}
										else
										{
											m_From.SendMessage("Account email invalid, unable to reset password.");
										}
									}
								}
								else
								{
									m_From.SendMessage("Account not activated, unable to reset password.");
								}
								break;
							}
						}
					}

					break;
				default:
					break;
			}
		}


		//helper functions
		public void AddTextField(int x, int y, int width, int height, int index)
		{
			AddBackground(x - 2, y - 2, width + 4, height + 4, 0x2486);
			AddTextEntry(x + 2, y + 2, width - 4, height - 4, 0, index, "");
		}
		public void AddTextField(int x, int y, int width, int height, int index, string initialvalue)
		{
			AddBackground(x - 2, y - 2, width + 4, height + 4, 0x2486);
			AddTextEntry(x + 2, y + 2, width - 4, height - 4, 0, index, initialvalue);
		}
		public void AddButtonLabeled(int x, int y, int buttonID, string text)
		{
			AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
			AddHtml(x + 35, y, 240, 20, Color(text, LabelColor32), false, false);
		}
		public string Color(string text, int color)
		{
			return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
		}
	}
}
