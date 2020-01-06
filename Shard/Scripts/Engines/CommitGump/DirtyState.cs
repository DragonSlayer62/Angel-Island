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
 *			March 27, 2007
 */

/* Scripts/Engines/CommitGump/DirtyState.cs
 * CHANGELOG:
 *	06/07/09, plasma
 *		Added dirty reference
 *	01/26/09 - Plasma,
 *		Initial creation
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.CommitGump
{

	public class DirtyState
	{
		private sealed class DirtyValue<T> where T : struct
		{
			public T m_Value = default(T);
			private T m_OriginalValue = default(T);

			public bool IsDirty()
			{
				return !(m_Value.Equals(OriginalValue));
			}

			public DirtyValue(T initialValue)
			{
				m_OriginalValue = m_Value = initialValue;
			}

			public void SetValue(T newValue)
			{
				m_Value = newValue;
			}

			public T OriginalValue
			{
				get { return m_OriginalValue; }
			}
		}

		private sealed class DirtyReference<T> where T : class, IEquatable<T>, ICloneable
		{
			public T m_Value = default(T);
			private T m_OriginalValue = default(T);

			public bool IsDirty()
			{
				return !(m_Value.Equals(OriginalValue));
			}

			public DirtyReference(T initialValue)
			{
				m_Value = initialValue;
				//clone here so we aren't comparing whatever is required with ourselves :p
				m_OriginalValue = (T)m_Value.Clone();
			}

			public void SetValue(T newValue)
			{
				m_Value = newValue;
			}

			public T OriginalValue
			{
				get { return m_OriginalValue; }
			}
		}

		private CommitGumpBase.GumpSession m_DirtyFields = new CommitGumpBase.GumpSession();

		//value

		public void SetValue<T>(string key, T value) where T : struct
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyValue<T>(value);
			((DirtyValue<T>)m_DirtyFields[key]).SetValue(value);
		}

		public T GetValue<T>(string key) where T : struct
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyValue<T>(default(T));
			return ((DirtyValue<T>)m_DirtyFields[key]).m_Value;
		}

		public T GetOriginalValue<T>(string key) where T : struct
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyValue<T>(default(T));
			return ((DirtyValue<T>)m_DirtyFields[key]).OriginalValue;
		}

		public bool IsValueDirty<T>(string key) where T : struct
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyValue<T>(default(T));
			return ((DirtyValue<T>)m_DirtyFields[key]).IsDirty();
		}

		//reference

		public void SetReference<T>(string key, T value) where T : class, IEquatable<T>, ICloneable
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyReference<T>(value);
			((DirtyReference<T>)m_DirtyFields[key]).SetValue(value);
		}

		public T GetReference<T>(string key) where T : class, IEquatable<T>, ICloneable
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyReference<T>(default(T));
			return ((DirtyReference<T>)m_DirtyFields[key]).m_Value;
		}

		public T GetOriginalReference<T>(string key) where T : class, IEquatable<T>, ICloneable
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyReference<T>(default(T));
			return ((DirtyReference<T>)m_DirtyFields[key]).OriginalValue;
		}

		public bool IsReferenceDirty<T>(string key) where T : class, IEquatable<T>, ICloneable
		{
			if (m_DirtyFields[key] == null)
				m_DirtyFields[key] = new DirtyReference<T>(default(T));
			return ((DirtyReference<T>)m_DirtyFields[key]).IsDirty();
		}


	}

}
