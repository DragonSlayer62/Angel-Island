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
using System.Runtime.InteropServices;

namespace Server
{
	public enum ZLibError : int
	{
		Z_OK = 0,
		Z_STREAM_END = 1,
		Z_NEED_DICT = 2,
		Z_ERRNO = (-1),
		Z_STREAM_ERROR = (-2),
		Z_DATA_ERROR = (-3),
		Z_MEM_ERROR = (-4),
		Z_BUF_ERROR = (-5),
		Z_VERSION_ERROR = (-6),
	}

	public enum ZLibCompressionLevel : int
	{
		Z_NO_COMPRESSION = 0,
		Z_BEST_SPEED = 1,
		Z_BEST_COMPRESSION = 9,
		Z_DEFAULT_COMPRESSION = (-1)
	}

	public class ZLib
	{
		[DllImport("./server/bin/zlib.dll")]
		public static extern string zlibVersion();
		[DllImport("./server/bin/zlib.dll")]
		public static extern ZLibError compress(byte[] dest, ref int destLength, byte[] source, int sourceLength);
		[DllImport("./server/bin/zlib.dll")]
		public static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, ZLibCompressionLevel level);
		[DllImport("./server/bin/zlib.dll")]
		public static extern ZLibError uncompress(byte[] dest, ref int destLen, byte[] source, int sourceLen);
	}
}
