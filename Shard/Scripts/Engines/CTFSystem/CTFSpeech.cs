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

/* Scripts\Engines\CTFSystem\CTFSpeech.cs
 * CHANGELOG:
 * 4/10/10, adam
 *		initial framework.
 */

using System;
using System.Text;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines
{
	public partial class CTFControl : Server.Items.RegionControl
	{
		// passed up from the region. Return true if handled, false otherwise
		public bool OnRegionSpeech(SpeechEventArgs e)
		{	// the CTF region supports DOT commands, i.e., .stats or .help
			string text = e.Speech.ToLower();	// lowercase
			string[] tokens = text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			switch (tokens[0])
			{
				case ".help":
					if (tokens.Length == 1)
					{
						e.Mobile.SendMessage(e.Mobile.SpeechHue, ".help commands: list commands.");
						e.Mobile.SendMessage(e.Mobile.SpeechHue, ".help game: get game help and tips.");
						return true;
					}
					else
					{
						switch (tokens[1])
						{
							case "commands":
							case "command":
								{
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".help: lists kinds of help.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".help commands: list commands.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".help game: get game help and tips.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".stats: Game score and player kills.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".time: Time left in the game/round.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".kick <name>: Kick a team member for betrayal.");
									e.Mobile.SendMessage(e.Mobile.SpeechHue, ".exit: Exit the game and go home (pussy-cake).");
								}
								return true;

							case "game":
								{
									if (GetTeam(e.Mobile) == Offense)
									{
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "You are on offense. Objective:");
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "Go to the enemy base and capture the flag and bring it back to your home base.");
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "The flag is a bright red staff. Just walk over it to pick it up.");
									}
									else if (GetTeam(e.Mobile) == Defense)
									{
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "You are on defense. Objective:");
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "Prevent the enemy from taking your flag!");
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "The flag is the bright red staff.");
										e.Mobile.SendMessage(e.Mobile.SpeechHue, "If the enemy takes your flag, you must kill them before they can take it back to their base.");
									}
								}
								return true;

							default:
								break;
						}
					}
					break;

				case ".stats":
					{
						if (CurrentState == States.PlayRound)
						{
							e.Mobile.SendMessage(e.Mobile.SpeechHue, "The score is {0}-{1}. ({2})",
								Math.Max(OffenseScore, DefenseScore), Math.Min(OffenseScore, DefenseScore),
								(OffenseScore > DefenseScore && GetTeam(e.Mobile) == Offense) ? "You're winning" :
								(DefenseScore > OffenseScore && GetTeam(e.Mobile) == Defense) ? "You're winning" :
								(DefenseScore == OffenseScore) ? "Tied" : "You're loosing");
						}
						else
							e.Mobile.SendMessage(e.Mobile.SpeechHue, "No score data is available.");

						if (CurrentState == States.PlayRound)
						{
							Dictionary<PlayerMobile, PlayerContextData> team = GetTeam((GetTeam(e.Mobile)));
							if (team != null)
							{
								e.Mobile.SendMessage(e.Mobile.SpeechHue, "You have {0} kill{1}.", team[e.Mobile as PlayerMobile].Points, team[e.Mobile as PlayerMobile].Points == 1 ? "" : "s");
							}
							else
							{	// you are not on a team
								e.Mobile.SendMessage(e.Mobile.SpeechHue, "No kills data is available.");
							}
						}
						else
							e.Mobile.SendMessage(e.Mobile.SpeechHue, "No kills data is available.");

						if (CurrentState == States.PlayRound)
						{
							if (IsFlagHome() == false)
							{
								if (GetTeam(e.Mobile) == Defense && Flag.RootParent == null)
									e.Mobile.SendMessage(e.Mobile.SpeechHue, "Your flag is away.");				// flag is on the ground somewhere
								else if (GetTeam(e.Mobile) == Defense && Flag.RootParent != null)
									e.Mobile.SendMessage(e.Mobile.SpeechHue, "Your flag has been taken.");		// enemy has your flag
								if (GetTeam(e.Mobile) == Offense && Flag.RootParent != null)
									e.Mobile.SendMessage(e.Mobile.SpeechHue, "Your team has the enemy flag.");	// your team has the enmy flag.
							}
						}
					}
					return true;

				case ".time":
					{
						e.Mobile.SendMessage(e.Mobile.SpeechHue, "There are {0:0.##} minutes left in the round with {1:0.##} minutes left in the game.",
							GetRoundRemainingTime().TotalMinutes, GetGameRemainingTime().TotalMinutes);
					}
					return true;

				case ".kick":
					{
						if (tokens.Length == 1)
							e.Mobile.SendMessage(e.Mobile.SpeechHue, "use .kick Player Name");
						else
						{	// get the name of the player to kick 
							string name = e.Speech.Substring(".kick".Length).Trim();

							// you may only kick someone your own team
							if (GetTeam(GetTeam(e.Mobile)) != null)
							{
								PlayerMobile pm = null;
								foreach (KeyValuePair<PlayerMobile, PlayerContextData> kvp in GetTeam(GetTeam(e.Mobile)))
								{
									if (kvp.Key.Name.ToLower() == name)
									{
										pm = kvp.Key;
										KickPlayer(kvp.Key, kvp.Value, "You have been kicked!", KickReason.Kicked);
										break;
									}
								}

								// now remove the player from the team list
								if (pm != null)
									if (GetTeam(GetTeam(e.Mobile)).ContainsKey(pm))
										GetTeam(GetTeam(e.Mobile)).Remove(pm);
							}
						}
					}
					return true;

				case ".exit":
					{	// if I'm on a team, select the team and delete me from it
						if (GetTeam(GetTeam(e.Mobile)) != null)
						{
							PlayerContextData pcd = GetTeam(GetTeam(e.Mobile))[e.Mobile as PlayerMobile];
							KickPlayer(e.Mobile, pcd, "You have left the game.", KickReason.Exited);
							GetTeam(GetTeam(e.Mobile)).Remove(e.Mobile as PlayerMobile);
						}
					}
					return true;
			}

			return false;
		}

		// do we understand what is being said?
		// put all text that should be understood by the fightbroker here.
		public static bool UnderstandsSpeech(SpeechEventArgs e)
		{
			if (e == null || e.Speech == null || e.Speech.Length == 0)
				return false;

			string text = e.Speech.ToLower();

			string[] command_tab = {
				"help ctf",					// get help
				"register ctf",				// start a new match
				"yes", "accept",			// do you accept this CTF match?
				"capture the flag",
				"ctf"
				};

			for (int ix = 0; ix < command_tab.Length; ix++)
			{
				if (Utility.Token(command_tab[ix], text))
					return true;
			}

			return false;
		}

		// text from the fight broker.
		public void OnSpeech(Mobile broker, SpeechEventArgs e)
		{	// do we understand what is being said?
			if (UnderstandsSpeech(e) == false)
				return;

			// sanity
			if (e == null || e.Mobile == null || !MobileOk(broker))
				return;

			// player is dead, or in jail, or a criminal or something
			if (!MobileOk(e.Mobile))
			{
				broker.SayTo(e.Mobile, "Something about you or your location is preventing me from helping you.");
				return;
			}

			// since we understand what is being said, it shall be handled
			e.Handled = true;
			string text = e.Speech.ToLower();

			// block all access to the CTF system if system is not enabled
			if (this.Enabled == false)
			{
				broker.SayTo(e.Mobile, "Capture the Flag is not available at this time.");
				return;
			}

			if (Utility.Token("ctf", text) || Utility.Token("capture the flag", text))
			{
				broker.SayTo(e.Mobile, "Indeed, I know something of this subject.");
				broker.SayTo(e.Mobile, "Just say \"help ctf\" and I would be happy to help you.");
			}
			else if (Utility.Token("help ctf", text))
			{
				if (m_WatchTable.ContainsKey(e.Mobile) && m_WatchTable[e.Mobile].Valid == false)
				{
					broker.SayTo(e.Mobile, "Quit bothering me!");
				}
				else
				{	// see if the playe has this TYPE and VERSION of the book, if not, give it
					if (PlayerHasBook(e.Mobile, BaseBook.BookSubtype.CTFHelp, 1.0) == false)
					{
						BaseBook book = WriteHelpBook();
						e.Mobile.Backpack.AddItem(book);
						broker.SayTo(e.Mobile, "I have placed a book in your backpack, what you need to know is contained within.");
					}
					else
					{
						broker.SayTo(e.Mobile, "You already have the book, what you need to know is contained within.");
					}

					// track how many times this player asks for registration (exploit prevention.)
					if (m_WatchTable.ContainsKey(e.Mobile) == false)
						m_WatchTable[e.Mobile] = new PlayerRequests();
				}
			}
			else if (Utility.Token("register ctf", text))
			{
				if (CTFSetupOK() == false)
				{
					broker.SayTo(e.Mobile, "I'm sorry, but Capture The Flag is not yet setup.");
					broker.SayTo(e.Mobile, "Please try back later.");
				}
				else if (CurrentState != States.Quiescent)
				{
					broker.SayTo(e.Mobile, "I'm sorry, but all Capture the Flag maps are currently in use.");
					broker.SayTo(e.Mobile, "Please try back later.");
				}
				else if (m_WatchTable.ContainsKey(e.Mobile) && m_WatchTable[e.Mobile].Valid == false)
				{
					broker.SayTo(e.Mobile, "Quit bothering me!");
				}
				else
				{	// start fresh
					Reset();

					// save some important state and kick off the process!
					Captain1 = e.Mobile;
					Broker = broker;
					CurrentState = States.Registration;

					// track how many times this player asks for registration (exploit prevention.)
					if (m_WatchTable.ContainsKey(e.Mobile) == false)
						m_WatchTable[e.Mobile] = new PlayerRequests();

				}
			}
			// if Captain2 said YES or ACCEPT to the challenge AND we're in the WaitAcceptChallenge state 
			else if (e.Mobile == Captain2 && (Utility.Token("yes", text) || Utility.Token("accept", text)) && CurrentState == States.WaitAcceptChallenge)
			{
				// okay, give them the book, and await its return
				CurrentState = States.GiveBook;
			}

		}
	}
}