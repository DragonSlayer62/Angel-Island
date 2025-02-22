using System;
using Server;
using Server.Items;

namespace Server.Items
{
	// TODO: Commodity?
	public class DaemonBone : BaseReagent
	{
		[Constructable]
		public DaemonBone()
			: this(1)
		{
		}

		[Constructable]
		public DaemonBone(int amount)
			: base(0xF80, amount)
		{
			Weight = 1.0;
		}

		public DaemonBone(Serial serial)
			: base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new DaemonBone(amount), amount);
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
	}
}