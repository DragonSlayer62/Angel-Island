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

/* Engines/CoreManagement/CoreManagementConsole.cs
 * ChangeLog
 *	3/5/16, Adam
 *		Add a new FeatureBits.PlayerAccountWipe for wiping all player accounts as part of usual 
 *			account cleanup logic (it's fully logged.) 
 *		This *should* be what all that is required for a shard wipe...
 *		See implementation in CronTasks.cs
 *	8/20/11, Adam
 *		Add IPBinderEnabled
 *	9/7/10, Adam
 *		Add SeaGypsyUsageReport
 *	8/8/10, Adam
 *		Add: OldFlee switch
 *			BaseAI seems to have a bug where they clear FocusMob in OnActionChanged().Flee
 *	7/23/10, adam
 *		Add SpiritSpeakUsageReport
 *		used to turing on/off SpiritSpeak UsageReports
 *	6/12/10, adam
 *		Add a switch for controling the algo used for calculating armor absorb (ArmorAbsorbClassic)
 *	5/14/10, adam
 *		Update the mindblast delay as per Az recomendations 12/30 to 1.55
 *	5/12/10, adam
 *		Add CoreAI.SlayerWeaponDropRate
 *	4/10/10, adam
 *		Add speed management MCi to tune dragon speeds.
 *	4/9/10, adam
 *		Add a SmartSpellMCi for managing the 8th level default spell behavior of high-level magical mobs
 *	4/4/10, adam
 *		Oops, make the min/max values as per Az recomendations 12/30
 *	3/19/10, adam
 *		Add MindBlastMCi
 *		Change Poison Resistance from Spells.SpellCircle.Third to Spells.SpellCircle.Sixth
 *		This change is based on testing Akarius Alexios, and Answer
 *	3/18/10, adam
 *		Add new stun management console prototype
 *		StunPunchMCi
 *	11/25/08, Adam
 *		Make MaxAddresses a console value
 *			in IPLimiter; controls how many of the same IP address can be concurrently logged in
 *	2/18/04, Adam
 *		Make MaxAccountsPerIP a console value
 *  01/04/08, Pix
 *      Added GWRChangeDelayMinutes setting.
 *  12/9/07, Adam
 *      Added NewPlayerGuild feature bit.
 *	8/26/07 - Pix
 *		Added NewPlayerStartingArea feature bit.
 *	8/1/07, Pix
 *		Added RazorNegFeaturesEnabled and RazorNegWarnAndKick settings.
 *  4/3/07, Adam
 *      Add a BreedingEnabled bit
 *      Add a RTTNotifyEnabled bit
 *	1/08/07 Taran Kain
 *		Changed GSGG lookups to reflect new location in PlayerMobile
 *	1/02/07 Pixie
 *		Added RangedCorrosionModifier.
 *		Added RangedCorrosion featurebit.
 *	10/16/06, Adam
 *		Add flag to disable tower placement
 *	10/16/06, Adam
 *		Add global override for SecurePremises
 *			i.e., CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.SecurePremises)
 *	8/24/06, Adam
 *		Added TCAcctCleanupEnable to allow disabling of auto account cleanup
 *  8/19/06, Rhiannon
 *		Added accessors for CoreAI.PlayAccessLevel to control access to the Play command.
 *	8/5/06, weaver
 *		Added HelpStuckDisabled to disable help stuck.
 *	7/23/06, Pix
 *		Added GuildKinChangeDisabled
 *	6/2/06, Adam
 *		Give this console a special hue
 *  3/26/06, Pix
 *		Added IOBJoinEnabled;
 *	1/29/06, Adam
 *		TCAcctCleanupDays; trim all non-staff account and not logged in for N days - heartbeat
 *		Remove CommDeedBatch param
 *	12/28/05, Adam
 *		Add CommDeedBatch to set the number of containers processed per pass
 *  12/13/05 Taran Kain
 *		Added FreezeDryEnabled, removed FreezeDryLogEnabled
 *	12/13/05, Pix
 *		Added DebugDecay
 *	12/01/05 Pix
 *		Added WorldSaveFrequency
 *  10/10/05 Taran Kain
 *		Made ArmorDexLossStrFactor Admin-only access.
 *	10/02/05, erlein
 *		Added ConfusionBaseDelay to control tamed creature confusion.
 *	9/20/05, Adam
 *		a. Add setting for OpposePlayers. This flag modifies the bahavior of IsOpposition()
 *		in BaseCreature such that aligned PLAYERS appear as enimies to NPCs of a different alignment.
 *		b. Add a setting for OpposePlayersPets
 *	9/13/05, erlein
 *		Added MeleePoisonSkillFactor bool to control poison skill factoring in
 *		OnHit() equations of melee weapons.
 *	9/03/05, Adam
 *		Add Global FreezeHouseDecay function - World crisis mode :\
 *	9/02/05, erlein
 *		Added ReaggressIgnoreChance
 *  8/25/05, Taran Kain
 *		Added IDOCBroadcastChance
 *	7/13/05, erlein
 *		Added EScrollChance, EScrollSuccess.
 *	7/6/05, erlein
 *		Fixed Spirit Cohesion factors so accessed via CoreAI.
 *	6/13/05, erlein
 *		Added CohesionLowerDelay, CohesionBaseDelay and CohesionFactor
 *		to control new res delay.
 *	6/7/05, Adam
 *		Make TownCrierWordMinuteCost public via [CommandProperty,,]
 *	06/03/05 Taran Kain
 *		Added TownCrierWordMinuteCost
 *	6/3/05, Adam
 *		Add in ExplosionPotionThreshold to control the tossers
 *		health requirement
 *	4/30/05, Pix
 *		Removed ExplosionPotionAlchemyReduction.
 *	4/28/05, Adam
 *		add ServerWarMinutes to set the length of time for server wars
 *	4/26/05, Pix
 *		Made explode pots targetting method toggleable based on CoreAI/ConsoleManagement setting.
 *	4/23/05, Pix
 *		Added ExplosionPotionAlchemyReduction
 *	4/18/05, Adam
 *		Add TempDouble and TempInt for testing ingame settings
 *	04/18/05, Pix
 *		Added offline short term murder decay (only if it's turned on).
 *		Added potential exploding of carried explosion potions.
 *	4/15/05, Adam
 *		Add TreasureMapDrop as a global variable
 *	4/8/05, Adam
 *		Add handlers for the following CoreAI values
 *		SpiritDepotTRPots, SpiritFirstWaveVirtualArmor, SecondWaveVirtualArmor,
 *		SpiritThirdWaveVirtualArmor, SpiritBossVirtualArmor
 *	3/31/05, Adam
 *		Add the new global switch IOBShardWide to turn on/off IOB globalness
 *	3/7/05, Adam
 *		Add the new DynamicFeatures property so we can turn on/off shard wide features.
 *	2/4/05, Adam
 *		Created.
 *		Management console for the global values stored in Engines/AngelIsland/CoreAI.cs
 *		Manage Server.Misc.SkillCheck.GSGG
 *		Manage CoreAI.PowderOfTranslocationAvail
 */

