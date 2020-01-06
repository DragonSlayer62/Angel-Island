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
using Server.Items;

namespace Server.Spells
{
    public class Reagent
    {
        private static Type[] m_Types = new Type[13]
			{
				typeof( BlackPearl ),
				typeof( Bloodmoss ),
				typeof( Garlic ),
				typeof( Ginseng ),
				typeof( MandrakeRoot ),
				typeof( Nightshade ),
				typeof( SulfurousAsh ),
				typeof( SpidersSilk ),
				typeof( BatWing ),
				typeof( GraveDust ),
				typeof( DaemonBlood ),
				typeof( NoxCrystal ),
				typeof( PigIron )
			};

        public Type[] Types
        {
            get { return m_Types; }
        }

        public static Type BlackPearl
        {
            get { return m_Types[0]; }
            set { m_Types[0] = value; }
        }

        public static Type Bloodmoss
        {
            get { return m_Types[1]; }
            set { m_Types[1] = value; }
        }

        public static Type Garlic
        {
            get { return m_Types[2]; }
            set { m_Types[2] = value; }
        }

        public static Type Ginseng
        {
            get { return m_Types[3]; }
            set { m_Types[3] = value; }
        }

        public static Type MandrakeRoot
        {
            get { return m_Types[4]; }
            set { m_Types[4] = value; }
        }

        public static Type Nightshade
        {
            get { return m_Types[5]; }
            set { m_Types[5] = value; }
        }

        public static Type SulfurousAsh
        {
            get { return m_Types[6]; }
            set { m_Types[6] = value; }
        }

        public static Type SpidersSilk
        {
            get { return m_Types[7]; }
            set { m_Types[7] = value; }
        }

        public static Type BatWing
        {
            get { return m_Types[8]; }
            set { m_Types[8] = value; }
        }

        public static Type GraveDust
        {
            get { return m_Types[9]; }
            set { m_Types[9] = value; }
        }

        public static Type DaemonBlood
        {
            get { return m_Types[10]; }
            set { m_Types[10] = value; }
        }

        public static Type NoxCrystal
        {
            get { return m_Types[11]; }
            set { m_Types[11] = value; }
        }

        public static Type PigIron
        {
            get { return m_Types[12]; }
            set { m_Types[12] = value; }
        }
    }
}