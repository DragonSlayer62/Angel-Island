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

namespace Server.Engines.Chat
{
	public enum ChatCommand
	{
		/// <summary>
		/// Add a channel to top list.
		/// </summary>
		AddChannel = 0x3E8,
		/// <summary>
		/// Remove channel from top list.
		/// </summary>
		RemoveChannel = 0x3E9,
		/// <summary>
		/// Queries for a new chat nickname.
		/// </summary>
		AskNewNickname = 0x3EB,
		/// <summary>
		/// Closes the chat window.
		/// </summary>
		CloseChatWindow = 0x3EC,
		/// <summary>
		/// Opens the chat window.
		/// </summary>
		OpenChatWindow = 0x3ED,
		/// <summary>
		/// Add a user to current channel.
		/// </summary>
		AddUserToChannel = 0x3EE,
		/// <summary>
		/// Remove a user from current channel.
		/// </summary>
		RemoveUserFromChannel = 0x3EF,
		/// <summary>
		/// Send a message putting generic conference name at top when player leaves a channel.
		/// </summary>
		LeaveChannel = 0x3F0,
		/// <summary>
		/// Send a message putting Channel name at top and telling player he joined the channel.
		/// </summary>
		JoinedChannel = 0x3F1
	}
}