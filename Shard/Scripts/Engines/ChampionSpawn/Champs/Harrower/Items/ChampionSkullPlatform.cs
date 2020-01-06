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

/* Scripts\Engines\ChampionSpawn\Champs\Harrower\Items\ChampionSkullPlatform.cs
 * CHANGELOG
 *  03/09/07, plasma    
 *      Change to ChampionSapwn namespace (again) 
 *  01/05/07, plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 *	12/21/04, Pigpen
 *		Added flamestrike effect to skull platforms when the gate is opened and skulls are deleted.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.ChampionSpawn
{
	public class ChampionSkullPlatform : BaseAddon
	{
		private ChampionSkullBrazier m_Power, m_Enlightenment, m_Venom, m_Pain, m_Greed, m_Death;

		[Constructable]
		public ChampionSkullPlatform()
		{
			AddComponent(new AddonComponent(0x71A), -1, -1, -1);
			AddComponent(new AddonComponent(0x709), 0, -1, -1);
			AddComponent(new AddonComponent(0x709), 1, -1, -1);
			AddComponent(new AddonComponent(0x709), -1, 0, -1);
			AddComponent(new AddonComponent(0x709), 0, 0, -1);
			AddComponent(new AddonComponent(0x709), 1, 0, -1);
			AddComponent(new AddonComponent(0x709), -1, 1, -1);
			AddComponent(new AddonComponent(0x709), 0, 1, -1);
			AddComponent(new AddonComponent(0x71B), 1, 1, -1);

			AddComponent(new AddonComponent(0x50F), 0, -1, 4);
			AddComponent(m_Power = new ChampionSkullBrazier(this, ChampionSkullType.Power), 0, -1, 5);

			AddComponent(new AddonComponent(0x50F), 1, -1, 4);
			AddComponent(m_Enlightenment = new ChampionSkullBrazier(this, ChampionSkullType.Enlightenment), 1, -1, 5);

			AddComponent(new AddonComponent(0x50F), -1, 0, 4);
			AddComponent(m_Venom = new ChampionSkullBrazier(this, ChampionSkullType.Venom), -1, 0, 5);

			AddComponent(new AddonComponent(0x50F), 1, 0, 4);
			AddComponent(m_Pain = new ChampionSkullBrazier(this, ChampionSkullType.Pain), 1, 0, 5);

			AddComponent(new AddonComponent(0x50F), -1, 1, 4);
			AddComponent(m_Greed = new ChampionSkullBrazier(this, ChampionSkullType.Greed), -1, 1, 5);

			AddComponent(new AddonComponent(0x50F), 0, 1, 4);
			AddComponent(m_Death = new ChampionSkullBrazier(this, ChampionSkullType.Death), 0, 1, 5);

			AddonComponent comp = new LocalizedAddonComponent(0x20D2, 1049495);
			comp.Hue = 0x482;
			AddComponent(comp, 0, 0, 5);

			comp = new LocalizedAddonComponent(0x0BCF, 1049496);
			comp.Hue = 0x482;
			AddComponent(comp, 0, 2, -7);

			comp = new LocalizedAddonComponent(0x0BD0, 1049497);
			comp.Hue = 0x482;
			AddComponent(comp, 2, 0, -7);
		}

		public void Validate()
		{
			if (Validate(m_Power) && Validate(m_Enlightenment) && Validate(m_Venom) && Validate(m_Pain) && Validate(m_Greed) && Validate(m_Death))
			{
				Mobile harrower = Harrower.Spawn(new Point3D(X, Y, Z + 6), this.Map);

				if (harrower == null)
					return;

				Clear(m_Power);
				Clear(m_Enlightenment);
				Clear(m_Venom);
				Clear(m_Pain);
				Clear(m_Greed);
				Clear(m_Death);
			}
		}

		public void Clear(ChampionSkullBrazier brazier)
		{
			if (brazier != null)
			{
				//Effects.SendFlamestrikeEffect( brazier ); Pigpen, Old effects for platform. Wasnt working at all.
				Effects.SendLocationParticles(EffectItem.Create(brazier.Location, brazier.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

				if (brazier.Skull != null)
					brazier.Skull.Delete();
			}
		}

		public bool Validate(ChampionSkullBrazier brazier)
		{
			return (brazier != null && brazier.Skull != null && !brazier.Skull.Deleted);
		}

		public ChampionSkullPlatform(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Power);
			writer.Write(m_Enlightenment);
			writer.Write(m_Venom);
			writer.Write(m_Pain);
			writer.Write(m_Greed);
			writer.Write(m_Death);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Power = reader.ReadItem() as ChampionSkullBrazier;
						m_Enlightenment = reader.ReadItem() as ChampionSkullBrazier;
						m_Venom = reader.ReadItem() as ChampionSkullBrazier;
						m_Pain = reader.ReadItem() as ChampionSkullBrazier;
						m_Greed = reader.ReadItem() as ChampionSkullBrazier;
						m_Death = reader.ReadItem() as ChampionSkullBrazier;

						break;
					}
			}
		}
	}
}