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
using Server;

namespace Server.Items
{
	public class Torso : Item, ICarvable
	{
		private IOBAlignment m_IOBAlignment = IOBAlignment.None;
		public IOBAlignment IOBAlignment { get { return m_IOBAlignment; } }

		public override TimeSpan DecayTime
		{
			get
			{
				return TimeSpan.FromMinutes(15.0);
			}
		}

		[Constructable]
		public Torso()
			: base(0x1D9F)
		{
			Weight = 2.0;
		}

		public Torso(IOBAlignment iob)
			: this()
		{
			m_IOBAlignment = iob;
		}

		public Torso(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		void ICarvable.Carve(Mobile from, Item item)
		{
			Point3D loc = this.Location;
			if (this.ParentContainer != null)
			{
				if (this.ParentMobile != null)
				{
					if (this.ParentMobile != from)
					{
						from.SendMessage("You can't carve that there");
						return;
					}

					loc = this.ParentMobile.Location;
				}
				else
				{
					loc = this.ParentContainer.Location;
					if (!from.InRange(loc, 1))
					{
						from.SendMessage("That is too far away.");
						return;
					}
				}
			}


			//add blood
			Blood blood = new Blood(Utility.Random(0x122A, 5), Utility.Random(15 * 60, 5 * 60));
			blood.MoveToWorld(loc, Map);
			//add meat
			Jerky jerky = new Jerky(m_IOBAlignment);
			Jerky jerky2 = new Jerky(m_IOBAlignment);
			Jerky jerky3 = new Jerky(m_IOBAlignment);

			BodyPart heart = new BodyPart(BodyPart.Part.HEART);
			BodyPart liver = new BodyPart(BodyPart.Part.LIVER);
			BodyPart ent = new BodyPart(BodyPart.Part.ENTRAILS);

			if (this.ParentContainer == null)
			{
				jerky.MoveToWorld(loc, Map);
				jerky2.MoveToWorld(loc, Map);
				jerky3.MoveToWorld(loc, Map);
				heart.MoveToWorld(loc, Map);
				liver.MoveToWorld(loc, Map);
				ent.MoveToWorld(loc, Map);
			}
			else
			{
				this.ParentContainer.DropItem(jerky);
				this.ParentContainer.DropItem(jerky2);
				this.ParentContainer.DropItem(jerky3);
				this.ParentContainer.DropItem(heart);
				this.ParentContainer.DropItem(liver);
				this.ParentContainer.DropItem(ent);
			}

			this.Delete();
		}
	}
}