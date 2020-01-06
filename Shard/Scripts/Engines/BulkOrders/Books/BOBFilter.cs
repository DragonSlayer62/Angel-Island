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

namespace Server.Engines.BulkOrders
{
	public class BOBFilter
	{
		private int m_Type;
		private int m_Quality;
		private int m_Material;
		private int m_Quantity;

		public bool IsDefault
		{
			get { return (m_Type == 0 && m_Quality == 0 && m_Material == 0 && m_Quantity == 0); }
		}

		public void Clear()
		{
			m_Type = 0;
			m_Quality = 0;
			m_Material = 0;
			m_Quantity = 0;
		}

		public int Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public int Quality
		{
			get { return m_Quality; }
			set { m_Quality = value; }
		}

		public int Material
		{
			get { return m_Material; }
			set { m_Material = value; }
		}

		public int Quantity
		{
			get { return m_Quantity; }
			set { m_Quantity = value; }
		}

		public BOBFilter()
		{
		}

		public BOBFilter(GenericReader reader)
		{
			int version = reader.ReadEncodedInt();

			switch (version)
			{
				case 1:
					{
						m_Type = reader.ReadEncodedInt();
						m_Quality = reader.ReadEncodedInt();
						m_Material = reader.ReadEncodedInt();
						m_Quantity = reader.ReadEncodedInt();

						break;
					}
			}
		}

		public void Serialize(GenericWriter writer)
		{
			if (IsDefault)
			{
				writer.WriteEncodedInt(0); // version
			}
			else
			{
				writer.WriteEncodedInt(1); // version

				writer.WriteEncodedInt(m_Type);
				writer.WriteEncodedInt(m_Quality);
				writer.WriteEncodedInt(m_Material);
				writer.WriteEncodedInt(m_Quantity);
			}
		}
	}
}