using System;

using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server;
using Server.Spells;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
	public class CoreManagementConsole : Item
	{
		[Constructable]
		public CoreManagementConsole()
			: base(0x1F14)
		{
			Weight = 1.0;
			Hue = 0x534;
			Name = "Core Management Console";
		}

		public CoreManagementConsole(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.Administrator)]
		public CoreAI.WorldSize CurrentWorldSize
		{
			get
			{
				return CoreAI.CurrentWorldSize;
			}
			set
			{
				CoreAI.CurrentWorldSize = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool IPBinderEnabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.IPBinderEnabled);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.IPBinderEnabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.IPBinderEnabled);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool SeaGypsyUsageReport
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.SeaGypsyUsageReport);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.SeaGypsyUsageReport);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.SeaGypsyUsageReport);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool OldFlee
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.OldFlee);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.OldFlee);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.OldFlee);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool TreasureMapUsageReport
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.TreasureMapUsageReport);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.TreasureMapUsageReport);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.TreasureMapUsageReport);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool SpiritSpeakUsageReport
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.SpiritSpeakUsageReport);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.SpiritSpeakUsageReport);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.SpiritSpeakUsageReport);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool ArmorAbsorbClassic
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.ArmorAbsorbClassic);
			}
			set
			{
				if (value == true)
					// use the scaled armor AR to calculate how much damage is absorbed
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.ArmorAbsorbClassic);
				else
					// use full armor AR to calculate how much damage is absorbed
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.ArmorAbsorbClassic);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public VisibleDamageType VisibleDamageType
		{
			get
			{
				return Mobile.VisibleDamageType;
			}
			set
			{
				Mobile.VisibleDamageType = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double SlayerWeaponDropRate
		{
			get
			{
				return CoreAI.SlayerWeaponDropRate;
			}
			set
			{
				CoreAI.SlayerWeaponDropRate = value;
			}
		}

		public bool LogStableCharges
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.LogStableCharges);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.LogStableCharges);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.LogStableCharges);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool PetNeedsLOS
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.PetNeedsLOS);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.PetNeedsLOS);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.PetNeedsLOS);
			}
		}

		[CommandProperty(AccessLevel.Owner)]
		public int MaxAddresses
		{
			get { return Misc.IPLimiter.MaxAddresses; }
			set { Misc.IPLimiter.MaxAddresses = value; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		public static int MaxAccountsPerIP
		{
			get { return CoreAI.MaxAccountsPerIP; }
			set { CoreAI.MaxAccountsPerIP = value; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool NewPlayerStartingArea
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.NewPlayerStartingArea);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.NewPlayerStartingArea);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.NewPlayerStartingArea);
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool NewPlayerGuild
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.NewPlayerGuild);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.NewPlayerGuild);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.NewPlayerGuild);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool RazorNegFeaturesEnabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RazorNegotiateFeaturesEnabled);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.RazorNegotiateFeaturesEnabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.RazorNegotiateFeaturesEnabled);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool RazorNegWarnAndKick
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RazorNegotiateWarnAndKick);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.RazorNegotiateWarnAndKick);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.RazorNegotiateWarnAndKick);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool RTTNotifyEnabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RTTNotifyEnabled);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.RTTNotifyEnabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.RTTNotifyEnabled);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool BreedingEnabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.BreedingEnabled);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.BreedingEnabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.BreedingEnabled);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool RangedCorrosion
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RangedCorrosion);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.RangedCorrosion);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.RangedCorrosion);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int RangedCorrosionModifier
		{
			get
			{
				return CoreAI.RangedCorrosionModifier;
			}
			set
			{
				CoreAI.RangedCorrosionModifier = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int GWRChangeDelayMinutes
		{
			get
			{
				return CoreAI.GWRChangeDelayMinutes;
			}
			set
			{
				CoreAI.GWRChangeDelayMinutes = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool TowerAllowed
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.TowerAllowed);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.TowerAllowed);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.TowerAllowed);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool SecurePremises
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.SecurePremises);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.SecurePremises);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.SecurePremises);
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool TCAcctCleanupEnable
		{
			get
			{
				return CoreAI.TCAcctCleanupEnable;
			}
			set
			{
				CoreAI.TCAcctCleanupEnable = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public AccessLevel PlayAccessLevel
		{
			get
			{
				return CoreAI.PlayAccessLevel;
			}
			set
			{
				CoreAI.PlayAccessLevel = value;
			}
		}

		/// <summary>
		/// StandingDelay: denotes the minimum time (in seconds) an archer must stand still
		/// before being able to fire.
		/// </summary>
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double StandingDelay
		{
			get
			{
				return CoreAI.StandingDelay;
			}
			set
			{
				CoreAI.StandingDelay = value;
			}
		}

		// trim all non-staff account and not logged in for N days - heartbeat
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int TCAcctCleanupDays
		{
			get
			{
				return CoreAI.TCAcctCleanupDays;
			}
			set
			{
				CoreAI.TCAcctCleanupDays = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int WorldSaveFrequency
		{
			get
			{
				return CoreAI.WorldSaveFrequency;
			}
			set
			{
				if (value > 1)
				{
					CoreAI.WorldSaveFrequency = value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool GuildKinChangeDisabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.GuildKinChangeDisabled);
			}
			set
			{
				if (value)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.GuildKinChangeDisabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.GuildKinChangeDisabled);
			}
		}


		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool HelpStuckDisabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.HelpStuckDisabled);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.HelpStuckDisabled);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.HelpStuckDisabled);
			}
		}


		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool OpposePlayersPets
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.OpposePlayersPets);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.OpposePlayersPets);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.OpposePlayersPets);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool OpposePlayers
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.OpposePlayers);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.OpposePlayers);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.OpposePlayers);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double PowderOfTranslocation
		{
			get
			{
				return CoreAI.PowderOfTranslocationAvail;
			}
			set
			{
				CoreAI.PowderOfTranslocationAvail = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double GSGG
		{
			get
			{
				return Server.Mobiles.PlayerMobile.GSGG;
			}
			set
			{
				Server.Mobiles.PlayerMobile.GSGG = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public string DynamicFeatures
		{
			get
			{
				return "0x" + CoreAI.DynamicFeatures.ToString("X");
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool InmateEplt01
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.InmateRecallExploitCheck);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.InmateRecallExploitCheck);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.InmateRecallExploitCheck);
			}
		}

		[CommandProperty(AccessLevel.Owner, AccessLevel.Owner)]
		public bool PlayerAccountWipe
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.PlayerAccountWipe);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.PlayerAccountWipe);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.PlayerAccountWipe);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool IOBShardWide
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.IOBShardWide);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.IOBShardWide);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.IOBShardWide);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool IOBJoinEnabled
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.IOBJoinEnabled);
			}
			set
			{
				if (value == true)
				{
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.IOBJoinEnabled);
				}
				else
				{
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.IOBJoinEnabled);
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool FreezeHouseDecay
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.FreezeHouseDecay);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.FreezeHouseDecay);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.FreezeHouseDecay);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool MeleePoisonSkillFactor
		{
			get
			{
				return CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.MeleePoisonSkillFactor);
			}
			set
			{
				if (value == true)
					CoreAI.SetDynamicFeature(CoreAI.FeatureBits.MeleePoisonSkillFactor);
				else
					CoreAI.ClearDynamicFeature(CoreAI.FeatureBits.MeleePoisonSkillFactor);
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritDepotBandies
		{
			get
			{
				return CoreAI.SpiritDepotBandies;
			}
			set
			{
				CoreAI.SpiritDepotBandies = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritDepotGHPots
		{
			get
			{
				return CoreAI.SpiritDepotGHPots;
			}
			set
			{
				CoreAI.SpiritDepotGHPots = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritDepotReagents
		{
			get
			{
				return CoreAI.SpiritDepotReagents;
			}
			set
			{
				CoreAI.SpiritDepotReagents = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritDepotTRPots
		{
			get
			{
				return CoreAI.SpiritDepotTRPots;
			}
			set
			{
				CoreAI.SpiritDepotTRPots = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritFirstWaveVirtualArmor
		{
			get
			{
				return CoreAI.SpiritFirstWaveVirtualArmor;
			}
			set
			{
				CoreAI.SpiritFirstWaveVirtualArmor = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritSecondWaveVirtualArmor
		{
			get
			{
				return CoreAI.SpiritSecondWaveVirtualArmor;
			}
			set
			{
				CoreAI.SpiritSecondWaveVirtualArmor = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritThirdWaveVirtualArmor
		{
			get
			{
				return CoreAI.SpiritThirdWaveVirtualArmor;
			}
			set
			{
				CoreAI.SpiritThirdWaveVirtualArmor = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int SpiritBossVirtualArmor
		{
			get
			{
				return CoreAI.SpiritBossVirtualArmor;
			}
			set
			{
				CoreAI.SpiritBossVirtualArmor = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double TreasureMapDrop
		{
			get
			{
				return CoreAI.TreasureMapDrop;
			}
			set
			{
				CoreAI.TreasureMapDrop = value;
			}
		}

		// a temp value you can use for ingame tweakage, then replace with a
		//	constand once you know what you want to set it to
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double TempDouble
		{
			get
			{
				return CoreAI.TempDouble;
			}
			set
			{
				CoreAI.TempDouble = value;
			}
		}

		// a temp value you can use for ingame tweakage, then replace with a
		//	constand once you know what you want to set it to
		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int TempInt
		{
			get
			{
				return CoreAI.TempInt;
			}
			set
			{
				CoreAI.TempInt = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int ExplPotSensitivity
		{
			get
			{
				return CoreAI.ExplosionPotionSensitivityLevel;
			}
			set
			{
				if (value > 0) //ignore negative numbers, they don't make sense
				{
					CoreAI.ExplosionPotionSensitivityLevel = value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double ExplPotChance
		{
			get
			{
				return CoreAI.ExplosionPotionChance;
			}
			set
			{
				if (value >= 0.0 && value <= 1.0) //only valid values are 0.0-1.0
				{
					CoreAI.ExplosionPotionChance = value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public double ExplPotHealthThreshold
		{
			get
			{
				return CoreAI.ExplosionPotionThreshold;
			}
			set
			{
				if (value >= 0.0 && value <= 1.0) //only valid values are 0.0-1.0
				{
					CoreAI.ExplosionPotionThreshold = value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int OfflineShortsHours
		{
			get
			{
				return CoreAI.OfflineShortsDecayHours;
			}
			set
			{
				CoreAI.OfflineShortsDecayHours = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public bool OfflineShortsDecay
		{
			get
			{
				return (CoreAI.OfflineShortsDecay != 0);
			}
			set
			{
				if (value)
				{
					CoreAI.OfflineShortsDecay = 1;
				}
				else
				{
					CoreAI.OfflineShortsDecay = 0;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public CoreAI.EPTM ExplPotTargetMethod
		{
			get
			{
				return CoreAI.ExplosionPotionTargetMethod;
			}
			set
			{
				CoreAI.ExplosionPotionTargetMethod = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int ServerWarMinutes
		{
			get
			{
				return Server.Misc.AutoRestart.ServerWarsMinutes;
			}
			set
			{
				Server.Misc.AutoRestart.ServerWarsMinutes = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public int TownCrierWordMinuteCost
		{
			get { return CoreAI.TownCrierWordMinuteCost; }
			set { CoreAI.TownCrierWordMinuteCost = value; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int CohesionLowerDelay
		{
			get
			{
				return CoreAI.CohesionLowerDelay;
			}
			set
			{
				CoreAI.CohesionLowerDelay = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int CohesionFactor
		{
			get
			{
				return CoreAI.CohesionFactor;
			}
			set
			{
				CoreAI.CohesionFactor = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int CohesionBaseDelay
		{
			get
			{
				return CoreAI.CohesionBaseDelay;
			}
			set
			{
				CoreAI.CohesionBaseDelay = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double EScrollChance
		{
			get
			{
				return CoreAI.EScrollChance;
			}
			set
			{
				CoreAI.EScrollChance = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double EScrollSuccess
		{
			get
			{
				return CoreAI.EScrollSuccess;
			}
			set
			{
				CoreAI.EScrollSuccess = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double IDOCBroadcastChance
		{
			get
			{
				return CoreAI.IDOCBroadcastChance;
			}
			set
			{
				CoreAI.IDOCBroadcastChance = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public double ReaggressIgnoreChance
		{
			get
			{
				return CoreAI.ReaggressIgnoreChance;
			}
			set
			{
				CoreAI.ReaggressIgnoreChance = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int ConfusionBaseDelay
		{
			get
			{
				return CoreAI.ConfusionBaseDelay;
			}
			set
			{
				CoreAI.ConfusionBaseDelay = value;
			}
		}

		[CommandProperty(AccessLevel.Administrator)]
		public int ArmorDexLossStrFactor
		{
			get { return BaseArmor.StrFactor; }
			set { BaseArmor.StrFactor = value; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool DebugDecay
		{
			get { return CoreAI.DebugItemDecayOutput; }
			set { CoreAI.DebugItemDecayOutput = value; }
		}

		[CommandProperty(AccessLevel.Administrator)]
		public bool FreezeDryEnabled
		{
			get
			{
				return World.FreezeDryEnabled;
			}
			set
			{
				World.FreezeDryEnabled = value;
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.AccessLevel > AccessLevel.Administrator)
			{
				from.SendGump(new Server.Gumps.PropertiesGump(from, this));
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

}

namespace Server.Items.Consoles
{
	public class DragonSpeedMCi : TuningConsole
	{
		// regular dragon
		private const double c_DragonActiveSpeed = 0.25;		// AI 1.0 default: 0.25
		private const double c_DragonPassiveSpeed = 0.5;		// AI 1.0 default: 0.5
		private static double m_DragonActiveSpeed = c_DragonActiveSpeed;
		private static double m_DragonPassiveSpeed = c_DragonPassiveSpeed;
		[CommandProperty(AccessLevel.Player)]
		public static double DragonActiveSpeed { get { return m_DragonActiveSpeed; } set { m_DragonActiveSpeed = value; } }
		[CommandProperty(AccessLevel.Player)]
		public static double DragonPassiveSpeed { get { return m_DragonPassiveSpeed; } set { m_DragonPassiveSpeed = value; } }

		// regular white wyrm
		private const double c_WyrmActiveSpeed = 0.2;		// AI 1.0 default: 0.2
		private const double c_WyrmPassiveSpeed = 0.4;		// AI 1.0 default: 0.4
		private static double m_WyrmActiveSpeed = c_WyrmActiveSpeed;
		private static double m_WyrmPassiveSpeed = c_WyrmPassiveSpeed;
		[CommandProperty(AccessLevel.Player)]
		public static double WyrmActiveSpeed { get { return m_WyrmActiveSpeed; } set { m_WyrmActiveSpeed = value; } }
		[CommandProperty(AccessLevel.Player)]
		public static double WyrmPassiveSpeed { get { return m_WyrmPassiveSpeed; } set { m_WyrmPassiveSpeed = value; } }

		// regular Mare
		private const double c_MareActiveSpeed = 0.2;		// AI 1.0 default: 0.2
		private const double c_MarePassiveSpeed = 0.4;		// AI 1.0 default: 0.4
		private static double m_MareActiveSpeed = c_MareActiveSpeed;
		private static double m_MarePassiveSpeed = c_MarePassiveSpeed;
		[CommandProperty(AccessLevel.Player)]
		public static double MareActiveSpeed { get { return m_MareActiveSpeed; } set { m_MareActiveSpeed = value; } }
		[CommandProperty(AccessLevel.Player)]
		public static double MarePassiveSpeed { get { return m_MarePassiveSpeed; } set { m_MarePassiveSpeed = value; } }

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech(SpeechEventArgs e)
		{
			if (this.RootParent != e.Mobile)
				return;

			string text = e.Speech.ToLower().Trim();
			if (text == ".displayspeed")
			{
				e.Mobile.SendMessage("Please target the creature.");
				e.Mobile.Target = new SpeedDisplayTarget(this, 0);
			}
		}

		[Constructable]
		public DragonSpeedMCi()
		{
			Hue = 1533;
			Name = "Dragon Speed Management Console";
		}

		public DragonSpeedMCi(Serial serial)
			: base(serial)
		{
		}

		private class SpeedDisplayTarget : Target
		{
			public SpeedDisplayTarget(object ctfc, int SessionId)
				: base(12, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobiles.BaseCreature == false)
				{
					from.SendMessage("You may only target creatures with this command");
				}
				else
				{
					(targeted as Mobiles.BaseCreature).DebugSpeed = !(targeted as Mobiles.BaseCreature).DebugSpeed;
					from.SendMessage(string.Format("The settings is now {0}", (targeted as Mobiles.BaseCreature).DebugSpeed));
				}
			}
		}

		public override void RestoreDefaults()
		{
			m_DragonActiveSpeed = c_DragonActiveSpeed;
			m_DragonPassiveSpeed = c_DragonPassiveSpeed;
			m_WyrmActiveSpeed = c_WyrmActiveSpeed;
			m_WyrmPassiveSpeed = c_WyrmPassiveSpeed;
			m_MareActiveSpeed = c_MareActiveSpeed;
			m_MarePassiveSpeed = c_MarePassiveSpeed;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class SmartSpellMCi : TuningConsole
	{
		public enum SmartSpellLogic { Classic, Debuff, FSInhibitor }
		private const SmartSpellLogic c_UseLogic = SmartSpellLogic.Classic;		// AI 1.0 default: Classic
		private static SmartSpellLogic m_UseLogic = c_UseLogic;
		[CommandProperty(AccessLevel.Player)]
		public static SmartSpellLogic UseLogic { get { return m_UseLogic; } set { m_UseLogic = value; } }

		[Constructable]
		public SmartSpellMCi()
		{
			Hue = 1533;
			Name = "SmartSpell Management Console";
		}

		public SmartSpellMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_UseLogic = c_UseLogic;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class HarmMCi : TuningConsole
	{
		private const bool c_UseNewScale = true;		// AI 1.0 default: false
		private static bool m_UseNewScale = c_UseNewScale;
		[CommandProperty(AccessLevel.Player)]
		public static bool UseNewScale { get { return m_UseNewScale; } set { m_UseNewScale = value; } }

		// adam: new default value: XX min damage.
		private const double c_DamageRangeLow = 1;						// AI 1.0 default was 1
		private static double m_DamageRangeLow = c_DamageRangeLow;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageRangeLow { get { return m_DamageRangeLow; } set { m_DamageRangeLow = value; } }

		// adam: new default value: XX max damage
		private const double c_DamageRangeHigh = 15;					// AI 1.0 default was 15
		private static double m_DamageRangeHigh = c_DamageRangeHigh;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageRangeHigh { get { return m_DamageRangeHigh; } set { m_DamageRangeHigh = value; } }

		[Constructable]
		public HarmMCi()
		{
			Hue = 1533;
			Name = "Harm Management Console";
		}

		public HarmMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_UseNewScale = c_UseNewScale;
			m_DamageRangeLow = c_DamageRangeLow;
			m_DamageRangeHigh = c_DamageRangeHigh;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MagicArrowMCi : TuningConsole
	{
		// adam: new default XX
		private const double c_DamageDelay = 1.2;		// Ai 1.0 was 0.65
		private static double m_DamageDelay = c_DamageDelay;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageDelay { get { return m_DamageDelay; } set { m_DamageDelay = value; } }

		[Constructable]
		public MagicArrowMCi()
		{
			Hue = 1533;
			Name = "Magic Arrow Management Console";
		}

		public MagicArrowMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_DamageDelay = c_DamageDelay;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MindBlastMCi : TuningConsole
	{
		// adam: new default value: true
		private const bool c_UseNewScale = true;		// AI 1.0 default: false
		private static bool m_UseNewScale = c_UseNewScale;
		[CommandProperty(AccessLevel.Player)]
		public static bool UseNewScale { get { return m_UseNewScale; } set { m_UseNewScale = value; } }

		// adam: new default value: 12 min damage.
		private const double c_DamageRangeLow = 12;
		//_LightningMin + (_LightningMax - _LightningMin) / 2;		// 4th circle lightning, median;
		private static double m_DamageRangeLow = c_DamageRangeLow;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageRangeLow { get { return m_DamageRangeLow; } set { m_DamageRangeLow = value; } }

		// adam: new default value: 30 max damage
		private const double c_DamageRangeHigh = 30;
		//_eBoltMin + (_eBoltMax - _eBoltMin) / 2;					// 6th circle ebolt, median;
		private static double m_DamageRangeHigh = c_DamageRangeHigh;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageRangeHigh { get { return m_DamageRangeHigh; } set { m_DamageRangeHigh = value; } }

		// adam: new default 1.55
		private const double c_DamageDelay = 1.55;		// Ai 1.0 was 0.65
		private static double m_DamageDelay = c_DamageDelay;
		[CommandProperty(AccessLevel.Player)]
		public static double DamageDelay { get { return m_DamageDelay; } set { m_DamageDelay = value; } }

		// the next 4 values are only for reference
		public const int _LightningMin = 9;
		[CommandProperty(AccessLevel.Player)]
		public static int LightningMin { get { return _LightningMin; } }

		public const int _LightningMax = 12;
		[CommandProperty(AccessLevel.Player)]
		public static int LightningMax { get { return _LightningMax; } }

		public const int _eBoltMin = 17;
		[CommandProperty(AccessLevel.Player)]
		public static int eBoltMin { get { return _eBoltMin; } }

		public const int _eBoltMax = 21;
		[CommandProperty(AccessLevel.Player)]
		public static int eBoltMax { get { return _eBoltMax; } }


		[Constructable]
		public MindBlastMCi()
		{
			Hue = 1533;
			Name = "Mind Blast Management Console";
		}

		public MindBlastMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_UseNewScale = c_UseNewScale;
			m_DamageRangeLow = c_DamageRangeLow;
			m_DamageRangeHigh = c_DamageRangeHigh;
			m_DamageDelay = c_DamageDelay;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class PoisonStickMCi : TuningConsole
	{
		private const SpellCircle c_SpellCircle = Spells.SpellCircle.Sixth; // Ai 1.0 was Spells.SpellCircle.Third
		private static SpellCircle m_SpellCircle = c_SpellCircle;
		[CommandProperty(AccessLevel.Player)]
		public static SpellCircle SpellCircle { get { return m_SpellCircle; } set { m_SpellCircle = value; } }

		[Constructable]
		public PoisonStickMCi()
		{
			Hue = 1533;
			Name = "Poison Stick Management Console";
		}

		public PoisonStickMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_SpellCircle = c_SpellCircle;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class StunPunchMCi : TuningConsole
	{
		private const int c_StamCost = 15;
		private static int m_StamCost = c_StamCost;
		[CommandProperty(AccessLevel.Player)]
		public static int StamCost { get { return m_StamCost; } set { m_StamCost = value; } }

		private const bool c_StamCostAlways = true;
		private static bool m_StamCostAlways = c_StamCostAlways;
		[CommandProperty(AccessLevel.Player)]
		public static bool StamCostAlways { get { return m_StamCostAlways; } set { m_StamCostAlways = value; } }

		private const double c_FreezeTime = 3.0;	// AI 1.0 default: 4.0
		private static double m_FreezeTime = c_FreezeTime;
		[CommandProperty(AccessLevel.Player)]
		public static double FreezeTime { get { return m_FreezeTime; } set { m_FreezeTime = value; } }

		[Constructable]
		public StunPunchMCi()
		{
			Hue = 1533;
			Name = "Stun Punch Management Console";
		}

		public StunPunchMCi(Serial serial)
			: base(serial)
		{
		}

		public override void RestoreDefaults()
		{
			m_StamCost = c_StamCost;
			m_StamCostAlways = c_StamCostAlways;
			m_FreezeTime = c_FreezeTime;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}


	[FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
	public abstract class TuningConsole : Item
	{
		private Mobile m_owner = null;												// cannot give to another player
		private DateTime m_expiration = DateTime.Now + TimeSpan.FromHours(4.0);		// self destruct in 4 hours

		public TuningConsole()
			: base(0x1F14)
		{
			Weight = 1.0;
			Hue = 1533;
			Name = "Tuning Console";
		}

		public TuningConsole(Serial serial)
			: base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			// any access level as long as it's TC
			if (Misc.TestCenter.Enabled || from.AccessLevel > AccessLevel.Player)
			{
				if (m_owner == null)
					m_owner = from;

				if (m_owner != from)
				{
					from.SendMessage("You are not authorized to use this console.");
					from.SendMessage("Deleting...");
					this.Delete();
					return;
				}

				if (DateTime.Now > m_expiration)
				{
					from.SendMessage("This console has expired.");
					from.SendMessage("Deleting...");
					this.Delete();
					return;
				}

				from.SendGump(new Server.Gumps.PropertiesGump(from, this));
			}
			else
			{
				from.SendMessage("You cannot use this here.");
				from.SendMessage("Deleting...");
				this.Delete();
				return;
			}
		}

		public abstract void RestoreDefaults();

		public override void OnDelete()
		{
			RestoreDefaults();
			base.OnDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
