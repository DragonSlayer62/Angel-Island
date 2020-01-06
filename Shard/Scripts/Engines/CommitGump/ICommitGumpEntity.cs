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
 *			March 27, 2007
 */

/* Scripts/Engines/CommitGump/ICommitGumpEntity.cs
 * CHANGELOG:
 *	01/10/09 - Plasma,
 *		Initial creation
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.CommitGump
{

	public interface ICommitGumpEntity
	{
		/// <summary>
		/// Unqiue ID use as a key in the sesssion
		/// </summary>
		string ID { get; }
		//bool IsDirty { get; }

		/// <summary>
		/// Commit any outstanding changes
		/// </summary>
		void CommitChanges();

		/// <summary>
		/// Creation of the entity's graphics
		/// </summary>
		void Create();

		/// <summary>
		/// Restore data from the session
		/// </summary>
		void LoadStateInfo();

		/// <summary>
		/// Handle user response 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		CommitGumpBase.GumpReturnType OnResponse(Server.Network.NetState sender, RelayInfo info);

		/// <summary>
		/// Update the state / session with any changes in memory
		/// </summary>
		void SaveStateInfo();

		/// <summary>
		/// Validate outstanding changes
		/// </summary>
		/// <returns></returns>
		bool Validate();
	}

}
