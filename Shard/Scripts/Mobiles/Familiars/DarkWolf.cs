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

/* ./Scripts/Mobiles/Familiars/DarkWolf.cs
 *	ChangeLog :
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName("a dark wolf corpse")]
	public class DarkWolfFamiliar : BaseFamiliar
	{
		public DarkWolfFamiliar()
		{
			Name = "a dark wolf";
			Body = 99;
			Hue = 0x901;
			BaseSoundID = 0xE5;

			SetStr(100);
			SetDex(90);
			SetInt(90);

			SetHits(60);
			SetStam(90);
			SetMana(0);

			SetDamage(5, 10);

			SetSkill(SkillName.Wrestling, 85.1, 90.0);
			SetSkill(SkillName.Tactics, 50.0);

			ControlSlots = 1;
		}

		private DateTime m_NextRestore;

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now < m_NextRestore)
				return;

			m_NextRestore = DateTime.Now + TimeSpan.FromSeconds(2.0);

			Mobile caster = ControlMaster;

			if (caster == null)
				caster = SummonMaster;

			if (caster != null)
				++caster.Stam;
		}

		public DarkWolfFamiliar(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
