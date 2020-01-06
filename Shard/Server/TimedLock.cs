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

/* Server/TimedLock.cs
 * Changelog
 *  01/18/06 Taran Kain
 *		Changed default timeout from 10 sec to 1.0 sec.
 *  01/18/06 Taran Kain
 *		Initial version. Thanks go to Ian Griffth (www.interact-sw.co.uk/iangblog/) for this code.
 */

using System;
using System.Threading;

// Thanks to Eric Gunnerson for recommending this be a struct rather
// than a class - avoids a heap allocation.
// Thanks to Change Gillespie and Jocelyn Coulmance for pointing out
// the bugs that then crept in when I changed it to use struct...
// Thanks to John Sands for providing the necessary incentive to make
// me invent a way of using a struct in both release and debug builds
// without losing the debug leak tracking.

namespace Server
{
	public struct TimedLock : IDisposable
	{
		public static TimedLock Lock(object o)
		{
			return Lock(o, TimeSpan.FromMilliseconds(1000));
		}

		public static TimedLock Lock(object o, TimeSpan timeout)
		{
			TimedLock tl = new TimedLock(o);
			if (!Monitor.TryEnter(o, timeout))
			{
#if DEBUG
				System.GC.SuppressFinalize(tl.leakDetector);
#endif
				throw new LockTimeoutException();
			}

			return tl;
		}

		private TimedLock(object o)
		{
			target = o;
#if DEBUG
			leakDetector = new Sentinel();
#endif
		}
		private object target;

		public void Dispose()
		{
			Monitor.Exit(target);

			// It's a bad error if someone forgets to call Dispose,
			// so in Debug builds, we put a finalizer in to detect
			// the error. If Dispose is called, we suppress the
			// finalizer.
#if DEBUG
			GC.SuppressFinalize(leakDetector);
#endif
		}

#if DEBUG
		// (In Debug mode, we make it a class so that we can add a finalizer
		// in order to detect when the object is not freed.)
		private class Sentinel
		{
			~Sentinel()
			{
				// If this finalizer runs, someone somewhere failed to
				// call Dispose, which means we've failed to leave
				// a monitor!
				System.Diagnostics.Debug.Fail("Undisposed lock");
			}
		}
		private Sentinel leakDetector;
#endif

	}

	public class LockTimeoutException : ApplicationException
	{
		public LockTimeoutException()
			: base("Timeout waiting for lock")
		{
		}
	}
}