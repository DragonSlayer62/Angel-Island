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

/* Engines/CoreManagement/SecurityManagementConsole.cs
 * CHANGELOG
 * 
 *  1/18/07 Taran Kain
 *      Initial version.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
	[FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
	class SecurityManagementConsole : Item
	{
		[CommandProperty(AccessLevel.Administrator)]
		public static TimeSpan MovementPacketThrottleThreshold
		{
			get
			{
				return PlayerMobile.FastwalkThreshold;
			}
			set
			{
				if (value >= TimeSpan.Zero)
					PlayerMobile.FastwalkThreshold = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static bool MovementPacketThrottlingEnabled
		{
			get
			{
				return PlayerMobile.FastwalkPrevention;
			}
			set
			{
				PlayerMobile.FastwalkPrevention = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static int PacketThrottleCountWarningThreshold
		{
			get
			{
				return PlayerMobile.ThrottleRunWarningThreshold;
			}
			set
			{
				PlayerMobile.ThrottleRunWarningThreshold = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static AccessLevel FastwalkAccessOverride
		{
			get
			{
				return PlayerMobile.FastWalkAccessOverride;
			}
			set
			{
				PlayerMobile.FastWalkAccessOverride = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static int FwdMaxSteps
		{
			get
			{
				return Mobile.FwdMaxSteps;
			}
			set
			{
				if (value >= 0)
					Mobile.FwdMaxSteps = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static bool FastWalkDetectionEnabled
		{
			get
			{
				return Mobile.FwdEnabled;
			}
			set
			{
				Mobile.FwdEnabled = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static bool FastWalkProtectionEnabled
		{
			get
			{
				return Fastwalk.ProtectionEnabled;
			}
			set
			{
				Fastwalk.ProtectionEnabled = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static bool FwdUOTDOverride
		{
			get
			{
				return Mobile.FwdUOTDOverride;
			}
			set
			{
				Mobile.FwdUOTDOverride = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static AccessLevel FwdAccessOverride
		{
			get
			{
				return Mobile.FwdAccessOverride;
			}
			set
			{
				Mobile.FwdAccessOverride = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static int FwdWarningThreshold
		{
			get
			{
				return Fastwalk.WarningThreshold;
			}
			set
			{
				Fastwalk.WarningThreshold = value;
			}
		}

		public static void Configure()
		{
			SetDefaults();
		}

		public static void SetDefaults()
		{
			FastWalkDetectionEnabled = true;
			FastWalkProtectionEnabled = false;
			FwdAccessOverride = AccessLevel.GameMaster;
			FwdMaxSteps = 2;
			FwdUOTDOverride = false;
			FwdWarningThreshold = 5;

			MovementPacketThrottlingEnabled = true;
			MovementPacketThrottleThreshold = TimeSpan.FromSeconds(0.1);
			PacketThrottleCountWarningThreshold = 5;
			FastwalkAccessOverride = AccessLevel.GameMaster;
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static string ResetDefaults
		{
			get
			{
				return "Enter \"yes\" to reset defaults.";
			}
			set
			{
				if (Insensitive.Equals(value, "yes"))
					SetDefaults();
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(FastWalkDetectionEnabled);
			writer.Write(FastWalkProtectionEnabled);
			writer.Write((int)FwdAccessOverride);
			writer.Write(FwdMaxSteps);
			writer.Write(FwdUOTDOverride);
			writer.Write(FwdWarningThreshold);

			writer.Write(MovementPacketThrottleThreshold);
			writer.Write(MovementPacketThrottlingEnabled);
			writer.Write(PacketThrottleCountWarningThreshold);
			writer.Write((int)FastwalkAccessOverride);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						FastWalkDetectionEnabled = reader.ReadBool();
						FastWalkProtectionEnabled = reader.ReadBool();
						FwdAccessOverride = (AccessLevel)reader.ReadInt();
						FwdMaxSteps = reader.ReadInt();
						FwdUOTDOverride = reader.ReadBool();
						FwdWarningThreshold = reader.ReadInt();

						MovementPacketThrottleThreshold = reader.ReadTimeSpan();
						MovementPacketThrottlingEnabled = reader.ReadBool();
						PacketThrottleCountWarningThreshold = reader.ReadInt();
						FastwalkAccessOverride = (AccessLevel)reader.ReadInt();

						break;
					}
				default:
					{
						throw new Exception("Invalid Security Management Console save version.");
					}
			}
		}

		[Constructable]
		public SecurityManagementConsole()
			: base(0x1F14)
		{
			Weight = 1.0;
			Hue = 0x533;
			Name = "Security Management Console";
		}

		public SecurityManagementConsole(Serial s)
			: base(s)
		{
		}
	}
}
