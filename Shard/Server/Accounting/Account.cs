/***************************************************************************
 *                                 Account.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Account.cs,v 1.7 2011/02/24 18:32:33 luket Exp $
 *   $Author: luket $
 *   $Date: 2011/02/24 18:32:33 $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Xml;

namespace Server.Accounting
{
	public interface IAccount
	{
		int Length { get; }
		int Limit { get; }
		int Count { get; }
		Mobile this[int index] { get; set; }
	}
}
