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

/* Scripts/Commands/RemapCommDeeds.cs
 * Changelog
 *	12/30/05, weaver
 *		Added check to see if a found commodity deed already has a quantity
 *		of something attached to it and logging of these instances.
 *	12/30/05, weaver
 *		Added additional search against serial number for any that are not
 *		inside containers.
 *	12/29/05, weaver
 *		Added logging of any data from the RCDMapData file not matched
 *		by the remapping process.		
 *	12/28/05, Adam
 *		Add messages when a run is complete, and when totally done
 *		Also add the number of deeds per run to the Command Console.
 *	12/25/05, weaver
 *		Initial creation.
 */

using System;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Scripts.Commands;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace Server.Scripts.Commands
{

	public class RemapCommDeeds
	{

		public static bool RCDActivated;				// active flag
		
		public static int RCDProcessed = 0;				// number of containers processed so far

		public static ArrayList RCDData;				// list of data which gives us info we need to remap
		public static ArrayList RCDConlist;				// list of containers we're working through
		
		public static IEnumerator RCDEnum;				// enumerator that allows us to traverse the containers
	
		public static Mobile RCDCaller;					// initiating mobile

		public static LogHelper RCDLogger;				// logfile
		public static Remap_Timer RCDTimer;

		public static int RCDProcRate					// number per 10 seconds
		{
			get
			{
				return CoreAI.CommDeedBatch;
			}
			set
			{
				CoreAI.CommDeedBatch = value;
			}
		}
		
		public static void Initialize()
		{
			Server.Commands.Register( "RemapCommDeeds", AccessLevel.Administrator, new CommandEventHandler( RemapCommDeeds_OnCommand ) );
			Server.Commands.Register( "RemapCommDeedsCancel", AccessLevel.Administrator, new CommandEventHandler( RemapCommDeedsCancel_OnCommand ) );
		}

		[Usage( "RemapCommDeeds" )]
		[Description( "Remaps commodity deeds according to backed up data." )]
		public static void RemapCommDeeds_OnCommand( CommandEventArgs e )
		{
			// Is this currently underway? If so, say and do nothing further... otherwise init

			if( RCDActivated )
			{
				e.Mobile.SendMessage( "Commodity deeds are already being remapped... use [remapcommdeedscancel to cancel the process." );
				return;
			}
			else
				Remap_Init( e.Mobile );
		}

		public static void Remap_Init( Mobile from )
		{
			RCDCaller = from;
			
			// Load the remap data into memory
			string sRemapFile =  "Data/RCDMapData.txt";
			bool bLoadRes = LoadRemapData( sRemapFile );

            	
			if( !bLoadRes || RCDData.Count == 0 )
			{
				RCDCaller.SendMessage( string.Format("Failed to load remap data from {0}!", sRemapFile) );
				return;
			}
            
			// First off, get a list of all the containers we're going
			// to work through and record the serial numbers

			RCDConlist = new ArrayList();
            
			foreach( Item item in World.Items.Values )
			{
				if( item is Container )
					RCDConlist.Add( item.Serial );
			}

			// Retrieve enumerator for this collection
			RCDEnum = RCDConlist.GetEnumerator();

			// Make sure we have something on the enumerator
			if( RCDEnum == null )
			{
				RCDCaller.SendMessage("Null enumerator detected! Cannot proceed!");
				return;
			}

			// Open up the file so we don't have to perform extra
			// I/O throughout the search

			RCDLogger = new LogHelper("remapcommdeeds.log", RCDCaller, true);
            			
			// Kick off everything

			RCDProcessed = 0;
			RCDActivated = true;
			RCDTimer = new Remap_Timer();
			RCDTimer.Start();

			RCDCaller.SendMessage( "Remapping {0} commodity deeds...", RCDConlist.Count );
			
		}

		public static void Remap_Process()
		{	
			RCDCaller.SendMessage( string.Format("Processing next {0} containers...", RCDProcRate) );

			// Loop through the next load of serial numbers, unfreezing each container in
			// turn and matching it against out stored list

			int idone  = 0;
			int itotal = RCDConlist.Count;
			int endpos = RCDProcessed + RCDProcRate;
			            
			while( RCDProcessed < endpos && RCDProcessed < itotal)
			{
				RCDProcessed++;
				idone++;

				RCDEnum.MoveNext();

				// Use the serial number to find the item in the world
				Item item = World.FindItem((Serial)RCDEnum.Current);

				// Make sure it still exists
				if( item == null )
					continue;
				                                                                
				if( item is Container )
				{
					// It's still there!
					Container cont = (Container) item;
					
					// Rehydrate it if necessary
					if( cont.CanFreezeDry )
						cont.Rehydrate();

					ArrayList ContQueue = new ArrayList();
                    					
					foreach( Item content in cont.Items )
						ContQueue.Add( content );
					
					while( ContQueue.Count > 0 )
					{
						Item content;

						// Make sure the object in the queue is still an item

						if( ContQueue[0] is Item )
						{
							content = (Item) ContQueue[0];
							ContQueue.RemoveAt(0);
						}
						else
						{
							ContQueue.RemoveAt(0);
							continue;
						}

						if( content is CommodityDeed )
						{
							// Check it against the ones we loaded into memory on init

							bool match = false;
							int ipos;

							for( ipos=0; ipos < RCDData.Count; ipos++ )
							{
								if( RCDData[ipos].ToString().IndexOf(content.Serial.ToString()) > -1 )
								{
									// We have a match, so break the loop
							
									match = true;
									break;       
								}
							}
							if( match )
							{
								if( RCDEncode( ((CommodityDeed) content), RCDData[ipos].ToString()) )
								{
									// We were successfully able to encode the deed
									RCDData.Remove( RCDData[ipos] );
								}
								else
								{
									// We failed to encode the deed
									continue;
								}
							}
						}
						else if( content is Container )
						{
							foreach( Item ci in ((Container) content).Items )
							{
								// Queue it up!
								ContQueue.Add( ci );
							}
						}
					}
				}
			}

			// Re-process any left in the RCDData list to ensure that they
			// cannot be located outside of rehydrated containers

			for( int ipos=0; ipos < RCDData.Count; ipos++ )
			{
				string sbase = RCDData[ipos].ToString().Substring(2,8);
				int iserial;

				// Try and convert serial string into a value
				
				try
				{
					iserial = Int32.Parse(sbase, System.Globalization.NumberStyles.HexNumber);
				}
				catch
				{
					Console.WriteLine("Failed to convert serial into a value!");
					
					RCDLogger.Log(LogType.Text,  
						string.Format("INVALID SERIAL DETECTED - :{0}:", sbase ));								

					RCDCaller.SendMessage( "Warning! Invalid serial detected... see logfile!" );

					// Loop to next data entry
					continue;
				}
						
				Item item = World.FindItem( iserial );

				// Make sure it still exists
				if( item == null )
					continue;

				// If it's a commodity deed, try and encode
				if( item is CommodityDeed )
					if( RCDEncode( ((CommodityDeed) item), RCDData[ipos].ToString() ))
						RCDData.Remove( RCDData[ipos] );

			}

			// Adam: tell the caller this run has completed
			RCDCaller.SendMessage( string.Format("Finished processing {0} containers.", RCDProcRate) );

			if( RCDProcessed == itotal )
			{
				Remap_Finish();

				// Adam: tell the caller we are done.
				RCDCaller.SendMessage( "Commodity deed processing complete." );

			}
			else
			{
				// Set up another timer to re-call another process run
				RCDTimer = new Remap_Timer();
				RCDTimer.Start();
			}

		}
		
		// Attempt to encode a commodity deed with new data

		public static bool RCDEncode( CommodityDeed cd, string sData )
		{
			ClassNameTranslator cnt = new ClassNameTranslator();

			// Make sure there isn't already a resource attached to the deed			

			if( cd.CommodityAmount > 0 )
			{
				RCDLogger.Log(LogType.Text,  
					string.Format("{0}:{1}:{2}:{3}:{4}", 
					cd.Serial.ToString(),
					cnt.TranslateClass(cd.Type),
					cd.CommodityAmount.ToString(),
					cd.Location,
					"ALREADY FILLED") );	

				RCDCaller.SendMessage( "Warning! Filled deed detected... see logfile!" );

				return false;			
			}

			// Perform the replacement
           		
			int iStartIDX = sData.IndexOf("\"");
			int iEndIDX = sData.IndexOf("\"", iStartIDX + 1);
								                                                                
			string sType = sData.Substring((iStartIDX + 1), (iEndIDX - iStartIDX - 1));

			Type tType = ScriptCompiler.FindTypeByName( sType );

			if ( tType == null )
			{
				// Invalid

				RCDLogger.Log(LogType.Text,  
					string.Format("{0}:{1}:{2}:{3}:{4}", 
					cd.Serial.ToString(),
					sType,
					"",
					cd.Location,
					"INVALID") );								
				
				RCDCaller.SendMessage( "Warning! Invalid type detected... see logfile!" );
				return false;
			}

			// Next work out the quantity of the data we're going to map to the deed
								
			iStartIDX = sData.IndexOf(",", iEndIDX) + 1;
			iEndIDX = sData.Length;
                                
			string sQuantity = sData.Substring(iStartIDX , (iEndIDX - iStartIDX));
			int iQuantity;

			try
			{
				iQuantity =  Convert.ToInt32(sQuantity);
			}
			catch (Exception e)
			{
				Console.WriteLine( "RemapCommDeeds: Exception - (trying to convert {0} to an integer)", sQuantity);

				RCDLogger.Log(LogType.Text,  
					string.Format("{0}:{1}:{2}:{3}:{4}", 
					cd.Serial.ToString(),
					cd.Type.ToString(),
					sQuantity,
					cd.Location,
					"INVALID") );								

				RCDCaller.SendMessage( "Warning! Invalid quantity detected (non numeric)... see logfile!" );
				return false; 
			}

			// All good, encode it...

			cd.Type = tType;
			cd.CommodityAmount = iQuantity;
			cd.Description = string.Format("{0} {1}", iQuantity, cnt.TranslateClass( tType ));
                                																								                               								
			RCDLogger.Log(LogType.Text,  
				string.Format("{0}:{1}:{2}:{3}:{4}", 
				cd.Serial.ToString(),
				sType,
				iQuantity,
				cd.Location,
				"PATCHED") );								

			return true;
		}


		public static void Remap_Finish()
		{
			// Note that we're done
			RCDActivated = false;

			// Work out and log any we've missed
			for( int ipos=0; ipos < RCDData.Count; ipos++ )
				RCDLogger.Log( LogType.Text, string.Format("{0}::::{1}", RCDData[ipos].ToString().Substring(0, 10), "UNFOUND") );								
			
			// Finish the logging session
			RCDLogger.Finish();
		}

		public class Remap_Timer : Timer
		{
			public Remap_Timer() : base( TimeSpan.FromSeconds( 10.0 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				Stop();
				Remap_Process();
			}
		}

		public static bool LoadRemapData( string sFilename )
		{
			StreamReader sr;

			// Init the RCD data array
			RCDData = new ArrayList();

			// Open the file for sequential access

			try 
			{
				sr = new StreamReader( sFilename );
			}
			catch (Exception e) 
			{
				// Failed to load data file
				Console.WriteLine("Failed to open commodity remap data file '{0}' for writing : {1}", sFilename, e);
				return false;
			}

            string line;
            
			// Load contents into memory
            while ((line = sr.ReadLine()) != null) 
            {
				RCDData.Add(line);
			}

			sr.Close();	
            
			return true;
		}

		[Usage( "RemapCommDeedsCancel" )]
		[Description( "Cancels RemapCommDeeds command." )]
		public static void RemapCommDeedsCancel_OnCommand( CommandEventArgs e )
		{
			if( RCDActivated )
			{
				// Cancel it!
				RCDTimer.Stop();
				RCDLogger.Finish();
				RCDActivated = false;
				RCDCaller.SendMessage("Commodity deed remap cancelled... results so far written to logfile.");
			}
			else
				e.Mobile.SendMessage("Commodity deeds are not currently being remapped! There is nothing to cancel.");
		}
	}
}


