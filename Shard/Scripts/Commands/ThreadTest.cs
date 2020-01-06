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

/* Scripts\Commands\ThreadTest.cs
 *  CHANGELOG:
 *  5/6/08, Adam
 *		Turned off taran's thread manager as we're not using it and it makes profiling a bit tougher
 */

using System;
using System.Threading;

#if JobManager
namespace Server
{
	/// <summary>
	/// Summary description for ThreadTest.
	/// </summary>
	public class  ThreadTest
	{
		public static void Initialize()
		{
			Server.Commands.Register("StartJM", AccessLevel.Administrator, new CommandEventHandler(StartJM_Cmd));
			Server.Commands.Register("JMStats", AccessLevel.Administrator, new CommandEventHandler(JMStats_Cmd));
		}

		public static void StartJM_Cmd(CommandEventArgs e)
		{
			Timer.DelayCall(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), 100, new TimerCallback(StressTester));
		}

		public static void StressTester()
		{	
			ThreadJob job = new ThreadJob(new JobWorker(StressWorker), Utility.RandomMinMax(50, 5000), new JobCompletedCallback(StressCallback));
			job.Start((JobPriority)Utility.RandomMinMax(0, 4));
		}

		public static object StressWorker(object parms)
		{
			int delay = (int)parms;

			Console.WriteLine("Beginning sleep for {0}ms.", delay);
			Thread.Sleep(delay);
			Console.WriteLine("Finished sleeping for {0}ms.", delay);

			return delay;
		}

		public static void StressCallback(ThreadJob job)
		{
			Console.WriteLine("Callback with results for job {0}", job.Results);
		}

		public static void JMStats_Cmd(CommandEventArgs e)
		{
			Console.WriteLine("JobManager stats:");
			Console.WriteLine("MinThreadCount:				{0}", JobManager.MinThreadCount);
			Console.WriteLine("MaxThreadCount:				{0}", JobManager.MaxThreadCount);
			Console.WriteLine("CurrentThreadCount:			{0}", JobManager.CurrentThreadCount);
			Console.WriteLine("ReadyThreadCount:			{0}", JobManager.ReadyThreadCount);
			Console.WriteLine("RunningThreadCount:			{0}", JobManager.RunningThreadCount);
			Console.WriteLine("TotalEnqueuedJobs:			{0}", JobManager.TotalEnqueuedJobs);
			Console.WriteLine("IdleThreadLifespan:			{0}ms", JobManager.IdleThreadLifespan);
			Console.WriteLine("PriorityPromotionDelay:			{0}ms", JobManager.PriorityPromotionDelay);
			Console.WriteLine("LastError: {0}", JobManager.LastError);
		}
	}
}
#endif
