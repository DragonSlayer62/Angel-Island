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

/* Items/Misc/FlipableAttribute.cs
 * ChangeLog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using System.Reflection;
using Server.Targeting;

namespace Server.Items
{
	public class FlipCommandHandlers
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Flip", AccessLevel.GameMaster, new CommandEventHandler(Flip_OnCommand));
		}

		[Usage("Flip")]
		[Description("Turns an item.")]
		public static void Flip_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new FlipTarget();
		}

		private class FlipTarget : Target
		{
			public FlipTarget()
				: base(-1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Item)
				{
					Item item = (Item)targeted;

					if (item.Movable == false && from.AccessLevel == AccessLevel.Player)
						return;

					Type type = targeted.GetType();

					FlipableAttribute[] AttributeArray = (FlipableAttribute[])type.GetCustomAttributes(typeof(FlipableAttribute), false);

					if (AttributeArray.Length == 0)
					{
						return;
					}

					FlipableAttribute fa = AttributeArray[0];

					fa.Flip((Item)targeted);
				}
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class DynamicFlipingAttribute : Attribute
	{
		public DynamicFlipingAttribute()
		{
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class FlipableAttribute : Attribute
	{
		private int[] m_ItemIDs;

		public int[] ItemIDs
		{
			get { return m_ItemIDs; }
		}

		public FlipableAttribute()
			: this(null)
		{
		}

		public FlipableAttribute(params int[] itemIDs)
		{
			m_ItemIDs = itemIDs;
		}

		public virtual void Flip(Item item)
		{
			if (m_ItemIDs == null)
			{
				try
				{
					MethodInfo flipMethod = item.GetType().GetMethod("Flip", Type.EmptyTypes);
					if (flipMethod != null)
						flipMethod.Invoke(item, new object[0]);
				}
				catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			}
			else
			{
				int index = 0;
				for (int i = 0; i < m_ItemIDs.Length; i++)
				{
					if (item.ItemID == m_ItemIDs[i])
					{
						index = i + 1;
						break;
					}
				}

				if (index > m_ItemIDs.Length - 1)
					index = 0;

				item.ItemID = m_ItemIDs[index];
			}
		}
	}
}
