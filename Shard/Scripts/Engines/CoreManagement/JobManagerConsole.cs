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

/* Scripts/Engines/CoreManagement/JobManagerConsole.cs
 * Changelog
 *	01/01/06 Taran Kain
 *		Initial version.
 */

using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
	public class JobManagerConsole : Item
	{
		[Constructable]
		public JobManagerConsole()
			: base(0x1F14)
		{
			Name = "JobManager Console";
			Weight = 1.0;
			Hue = 0x47E;
		}

		public JobManagerConsole(Serial s)
			: base(s)
		{
		}
#if JobManager
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int CurrentThreadCount
		{
			get
			{
				return JobManager.CurrentThreadCount;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int IdleThreadLifespan
		{
			get
			{
				return JobManager.IdleThreadLifespan;
			}
			set
			{
				JobManager.IdleThreadLifespan = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int MaxThreadCount
		{
			get
			{
				return JobManager.MaxThreadCount;
			}
			set
			{
				JobManager.MaxThreadCount = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int MinThreadCount
		{
			get
			{
				return JobManager.MinThreadCount;
			}
			set
			{
				JobManager.MinThreadCount = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int PriorityPromotionDelay
		{
			get
			{
				return JobManager.PriorityPromotionDelay;
			}
			set
			{
				JobManager.PriorityPromotionDelay = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int QueueLengthLimit
		{
			get
			{
				return JobManager.QueueLengthLimit;
			}
			set
			{
				JobManager.QueueLengthLimit = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int ReadyThreadCount
		{
			get
			{
				return JobManager.ReadyThreadCount;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int RunningThreadCount
		{
			get
			{
				return JobManager.RunningThreadCount;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int TotalEnqueuedJobs
		{
			get
			{
				return JobManager.TotalEnqueuedJobs;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int TotalJobsRun
		{
			get
			{
				return JobManager.TotalJobsRun;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public TimeSpan TotalJobTime
		{
			get
			{
				return JobManager.TotalJobTime;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize (writer);

			writer.Write(0); // version

			writer.Write(IdleThreadLifespan);
			writer.Write(MaxThreadCount);
			writer.Write(MinThreadCount);
			writer.Write(PriorityPromotionDelay);
			writer.Write(QueueLengthLimit);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize (reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					IdleThreadLifespan = reader.ReadInt();
					MaxThreadCount = reader.ReadInt();
					MinThreadCount = reader.ReadInt();
					PriorityPromotionDelay = reader.ReadInt();
					QueueLengthLimit = reader.ReadInt();
				
					break;
				}
				default:
				{
					throw new Exception("Invalid JobManagerConsole save version.");
				}
			}
		}
#endif

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						reader.ReadInt();
						reader.ReadInt();
						reader.ReadInt();
						reader.ReadInt();
						reader.ReadInt();
						break;
					}
			}
		}

	}
}