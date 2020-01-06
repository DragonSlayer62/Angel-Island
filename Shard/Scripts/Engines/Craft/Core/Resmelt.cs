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

/* Engines/Crafting/Core/Resmelt.cs
 * CHANGELOG:
 *	11/10/05, erlein
 *		Replaced SaveHue check with PlayerCrafted check.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Engines.Craft
{
	public class Resmelt
	{
		public Resmelt()
		{
		}

		public static void Do(Mobile from, CraftSystem craftSystem, BaseTool tool)
		{
			int num = craftSystem.CanCraft(from, tool, null);

			if (num > 0)
			{
				from.SendGump(new CraftGump(from, craftSystem, tool, num));
			}
			else
			{
				from.Target = new InternalTarget(craftSystem, tool);
				from.SendLocalizedMessage(1044273); // Target an item to recycle.
			}
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;

			public InternalTarget(CraftSystem craftSystem, BaseTool tool)
				: base(2, false, TargetFlags.None)
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			private bool Resmelt(Mobile from, Item item, CraftResource resource)
			{
				try
				{
					if (CraftResources.GetType(resource) != CraftResourceType.Metal)
						return false;

					CraftResourceInfo info = CraftResources.GetInfo(resource);

					if (info == null || info.ResourceTypes.Length == 0)
						return false;

					CraftItem craftItem = m_CraftSystem.CraftItems.SearchFor(item.GetType());

					if (craftItem == null || craftItem.Ressources.Count == 0)
						return false;

					CraftRes craftResource = craftItem.Ressources.GetAt(0);

					if (craftResource.Amount < 2)
						return false; // Not enough metal to resmelt

					Type resourceType = info.ResourceTypes[0];
					Item ingot = (Item)Activator.CreateInstance(resourceType);

					if (item is DragonBardingDeed || (item is BaseArmor && item.PlayerCrafted) || (item is BaseWeapon && item.PlayerCrafted) || (item is BaseClothing && item.PlayerCrafted))
						ingot.Amount = craftResource.Amount / 2;
					else
						ingot.Amount = 1;

					item.Delete();
					from.AddToBackpack(ingot);

					from.PlaySound(0x2A);
					from.PlaySound(0x240);
					return true;
				}
				catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

				return false;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				int num = m_CraftSystem.CanCraft(from, m_Tool, null);

				if (num > 0)
				{
					from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, num));
				}
				else
				{
					bool success = false;
					bool isStoreBought = false;

					if (targeted is BaseArmor)
					{
						success = Resmelt(from, (BaseArmor)targeted, ((BaseArmor)targeted).Resource);
						isStoreBought = !((Item)targeted).PlayerCrafted;
					}
					else if (targeted is BaseWeapon)
					{
						success = Resmelt(from, (BaseWeapon)targeted, ((BaseWeapon)targeted).Resource);
						isStoreBought = !((Item)targeted).PlayerCrafted;
					}
					else if (targeted is DragonBardingDeed)
					{
						success = Resmelt(from, (DragonBardingDeed)targeted, ((DragonBardingDeed)targeted).Resource);
						isStoreBought = false;
					}

					if (success)
						from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, isStoreBought ? 500418 : 1044270)); // You melt the item down into ingots.
					else
						from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, 1044272)); // You can't melt that down into ingots.
				}
			}
		}
	}
}