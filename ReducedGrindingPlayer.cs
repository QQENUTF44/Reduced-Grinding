using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria;

/*To debug, use:
ErrorLogger.Log(<string>);

To turn into a string use:
Value.ToString()

To show text in chat use:
Main.NewText(string);
or
Main.NewText(string, red, green, blue);

Chatting a value:
Main.NewText(Value.ToString(), 255, 255, 255);
*/

namespace ReducedGrinding
{
	
	class ReducedGrindingPlayer : ModPlayer
    {

		public override void PreUpdate()
		{
			/*if (Main.time % 180 == 0) //Debug
			{
				ReducedGrindingPlayer mPlayer = Main.LocalPlayer.GetModPlayer<ReducedGrindingPlayer>(mod);
				Main.NewText("Debug Client:");
				Main.NewText("mplayer.clientConf.CrateJungleFeralClawsIncrease: "+mPlayer.clientConf.AllEnemiesLootBiomeMatchingFoundOnlyChestDrop.ToString());
				Main.NewText("");
				
				Console.WriteLine("Debug Server:");
				Console.WriteLine("mplayer.clientConf.CrateJungleFeralClawsIncrease: "+mPlayer.clientConf.AllEnemiesLootBiomeMatchingFoundOnlyChestDrop.ToString());
				Console.WriteLine("");
			}*/
			
			Player player = Main.player[Main.myPlayer];
			bool biomeChestMined = false;
			if (player.HasItem(1528)) //Jungle Chest
			{
				biomeChestMined = ReducedGrindingWorld.jungleChestMined;
				ReducedGrindingWorld.jungleChestMined = true;
			}
			if (player.HasItem(1529)) //Corruption Chest
			{
				biomeChestMined = ReducedGrindingWorld.infectionChestMined;
				ReducedGrindingWorld.infectionChestMined = true;
			}
			if (player.HasItem(1530)) //Crimson Chest
			{
				biomeChestMined = ReducedGrindingWorld.infectionChestMined;
				ReducedGrindingWorld.infectionChestMined = true;
			}
			if (player.HasItem(1531)) //Hallowed Chest
			{
				biomeChestMined = ReducedGrindingWorld.hallowedChestMined;
				ReducedGrindingWorld.hallowedChestMined = true;
			}
			if (player.HasItem(1532)) //Frozen Chest
			{
				biomeChestMined = ReducedGrindingWorld.frozenChestMined;
				ReducedGrindingWorld.frozenChestMined = true;
			}
			if (biomeChestMined == false)
			{
				if (Main.netMode > 0)
				{
					var netMessage = mod.GetPacket();
					netMessage.Write((byte)ReducedGrindingMessageType.biomeChestMined);
					netMessage.Write(ReducedGrindingWorld.jungleChestMined);
					netMessage.Write(ReducedGrindingWorld.infectionChestMined);
					netMessage.Write(ReducedGrindingWorld.hallowedChestMined);
					netMessage.Write(ReducedGrindingWorld.frozenChestMined);
					netMessage.Send();
				}
			}
			
		}
		
		public bool currentlyActive = false;

        public ClientConf clientConf = new ClientConf(
			1, //DropTriesForAllEnemyDroppedLoot
			0.5f, //NormalModeLootMultiplierForLootWithSeperateDifficultyRates
			
			0.2f, //CrateDungeonBoneWelder
			0.45f, //CrateEnchantedSundialGoldenIncrease
			0.2333f, //CrateEnchantedSundialIronIncrease
			0.12f, //CrateEnchantedSundialWoodenIncrease
			0f, //CrateJungleAnkeltOfTheWindIncrease
			0f, //CrateJungleFeralClawsIncrease
			0.25f, //CrateJungleFlowerBoots
			0f, //CrateJungleLeafWand
			0f, //CrateJungleLivingLoom
			0f, //CrateJungleLivingMahoganyWand
			0f, //CrateJungleLivingWoodWand
			0f, //CrateJungleRichMahoganyLeafWand
			0.25f, //CrateJungleSeaweed
			0f, //CrateJungleStaffOfRegrowth
			0.3333f, //CrateSkySkyMill
			0f, //CrateFlippersGolden
			0f, //CrateFlippersIron
			0f, //CrateFlippersWooden
			0f, //CrateWaterWalkingBootsGolden
			0f, //CrateWaterWalkingBootsIron
			0f, //CrateWaterWalkingBootsWooden
			0f, //CrateWoodenAgletIncrease
			0f, //CrateWoodenClimbingClawsIncrease
			0f, //CrateWoodenRadarIncrease
			0f, //PresentCandyCaneBlock
			0f, //PresentCandyCaneHook
			0f, //PresentCandyCanePickaxe
			0f, //PresentCandyCaneSword
			0f, //PresentChristmasPudding
			0f, //PresentCoal
			0f, //PresentDogWhistle
			0f, //PresentEggnog
			0f, //PresentFruitcakeChakram
			0f, //PresentGingerbreadCookie
			0f, //PresentGreenCandyCaneBlock
			0f, //PresentHandWarmer
			0f, //PresentHardmodeSnowGlobe
			0f, //PresentHolly
			0f, //PresentMrsClausCostume
			0f, //PresentParkaOutfit
			0f, //PresentPineTreeBlock
			0f, //PresentRedRyderPlusMusketBall
			0f, //PresentReindeerAntlers
			0f, //PresentSnowHat
			0f, //PresentStarAnise
			0f, //PresentSugarCookie
			0f, //PresentToolbox
			0f, //PresentTreeCostume
			0f, //PresentUglySweater
			
			0f, //LootBookofSkullsIncrease
			0.375f, //LootPicksawIncrease
			0.15f, //LootSeedlingIncrease
			0f, //LootSkeletronBoneKey
			0.467f, //LootBinocularsIncrease
			0.2f, //LootBoneRattleIncrease
			0.1071f, //LootBossMaskIncrease
			0f, //LootBossTrophyIncrease
			0.2f, //LootEatersBoneIncrease
			0.5f, //LootFishronTruffleworm
			0.15f, //LootFishronWingsIncrease
			0.14f, //LootHoneyedGogglesIncrease
			0.14f, //LootNectarIncrease
			0.20f, //LootTheAxeIncrease
			
			0.0004f, //BiomeKeyIncreaseForOneMechBossDown
			0.0012f, //BiomeKeyIncreaseForTwoMechBossDown
			0.0028f, //BiomeKeyIncreaseForThreeMechBossDown
			
			0.01f, //AllEnemiesLootBiomeMatchingFoundOnlyChestDrop
			0f, //HellBatLootMagmaStoneIncrease
			0f, //LavaBatLootMagmaStoneIncrease
			0f, //LootAdhesiveBandageIncrease
			0.833f, //LootAleTosser
			0.0067f, //LootAmarokIncrease
			0f, //LootAncientClothIncrease
			0f, //LootAncientCobaltBreastplateIncrease
			0f, //LootAncientCobaltHelmetIncrease
			0f, //LootAncientCobaltLeggingsIncrease
			0.015f, //LootAncientGoldHelmetIncrease
			0.03f, //LootAncientHornIncrease
			0f, //LootAncientIronHelmetIncrease
			0.0028f, //LootAncientNecroHelmetIncrease
			0f, //LootAncientShadowGreavesIncrease
			0f, //LootAncientShadowHelmetIncrease
			0f, //LootAncientShadowScalemailIncrease
			0.0024f, //LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory
			0f, //LootArmorPolishIncrease
			0.05f, //LootBabyGrinchsMischiefWhistleIncrease
			0.3f, //LootBananarangIncrease
			0f, //LootBeamSwordIncrease
			0f, //LootBezoarIncrease
			0f, //LootBlackBeltIncrease
			0f, //LootBlackLensIncrease
			0.0066f, //LootBlessedAppleIncrease
			0f, //LootBlindfoldIncrease
			0.0015f, //LootBloodyMacheteAndBladedGlovesIncrease
			0.0078f, //LootBoneFeatherIncrease
			0.0867f, //LootBonePickaxeIncrease
			0.0051f, //LootBoneSwordIncrease
			0.006f, //LootBoneWandIncrease
			0.01f, //LootBrainScramblerIncrease
			0.075f, //LootBrokenBatWingIncrease
			0f, //LootBunnyHoodIncrease
			0.0025f, //LootCascadeIncrease
			0f, //LootChainGuillotinesIncrease
			0.0027f, //LootChainKnifeIncrease
			0.875f, //LootClassyCane
			0f, //LootClingerStaffIncrease
			0.0467f, //LootClothierVoodooDollIncrease
			0f, //LootCompassIncrease
			0f, //LootCrossNecklaceIncrease
			0f, //LootCrystalVileShardIncrease
			0f, //LootDaedalusStormbowIncrease
			0f, //LootDarkShardIncrease
			0f, //LootDartPistolIncrease
			0f, //LootDartRifleIncrease
			0.025f, //LootDeathSickleIncrease
			0.0214f, //LootDemonScytheIncrease
			0f, //LootDepthMeterIncrease
			0f, //LootDesertSpiritLampIncrease
			0.03f, //LootDivingHelmetIncrease
			0f, //LootDualHookIncrease
			0.00833f, //LootElfHatIncrease
			0.00833f, //LootElfPantsIncrease
			0.00833f, //LootElfShirtIncrease
			0f, //LootEskimoCoatIncrease
			0f, //LootEskimoHoodIncrease
			0f, //LootEskimoPantsIncrease
			0.875f, //LootExoticScimitar
			0f, //LootEyePatchIncrease
			0f, //LootEyeSpringIncrease
			0f, //LootFastClockBaseIncrease
			0.05f, //LootFestiveWingsIncrease
			0f, //LootFetidBaghnakhsIncrease
			0.0367f, //LootFireFeatherIncrease
			0f, //LootFleshKnucklesIncrease
			0f, //LootFlyingKnifeIncrease
			0f, //LootFrostStaffIncrease
			0.19f, //LootFrozenTurtleShellIncrease
			0f, //LootGiantBowIncrease
			0.005f, //LootGiantHarpyFeatherIncrease
			0f, //LootGoldenFurnitureIncrease
			0f, //LootGoldenKeyIncrease
			0f, //LootGoodieBagIncrease
			1f, //LootGreenCap
			0.75f, //LootHappyGrenade
			0.0075f, //LootHarpoonIncrease
			0.0025f, //LootHelFireIncrease
			0f, //LootHookIncrease
			0.0011f, //LootIceSickleIncrease
			0f, //LootIlluminantHookIncrease
			0.04f, //LootJellyfishNecklaceIncrease
			0.001f, //LootKOCannonIncrease
			0f, //LootKeybrandIncrease
			0.0075f, //LootKrakenIncrease
			0.01f, //LootLamiaClothesIncrease
			0f, //LootLifeDrainIncrease
			0f, //LootLightShardIncrease
			0f, //LootLihzahrdPowerCellIncrease
			0f, //LootLivingFireBlockIncrease
			0.009f, //LootLizardEggIncrease
			0f, //LootMagicDaggerIncrease
			0.0375f, //LootMagicQuiverIncrease
			0.0017f, //LootMagnetSphereIncrease
			0f, //LootMandibleBladeIncrease
			0.045f, //LootMarrowIncrease
			0f, //LootMeatGrinderIncrease
			0f, //LootMegaphoneBaseIncrease
			0f, //LootMeteoriteIncrease
			0.2833f, //LootMiningHelmetIncrease
			0.3093f, //LootMiningPantsIncrease
			0.3093f, //LootMiningShirtIncrease
			0.04f, //LootMoneyTroughIncrease
			0f, //LootMoonCharmIncrease
			0f, //LootMoonMaskIncrease
			0f, //LootMoonStoneIncrease
			0.1381f, //LootMothronWingsIncrease
			0f, //LootMummyCostumeIncrease
			0f, //LootNazarIncrease
			0f, //LootNimbusRodIncrease
			0.03f, //LootObsidianRoseIncrease
			0.9f, //LootPaintballGun
			0.35f, //LootPaladinsShieldIncrease
			0f, //LootPedguinssuitIncrease
			0f, //LootPhilosophersStoneIncrease
			0.015f, //LootPirateMapIncrease
			0.048f, //LootPlumbersHatIncrease
			0.105f, //LootPocketMirrorIncrease
			0f, //LootPresentIncrease
			0.1125f, //LootPsychoKnifeIncrease
			0f, //LootPutridScentIncrease
			0f, //LootRainArmorIncrease
			0, //LootRainbowBlockDropMaxIncrease
			0, //LootRainbowBlockDropMinIncrease
			0.061f, //LootRallyIncrease
			0.0917f, //LootReindeerBellsIncrease
			0f, //LootRifleScopeIncrease
			0f, //LootRobotHatIncrease
			0f, //LootRocketLauncherIncrease
			0.1291f, //LootRodofDiscordIncrease
			0f, //LootSWATHelmetIncrease
			0f, //LootSailorHatIncrease
			0f, //LootSailorPantsIncrease
			0f, //LootSailorShirtIncrease
			0f, //LootShackleIncrease
			0.048f, //LootSkullIncrease
			0.075f, //LootSlimeStaffIncrease
			0f, //LootSniperRifleIncrease
			0.0133f, //LootSnowballLauncherIncrease
			0f, //LootSoulofLightIncrease
			0f, //LootSoulofNightIncrease
			0f, //LootStarCloakIncrease
			0.875f, //LootStylishScissors
			0f, //LootSunMaskIncrease
			0f, //LootTabiIncrease
			0f, //LootTacticalShotgunIncrease
			0f, //LootTallyCounterIncrease
			0f, //LootTatteredBeeWingIncrease
			0f, //LootTendonHookIncrease
			0f, //LootTitanGloveIncrease
			0.15f, //LootToySledIncrease
			0f, //LootTrifoldMapIncrease
			0.1412f, //LootTurtleShellIncrease
			0f, //LootUmbrellaHatIncrease
			0.04f, //LootUnicornonaStickIncrease
			0f, //LootUziIncrease
			0f, //LootVikingHelmetIncrease
			0f, //LootVitaminsIncrease
			0f, //LootWhoopieCushionIncrease
			0.0063f, //LootWispinaBottleIncrease
			0f, //LootWormHookIncrease
			0f, //LootYeletsIncrease
			0.016f, //LootZombieArmIncrease
			0.0027f, //PirateLootCoinGunBaseIncrease
			0.0117f, //PirateLootCutlassBaseIncrease
			0.0045f, //PirateLootDiscountCardBaseIncrease
			0.0043f, //PirateLootGoldRingBaseIncrease
			0.0039f, //PirateLootLuckyCoinBaseIncrease
			0.0045f, //PirateLootPirateStaffBaseIncrease
			true, //LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense
			false, //SlimeStaffIncreaseToSurfaceSlimes
			false, //SlimeStaffIncreaseToUndergroundSlimes
			false, //SlimeStaffIncreaseToCavernSlimess
			true, //SlimeStaffIncreaseToIceSpikedSlimes
			true, //SlimeStaffIncreaseToSpikedJungleSlimes

			0.01f, //CavernModdedCavernLockBoxLoot
			0.01f, //DungeonModdedBiomeLockBoxLoot
			0.01f, //DungeonFurnitureLockBoxLoot
			2.0f, //HardmodeModdedLockBoxDropRateModifier
			0.01f, //HellBiomeModdedShadowLockBoxLoot
			0.01f, //JungleTempleLihzahrd_Lock_Box
			1f, //NormalmodeModdedLockBoxDropRateModifier
			0.01f, //SandstormAndUndergroundDesertPyramidLockBoxLoot
			0.01f, //SkyModdedSkywareLockBoxLoot
			0.01f, //SpiderNestWebCoveredLockBoxLoot
			0.01f, //SurfaceModdedLivingWoodLockBoxLoot
			0.01f, //UndergroundJungleBiomeModdedIvyLockBoxLoot
			0.01f, //UndergroundSnowBiomeModdedIceLockBoxLoot
			0.01f, //WaterEnemyModdedWaterLockBoxLoot
			
			0f, //TravelingMerchantAcornsIncrease
			0f, //TravelingMerchantAmmoBoxIncrease
			0f, //TravelingMerchantAngelHaloIncrease
			0f, //TravelingMerchantArcaneRuneWallIncrease
			0f, //TravelingMerchantBlackCounterweightIncrease
			0f, //TravelingMerchantBlueDynastyShinglesIncrease
			0f, //TravelingMerchantBlueTeamBlockIncrease
			0f, //TravelingMerchantBlueTeamPlatformIncrease
			0f, //TravelingMerchantBrickLayerIncrease
			0f, //TravelingMerchantCastleMarsbergIncrease
			0f, //TravelingMerchantCelestialMagnetIncrease
			0f, //TravelingMerchantChaliceIncrease
			0f, //TravelingMerchantCode1Increase
			0f, //TravelingMerchantCode2Increase
			0f, //TravelingMerchantColdSnapIncrease
			0f, //TravelingMerchantCompanionCubeIncrease
			0f, //TravelingMerchantCrimsonCapeIncrease
			0f, //TravelingMerchantCursedSaintIncrease
			0f, //TravelingMerchantDPSMeterIncrease
			0f, //TravelingMerchantDiamondRingIncrease
			0f, //TravelingMerchantDynastyWoodIncrease
			0f, //TravelingMerchantExtendoGripIncrease
			0f, //TravelingMerchantFancyDishesIncrease
			0f, //TravelingMerchantFezIncrease
			0f, //TravelingMerchantGatligatorIncrease
			0f, //TravelingMerchantGiIncrease
			0f, //TravelingMerchantGreenTeamBlockIncrease
			0f, //TravelingMerchantGreenTeamPlatformIncrease
			0f, //TravelingMerchantGypsyRobeIncrease
			0f, //TravelingMerchantKatanaIncrease
			0f, //TravelingMerchantKimonoIncrease
			0f, //TravelingMerchantLeopardSkinIncrease
			0f, //TravelingMerchantLifeformAnalyzerIncrease
			0f, //TravelingMerchantMagicHatIncrease
			0f, //TravelingMerchantMartiaLisaIncrease
			0f, //TravelingMerchantMetalDetector
			0f, //TravelingMerchantMysteriousCapeIncrease
			0f, //TravelingMerchantNotAKidNorASquidIncrease
			0f, //TravelingMerchantPadThaiIncrease
			0f, //TravelingMerchantPaintSprayerIncrease
			0f, //TravelingMerchantPhoIncrease
			0f, //TravelingMerchantPinkTeamBlockIncrease
			0f, //TravelingMerchantPinkTeamPlatformIncrease
			0f, //TravelingMerchantPortableCementMixerIncrease
			0f, //TravelingMerchantPresseratorIncrease
			0f, //TravelingMerchantPulseBowIncrease
			0f, //TravelingMerchantRedCapeIncrease
			0f, //TravelingMerchantRedDynastyShinglesIncrease
			0f, //TravelingMerchantRedTeamBlockIncrease
			0f, //TravelingMerchantRedTeamPlatformIncrease
			0f, //TravelingMerchantRevolverIncrease
			0f, //TravelingMerchantSakeIncrease
			0f, //TravelingMerchantSittingDucksFishingPoleIncrease
			0f, //TravelingMerchantSnowfellasIncrease
			0f, //TravelingMerchantStopwatchIncrease
			0f, //TravelingMerchantTheSeasonIncrease
			0f, //TravelingMerchantTheTruthIsUpThereIncrease
			0f, //TravelingMerchantTigerSkinIncrease
			0f, //TravelingMerchantUltraBrightTorchIncrease
			0f, //TravelingMerchantWaterGunIncrease
			0f, //TravelingMerchantWhiteTeamBlockIncrease
			0f, //TravelingMerchantWhiteTeamPlatformIncrease
			0f, //TravelingMerchantWinterCapeIncrease
			0f, //TravelingMerchantYellowCounterweightIncrease
			0f, //TravelingMerchantYellowTeamBlockIncrease
			0f, //TravelingMerchantYellowTeamPlatformIncrease
			0f, //TravelingMerchantZebraSkinIncrease
			false, //TravelingMerchantAlwaysXMasForConfigurations
			0.02f, //ChanceThatEnemyKillWillResetTravelingMerchant
			true, //StationaryMerchant
			0.166f, //StationaryMerchantStockingChance
			0.1668f, //S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate
			
			0.025f, //QuestAnglerEarringIncrease
			0.05f, //QuestAnglerHatIncrease
			0.05f, //QuestAnglerPantsIncrease
			0.05f, //QuestAnglerVestIncrease
			0.04f, //QuestCoralstoneBlockIncrease
			0.04f, //QuestDecorativeFurnitureIncrease
			0f, //QuestFishCostumeIncrease
			0.033f, //QuestFishHookIncrease
			0.025f, //QuestFishermansGuideIncrease
			0.012f, //QuestGoldenBugNetIncrease
			0f, //QuestGoldenFishingRodIncrease
			0.04f, //QuestHardcoreBottomlessBucketIncrease
			0.04f, //QuestHardcoreFinWingsIncrease
			0.086f, //QuestHardcoreHotlineFishingHookIncrease
			0.086f, //QuestHardcoreSuperAbsorbantSpongeIncrease
			0.025f, //QuestHighTestFishingLineIncrease
			0f, //QuestMermaidCostumeIncrease
			0.025f, //QuestSextantIncrease
			0.025f, //QuestTackleBoxIncrease
			0.09f, //QuestTrophyIncrease
			0.025f, //QuestWeatherRadioIncrease
			
			false, //ChestSalesmanPreHardmodeChestsRequireHardmodeActivated
			true, //ChestSalesmanSellsBiomeChests
			true, //ChestSalesmanSellsGoldChest
			true, //ChestSalesmanSellsIceChest
			true, //ChestSalesmanSellsIvyChest
			true, //ChestSalesmanSellsLihzahrdChest
			false, //ChestSalesmanSellsLivingWoodChest
			true, //ChestSalesmanSellsOceanChest
			true, //ChestSalesmanSellsShadowChest
			true, //ChestSalesmanSellsSkywareChest
			true, //ChestSalesmanSellsWebCoveredChest
			false, //ChestSalesmanSpawnable
			
			true, //MechanicSellsDartTrapAfterSkeletronDefeated
			true, //MechanicSellsGeyserAfterWallofFleshDefeated
			true, //MechanicSellsLihzahrdTrapsAfterGolemDefeated
			false, //MechanicSellsWoodenSpikesAfterGolemDefeated
			true, //MerchantSellsAllMiningGear
			false, //MerchantSellsBlizzardInABottleWhenInSnow
			false, //MerchantSellsCloudInABottleWhenInSky
			false, //MerchantSellsFishItem
			false, //MerchantSellsPyramidItems
			false, //MerchantSellsSandstormInABottleWhenInDesert
			false, //WitchDoctorSellsFlowerBoots
			false, //WitchDoctorSellsHoneyDispenser
			false, //WitchDoctorSellsSeaweed
			false, //WitchDoctorSellsStaffofRegrowth
			50000, //TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight
			
			true, //GoblinTinkererSellsGoblinRetreatOrder
			true, //PirateSellsPirateRetreatOrder
			true, //WizardSellsMoonBall
			1f, //BattlePotionMaxSpawnsMultiplier
			1f, //BattlePotionSpawnrateMultiplier
			0.1f, //BloodZombieAndDripplerDropsBloodMoonMedallion
			20f, //ChaosPotionMaxSpawnsMultiplier
			20f, //ChaosPotionSpawnrateMultiplier
			10f, //WarPotionMaxSpawnsMultiplier
			10f, //WarPotionSpawnrateMultiplier
			
			0, //NewCharacterBarrels
			0, //NewCharacterCopperBars
			0, //NewCharacterCopperCoins
			0, //NewCharacterGoldBars
			0, //NewCharacterGoldCoins
			0, //NewCharacterIronBars
			3, //NewCharacterMiningPotions
			0, //NewCharacterPlatinumCoins
			0, //NewCharacterSilverBars
			0, //NewCharacterSilverCoins
			0, //NewCharacterWarPotions

			0f, //ExtractinatorGivesAmber
			0.0014f, //ExtractinatorGivesAmberMosquito
			0f, //ExtractinatorGivesAmethyst
			0f, //ExtractinatorGivesCopperCoin
			0f, //ExtractinatorGivesCopperOre
			0f, //ExtractinatorGivesDiamond
			0f, //ExtractinatorGivesEmerald
			0f, //ExtractinatorGivesFossilOre
			0f, //ExtractinatorGivesGoldCoin
			0f, //ExtractinatorGivesGoldOre
			0f, //ExtractinatorGivesIronOre
			0f, //ExtractinatorGivesLeadOre
			0f, //ExtractinatorGivesPlatinumCoin
			0f, //ExtractinatorGivesPlatinumOre
			0f, //ExtractinatorGivesRuby
			0f, //ExtractinatorGivesSapphire
			0f, //ExtractinatorGivesSilverCoin
			0f, //ExtractinatorGivesSilverOre
			0f, //ExtractinatorGivesTinOre
			0f, //ExtractinatorGivesTopaz
			0f, //ExtractinatorGivesTungstenOre
			
			true, //CrateUpgradesDependingOnFishingPower
			0.01f, //FishCatchBecomesBalloonPufferfish
			0.02f, //FishCatchBecomesBladetongue
			0.01f, //FishCatchBecomesBlueJellyfish
			0f, //FishCatchBecomesChaosFish
			0f, //FishCatchBecomesCorruptCrate
			0f, //FishCatchBecomesCrimsonCrate
			0.02f, //FishCatchBecomesCrystalSerpent
			0f, //FishCatchBecomesDungeonCrate
			0.01f, //FishCatchBecomesFrogLeg
			0f, //FishCatchBecomesFrostDaggerfish
			0f, //FishCatchBecomesGoldenCarp
			0f, //FishCatchBecomesGoldenCrate
			0.01f, //FishCatchBecomesGreenJellyfish
			0f, //FishCatchBecomesHallowedCrate
			0f, //FishCatchBecomesIronCrate
			0f, //FishCatchBecomesJungleCrate
			0.01f, //FishCatchBecomesPinkJellyfish
			0f, //FishCatchBecomesPurpleClubberfish
			0f, //FishCatchBecomesReaverSharkAfterAllMechBossesDowned
			0.02f, //FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned
			0f, //FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned
			0f, //FishCatchBecomesRockfish
			0f, //FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned
			0.02f, //FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned
			0f, //FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned
			0.05f, //FishCatchBecomesScalyTruffle
			0f, //FishCatchBecomesSkyCrate
			0f, //FishCatchBecomesSwordfish
			0.02f, //FishCatchBecomesToxikarp
			0f, //FishCatchBecomesWoodenCrate
			0.01f //FishCatchBecomesZephyrFish
		);

        public struct ClientConf
        {
			public int DropTriesForAllEnemyDroppedLoot;
			public float NormalModeLootMultiplierForLootWithSeperateDifficultyRates;
			
			public float CrateDungeonBoneWelder;
			public float CrateEnchantedSundialGoldenIncrease;
			public float CrateEnchantedSundialIronIncrease;
			public float CrateEnchantedSundialWoodenIncrease;
			public float CrateJungleAnkeltOfTheWindIncrease;
			public float CrateJungleFeralClawsIncrease;
			public float CrateJungleFlowerBoots;
			public float CrateJungleLeafWand;
			public float CrateJungleLivingLoom;
			public float CrateJungleLivingMahoganyWand;
			public float CrateJungleLivingWoodWand;
			public float CrateJungleRichMahoganyLeafWand;
			public float CrateJungleSeaweed;
			public float CrateJungleStaffOfRegrowth;
			public float CrateSkySkyMill;
			public float CrateFlippersGolden;
			public float CrateFlippersIron;
			public float CrateFlippersWooden;
			public float CrateWaterWalkingBootsGolden;
			public float CrateWaterWalkingBootsIron;
			public float CrateWaterWalkingBootsWooden;
			public float CrateWoodenAgletIncrease;
			public float CrateWoodenClimbingClawsIncrease;
			public float CrateWoodenRadarIncrease;
			public float PresentCandyCaneBlock;
			public float PresentCandyCaneHook;
			public float PresentCandyCanePickaxe;
			public float PresentCandyCaneSword;
			public float PresentChristmasPudding;
			public float PresentCoal;
			public float PresentDogWhistle;
			public float PresentEggnog;
			public float PresentFruitcakeChakram;
			public float PresentGingerbreadCookie;
			public float PresentGreenCandyCaneBlock;
			public float PresentHandWarmer;
			public float PresentHardmodeSnowGlobe;
			public float PresentHolly;
			public float PresentMrsClausCostume;
			public float PresentParkaOutfit;
			public float PresentPineTreeBlock;
			public float PresentRedRyderPlusMusketBall;
			public float PresentReindeerAntlers;
			public float PresentSnowHat;
			public float PresentStarAnise;
			public float PresentSugarCookie;
			public float PresentToolbox ;
			public float PresentTreeCostume;
			public float PresentUglySweater;
			
			public float LootBookofSkullsIncrease;
			public float LootPicksawIncrease;
			public float LootSeedlingIncrease;
			public float LootSkeletronBoneKey;
			public float LootBinocularsIncrease;
			public float LootBoneRattleIncrease;
			public float LootBossMaskIncrease;
			public float LootBossTrophyIncrease;
			public float LootEatersBoneIncrease;
			public float LootFishronTruffleworm;
			public float LootFishronWingsIncrease;
			public float LootHoneyedGogglesIncrease;
			public float LootNectarIncrease;
			public float LootTheAxeIncrease;
			
			public float BiomeKeyIncreaseForOneMechBossDown;
			public float BiomeKeyIncreaseForTwoMechBossDown;
			public float BiomeKeyIncreaseForThreeMechBossDown;
			
			public float AllEnemiesLootBiomeMatchingFoundOnlyChestDrop;
			public float HellBatLootMagmaStoneIncrease;
			public float LavaBatLootMagmaStoneIncrease;
			public float LootAdhesiveBandageIncrease;
			public float LootAleTosser;
			public float LootAmarokIncrease;
			public float LootAncientClothIncrease;
			public float LootAncientCobaltBreastplateIncrease;
			public float LootAncientCobaltHelmetIncrease;
			public float LootAncientCobaltLeggingsIncrease;
			public float LootAncientGoldHelmetIncrease;
			public float LootAncientHornIncrease;
			public float LootAncientIronHelmetIncrease;
			public float LootAncientNecroHelmetIncrease;
			public float LootAncientShadowGreavesIncrease;
			public float LootAncientShadowHelmetIncrease;
			public float LootAncientShadowScalemailIncrease;
			public float LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory;
			public float LootArmorPolishIncrease;
			public float LootBabyGrinchsMischiefWhistleIncrease;
			public float LootBananarangIncrease;
			public float LootBeamSwordIncrease;
			public float LootBezoarIncrease;
			public float LootBlackBeltIncrease;
			public float LootBlackLensIncrease;
			public float LootBlessedAppleIncrease;
			public float LootBlindfoldIncrease;
			public float LootBloodyMacheteAndBladedGlovesIncrease;
			public float LootBoneFeatherIncrease;
			public float LootBonePickaxeIncrease;
			public float LootBoneSwordIncrease;
			public float LootBoneWandIncrease;
			public float LootBrainScramblerIncrease;
			public float LootBrokenBatWingIncrease;
			public float LootBunnyHoodIncrease;
			public float LootCascadeIncrease;
			public float LootChainGuillotinesIncrease;
			public float LootChainKnifeIncrease;
			public float LootClassyCane;
			public float LootClingerStaffIncrease;
			public float LootClothierVoodooDollIncrease;
			public float LootCompassIncrease;
			public float LootCrossNecklaceIncrease;
			public float LootCrystalVileShardIncrease;
			public float LootDaedalusStormbowIncrease;
			public float LootDarkShardIncrease;
			public float LootDartPistolIncrease;
			public float LootDartRifleIncrease;
			public float LootDeathSickleIncrease;
			public float LootDemonScytheIncrease;
			public float LootDepthMeterIncrease;
			public float LootDesertSpiritLampIncrease;
			public float LootDivingHelmetIncrease;
			public float LootDualHookIncrease;
			public float LootElfHatIncrease;
			public float LootElfPantsIncrease;
			public float LootElfShirtIncrease;
			public float LootEskimoCoatIncrease;
			public float LootEskimoHoodIncrease;
			public float LootEskimoPantsIncrease;
			public float LootExoticScimitar;
			public float LootEyePatchIncrease;
			public float LootEyeSpringIncrease;
			public float LootFastClockBaseIncrease;
			public float LootFestiveWingsIncrease;
			public float LootFetidBaghnakhsIncrease;
			public float LootFireFeatherIncrease;
			public float LootFleshKnucklesIncrease;
			public float LootFlyingKnifeIncrease;
			public float LootFrostStaffIncrease;
			public float LootFrozenTurtleShellIncrease;
			public float LootGiantBowIncrease;
			public float LootGiantHarpyFeatherIncrease;
			public float LootGoldenFurnitureIncrease;
			public float LootGoldenKeyIncrease;
			public float LootGoodieBagIncrease;
			public float LootGreenCap;
			public float LootHappyGrenade;
			public float LootHarpoonIncrease;
			public float LootHelFireIncrease;
			public float LootHookIncrease;
			public float LootIceSickleIncrease;
			public float LootIlluminantHookIncrease;
			public float LootJellyfishNecklaceIncrease;
			public float LootKOCannonIncrease;
			public float LootKeybrandIncrease;
			public float LootKrakenIncrease;
			public float LootLamiaClothesIncrease;
			public float LootLifeDrainIncrease;
			public float LootLightShardIncrease;
			public float LootLihzahrdPowerCellIncrease;
			public float LootLivingFireBlockIncrease;
			public float LootLizardEggIncrease;
			public float LootMagicDaggerIncrease;
			public float LootMagicQuiverIncrease;
			public float LootMagnetSphereIncrease;
			public float LootMandibleBladeIncrease;
			public float LootMarrowIncrease;
			public float LootMeatGrinderIncrease;
			public float LootMegaphoneBaseIncrease;
			public float LootMeteoriteIncrease;
			public float LootMiningHelmetIncrease;
			public float LootMiningPantsIncrease;
			public float LootMiningShirtIncrease;
			public float LootMoneyTroughIncrease;
			public float LootMoonCharmIncrease;
			public float LootMoonMaskIncrease;
			public float LootMoonStoneIncrease;
			public float LootMothronWingsIncrease;
			public float LootMummyCostumeIncrease;
			public float LootNazarIncrease;
			public float LootNimbusRodIncrease;
			public float LootObsidianRoseIncrease;
			public float LootPaintballGun;
			public float LootPaladinsShieldIncrease;
			public float LootPedguinssuitIncrease;
			public float LootPhilosophersStoneIncrease;
			public float LootPirateMapIncrease;
			public float LootPlumbersHatIncrease;
			public float LootPocketMirrorIncrease;
			public float LootPresentIncrease;
			public float LootPsychoKnifeIncrease;
			public float LootPutridScentIncrease;
			public float LootRainArmorIncrease;
			public int LootRainbowBlockDropMaxIncrease;
			public int LootRainbowBlockDropMinIncrease;
			public float LootRallyIncrease;
			public float LootReindeerBellsIncrease;
			public float LootRifleScopeIncrease;
			public float LootRobotHatIncrease;
			public float LootRocketLauncherIncrease;
			public float LootRodofDiscordIncrease;
			public float LootSWATHelmetIncrease;
			public float LootSailorHatIncrease;
			public float LootSailorPantsIncrease;
			public float LootSailorShirtIncrease;
			public float LootShackleIncrease;
			public float LootSkullIncrease;
			public float LootSlimeStaffIncrease;
			public float LootSniperRifleIncrease;
			public float LootSnowballLauncherIncrease;
			public float LootSoulofLightIncrease;
			public float LootSoulofNightIncrease;
			public float LootStarCloakIncrease;
			public float LootStylishScissors;
			public float LootSunMaskIncrease;
			public float LootTabiIncrease;
			public float LootTacticalShotgunIncrease;
			public float LootTallyCounterIncrease;
			public float LootTatteredBeeWingIncrease;
			public float LootTendonHookIncrease;
			public float LootTitanGloveIncrease;
			public float LootToySledIncrease;
			public float LootTrifoldMapIncrease;
			public float LootTurtleShellIncrease;
			public float LootUmbrellaHatIncrease;
			public float LootUnicornonaStickIncrease;
			public float LootUziIncrease;
			public float LootVikingHelmetIncrease;
			public float LootVitaminsIncrease;
			public float LootWhoopieCushionIncrease;
			public float LootWispinaBottleIncrease;
			public float LootWormHookIncrease;
			public float LootYeletsIncrease;
			public float LootZombieArmIncrease;
			public float PirateLootCoinGunBaseIncrease;
			public float PirateLootCutlassBaseIncrease;
			public float PirateLootDiscountCardBaseIncrease;
			public float PirateLootGoldRingBaseIncrease;
			public float PirateLootLuckyCoinBaseIncrease;
			public float PirateLootPirateStaffBaseIncrease;
			public bool LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense;
			public bool SlimeStaffIncreaseToSurfaceSlimes;
			public bool SlimeStaffIncreaseToUndergroundSlimes;
			public bool SlimeStaffIncreaseToCavernSlimess;
			public bool SlimeStaffIncreaseToIceSpikedSlimes;
			public bool SlimeStaffIncreaseToSpikedJungleSlimes;

			public float CavernModdedCavernLockBoxLoot;
			public float DungeonModdedBiomeLockBoxLoot;
			public float DungeonFurnitureLockBoxLoot;
			public float HardmodeModdedLockBoxDropRateModifier;
			public float HellBiomeModdedShadowLockBoxLoot;
			public float JungleTempleLihzahrd_Lock_Box;
			public float NormalmodeModdedLockBoxDropRateModifier;
			public float SandstormAndUndergroundDesertPyramidLockBoxLoot;
			public float SkyModdedSkywareLockBoxLoot;
			public float SpiderNestWebCoveredLockBoxLoot;
			public float SurfaceModdedLivingWoodLockBoxLoot;
			public float UndergroundJungleBiomeModdedIvyLockBoxLoot;
			public float UndergroundSnowBiomeModdedIceLockBoxLoot;
			public float WaterEnemyModdedWaterLockBoxLoot;
			
			public float TravelingMerchantAcornsIncrease;
			public float TravelingMerchantAmmoBoxIncrease;
			public float TravelingMerchantAngelHaloIncrease;
			public float TravelingMerchantArcaneRuneWallIncrease;
			public float TravelingMerchantBlackCounterweightIncrease;
			public float TravelingMerchantBlueDynastyShinglesIncrease;
			public float TravelingMerchantBlueTeamBlockIncrease;
			public float TravelingMerchantBlueTeamPlatformIncrease;
			public float TravelingMerchantBrickLayerIncrease;
			public float TravelingMerchantCastleMarsbergIncrease;
			public float TravelingMerchantCelestialMagnetIncrease;
			public float TravelingMerchantChaliceIncrease;
			public float TravelingMerchantCode1Increase;
			public float TravelingMerchantCode2Increase;
			public float TravelingMerchantColdSnapIncrease;
			public float TravelingMerchantCompanionCubeIncrease;
			public float TravelingMerchantCrimsonCapeIncrease;
			public float TravelingMerchantCursedSaintIncrease;
			public float TravelingMerchantDPSMeterIncrease;
			public float TravelingMerchantDiamondRingIncrease;
			public float TravelingMerchantDynastyWoodIncrease;
			public float TravelingMerchantExtendoGripIncrease;
			public float TravelingMerchantFancyDishesIncrease;
			public float TravelingMerchantFezIncrease;
			public float TravelingMerchantGatligatorIncrease;
			public float TravelingMerchantGiIncrease;
			public float TravelingMerchantGreenTeamBlockIncrease;
			public float TravelingMerchantGreenTeamPlatformIncrease;
			public float TravelingMerchantGypsyRobeIncrease;
			public float TravelingMerchantKatanaIncrease;
			public float TravelingMerchantKimonoIncrease;
			public float TravelingMerchantLeopardSkinIncrease;
			public float TravelingMerchantLifeformAnalyzerIncrease;
			public float TravelingMerchantMagicHatIncrease;
			public float TravelingMerchantMartiaLisaIncrease;
			public float TravelingMerchantMetalDetector;
			public float TravelingMerchantMysteriousCapeIncrease;
			public float TravelingMerchantNotAKidNorASquidIncrease;
			public float TravelingMerchantPadThaiIncrease;
			public float TravelingMerchantPaintSprayerIncrease;
			public float TravelingMerchantPhoIncrease;
			public float TravelingMerchantPinkTeamBlockIncrease;
			public float TravelingMerchantPinkTeamPlatformIncrease;
			public float TravelingMerchantPortableCementMixerIncrease;
			public float TravelingMerchantPresseratorIncrease;
			public float TravelingMerchantPulseBowIncrease;
			public float TravelingMerchantRedCapeIncrease;
			public float TravelingMerchantRedDynastyShinglesIncrease;
			public float TravelingMerchantRedTeamBlockIncrease;
			public float TravelingMerchantRedTeamPlatformIncrease;
			public float TravelingMerchantRevolverIncrease;
			public float TravelingMerchantSakeIncrease;
			public float TravelingMerchantSittingDucksFishingPoleIncrease;
			public float TravelingMerchantSnowfellasIncrease;
			public float TravelingMerchantStopwatchIncrease;
			public float TravelingMerchantTheSeasonIncrease;
			public float TravelingMerchantTheTruthIsUpThereIncrease;
			public float TravelingMerchantTigerSkinIncrease;
			public float TravelingMerchantUltraBrightTorchIncrease;
			public float TravelingMerchantWaterGunIncrease;
			public float TravelingMerchantWhiteTeamBlockIncrease;
			public float TravelingMerchantWhiteTeamPlatformIncrease;
			public float TravelingMerchantWinterCapeIncrease;
			public float TravelingMerchantYellowCounterweightIncrease;
			public float TravelingMerchantYellowTeamBlockIncrease;
			public float TravelingMerchantYellowTeamPlatformIncrease;
			public float TravelingMerchantZebraSkinIncrease;
			public bool TravelingMerchantAlwaysXMasForConfigurations;
			public float ChanceThatEnemyKillWillResetTravelingMerchant;
			public bool StationaryMerchant;
			public float StationaryMerchantStockingChance;
			public float S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate;
			
			public float QuestAnglerEarringIncrease;
			public float QuestAnglerHatIncrease;
			public float QuestAnglerPantsIncrease;
			public float QuestAnglerVestIncrease;
			public float QuestCoralstoneBlockIncrease;
			public float QuestDecorativeFurnitureIncrease;
			public float QuestFishCostumeIncrease;
			public float QuestFishHookIncrease;
			public float QuestFishermansGuideIncrease;
			public float QuestGoldenBugNetIncrease;
			public float QuestGoldenFishingRodIncrease;
			public float QuestHardcoreBottomlessBucketIncrease;
			public float QuestHardcoreFinWingsIncrease;
			public float QuestHardcoreHotlineFishingHookIncrease;
			public float QuestHardcoreSuperAbsorbantSpongeIncrease;
			public float QuestHighTestFishingLineIncrease;
			public float QuestMermaidCostumeIncrease;
			public float QuestSextantIncrease;
			public float QuestTackleBoxIncrease;
			public float QuestTrophyIncrease;
			public float QuestWeatherRadioIncrease;
			
			public bool ChestSalesmanPreHardmodeChestsRequireHardmodeActivated;
			public bool ChestSalesmanSellsBiomeChests;
			public bool ChestSalesmanSellsGoldChest;
			public bool ChestSalesmanSellsIceChest;
			public bool ChestSalesmanSellsIvyChest;
			public bool ChestSalesmanSellsLihzahrdChest;
			public bool ChestSalesmanSellsLivingWoodChest;
			public bool ChestSalesmanSellsOceanChest;
			public bool ChestSalesmanSellsShadowChest;
			public bool ChestSalesmanSellsSkywareChest;
			public bool ChestSalesmanSellsWebCoveredChest;
			public bool ChestSalesmanSpawnable;
			
			public bool MechanicSellsDartTrapAfterSkeletronDefeated;
			public bool MechanicSellsGeyserAfterWallofFleshDefeated;
			public bool MechanicSellsLihzahrdTrapsAfterGolemDefeated;
			public bool MechanicSellsWoodenSpikesAfterGolemDefeated;
			public bool MerchantSellsAllMiningGear;
			public bool MerchantSellsBlizzardInABottleWhenInSnow;
			public bool MerchantSellsCloudInABottleWhenInSky;
			public bool MerchantSellsFishItem;
			public bool MerchantSellsPyramidItems;
			public bool MerchantSellsSandstormInABottleWhenInDesert;
			public bool WitchDoctorSellsFlowerBoots;
			public bool WitchDoctorSellsHoneyDispenser;
			public bool WitchDoctorSellsSeaweed;
			public bool WitchDoctorSellsStaffofRegrowth;
			public int TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight;
			
			public bool GoblinTinkererSellsGoblinRetreatOrder;
			public bool PirateSellsPirateRetreatOrder;
			public bool WizardSellsMoonBall;
			public float BattlePotionMaxSpawnsMultiplier;
			public float BattlePotionSpawnrateMultiplier;
			public float BloodZombieAndDripplerDropsBloodMoonMedallion;
			public float ChaosPotionMaxSpawnsMultiplier;
			public float ChaosPotionSpawnrateMultiplier;
			public float WarPotionMaxSpawnsMultiplier;
			public float WarPotionSpawnrateMultiplier;
			
			public int NewCharacterBarrels;
			public int NewCharacterCopperBars;
			public int NewCharacterCopperCoins;
			public int NewCharacterGoldBars;
			public int NewCharacterGoldCoins;
			public int NewCharacterIronBars;
			public int NewCharacterMiningPotions;
			public int NewCharacterPlatinumCoins;
			public int NewCharacterSilverBars;
			public int NewCharacterSilverCoins;
			public int NewCharacterWarPotions;

			public float ExtractinatorGivesAmber;
			public float ExtractinatorGivesAmberMosquito;
			public float ExtractinatorGivesAmethyst;
			public float ExtractinatorGivesCopperCoin;
			public float ExtractinatorGivesCopperOre;
			public float ExtractinatorGivesDiamond;
			public float ExtractinatorGivesEmerald;
			public float ExtractinatorGivesFossilOre;
			public float ExtractinatorGivesGoldCoin;
			public float ExtractinatorGivesGoldOre;
			public float ExtractinatorGivesIronOre;
			public float ExtractinatorGivesLeadOre;
			public float ExtractinatorGivesPlatinumCoin;
			public float ExtractinatorGivesPlatinumOre;
			public float ExtractinatorGivesRuby;
			public float ExtractinatorGivesSapphire;
			public float ExtractinatorGivesSilverCoin;
			public float ExtractinatorGivesSilverOre;
			public float ExtractinatorGivesTinOre;
			public float ExtractinatorGivesTopaz;
			public float ExtractinatorGivesTungstenOre;
			
			public bool CrateUpgradesDependingOnFishingPower;
			public float FishCatchBecomesBalloonPufferfish;
			public float FishCatchBecomesBladetongue;
			public float FishCatchBecomesBlueJellyfish;
			public float FishCatchBecomesChaosFish;
			public float FishCatchBecomesCorruptCrate;
			public float FishCatchBecomesCrimsonCrate;
			public float FishCatchBecomesCrystalSerpent;
			public float FishCatchBecomesDungeonCrate;
			public float FishCatchBecomesFrogLeg;
			public float FishCatchBecomesFrostDaggerfish;
			public float FishCatchBecomesGoldenCarp;
			public float FishCatchBecomesGoldenCrate;
			public float FishCatchBecomesGreenJellyfish;
			public float FishCatchBecomesHallowedCrate;
			public float FishCatchBecomesIronCrate;
			public float FishCatchBecomesJungleCrate;
			public float FishCatchBecomesPinkJellyfish;
			public float FishCatchBecomesPurpleClubberfish;
			public float FishCatchBecomesReaverSharkAfterAllMechBossesDowned;
			public float FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
			public float FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
			public float FishCatchBecomesRockfish;
			public float FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned;
			public float FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
			public float FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
			public float FishCatchBecomesScalyTruffle;
			public float FishCatchBecomesSkyCrate;
			public float FishCatchBecomesSwordfish;
			public float FishCatchBecomesToxikarp;
			public float FishCatchBecomesWoodenCrate;
			public float FishCatchBecomesZephyrFish;

            public ClientConf(
				int CC_DropTriesForAllEnemyDroppedLoot,
				float CC_NormalModeLootMultiplierForLootWithSeperateDifficultyRates,
				
				float CC_CrateDungeonBoneWelder,
				float CC_CrateEnchantedSundialGoldenIncrease,
				float CC_CrateEnchantedSundialIronIncrease,
				float CC_CrateEnchantedSundialWoodenIncrease,
				float CC_CrateJungleAnkeltOfTheWindIncrease,
				float CC_CrateJungleFeralClawsIncrease,
				float CC_CrateJungleFlowerBoots,
				float CC_CrateJungleLeafWand,
				float CC_CrateJungleLivingLoom,
				float CC_CrateJungleLivingMahoganyWand,
				float CC_CrateJungleLivingWoodWand,
				float CC_CrateJungleRichMahoganyLeafWand,
				float CC_CrateJungleSeaweed,
				float CC_CrateJungleStaffOfRegrowth,
				float CC_CrateSkySkyMill,
				float CC_CrateFlippersGolden,
				float CC_CrateFlippersIron,
				float CC_CrateFlippersWooden,
				float CC_CrateWaterWalkingBootsGolden,
				float CC_CrateWaterWalkingBootsIron,
				float CC_CrateWaterWalkingBootsWooden,
				float CC_CrateWoodenAgletIncrease,
				float CC_CrateWoodenClimbingClawsIncrease,
				float CC_CrateWoodenRadarIncrease,
				float CC_PresentCandyCaneBlock,
				float CC_PresentCandyCaneHook,
				float CC_PresentCandyCanePickaxe,
				float CC_PresentCandyCaneSword,
				float CC_PresentChristmasPudding,
				float CC_PresentCoal,
				float CC_PresentDogWhistle,
				float CC_PresentEggnog,
				float CC_PresentFruitcakeChakram,
				float CC_PresentGingerbreadCookie,
				float CC_PresentGreenCandyCaneBlock,
				float CC_PresentHandWarmer,
				float CC_PresentHardmodeSnowGlobe,
				float CC_PresentHolly,
				float CC_PresentMrsClausCostume,
				float CC_PresentParkaOutfit,
				float CC_PresentPineTreeBlock,
				float CC_PresentRedRyderPlusMusketBall,
				float CC_PresentReindeerAntlers,
				float CC_PresentSnowHat,
				float CC_PresentStarAnise,
				float CC_PresentSugarCookie,
				float CC_PresentToolbox,
				float CC_PresentTreeCostume,
				float CC_PresentUglySweater,
				
				float CC_LootBookofSkullsIncrease,
				float CC_LootPicksawIncrease,
				float CC_LootSeedlingIncrease,
				float CC_LootSkeletronBoneKey,
				float CC_LootBinocularsIncrease,
				float CC_LootBoneRattleIncrease,
				float CC_LootBossMaskIncrease,
				float CC_LootBossTrophyIncrease,
				float CC_LootEatersBoneIncrease,
				float CC_LootFishronTruffleworm,
				float CC_LootFishronWingsIncrease,
				float CC_LootHoneyedGogglesIncrease,
				float CC_LootNectarIncrease,
				float CC_LootTheAxeIncrease,
				
				float CC_BiomeKeyIncreaseForOneMechBossDown,
				float CC_BiomeKeyIncreaseForTwoMechBossDown,
				float CC_BiomeKeyIncreaseForThreeMechBossDown,
				
				float CC_AllEnemiesLootBiomeMatchingFoundOnlyChestDrop,
				float CC_HellBatLootMagmaStoneIncrease,
				float CC_LavaBatLootMagmaStoneIncrease,
				float CC_LootAdhesiveBandageIncrease,
				float CC_LootAleTosser,
				float CC_LootAmarokIncrease,
				float CC_LootAncientClothIncrease,
				float CC_LootAncientCobaltBreastplateIncrease,
				float CC_LootAncientCobaltHelmetIncrease,
				float CC_LootAncientCobaltLeggingsIncrease,
				float CC_LootAncientGoldHelmetIncrease,
				float CC_LootAncientHornIncrease,
				float CC_LootAncientIronHelmetIncrease,
				float CC_LootAncientNecroHelmetIncrease,
				float CC_LootAncientShadowGreavesIncrease,
				float CC_LootAncientShadowHelmetIncrease,
				float CC_LootAncientShadowScalemailIncrease,
				float CC_LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory,
				float CC_LootArmorPolishIncrease,
				float CC_LootBabyGrinchsMischiefWhistleIncrease,
				float CC_LootBananarangIncrease,
				float CC_LootBeamSwordIncrease,
				float CC_LootBezoarIncrease,
				float CC_LootBlackBeltIncrease,
				float CC_LootBlackLensIncrease,
				float CC_LootBlessedAppleIncrease,
				float CC_LootBlindfoldIncrease,
				float CC_LootBloodyMacheteAndBladedGlovesIncrease,
				float CC_LootBoneFeatherIncrease,
				float CC_LootBonePickaxeIncrease,
				float CC_LootBoneSwordIncrease,
				float CC_LootBoneWandIncrease,
				float CC_LootBrainScramblerIncrease,
				float CC_LootBrokenBatWingIncrease,
				float CC_LootBunnyHoodIncrease,
				float CC_LootCascadeIncrease,
				float CC_LootChainGuillotinesIncrease,
				float CC_LootChainKnifeIncrease,
				float CC_LootClassyCane,
				float CC_LootClingerStaffIncrease,
				float CC_LootClothierVoodooDollIncrease,
				float CC_LootCompassIncrease,
				float CC_LootCrossNecklaceIncrease,
				float CC_LootCrystalVileShardIncrease,
				float CC_LootDaedalusStormbowIncrease,
				float CC_LootDarkShardIncrease,
				float CC_LootDartPistolIncrease,
				float CC_LootDartRifleIncrease,
				float CC_LootDeathSickleIncrease,
				float CC_LootDemonScytheIncrease,
				float CC_LootDepthMeterIncrease,
				float CC_LootDesertSpiritLampIncrease,
				float CC_LootDivingHelmetIncrease,
				float CC_LootDualHookIncrease,
				float CC_LootElfHatIncrease,
				float CC_LootElfPantsIncrease,
				float CC_LootElfShirtIncrease,
				float CC_LootEskimoCoatIncrease,
				float CC_LootEskimoHoodIncrease,
				float CC_LootEskimoPantsIncrease,
				float CC_LootExoticScimitar,
				float CC_LootEyePatchIncrease,
				float CC_LootEyeSpringIncrease,
				float CC_LootFastClockBaseIncrease,
				float CC_LootFestiveWingsIncrease,
				float CC_LootFetidBaghnakhsIncrease,
				float CC_LootFireFeatherIncrease,
				float CC_LootFleshKnucklesIncrease,
				float CC_LootFlyingKnifeIncrease,
				float CC_LootFrostStaffIncrease,
				float CC_LootFrozenTurtleShellIncrease,
				float CC_LootGiantBowIncrease,
				float CC_LootGiantHarpyFeatherIncrease,
				float CC_LootGoldenFurnitureIncrease,
				float CC_LootGoldenKeyIncrease,
				float CC_LootGoodieBagIncrease,
				float CC_LootGreenCap,
				float CC_LootHappyGrenade,
				float CC_LootHarpoonIncrease,
				float CC_LootHelFireIncrease,
				float CC_LootHookIncrease,
				float CC_LootIceSickleIncrease,
				float CC_LootIlluminantHookIncrease,
				float CC_LootJellyfishNecklaceIncrease,
				float CC_LootKOCannonIncrease,
				float CC_LootKeybrandIncrease,
				float CC_LootKrakenIncrease,
				float CC_LootLamiaClothesIncrease,
				float CC_LootLifeDrainIncrease,
				float CC_LootLightShardIncrease,
				float CC_LootLihzahrdPowerCellIncrease,
				float CC_LootLivingFireBlockIncrease,
				float CC_LootLizardEggIncrease,
				float CC_LootMagicDaggerIncrease,
				float CC_LootMagicQuiverIncrease,
				float CC_LootMagnetSphereIncrease,
				float CC_LootMandibleBladeIncrease,
				float CC_LootMarrowIncrease,
				float CC_LootMeatGrinderIncrease,
				float CC_LootMegaphoneBaseIncrease,
				float CC_LootMeteoriteIncrease,
				float CC_LootMiningHelmetIncrease,
				float CC_LootMiningPantsIncrease,
				float CC_LootMiningShirtIncrease,
				float CC_LootMoneyTroughIncrease,
				float CC_LootMoonCharmIncrease,
				float CC_LootMoonMaskIncrease,
				float CC_LootMoonStoneIncrease,
				float CC_LootMothronWingsIncrease,
				float CC_LootMummyCostumeIncrease,
				float CC_LootNazarIncrease,
				float CC_LootNimbusRodIncrease,
				float CC_LootObsidianRoseIncrease,
				float CC_LootPaintballGun,
				float CC_LootPaladinsShieldIncrease,
				float CC_LootPedguinssuitIncrease,
				float CC_LootPhilosophersStoneIncrease,
				float CC_LootPirateMapIncrease,
				float CC_LootPlumbersHatIncrease,
				float CC_LootPocketMirrorIncrease,
				float CC_LootPresentIncrease,
				float CC_LootPsychoKnifeIncrease,
				float CC_LootPutridScentIncrease,
				float CC_LootRainArmorIncrease,
				int CC_LootRainbowBlockDropMaxIncrease,
				int CC_LootRainbowBlockDropMinIncrease,
				float CC_LootRallyIncrease,
				float CC_LootReindeerBellsIncrease,
				float CC_LootRifleScopeIncrease,
				float CC_LootRobotHatIncrease,
				float CC_LootRocketLauncherIncrease,
				float CC_LootRodofDiscordIncrease,
				float CC_LootSWATHelmetIncrease,
				float CC_LootSailorHatIncrease,
				float CC_LootSailorPantsIncrease,
				float CC_LootSailorShirtIncrease,
				float CC_LootShackleIncrease,
				float CC_LootSkullIncrease,
				float CC_LootSlimeStaffIncrease,
				float CC_LootSniperRifleIncrease,
				float CC_LootSnowballLauncherIncrease,
				float CC_LootSoulofLightIncrease,
				float CC_LootSoulofNightIncrease,
				float CC_LootStarCloakIncrease,
				float CC_LootStylishScissors,
				float CC_LootSunMaskIncrease,
				float CC_LootTabiIncrease,
				float CC_LootTacticalShotgunIncrease,
				float CC_LootTallyCounterIncrease,
				float CC_LootTatteredBeeWingIncrease,
				float CC_LootTendonHookIncrease,
				float CC_LootTitanGloveIncrease,
				float CC_LootToySledIncrease,
				float CC_LootTrifoldMapIncrease,
				float CC_LootTurtleShellIncrease,
				float CC_LootUmbrellaHatIncrease,
				float CC_LootUnicornonaStickIncrease,
				float CC_LootUziIncrease,
				float CC_LootVikingHelmetIncrease,
				float CC_LootVitaminsIncrease,
				float CC_LootWhoopieCushionIncrease,
				float CC_LootWispinaBottleIncrease,
				float CC_LootWormHookIncrease,
				float CC_LootYeletsIncrease,
				float CC_LootZombieArmIncrease,
				float CC_PirateLootCoinGunBaseIncrease,
				float CC_PirateLootCutlassBaseIncrease,
				float CC_PirateLootDiscountCardBaseIncrease,
				float CC_PirateLootGoldRingBaseIncrease,
				float CC_PirateLootLuckyCoinBaseIncrease,
				float CC_PirateLootPirateStaffBaseIncrease,
				bool CC_LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense,
				bool CC_SlimeStaffIncreaseToSurfaceSlimes,
				bool CC_SlimeStaffIncreaseToUndergroundSlimes,
				bool CC_SlimeStaffIncreaseToCavernSlimess,
				bool CC_SlimeStaffIncreaseToIceSpikedSlimes,
				bool CC_SlimeStaffIncreaseToSpikedJungleSlimes,

				float CC_CavernModdedCavernLockBoxLoot,
				float CC_DungeonModdedBiomeLockBoxLoot,
				float CC_DungeonFurnitureLockBoxLoot,
				float CC_HardmodeModdedLockBoxDropRateModifier,
				float CC_HellBiomeModdedShadowLockBoxLoot,
				float CC_JungleTempleLihzahrd_Lock_Box,
				float CC_NormalmodeModdedLockBoxDropRateModifier,
				float CC_SandstormAndUndergroundDesertPyramidLockBoxLoot,
				float CC_SkyModdedSkywareLockBoxLoot,
				float CC_SpiderNestWebCoveredLockBoxLoot,
				float CC_SurfaceModdedLivingWoodLockBoxLoot,
				float CC_UndergroundJungleBiomeModdedIvyLockBoxLoot,
				float CC_UndergroundSnowBiomeModdedIceLockBoxLoot,
				float CC_WaterEnemyModdedWaterLockBoxLoot,
				
				float CC_TravelingMerchantAcornsIncrease,
				float CC_TravelingMerchantAmmoBoxIncrease,
				float CC_TravelingMerchantAngelHaloIncrease,
				float CC_TravelingMerchantArcaneRuneWallIncrease,
				float CC_TravelingMerchantBlackCounterweightIncrease,
				float CC_TravelingMerchantBlueDynastyShinglesIncrease,
				float CC_TravelingMerchantBlueTeamBlockIncrease,
				float CC_TravelingMerchantBlueTeamPlatformIncrease,
				float CC_TravelingMerchantBrickLayerIncrease,
				float CC_TravelingMerchantCastleMarsbergIncrease,
				float CC_TravelingMerchantCelestialMagnetIncrease,
				float CC_TravelingMerchantChaliceIncrease,
				float CC_TravelingMerchantCode1Increase,
				float CC_TravelingMerchantCode2Increase,
				float CC_TravelingMerchantColdSnapIncrease,
				float CC_TravelingMerchantCompanionCubeIncrease,
				float CC_TravelingMerchantCrimsonCapeIncrease,
				float CC_TravelingMerchantCursedSaintIncrease,
				float CC_TravelingMerchantDPSMeterIncrease,
				float CC_TravelingMerchantDiamondRingIncrease,
				float CC_TravelingMerchantDynastyWoodIncrease,
				float CC_TravelingMerchantExtendoGripIncrease,
				float CC_TravelingMerchantFancyDishesIncrease,
				float CC_TravelingMerchantFezIncrease,
				float CC_TravelingMerchantGatligatorIncrease,
				float CC_TravelingMerchantGiIncrease,
				float CC_TravelingMerchantGreenTeamBlockIncrease,
				float CC_TravelingMerchantGreenTeamPlatformIncrease,
				float CC_TravelingMerchantGypsyRobeIncrease,
				float CC_TravelingMerchantKatanaIncrease,
				float CC_TravelingMerchantKimonoIncrease,
				float CC_TravelingMerchantLeopardSkinIncrease,
				float CC_TravelingMerchantLifeformAnalyzerIncrease,
				float CC_TravelingMerchantMagicHatIncrease,
				float CC_TravelingMerchantMartiaLisaIncrease,
				float CC_TravelingMerchantMetalDetector,
				float CC_TravelingMerchantMysteriousCapeIncrease,
				float CC_TravelingMerchantNotAKidNorASquidIncrease,
				float CC_TravelingMerchantPadThaiIncrease,
				float CC_TravelingMerchantPaintSprayerIncrease,
				float CC_TravelingMerchantPhoIncrease,
				float CC_TravelingMerchantPinkTeamBlockIncrease,
				float CC_TravelingMerchantPinkTeamPlatformIncrease,
				float CC_TravelingMerchantPortableCementMixerIncrease,
				float CC_TravelingMerchantPresseratorIncrease,
				float CC_TravelingMerchantPulseBowIncrease,
				float CC_TravelingMerchantRedCapeIncrease,
				float CC_TravelingMerchantRedDynastyShinglesIncrease,
				float CC_TravelingMerchantRedTeamBlockIncrease,
				float CC_TravelingMerchantRedTeamPlatformIncrease,
				float CC_TravelingMerchantRevolverIncrease,
				float CC_TravelingMerchantSakeIncrease,
				float CC_TravelingMerchantSittingDucksFishingPoleIncrease,
				float CC_TravelingMerchantSnowfellasIncrease,
				float CC_TravelingMerchantStopwatchIncrease,
				float CC_TravelingMerchantTheSeasonIncrease,
				float CC_TravelingMerchantTheTruthIsUpThereIncrease,
				float CC_TravelingMerchantTigerSkinIncrease,
				float CC_TravelingMerchantUltraBrightTorchIncrease,
				float CC_TravelingMerchantWaterGunIncrease,
				float CC_TravelingMerchantWhiteTeamBlockIncrease,
				float CC_TravelingMerchantWhiteTeamPlatformIncrease,
				float CC_TravelingMerchantWinterCapeIncrease,
				float CC_TravelingMerchantYellowCounterweightIncrease,
				float CC_TravelingMerchantYellowTeamBlockIncrease,
				float CC_TravelingMerchantYellowTeamPlatformIncrease,
				float CC_TravelingMerchantZebraSkinIncrease,
				bool CC_TravelingMerchantAlwaysXMasForConfigurations,
				float CC_ChanceThatEnemyKillWillResetTravelingMerchant,
				bool CC_StationaryMerchant,
				float CC_StationaryMerchantStockingChance,
				float CC_S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate,
				
				float CC_QuestAnglerEarringIncrease,
				float CC_QuestAnglerHatIncrease,
				float CC_QuestAnglerPantsIncrease,
				float CC_QuestAnglerVestIncrease,
				float CC_QuestCoralstoneBlockIncrease,
				float CC_QuestDecorativeFurnitureIncrease,
				float CC_QuestFishCostumeIncrease,
				float CC_QuestFishHookIncrease,
				float CC_QuestFishermansGuideIncrease,
				float CC_QuestGoldenBugNetIncrease,
				float CC_QuestGoldenFishingRodIncrease,
				float CC_QuestHardcoreBottomlessBucketIncrease,
				float CC_QuestHardcoreFinWingsIncrease,
				float CC_QuestHardcoreHotlineFishingHookIncrease,
				float CC_QuestHardcoreSuperAbsorbantSpongeIncrease,
				float CC_QuestHighTestFishingLineIncrease,
				float CC_QuestMermaidCostumeIncrease,
				float CC_QuestSextantIncrease,
				float CC_QuestTackleBoxIncrease,
				float CC_QuestTrophyIncrease,
				float CC_QuestWeatherRadioIncrease,
				
				bool CC_ChestSalesmanPreHardmodeChestsRequireHardmodeActivated,
				bool CC_ChestSalesmanSellsBiomeChests,
				bool CC_ChestSalesmanSellsGoldChest,
				bool CC_ChestSalesmanSellsIceChest,
				bool CC_ChestSalesmanSellsIvyChest,
				bool CC_ChestSalesmanSellsLihzahrdChest,
				bool CC_ChestSalesmanSellsLivingWoodChest,
				bool CC_ChestSalesmanSellsOceanChest,
				bool CC_ChestSalesmanSellsShadowChest,
				bool CC_ChestSalesmanSellsSkywareChest,
				bool CC_ChestSalesmanSellsWebCoveredChest,
				bool CC_ChestSalesmanSpawnable,
				
				bool CC_MechanicSellsDartTrapAfterSkeletronDefeated,
				bool CC_MechanicSellsGeyserAfterWallofFleshDefeated,
				bool CC_MechanicSellsLihzahrdTrapsAfterGolemDefeated,
				bool CC_MechanicSellsWoodenSpikesAfterGolemDefeated,
				bool CC_MerchantSellsAllMiningGear,
				bool CC_MerchantSellsBlizzardInABottleWhenInSnow,
				bool CC_MerchantSellsCloudInABottleWhenInSky,
				bool CC_MerchantSellsFishItem,
				bool CC_MerchantSellsPyramidItems,
				bool CC_MerchantSellsSandstormInABottleWhenInDesert,
				bool CC_WitchDoctorSellsFlowerBoots,
				bool CC_WitchDoctorSellsHoneyDispenser,
				bool CC_WitchDoctorSellsSeaweed,
				bool CC_WitchDoctorSellsStaffofRegrowth,
				int CC_TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight,
				
				bool CC_GoblinTinkererSellsGoblinRetreatOrder,
				bool CC_PirateSellsPirateRetreatOrder,
				bool CC_WizardSellsMoonBall,
				float CC_BattlePotionMaxSpawnsMultiplier,
				float CC_BattlePotionSpawnrateMultiplier,
				float CC_BloodZombieAndDripplerDropsBloodMoonMedallion,
				float CC_ChaosPotionMaxSpawnsMultiplier,
				float CC_ChaosPotionSpawnrateMultiplier,
				float CC_WarPotionMaxSpawnsMultiplier,
				float CC_WarPotionSpawnrateMultiplier,
				
				int CC_NewCharacterBarrels,
				int CC_NewCharacterCopperBars,
				int CC_NewCharacterCopperCoins,
				int CC_NewCharacterGoldBars,
				int CC_NewCharacterGoldCoins,
				int CC_NewCharacterIronBars,
				int CC_NewCharacterMiningPotions,
				int CC_NewCharacterPlatinumCoins,
				int CC_NewCharacterSilverBars,
				int CC_NewCharacterSilverCoins,
				int CC_NewCharacterWarPotions,

				float CC_ExtractinatorGivesAmber,
				float CC_ExtractinatorGivesAmberMosquito,
				float CC_ExtractinatorGivesAmethyst,
				float CC_ExtractinatorGivesCopperCoin,
				float CC_ExtractinatorGivesCopperOre,
				float CC_ExtractinatorGivesDiamond,
				float CC_ExtractinatorGivesEmerald,
				float CC_ExtractinatorGivesFossilOre,
				float CC_ExtractinatorGivesGoldCoin,
				float CC_ExtractinatorGivesGoldOre,
				float CC_ExtractinatorGivesIronOre,
				float CC_ExtractinatorGivesLeadOre,
				float CC_ExtractinatorGivesPlatinumCoin,
				float CC_ExtractinatorGivesPlatinumOre,
				float CC_ExtractinatorGivesRuby,
				float CC_ExtractinatorGivesSapphire,
				float CC_ExtractinatorGivesSilverCoin,
				float CC_ExtractinatorGivesSilverOre,
				float CC_ExtractinatorGivesTinOre,
				float CC_ExtractinatorGivesTopaz,
				float CC_ExtractinatorGivesTungstenOre,
				
				bool CC_CrateUpgradesDependingOnFishingPower,
				float CC_FishCatchBecomesBalloonPufferfish,
				float CC_FishCatchBecomesBladetongue,
				float CC_FishCatchBecomesBlueJellyfish,
				float CC_FishCatchBecomesChaosFish,
				float CC_FishCatchBecomesCorruptCrate,
				float CC_FishCatchBecomesCrimsonCrate,
				float CC_FishCatchBecomesCrystalSerpent,
				float CC_FishCatchBecomesDungeonCrate,
				float CC_FishCatchBecomesFrogLeg,
				float CC_FishCatchBecomesFrostDaggerfish,
				float CC_FishCatchBecomesGoldenCarp,
				float CC_FishCatchBecomesGoldenCrate,
				float CC_FishCatchBecomesGreenJellyfish,
				float CC_FishCatchBecomesHallowedCrate,
				float CC_FishCatchBecomesIronCrate,
				float CC_FishCatchBecomesJungleCrate,
				float CC_FishCatchBecomesPinkJellyfish,
				float CC_FishCatchBecomesPurpleClubberfish,
				float CC_FishCatchBecomesReaverSharkAfterAllMechBossesDowned,
				float CC_FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				float CC_FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				float CC_FishCatchBecomesRockfish,
				float CC_FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned,
				float CC_FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				float CC_FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				float CC_FishCatchBecomesScalyTruffle,
				float CC_FishCatchBecomesSkyCrate,
				float CC_FishCatchBecomesSwordfish,
				float CC_FishCatchBecomesToxikarp,
				float CC_FishCatchBecomesWoodenCrate,
				float CC_FishCatchBecomesZephyrFish
			)
            {
                DropTriesForAllEnemyDroppedLoot = CC_DropTriesForAllEnemyDroppedLoot;
				NormalModeLootMultiplierForLootWithSeperateDifficultyRates = CC_NormalModeLootMultiplierForLootWithSeperateDifficultyRates;
						
				CrateDungeonBoneWelder = CC_CrateDungeonBoneWelder;
				CrateEnchantedSundialGoldenIncrease = CC_CrateEnchantedSundialGoldenIncrease;
				CrateEnchantedSundialIronIncrease = CC_CrateEnchantedSundialIronIncrease;
				CrateEnchantedSundialWoodenIncrease = CC_CrateEnchantedSundialWoodenIncrease;
				CrateJungleAnkeltOfTheWindIncrease = CC_CrateJungleAnkeltOfTheWindIncrease;
				CrateJungleFeralClawsIncrease = CC_CrateJungleFeralClawsIncrease;
				CrateJungleFlowerBoots = CC_CrateJungleFlowerBoots;
				CrateJungleLeafWand = CC_CrateJungleLeafWand;
				CrateJungleLivingLoom = CC_CrateJungleLivingLoom;
				CrateJungleLivingMahoganyWand = CC_CrateJungleLivingMahoganyWand;
				CrateJungleLivingWoodWand = CC_CrateJungleLivingWoodWand;
				CrateJungleRichMahoganyLeafWand = CC_CrateJungleRichMahoganyLeafWand;
				CrateJungleSeaweed = CC_CrateJungleSeaweed;
				CrateJungleStaffOfRegrowth = CC_CrateJungleStaffOfRegrowth;
				CrateSkySkyMill = CC_CrateSkySkyMill;
				CrateFlippersGolden = CC_CrateFlippersGolden;
				CrateFlippersIron = CC_CrateFlippersIron;
				CrateFlippersWooden = CC_CrateFlippersWooden;
				CrateWaterWalkingBootsGolden = CC_CrateWaterWalkingBootsGolden;
				CrateWaterWalkingBootsIron = CC_CrateWaterWalkingBootsIron;
				CrateWaterWalkingBootsWooden = CC_CrateWaterWalkingBootsWooden;
				CrateWoodenAgletIncrease = CC_CrateWoodenAgletIncrease;
				CrateWoodenClimbingClawsIncrease = CC_CrateWoodenClimbingClawsIncrease;
				CrateWoodenRadarIncrease = CC_CrateWoodenRadarIncrease;
				PresentCandyCaneBlock = CC_PresentCandyCaneBlock;
				PresentCandyCaneHook = CC_PresentCandyCaneHook;
				PresentCandyCanePickaxe = CC_PresentCandyCanePickaxe;
				PresentCandyCaneSword = CC_PresentCandyCaneSword;
				PresentChristmasPudding = CC_PresentChristmasPudding;
				PresentCoal = CC_PresentCoal;
				PresentDogWhistle = CC_PresentDogWhistle;
				PresentEggnog = CC_PresentEggnog;
				PresentFruitcakeChakram = CC_PresentFruitcakeChakram;
				PresentGingerbreadCookie = CC_PresentGingerbreadCookie;
				PresentGreenCandyCaneBlock = CC_PresentGreenCandyCaneBlock;
				PresentHandWarmer = CC_PresentHandWarmer;
				PresentHardmodeSnowGlobe = CC_PresentHardmodeSnowGlobe;
				PresentHolly = CC_PresentHolly;
				PresentMrsClausCostume = CC_PresentMrsClausCostume;
				PresentParkaOutfit = CC_PresentParkaOutfit;
				PresentPineTreeBlock = CC_PresentPineTreeBlock;
				PresentRedRyderPlusMusketBall = CC_PresentRedRyderPlusMusketBall;
				PresentReindeerAntlers = CC_PresentReindeerAntlers;
				PresentSnowHat = CC_PresentSnowHat;
				PresentStarAnise = CC_PresentStarAnise;
				PresentSugarCookie = CC_PresentSugarCookie;
				PresentToolbox  = CC_PresentToolbox;
				PresentTreeCostume = CC_PresentTreeCostume;
				PresentUglySweater = CC_PresentUglySweater;
						
				LootBookofSkullsIncrease = CC_LootBookofSkullsIncrease;
				LootPicksawIncrease = CC_LootPicksawIncrease;
				LootSeedlingIncrease = CC_LootSeedlingIncrease;
				LootSkeletronBoneKey = CC_LootSkeletronBoneKey;
				LootBinocularsIncrease = CC_LootBinocularsIncrease;
				LootBoneRattleIncrease = CC_LootBoneRattleIncrease;
				LootBossMaskIncrease = CC_LootBossMaskIncrease;
				LootBossTrophyIncrease = CC_LootBossTrophyIncrease;
				LootEatersBoneIncrease = CC_LootEatersBoneIncrease;
				LootFishronTruffleworm = CC_LootFishronTruffleworm;
				LootFishronWingsIncrease = CC_LootFishronWingsIncrease;
				LootHoneyedGogglesIncrease = CC_LootHoneyedGogglesIncrease;
				LootNectarIncrease = CC_LootNectarIncrease;
				LootTheAxeIncrease = CC_LootTheAxeIncrease;
						
				BiomeKeyIncreaseForOneMechBossDown = CC_BiomeKeyIncreaseForOneMechBossDown;
				BiomeKeyIncreaseForTwoMechBossDown = CC_BiomeKeyIncreaseForTwoMechBossDown;
				BiomeKeyIncreaseForThreeMechBossDown = CC_BiomeKeyIncreaseForThreeMechBossDown;
						
				AllEnemiesLootBiomeMatchingFoundOnlyChestDrop = CC_AllEnemiesLootBiomeMatchingFoundOnlyChestDrop;
				HellBatLootMagmaStoneIncrease = CC_HellBatLootMagmaStoneIncrease;
				LavaBatLootMagmaStoneIncrease = CC_LavaBatLootMagmaStoneIncrease;
				LootAdhesiveBandageIncrease = CC_LootAdhesiveBandageIncrease;
				LootAleTosser = CC_LootAleTosser;
				LootAmarokIncrease = CC_LootAmarokIncrease;
				LootAncientClothIncrease = CC_LootAncientClothIncrease;
				LootAncientCobaltBreastplateIncrease = CC_LootAncientCobaltBreastplateIncrease;
				LootAncientCobaltHelmetIncrease = CC_LootAncientCobaltHelmetIncrease;
				LootAncientCobaltLeggingsIncrease = CC_LootAncientCobaltLeggingsIncrease;
				LootAncientGoldHelmetIncrease = CC_LootAncientGoldHelmetIncrease;
				LootAncientHornIncrease = CC_LootAncientHornIncrease;
				LootAncientIronHelmetIncrease = CC_LootAncientIronHelmetIncrease;
				LootAncientNecroHelmetIncrease = CC_LootAncientNecroHelmetIncrease;
				LootAncientShadowGreavesIncrease = CC_LootAncientShadowGreavesIncrease;
				LootAncientShadowHelmetIncrease = CC_LootAncientShadowHelmetIncrease;
				LootAncientShadowScalemailIncrease = CC_LootAncientShadowScalemailIncrease;
				LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory = CC_LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory;
				LootArmorPolishIncrease = CC_LootArmorPolishIncrease;
				LootBabyGrinchsMischiefWhistleIncrease = CC_LootBabyGrinchsMischiefWhistleIncrease;
				LootBananarangIncrease = CC_LootBananarangIncrease;
				LootBeamSwordIncrease = CC_LootBeamSwordIncrease;
				LootBezoarIncrease = CC_LootBezoarIncrease;
				LootBlackBeltIncrease = CC_LootBlackBeltIncrease;
				LootBlackLensIncrease = CC_LootBlackLensIncrease;
				LootBlessedAppleIncrease = CC_LootBlessedAppleIncrease;
				LootBlindfoldIncrease = CC_LootBlindfoldIncrease;
				LootBloodyMacheteAndBladedGlovesIncrease = CC_LootBloodyMacheteAndBladedGlovesIncrease;
				LootBoneFeatherIncrease = CC_LootBoneFeatherIncrease;
				LootBonePickaxeIncrease = CC_LootBonePickaxeIncrease;
				LootBoneSwordIncrease = CC_LootBoneSwordIncrease;
				LootBoneWandIncrease = CC_LootBoneWandIncrease;
				LootBrainScramblerIncrease = CC_LootBrainScramblerIncrease;
				LootBrokenBatWingIncrease = CC_LootBrokenBatWingIncrease;
				LootBunnyHoodIncrease = CC_LootBunnyHoodIncrease;
				LootCascadeIncrease = CC_LootCascadeIncrease;
				LootChainGuillotinesIncrease = CC_LootChainGuillotinesIncrease;
				LootChainKnifeIncrease = CC_LootChainKnifeIncrease;
				LootClassyCane = CC_LootClassyCane;
				LootClingerStaffIncrease = CC_LootClingerStaffIncrease;
				LootClothierVoodooDollIncrease = CC_LootClothierVoodooDollIncrease;
				LootCompassIncrease = CC_LootCompassIncrease;
				LootCrossNecklaceIncrease = CC_LootCrossNecklaceIncrease;
				LootCrystalVileShardIncrease = CC_LootCrystalVileShardIncrease;
				LootDaedalusStormbowIncrease = CC_LootDaedalusStormbowIncrease;
				LootDarkShardIncrease = CC_LootDarkShardIncrease;
				LootDartPistolIncrease = CC_LootDartPistolIncrease;
				LootDartRifleIncrease = CC_LootDartRifleIncrease;
				LootDeathSickleIncrease = CC_LootDeathSickleIncrease;
				LootDemonScytheIncrease = CC_LootDemonScytheIncrease;
				LootDepthMeterIncrease = CC_LootDepthMeterIncrease;
				LootDesertSpiritLampIncrease = CC_LootDesertSpiritLampIncrease;
				LootDivingHelmetIncrease = CC_LootDivingHelmetIncrease;
				LootDualHookIncrease = CC_LootDualHookIncrease;
				LootElfHatIncrease = CC_LootElfHatIncrease;
				LootElfPantsIncrease = CC_LootElfPantsIncrease;
				LootElfShirtIncrease = CC_LootElfShirtIncrease;
				LootEskimoCoatIncrease = CC_LootEskimoCoatIncrease;
				LootEskimoHoodIncrease = CC_LootEskimoHoodIncrease;
				LootEskimoPantsIncrease = CC_LootEskimoPantsIncrease;
				LootExoticScimitar = CC_LootExoticScimitar;
				LootEyePatchIncrease = CC_LootEyePatchIncrease;
				LootEyeSpringIncrease = CC_LootEyeSpringIncrease;
				LootFastClockBaseIncrease = CC_LootFastClockBaseIncrease;
				LootFestiveWingsIncrease = CC_LootFestiveWingsIncrease;
				LootFetidBaghnakhsIncrease = CC_LootFetidBaghnakhsIncrease;
				LootFireFeatherIncrease = CC_LootFireFeatherIncrease;
				LootFleshKnucklesIncrease = CC_LootFleshKnucklesIncrease;
				LootFlyingKnifeIncrease = CC_LootFlyingKnifeIncrease;
				LootFrostStaffIncrease = CC_LootFrostStaffIncrease;
				LootFrozenTurtleShellIncrease = CC_LootFrozenTurtleShellIncrease;
				LootGiantBowIncrease = CC_LootGiantBowIncrease;
				LootGiantHarpyFeatherIncrease = CC_LootGiantHarpyFeatherIncrease;
				LootGoldenFurnitureIncrease = CC_LootGoldenFurnitureIncrease;
				LootGoldenKeyIncrease = CC_LootGoldenKeyIncrease;
				LootGoodieBagIncrease = CC_LootGoodieBagIncrease;
				LootGreenCap = CC_LootGreenCap;
				LootHappyGrenade = CC_LootHappyGrenade;
				LootHarpoonIncrease = CC_LootHarpoonIncrease;
				LootHelFireIncrease = CC_LootHelFireIncrease;
				LootHookIncrease = CC_LootHookIncrease;
				LootIceSickleIncrease = CC_LootIceSickleIncrease;
				LootIlluminantHookIncrease = CC_LootIlluminantHookIncrease;
				LootJellyfishNecklaceIncrease = CC_LootJellyfishNecklaceIncrease;
				LootKOCannonIncrease = CC_LootKOCannonIncrease;
				LootKeybrandIncrease = CC_LootKeybrandIncrease;
				LootKrakenIncrease = CC_LootKrakenIncrease;
				LootLamiaClothesIncrease = CC_LootLamiaClothesIncrease;
				LootLifeDrainIncrease = CC_LootLifeDrainIncrease;
				LootLightShardIncrease = CC_LootLightShardIncrease;
				LootLihzahrdPowerCellIncrease = CC_LootLihzahrdPowerCellIncrease;
				LootLivingFireBlockIncrease = CC_LootLivingFireBlockIncrease;
				LootLizardEggIncrease = CC_LootLizardEggIncrease;
				LootMagicDaggerIncrease = CC_LootMagicDaggerIncrease;
				LootMagicQuiverIncrease = CC_LootMagicQuiverIncrease;
				LootMagnetSphereIncrease = CC_LootMagnetSphereIncrease;
				LootMandibleBladeIncrease = CC_LootMandibleBladeIncrease;
				LootMarrowIncrease = CC_LootMarrowIncrease;
				LootMeatGrinderIncrease = CC_LootMeatGrinderIncrease;
				LootMegaphoneBaseIncrease = CC_LootMegaphoneBaseIncrease;
				LootMeteoriteIncrease = CC_LootMeteoriteIncrease;
				LootMiningHelmetIncrease = CC_LootMiningHelmetIncrease;
				LootMiningPantsIncrease = CC_LootMiningPantsIncrease;
				LootMiningShirtIncrease = CC_LootMiningShirtIncrease;
				LootMoneyTroughIncrease = CC_LootMoneyTroughIncrease;
				LootMoonCharmIncrease = CC_LootMoonCharmIncrease;
				LootMoonMaskIncrease = CC_LootMoonMaskIncrease;
				LootMoonStoneIncrease = CC_LootMoonStoneIncrease;
				LootMothronWingsIncrease = CC_LootMothronWingsIncrease;
				LootMummyCostumeIncrease = CC_LootMummyCostumeIncrease;
				LootNazarIncrease = CC_LootNazarIncrease;
				LootNimbusRodIncrease = CC_LootNimbusRodIncrease;
				LootObsidianRoseIncrease = CC_LootObsidianRoseIncrease;
				LootPaintballGun = CC_LootPaintballGun;
				LootPaladinsShieldIncrease = CC_LootPaladinsShieldIncrease;
				LootPedguinssuitIncrease = CC_LootPedguinssuitIncrease;
				LootPhilosophersStoneIncrease = CC_LootPhilosophersStoneIncrease;
				LootPirateMapIncrease = CC_LootPirateMapIncrease;
				LootPlumbersHatIncrease = CC_LootPlumbersHatIncrease;
				LootPocketMirrorIncrease = CC_LootPocketMirrorIncrease;
				LootPresentIncrease = CC_LootPresentIncrease;
				LootPsychoKnifeIncrease = CC_LootPsychoKnifeIncrease;
				LootPutridScentIncrease = CC_LootPutridScentIncrease;
				LootRainArmorIncrease = CC_LootRainArmorIncrease;
				LootRainbowBlockDropMaxIncrease = CC_LootRainbowBlockDropMaxIncrease;
				LootRainbowBlockDropMinIncrease = CC_LootRainbowBlockDropMinIncrease;
				LootRallyIncrease = CC_LootRallyIncrease;
				LootReindeerBellsIncrease = CC_LootReindeerBellsIncrease;
				LootRifleScopeIncrease = CC_LootRifleScopeIncrease;
				LootRobotHatIncrease = CC_LootRobotHatIncrease;
				LootRocketLauncherIncrease = CC_LootRocketLauncherIncrease;
				LootRodofDiscordIncrease = CC_LootRodofDiscordIncrease;
				LootSWATHelmetIncrease = CC_LootSWATHelmetIncrease;
				LootSailorHatIncrease = CC_LootSailorHatIncrease;
				LootSailorPantsIncrease = CC_LootSailorPantsIncrease;
				LootSailorShirtIncrease = CC_LootSailorShirtIncrease;
				LootShackleIncrease = CC_LootShackleIncrease;
				LootSkullIncrease = CC_LootSkullIncrease;
				LootSlimeStaffIncrease = CC_LootSlimeStaffIncrease;
				LootSniperRifleIncrease = CC_LootSniperRifleIncrease;
				LootSnowballLauncherIncrease = CC_LootSnowballLauncherIncrease;
				LootSoulofLightIncrease = CC_LootSoulofLightIncrease;
				LootSoulofNightIncrease = CC_LootSoulofNightIncrease;
				LootStarCloakIncrease = CC_LootStarCloakIncrease;
				LootStylishScissors = CC_LootStylishScissors;
				LootSunMaskIncrease = CC_LootSunMaskIncrease;
				LootTabiIncrease = CC_LootTabiIncrease;
				LootTacticalShotgunIncrease = CC_LootTacticalShotgunIncrease;
				LootTallyCounterIncrease = CC_LootTallyCounterIncrease;
				LootTatteredBeeWingIncrease = CC_LootTatteredBeeWingIncrease;
				LootTendonHookIncrease = CC_LootTendonHookIncrease;
				LootTitanGloveIncrease = CC_LootTitanGloveIncrease;
				LootToySledIncrease = CC_LootToySledIncrease;
				LootTrifoldMapIncrease = CC_LootTrifoldMapIncrease;
				LootTurtleShellIncrease = CC_LootTurtleShellIncrease;
				LootUmbrellaHatIncrease = CC_LootUmbrellaHatIncrease;
				LootUnicornonaStickIncrease = CC_LootUnicornonaStickIncrease;
				LootUziIncrease = CC_LootUziIncrease;
				LootVikingHelmetIncrease = CC_LootVikingHelmetIncrease;
				LootVitaminsIncrease = CC_LootVitaminsIncrease;
				LootWhoopieCushionIncrease = CC_LootWhoopieCushionIncrease;
				LootWispinaBottleIncrease = CC_LootWispinaBottleIncrease;
				LootWormHookIncrease = CC_LootWormHookIncrease;
				LootYeletsIncrease = CC_LootYeletsIncrease;
				LootZombieArmIncrease = CC_LootZombieArmIncrease;
				PirateLootCoinGunBaseIncrease = CC_PirateLootCoinGunBaseIncrease;
				PirateLootCutlassBaseIncrease = CC_PirateLootCutlassBaseIncrease;
				PirateLootDiscountCardBaseIncrease = CC_PirateLootDiscountCardBaseIncrease;
				PirateLootGoldRingBaseIncrease = CC_PirateLootGoldRingBaseIncrease;
				PirateLootLuckyCoinBaseIncrease = CC_PirateLootLuckyCoinBaseIncrease;
				PirateLootPirateStaffBaseIncrease = CC_PirateLootPirateStaffBaseIncrease;
				LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense = CC_LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense;
				SlimeStaffIncreaseToSurfaceSlimes = CC_SlimeStaffIncreaseToSurfaceSlimes;
				SlimeStaffIncreaseToUndergroundSlimes = CC_SlimeStaffIncreaseToUndergroundSlimes;
				SlimeStaffIncreaseToCavernSlimess = CC_SlimeStaffIncreaseToCavernSlimess;
				SlimeStaffIncreaseToIceSpikedSlimes = CC_SlimeStaffIncreaseToIceSpikedSlimes;
				SlimeStaffIncreaseToSpikedJungleSlimes = CC_SlimeStaffIncreaseToSpikedJungleSlimes;
						
				CavernModdedCavernLockBoxLoot = CC_CavernModdedCavernLockBoxLoot;
				DungeonModdedBiomeLockBoxLoot = CC_DungeonModdedBiomeLockBoxLoot;
				DungeonFurnitureLockBoxLoot = CC_DungeonFurnitureLockBoxLoot;
				HardmodeModdedLockBoxDropRateModifier = CC_HardmodeModdedLockBoxDropRateModifier;
				HellBiomeModdedShadowLockBoxLoot = CC_HellBiomeModdedShadowLockBoxLoot;
				JungleTempleLihzahrd_Lock_Box = CC_JungleTempleLihzahrd_Lock_Box;
				NormalmodeModdedLockBoxDropRateModifier = CC_NormalmodeModdedLockBoxDropRateModifier;
				SandstormAndUndergroundDesertPyramidLockBoxLoot = CC_SandstormAndUndergroundDesertPyramidLockBoxLoot;
				SkyModdedSkywareLockBoxLoot = CC_SkyModdedSkywareLockBoxLoot;
				SpiderNestWebCoveredLockBoxLoot = CC_SpiderNestWebCoveredLockBoxLoot;
				SurfaceModdedLivingWoodLockBoxLoot = CC_SurfaceModdedLivingWoodLockBoxLoot;
				UndergroundJungleBiomeModdedIvyLockBoxLoot = CC_UndergroundJungleBiomeModdedIvyLockBoxLoot;
				UndergroundSnowBiomeModdedIceLockBoxLoot = CC_UndergroundSnowBiomeModdedIceLockBoxLoot;
				WaterEnemyModdedWaterLockBoxLoot = CC_WaterEnemyModdedWaterLockBoxLoot;
						
				TravelingMerchantAcornsIncrease = CC_TravelingMerchantAcornsIncrease;
				TravelingMerchantAmmoBoxIncrease = CC_TravelingMerchantAmmoBoxIncrease;
				TravelingMerchantAngelHaloIncrease = CC_TravelingMerchantAngelHaloIncrease;
				TravelingMerchantArcaneRuneWallIncrease = CC_TravelingMerchantArcaneRuneWallIncrease;
				TravelingMerchantBlackCounterweightIncrease = CC_TravelingMerchantBlackCounterweightIncrease;
				TravelingMerchantBlueDynastyShinglesIncrease = CC_TravelingMerchantBlueDynastyShinglesIncrease;
				TravelingMerchantBlueTeamBlockIncrease = CC_TravelingMerchantBlueTeamBlockIncrease;
				TravelingMerchantBlueTeamPlatformIncrease = CC_TravelingMerchantBlueTeamPlatformIncrease;
				TravelingMerchantBrickLayerIncrease = CC_TravelingMerchantBrickLayerIncrease;
				TravelingMerchantCastleMarsbergIncrease = CC_TravelingMerchantCastleMarsbergIncrease;
				TravelingMerchantCelestialMagnetIncrease = CC_TravelingMerchantCelestialMagnetIncrease;
				TravelingMerchantChaliceIncrease = CC_TravelingMerchantChaliceIncrease;
				TravelingMerchantCode1Increase = CC_TravelingMerchantCode1Increase;
				TravelingMerchantCode2Increase = CC_TravelingMerchantCode2Increase;
				TravelingMerchantColdSnapIncrease = CC_TravelingMerchantColdSnapIncrease;
				TravelingMerchantCompanionCubeIncrease = CC_TravelingMerchantCompanionCubeIncrease;
				TravelingMerchantCrimsonCapeIncrease = CC_TravelingMerchantCrimsonCapeIncrease;
				TravelingMerchantCursedSaintIncrease = CC_TravelingMerchantCursedSaintIncrease;
				TravelingMerchantDPSMeterIncrease = CC_TravelingMerchantDPSMeterIncrease;
				TravelingMerchantDiamondRingIncrease = CC_TravelingMerchantDiamondRingIncrease;
				TravelingMerchantDynastyWoodIncrease = CC_TravelingMerchantDynastyWoodIncrease;
				TravelingMerchantExtendoGripIncrease = CC_TravelingMerchantExtendoGripIncrease;
				TravelingMerchantFancyDishesIncrease = CC_TravelingMerchantFancyDishesIncrease;
				TravelingMerchantFezIncrease = CC_TravelingMerchantFezIncrease;
				TravelingMerchantGatligatorIncrease = CC_TravelingMerchantGatligatorIncrease;
				TravelingMerchantGiIncrease = CC_TravelingMerchantGiIncrease;
				TravelingMerchantGreenTeamBlockIncrease = CC_TravelingMerchantGreenTeamBlockIncrease;
				TravelingMerchantGreenTeamPlatformIncrease = CC_TravelingMerchantGreenTeamPlatformIncrease;
				TravelingMerchantGypsyRobeIncrease = CC_TravelingMerchantGypsyRobeIncrease;
				TravelingMerchantKatanaIncrease = CC_TravelingMerchantKatanaIncrease;
				TravelingMerchantKimonoIncrease = CC_TravelingMerchantKimonoIncrease;
				TravelingMerchantLeopardSkinIncrease = CC_TravelingMerchantLeopardSkinIncrease;
				TravelingMerchantLifeformAnalyzerIncrease = CC_TravelingMerchantLifeformAnalyzerIncrease;
				TravelingMerchantMagicHatIncrease = CC_TravelingMerchantMagicHatIncrease;
				TravelingMerchantMartiaLisaIncrease = CC_TravelingMerchantMartiaLisaIncrease;
				TravelingMerchantMetalDetector = CC_TravelingMerchantMetalDetector;
				TravelingMerchantMysteriousCapeIncrease = CC_TravelingMerchantMysteriousCapeIncrease;
				TravelingMerchantNotAKidNorASquidIncrease = CC_TravelingMerchantNotAKidNorASquidIncrease;
				TravelingMerchantPadThaiIncrease = CC_TravelingMerchantPadThaiIncrease;
				TravelingMerchantPaintSprayerIncrease = CC_TravelingMerchantPaintSprayerIncrease;
				TravelingMerchantPhoIncrease = CC_TravelingMerchantPhoIncrease;
				TravelingMerchantPinkTeamBlockIncrease = CC_TravelingMerchantPinkTeamBlockIncrease;
				TravelingMerchantPinkTeamPlatformIncrease = CC_TravelingMerchantPinkTeamPlatformIncrease;
				TravelingMerchantPortableCementMixerIncrease = CC_TravelingMerchantPortableCementMixerIncrease;
				TravelingMerchantPresseratorIncrease = CC_TravelingMerchantPresseratorIncrease;
				TravelingMerchantPulseBowIncrease = CC_TravelingMerchantPulseBowIncrease;
				TravelingMerchantRedCapeIncrease = CC_TravelingMerchantRedCapeIncrease;
				TravelingMerchantRedDynastyShinglesIncrease = CC_TravelingMerchantRedDynastyShinglesIncrease;
				TravelingMerchantRedTeamBlockIncrease = CC_TravelingMerchantRedTeamBlockIncrease;
				TravelingMerchantRedTeamPlatformIncrease = CC_TravelingMerchantRedTeamPlatformIncrease;
				TravelingMerchantRevolverIncrease = CC_TravelingMerchantRevolverIncrease;
				TravelingMerchantSakeIncrease = CC_TravelingMerchantSakeIncrease;
				TravelingMerchantSittingDucksFishingPoleIncrease = CC_TravelingMerchantSittingDucksFishingPoleIncrease;
				TravelingMerchantSnowfellasIncrease = CC_TravelingMerchantSnowfellasIncrease;
				TravelingMerchantStopwatchIncrease = CC_TravelingMerchantStopwatchIncrease;
				TravelingMerchantTheSeasonIncrease = CC_TravelingMerchantTheSeasonIncrease;
				TravelingMerchantTheTruthIsUpThereIncrease = CC_TravelingMerchantTheTruthIsUpThereIncrease;
				TravelingMerchantTigerSkinIncrease = CC_TravelingMerchantTigerSkinIncrease;
				TravelingMerchantUltraBrightTorchIncrease = CC_TravelingMerchantUltraBrightTorchIncrease;
				TravelingMerchantWaterGunIncrease = CC_TravelingMerchantWaterGunIncrease;
				TravelingMerchantWhiteTeamBlockIncrease = CC_TravelingMerchantWhiteTeamBlockIncrease;
				TravelingMerchantWhiteTeamPlatformIncrease = CC_TravelingMerchantWhiteTeamPlatformIncrease;
				TravelingMerchantWinterCapeIncrease = CC_TravelingMerchantWinterCapeIncrease;
				TravelingMerchantYellowCounterweightIncrease = CC_TravelingMerchantYellowCounterweightIncrease;
				TravelingMerchantYellowTeamBlockIncrease = CC_TravelingMerchantYellowTeamBlockIncrease;
				TravelingMerchantYellowTeamPlatformIncrease = CC_TravelingMerchantYellowTeamPlatformIncrease;
				TravelingMerchantZebraSkinIncrease = CC_TravelingMerchantZebraSkinIncrease;
				TravelingMerchantAlwaysXMasForConfigurations = CC_TravelingMerchantAlwaysXMasForConfigurations;
				ChanceThatEnemyKillWillResetTravelingMerchant = CC_ChanceThatEnemyKillWillResetTravelingMerchant;
				StationaryMerchant = CC_StationaryMerchant;
				StationaryMerchantStockingChance = CC_StationaryMerchantStockingChance;
				S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate = CC_S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate;
						
				QuestAnglerEarringIncrease = CC_QuestAnglerEarringIncrease;
				QuestAnglerHatIncrease = CC_QuestAnglerHatIncrease;
				QuestAnglerPantsIncrease = CC_QuestAnglerPantsIncrease;
				QuestAnglerVestIncrease = CC_QuestAnglerVestIncrease;
				QuestCoralstoneBlockIncrease = CC_QuestCoralstoneBlockIncrease;
				QuestDecorativeFurnitureIncrease = CC_QuestDecorativeFurnitureIncrease;
				QuestFishCostumeIncrease = CC_QuestFishCostumeIncrease;
				QuestFishHookIncrease = CC_QuestFishHookIncrease;
				QuestFishermansGuideIncrease = CC_QuestFishermansGuideIncrease;
				QuestGoldenBugNetIncrease = CC_QuestGoldenBugNetIncrease;
				QuestGoldenFishingRodIncrease = CC_QuestGoldenFishingRodIncrease;
				QuestHardcoreBottomlessBucketIncrease = CC_QuestHardcoreBottomlessBucketIncrease;
				QuestHardcoreFinWingsIncrease = CC_QuestHardcoreFinWingsIncrease;
				QuestHardcoreHotlineFishingHookIncrease = CC_QuestHardcoreHotlineFishingHookIncrease;
				QuestHardcoreSuperAbsorbantSpongeIncrease = CC_QuestHardcoreSuperAbsorbantSpongeIncrease;
				QuestHighTestFishingLineIncrease = CC_QuestHighTestFishingLineIncrease;
				QuestMermaidCostumeIncrease = CC_QuestMermaidCostumeIncrease;
				QuestSextantIncrease = CC_QuestSextantIncrease;
				QuestTackleBoxIncrease = CC_QuestTackleBoxIncrease;
				QuestTrophyIncrease = CC_QuestTrophyIncrease;
				QuestWeatherRadioIncrease = CC_QuestWeatherRadioIncrease;
						
				ChestSalesmanPreHardmodeChestsRequireHardmodeActivated = CC_ChestSalesmanPreHardmodeChestsRequireHardmodeActivated;
				ChestSalesmanSellsBiomeChests = CC_ChestSalesmanSellsBiomeChests;
				ChestSalesmanSellsGoldChest = CC_ChestSalesmanSellsGoldChest;
				ChestSalesmanSellsIceChest = CC_ChestSalesmanSellsIceChest;
				ChestSalesmanSellsIvyChest = CC_ChestSalesmanSellsIvyChest;
				ChestSalesmanSellsLihzahrdChest = CC_ChestSalesmanSellsLihzahrdChest;
				ChestSalesmanSellsLivingWoodChest = CC_ChestSalesmanSellsLivingWoodChest;
				ChestSalesmanSellsOceanChest = CC_ChestSalesmanSellsOceanChest;
				ChestSalesmanSellsShadowChest = CC_ChestSalesmanSellsShadowChest;
				ChestSalesmanSellsSkywareChest = CC_ChestSalesmanSellsSkywareChest;
				ChestSalesmanSellsWebCoveredChest = CC_ChestSalesmanSellsWebCoveredChest;
				ChestSalesmanSpawnable = CC_ChestSalesmanSpawnable;
						
				MechanicSellsDartTrapAfterSkeletronDefeated = CC_MechanicSellsDartTrapAfterSkeletronDefeated;
				MechanicSellsGeyserAfterWallofFleshDefeated = CC_MechanicSellsGeyserAfterWallofFleshDefeated;
				MechanicSellsLihzahrdTrapsAfterGolemDefeated = CC_MechanicSellsLihzahrdTrapsAfterGolemDefeated;
				MechanicSellsWoodenSpikesAfterGolemDefeated = CC_MechanicSellsWoodenSpikesAfterGolemDefeated;
				MerchantSellsAllMiningGear = CC_MerchantSellsAllMiningGear;
				MerchantSellsBlizzardInABottleWhenInSnow = CC_MerchantSellsBlizzardInABottleWhenInSnow;
				MerchantSellsCloudInABottleWhenInSky = CC_MerchantSellsCloudInABottleWhenInSky;
				MerchantSellsFishItem = CC_MerchantSellsFishItem;
				MerchantSellsPyramidItems = CC_MerchantSellsPyramidItems;
				MerchantSellsSandstormInABottleWhenInDesert = CC_MerchantSellsSandstormInABottleWhenInDesert;
				WitchDoctorSellsFlowerBoots = CC_WitchDoctorSellsFlowerBoots;
				WitchDoctorSellsHoneyDispenser = CC_WitchDoctorSellsHoneyDispenser;
				WitchDoctorSellsSeaweed = CC_WitchDoctorSellsSeaweed;
				WitchDoctorSellsStaffofRegrowth = CC_WitchDoctorSellsStaffofRegrowth;
				TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight = CC_TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight;
						
				GoblinTinkererSellsGoblinRetreatOrder = CC_GoblinTinkererSellsGoblinRetreatOrder;
				PirateSellsPirateRetreatOrder = CC_PirateSellsPirateRetreatOrder;
				WizardSellsMoonBall = CC_WizardSellsMoonBall;
				BattlePotionMaxSpawnsMultiplier = CC_BattlePotionMaxSpawnsMultiplier;
				BattlePotionSpawnrateMultiplier = CC_BattlePotionSpawnrateMultiplier;
				BloodZombieAndDripplerDropsBloodMoonMedallion = CC_BloodZombieAndDripplerDropsBloodMoonMedallion;
				ChaosPotionMaxSpawnsMultiplier = CC_ChaosPotionMaxSpawnsMultiplier;
				ChaosPotionSpawnrateMultiplier = CC_ChaosPotionSpawnrateMultiplier;
				WarPotionMaxSpawnsMultiplier = CC_WarPotionMaxSpawnsMultiplier;
				WarPotionSpawnrateMultiplier = CC_WarPotionSpawnrateMultiplier;
						
				NewCharacterBarrels = CC_NewCharacterBarrels;
				NewCharacterCopperBars = CC_NewCharacterCopperBars;
				NewCharacterCopperCoins = CC_NewCharacterCopperCoins;
				NewCharacterGoldBars = CC_NewCharacterGoldBars;
				NewCharacterGoldCoins = CC_NewCharacterGoldCoins;
				NewCharacterIronBars = CC_NewCharacterIronBars;
				NewCharacterMiningPotions = CC_NewCharacterMiningPotions;
				NewCharacterPlatinumCoins = CC_NewCharacterPlatinumCoins;
				NewCharacterSilverBars = CC_NewCharacterSilverBars;
				NewCharacterSilverCoins = CC_NewCharacterSilverCoins;
				NewCharacterWarPotions = CC_NewCharacterWarPotions;
						
				ExtractinatorGivesAmber = CC_ExtractinatorGivesAmber;
				ExtractinatorGivesAmberMosquito = CC_ExtractinatorGivesAmberMosquito;
				ExtractinatorGivesAmethyst = CC_ExtractinatorGivesAmethyst;
				ExtractinatorGivesCopperCoin = CC_ExtractinatorGivesCopperCoin;
				ExtractinatorGivesCopperOre = CC_ExtractinatorGivesCopperOre;
				ExtractinatorGivesDiamond = CC_ExtractinatorGivesDiamond;
				ExtractinatorGivesEmerald = CC_ExtractinatorGivesEmerald;
				ExtractinatorGivesFossilOre = CC_ExtractinatorGivesFossilOre;
				ExtractinatorGivesGoldCoin = CC_ExtractinatorGivesGoldCoin;
				ExtractinatorGivesGoldOre = CC_ExtractinatorGivesGoldOre;
				ExtractinatorGivesIronOre = CC_ExtractinatorGivesIronOre;
				ExtractinatorGivesLeadOre = CC_ExtractinatorGivesLeadOre;
				ExtractinatorGivesPlatinumCoin = CC_ExtractinatorGivesPlatinumCoin;
				ExtractinatorGivesPlatinumOre = CC_ExtractinatorGivesPlatinumOre;
				ExtractinatorGivesRuby = CC_ExtractinatorGivesRuby;
				ExtractinatorGivesSapphire = CC_ExtractinatorGivesSapphire;
				ExtractinatorGivesSilverCoin = CC_ExtractinatorGivesSilverCoin;
				ExtractinatorGivesSilverOre = CC_ExtractinatorGivesSilverOre;
				ExtractinatorGivesTinOre = CC_ExtractinatorGivesTinOre;
				ExtractinatorGivesTopaz = CC_ExtractinatorGivesTopaz;
				ExtractinatorGivesTungstenOre = CC_ExtractinatorGivesTungstenOre;
						
				CrateUpgradesDependingOnFishingPower = CC_CrateUpgradesDependingOnFishingPower;
				FishCatchBecomesBalloonPufferfish = CC_FishCatchBecomesBalloonPufferfish;
				FishCatchBecomesBladetongue = CC_FishCatchBecomesBladetongue;
				FishCatchBecomesBlueJellyfish = CC_FishCatchBecomesBlueJellyfish;
				FishCatchBecomesChaosFish = CC_FishCatchBecomesChaosFish;
				FishCatchBecomesCorruptCrate = CC_FishCatchBecomesCorruptCrate;
				FishCatchBecomesCrimsonCrate = CC_FishCatchBecomesCrimsonCrate;
				FishCatchBecomesCrystalSerpent = CC_FishCatchBecomesCrystalSerpent;
				FishCatchBecomesDungeonCrate = CC_FishCatchBecomesDungeonCrate;
				FishCatchBecomesFrogLeg = CC_FishCatchBecomesFrogLeg;
				FishCatchBecomesFrostDaggerfish = CC_FishCatchBecomesFrostDaggerfish;
				FishCatchBecomesGoldenCarp = CC_FishCatchBecomesGoldenCarp;
				FishCatchBecomesGoldenCrate = CC_FishCatchBecomesGoldenCrate;
				FishCatchBecomesGreenJellyfish = CC_FishCatchBecomesGreenJellyfish;
				FishCatchBecomesHallowedCrate = CC_FishCatchBecomesHallowedCrate;
				FishCatchBecomesIronCrate = CC_FishCatchBecomesIronCrate;
				FishCatchBecomesJungleCrate = CC_FishCatchBecomesJungleCrate;
				FishCatchBecomesPinkJellyfish = CC_FishCatchBecomesPinkJellyfish;
				FishCatchBecomesPurpleClubberfish = CC_FishCatchBecomesPurpleClubberfish;
				FishCatchBecomesReaverSharkAfterAllMechBossesDowned = CC_FishCatchBecomesReaverSharkAfterAllMechBossesDowned;
				FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned = CC_FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
				FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned = CC_FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
				FishCatchBecomesRockfish = CC_FishCatchBecomesRockfish;
				FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned = CC_FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned;
				FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned = CC_FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
				FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned = CC_FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned;
				FishCatchBecomesScalyTruffle = CC_FishCatchBecomesScalyTruffle;
				FishCatchBecomesSkyCrate = CC_FishCatchBecomesSkyCrate;
				FishCatchBecomesSwordfish = CC_FishCatchBecomesSwordfish;
				FishCatchBecomesToxikarp = CC_FishCatchBecomesToxikarp;
				FishCatchBecomesWoodenCrate = CC_FishCatchBecomesWoodenCrate;
				FishCatchBecomesZephyrFish = CC_FishCatchBecomesZephyrFish;
            }
        }

        /*public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ReducedGrindingPlayer clone = clientPlayer as ReducedGrindingPlayer;
            if (clone.currentlyActive != currentlyActive)
            {
                SendClientChangesPacket();
            }
        }*/

        private void SendClientChangesPacket()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)ReducedGrindingMessageType.SendClientChanges);
                packet.Write((byte)player.whoAmI);
                //BitsByte flags = new BitsByte();
                //flags[0] = currentlyActive;
                //packet.Write((byte)flags);
                packet.Send();
            }
        }
		
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            //server sends its config to player
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)ReducedGrindingMessageType.SyncPlayer);
            packet.Write((byte)player.whoAmI);
			
            packet.Write((int)Config.DropTriesForAllEnemyDroppedLoot);
			packet.Write((float)Config.NormalModeLootMultiplierForLootWithSeperateDifficultyRates);
			
			packet.Write((float)Config.CrateDungeonBoneWelder);
			packet.Write((float)Config.CrateEnchantedSundialGoldenIncrease);
			packet.Write((float)Config.CrateEnchantedSundialIronIncrease);
			packet.Write((float)Config.CrateEnchantedSundialWoodenIncrease);
			packet.Write((float)Config.CrateJungleAnkeltOfTheWindIncrease);
			packet.Write((float)Config.CrateJungleFeralClawsIncrease);
			packet.Write((float)Config.CrateJungleFlowerBoots);
			packet.Write((float)Config.CrateJungleLeafWand);
			packet.Write((float)Config.CrateJungleLivingLoom);
			packet.Write((float)Config.CrateJungleLivingMahoganyWand);
			packet.Write((float)Config.CrateJungleLivingWoodWand);
			packet.Write((float)Config.CrateJungleRichMahoganyLeafWand);
			packet.Write((float)Config.CrateJungleSeaweed);
			packet.Write((float)Config.CrateJungleStaffOfRegrowth);
			packet.Write((float)Config.CrateSkySkyMill);
			packet.Write((float)Config.CrateFlippersGolden);
			packet.Write((float)Config.CrateFlippersIron);
			packet.Write((float)Config.CrateFlippersWooden);
			packet.Write((float)Config.CrateWaterWalkingBootsGolden);
			packet.Write((float)Config.CrateWaterWalkingBootsIron);
			packet.Write((float)Config.CrateWaterWalkingBootsWooden);
			packet.Write((float)Config.CrateWoodenAgletIncrease);
			packet.Write((float)Config.CrateWoodenClimbingClawsIncrease);
			packet.Write((float)Config.CrateWoodenRadarIncrease);
			packet.Write((float)Config.PresentCandyCaneBlock);
			packet.Write((float)Config.PresentCandyCaneHook);
			packet.Write((float)Config.PresentCandyCanePickaxe);
			packet.Write((float)Config.PresentCandyCaneSword);
			packet.Write((float)Config.PresentChristmasPudding);
			packet.Write((float)Config.PresentCoal);
			packet.Write((float)Config.PresentDogWhistle);
			packet.Write((float)Config.PresentEggnog);
			packet.Write((float)Config.PresentFruitcakeChakram);
			packet.Write((float)Config.PresentGingerbreadCookie);
			packet.Write((float)Config.PresentGreenCandyCaneBlock);
			packet.Write((float)Config.PresentHandWarmer);
			packet.Write((float)Config.PresentHardmodeSnowGlobe);
			packet.Write((float)Config.PresentHolly);
			packet.Write((float)Config.PresentMrsClausCostume);
			packet.Write((float)Config.PresentParkaOutfit);
			packet.Write((float)Config.PresentPineTreeBlock);
			packet.Write((float)Config.PresentRedRyderPlusMusketBall);
			packet.Write((float)Config.PresentReindeerAntlers);
			packet.Write((float)Config.PresentSnowHat);
			packet.Write((float)Config.PresentStarAnise);
			packet.Write((float)Config.PresentSugarCookie);
			packet.Write((float)Config.PresentToolbox);
			packet.Write((float)Config.PresentTreeCostume);
			packet.Write((float)Config.PresentUglySweater);
			
			packet.Write((float)Config.LootBookofSkullsIncrease);
			packet.Write((float)Config.LootPicksawIncrease);
			packet.Write((float)Config.LootSeedlingIncrease);
			packet.Write((float)Config.LootSkeletronBoneKey);
			packet.Write((float)Config.LootBinocularsIncrease);
			packet.Write((float)Config.LootBoneRattleIncrease);
			packet.Write((float)Config.LootBossMaskIncrease);
			packet.Write((float)Config.LootBossTrophyIncrease);
			packet.Write((float)Config.LootEatersBoneIncrease);
			packet.Write((float)Config.LootFishronTruffleworm);
			packet.Write((float)Config.LootFishronWingsIncrease);
			packet.Write((float)Config.LootHoneyedGogglesIncrease);
			packet.Write((float)Config.LootNectarIncrease);
			packet.Write((float)Config.LootTheAxeIncrease);
			
			packet.Write((float)Config.BiomeKeyIncreaseForOneMechBossDown);
			packet.Write((float)Config.BiomeKeyIncreaseForTwoMechBossDown);
			packet.Write((float)Config.BiomeKeyIncreaseForThreeMechBossDown);
			
			packet.Write((float)Config.AllEnemiesLootBiomeMatchingFoundOnlyChestDrop);
			packet.Write((float)Config.HellBatLootMagmaStoneIncrease);
			packet.Write((float)Config.LavaBatLootMagmaStoneIncrease);
			packet.Write((float)Config.LootAdhesiveBandageIncrease);
			packet.Write((float)Config.LootAleTosser);
			packet.Write((float)Config.LootAmarokIncrease);
			packet.Write((float)Config.LootAncientClothIncrease);
			packet.Write((float)Config.LootAncientCobaltBreastplateIncrease);
			packet.Write((float)Config.LootAncientCobaltHelmetIncrease);
			packet.Write((float)Config.LootAncientCobaltLeggingsIncrease);
			packet.Write((float)Config.LootAncientGoldHelmetIncrease);
			packet.Write((float)Config.LootAncientHornIncrease);
			packet.Write((float)Config.LootAncientIronHelmetIncrease);
			packet.Write((float)Config.LootAncientNecroHelmetIncrease);
			packet.Write((float)Config.LootAncientShadowGreavesIncrease);
			packet.Write((float)Config.LootAncientShadowHelmetIncrease);
			packet.Write((float)Config.LootAncientShadowScalemailIncrease);
			packet.Write((float)Config.LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory);
			packet.Write((float)Config.LootArmorPolishIncrease);
			packet.Write((float)Config.LootBabyGrinchsMischiefWhistleIncrease);
			packet.Write((float)Config.LootBananarangIncrease);
			packet.Write((float)Config.LootBeamSwordIncrease);
			packet.Write((float)Config.LootBezoarIncrease);
			packet.Write((float)Config.LootBlackBeltIncrease);
			packet.Write((float)Config.LootBlackLensIncrease);
			packet.Write((float)Config.LootBlessedAppleIncrease);
			packet.Write((float)Config.LootBlindfoldIncrease);
			packet.Write((float)Config.LootBloodyMacheteAndBladedGlovesIncrease);
			packet.Write((float)Config.LootBoneFeatherIncrease);
			packet.Write((float)Config.LootBonePickaxeIncrease);
			packet.Write((float)Config.LootBoneSwordIncrease);
			packet.Write((float)Config.LootBoneWandIncrease);
			packet.Write((float)Config.LootBrainScramblerIncrease);
			packet.Write((float)Config.LootBrokenBatWingIncrease);
			packet.Write((float)Config.LootBunnyHoodIncrease);
			packet.Write((float)Config.LootCascadeIncrease);
			packet.Write((float)Config.LootChainGuillotinesIncrease);
			packet.Write((float)Config.LootChainKnifeIncrease);
			packet.Write((float)Config.LootClassyCane);
			packet.Write((float)Config.LootClingerStaffIncrease);
			packet.Write((float)Config.LootClothierVoodooDollIncrease);
			packet.Write((float)Config.LootCompassIncrease);
			packet.Write((float)Config.LootCrossNecklaceIncrease);
			packet.Write((float)Config.LootCrystalVileShardIncrease);
			packet.Write((float)Config.LootDaedalusStormbowIncrease);
			packet.Write((float)Config.LootDarkShardIncrease);
			packet.Write((float)Config.LootDartPistolIncrease);
			packet.Write((float)Config.LootDartRifleIncrease);
			packet.Write((float)Config.LootDeathSickleIncrease);
			packet.Write((float)Config.LootDemonScytheIncrease);
			packet.Write((float)Config.LootDepthMeterIncrease);
			packet.Write((float)Config.LootDesertSpiritLampIncrease);
			packet.Write((float)Config.LootDivingHelmetIncrease);
			packet.Write((float)Config.LootDualHookIncrease);
			packet.Write((float)Config.LootElfHatIncrease);
			packet.Write((float)Config.LootElfPantsIncrease);
			packet.Write((float)Config.LootElfShirtIncrease);
			packet.Write((float)Config.LootEskimoCoatIncrease);
			packet.Write((float)Config.LootEskimoHoodIncrease);
			packet.Write((float)Config.LootEskimoPantsIncrease);
			packet.Write((float)Config.LootExoticScimitar);
			packet.Write((float)Config.LootEyePatchIncrease);
			packet.Write((float)Config.LootEyeSpringIncrease);
			packet.Write((float)Config.LootFastClockBaseIncrease);
			packet.Write((float)Config.LootFestiveWingsIncrease);
			packet.Write((float)Config.LootFetidBaghnakhsIncrease);
			packet.Write((float)Config.LootFireFeatherIncrease);
			packet.Write((float)Config.LootFleshKnucklesIncrease);
			packet.Write((float)Config.LootFlyingKnifeIncrease);
			packet.Write((float)Config.LootFrostStaffIncrease);
			packet.Write((float)Config.LootFrozenTurtleShellIncrease);
			packet.Write((float)Config.LootGiantBowIncrease);
			packet.Write((float)Config.LootGiantHarpyFeatherIncrease);
			packet.Write((float)Config.LootGoldenFurnitureIncrease);
			packet.Write((float)Config.LootGoldenKeyIncrease);
			packet.Write((float)Config.LootGoodieBagIncrease);
			packet.Write((float)Config.LootGreenCap);
			packet.Write((float)Config.LootHappyGrenade);
			packet.Write((float)Config.LootHarpoonIncrease);
			packet.Write((float)Config.LootHelFireIncrease);
			packet.Write((float)Config.LootHookIncrease);
			packet.Write((float)Config.LootIceSickleIncrease);
			packet.Write((float)Config.LootIlluminantHookIncrease);
			packet.Write((float)Config.LootJellyfishNecklaceIncrease);
			packet.Write((float)Config.LootKOCannonIncrease);
			packet.Write((float)Config.LootKeybrandIncrease);
			packet.Write((float)Config.LootKrakenIncrease);
			packet.Write((float)Config.LootLamiaClothesIncrease);
			packet.Write((float)Config.LootLifeDrainIncrease);
			packet.Write((float)Config.LootLightShardIncrease);
			packet.Write((float)Config.LootLihzahrdPowerCellIncrease);
			packet.Write((float)Config.LootLivingFireBlockIncrease);
			packet.Write((float)Config.LootLizardEggIncrease);
			packet.Write((float)Config.LootMagicDaggerIncrease);
			packet.Write((float)Config.LootMagicQuiverIncrease);
			packet.Write((float)Config.LootMagnetSphereIncrease);
			packet.Write((float)Config.LootMandibleBladeIncrease);
			packet.Write((float)Config.LootMarrowIncrease);
			packet.Write((float)Config.LootMeatGrinderIncrease);
			packet.Write((float)Config.LootMegaphoneBaseIncrease);
			packet.Write((float)Config.LootMeteoriteIncrease);
			packet.Write((float)Config.LootMiningHelmetIncrease);
			packet.Write((float)Config.LootMiningPantsIncrease);
			packet.Write((float)Config.LootMiningShirtIncrease);
			packet.Write((float)Config.LootMoneyTroughIncrease);
			packet.Write((float)Config.LootMoonCharmIncrease);
			packet.Write((float)Config.LootMoonMaskIncrease);
			packet.Write((float)Config.LootMoonStoneIncrease);
			packet.Write((float)Config.LootMothronWingsIncrease);
			packet.Write((float)Config.LootMummyCostumeIncrease);
			packet.Write((float)Config.LootNazarIncrease);
			packet.Write((float)Config.LootNimbusRodIncrease);
			packet.Write((float)Config.LootObsidianRoseIncrease);
			packet.Write((float)Config.LootPaintballGun);
			packet.Write((float)Config.LootPaladinsShieldIncrease);
			packet.Write((float)Config.LootPedguinssuitIncrease);
			packet.Write((float)Config.LootPhilosophersStoneIncrease);
			packet.Write((float)Config.LootPirateMapIncrease);
			packet.Write((float)Config.LootPlumbersHatIncrease);
			packet.Write((float)Config.LootPocketMirrorIncrease);
			packet.Write((float)Config.LootPresentIncrease);
			packet.Write((float)Config.LootPsychoKnifeIncrease);
			packet.Write((float)Config.LootPutridScentIncrease);
			packet.Write((float)Config.LootRainArmorIncrease);
			packet.Write((int)Config.LootRainbowBlockDropMaxIncrease);
			packet.Write((int)Config.LootRainbowBlockDropMinIncrease);
			packet.Write((float)Config.LootRallyIncrease);
			packet.Write((float)Config.LootReindeerBellsIncrease);
			packet.Write((float)Config.LootRifleScopeIncrease);
			packet.Write((float)Config.LootRobotHatIncrease);
			packet.Write((float)Config.LootRocketLauncherIncrease);
			packet.Write((float)Config.LootRodofDiscordIncrease);
			packet.Write((float)Config.LootSWATHelmetIncrease);
			packet.Write((float)Config.LootSailorHatIncrease);
			packet.Write((float)Config.LootSailorPantsIncrease);
			packet.Write((float)Config.LootSailorShirtIncrease);
			packet.Write((float)Config.LootShackleIncrease);
			packet.Write((float)Config.LootSkullIncrease);
			packet.Write((float)Config.LootSlimeStaffIncrease);
			packet.Write((float)Config.LootSniperRifleIncrease);
			packet.Write((float)Config.LootSnowballLauncherIncrease);
			packet.Write((float)Config.LootSoulofLightIncrease);
			packet.Write((float)Config.LootSoulofNightIncrease);
			packet.Write((float)Config.LootStarCloakIncrease);
			packet.Write((float)Config.LootStylishScissors);
			packet.Write((float)Config.LootSunMaskIncrease);
			packet.Write((float)Config.LootTabiIncrease);
			packet.Write((float)Config.LootTacticalShotgunIncrease);
			packet.Write((float)Config.LootTallyCounterIncrease);
			packet.Write((float)Config.LootTatteredBeeWingIncrease);
			packet.Write((float)Config.LootTendonHookIncrease);
			packet.Write((float)Config.LootTitanGloveIncrease);
			packet.Write((float)Config.LootToySledIncrease);
			packet.Write((float)Config.LootTrifoldMapIncrease);
			packet.Write((float)Config.LootTurtleShellIncrease);
			packet.Write((float)Config.LootUmbrellaHatIncrease);
			packet.Write((float)Config.LootUnicornonaStickIncrease);
			packet.Write((float)Config.LootUziIncrease);
			packet.Write((float)Config.LootVikingHelmetIncrease);
			packet.Write((float)Config.LootVitaminsIncrease);
			packet.Write((float)Config.LootWhoopieCushionIncrease);
			packet.Write((float)Config.LootWispinaBottleIncrease);
			packet.Write((float)Config.LootWormHookIncrease);
			packet.Write((float)Config.LootYeletsIncrease);
			packet.Write((float)Config.LootZombieArmIncrease);
			packet.Write((float)Config.PirateLootCoinGunBaseIncrease);
			packet.Write((float)Config.PirateLootCutlassBaseIncrease);
			packet.Write((float)Config.PirateLootDiscountCardBaseIncrease);
			packet.Write((float)Config.PirateLootGoldRingBaseIncrease);
			packet.Write((float)Config.PirateLootLuckyCoinBaseIncrease);
			packet.Write((float)Config.PirateLootPirateStaffBaseIncrease);
			packet.Write((bool)Config.LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense);
			packet.Write((bool)Config.SlimeStaffIncreaseToSurfaceSlimes);
			packet.Write((bool)Config.SlimeStaffIncreaseToUndergroundSlimes);
			packet.Write((bool)Config.SlimeStaffIncreaseToCavernSlimess);
			packet.Write((bool)Config.SlimeStaffIncreaseToIceSpikedSlimes);
			packet.Write((bool)Config.SlimeStaffIncreaseToSpikedJungleSlimes);

			packet.Write((float)Config.CavernModdedCavernLockBoxLoot);
			packet.Write((float)Config.DungeonModdedBiomeLockBoxLoot);
			packet.Write((float)Config.DungeonFurnitureLockBoxLoot);
			packet.Write((float)Config.HardmodeModdedLockBoxDropRateModifier);
			packet.Write((float)Config.HellBiomeModdedShadowLockBoxLoot);
			packet.Write((float)Config.JungleTempleLihzahrd_Lock_Box);
			packet.Write((float)Config.NormalmodeModdedLockBoxDropRateModifier);
			packet.Write((float)Config.SandstormAndUndergroundDesertPyramidLockBoxLoot);
			packet.Write((float)Config.SkyModdedSkywareLockBoxLoot);
			packet.Write((float)Config.SpiderNestWebCoveredLockBoxLoot);
			packet.Write((float)Config.SurfaceModdedLivingWoodLockBoxLoot);
			packet.Write((float)Config.UndergroundJungleBiomeModdedIvyLockBoxLoot);
			packet.Write((float)Config.UndergroundSnowBiomeModdedIceLockBoxLoot);
			packet.Write((float)Config.WaterEnemyModdedWaterLockBoxLoot);
			
			packet.Write((float)Config.TravelingMerchantAcornsIncrease);
			packet.Write((float)Config.TravelingMerchantAmmoBoxIncrease);
			packet.Write((float)Config.TravelingMerchantAngelHaloIncrease);
			packet.Write((float)Config.TravelingMerchantArcaneRuneWallIncrease);
			packet.Write((float)Config.TravelingMerchantBlackCounterweightIncrease);
			packet.Write((float)Config.TravelingMerchantBlueDynastyShinglesIncrease);
			packet.Write((float)Config.TravelingMerchantBlueTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantBlueTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantBrickLayerIncrease);
			packet.Write((float)Config.TravelingMerchantCastleMarsbergIncrease);
			packet.Write((float)Config.TravelingMerchantCelestialMagnetIncrease);
			packet.Write((float)Config.TravelingMerchantChaliceIncrease);
			packet.Write((float)Config.TravelingMerchantCode1Increase);
			packet.Write((float)Config.TravelingMerchantCode2Increase);
			packet.Write((float)Config.TravelingMerchantColdSnapIncrease);
			packet.Write((float)Config.TravelingMerchantCompanionCubeIncrease);
			packet.Write((float)Config.TravelingMerchantCrimsonCapeIncrease);
			packet.Write((float)Config.TravelingMerchantCursedSaintIncrease);
			packet.Write((float)Config.TravelingMerchantDPSMeterIncrease);
			packet.Write((float)Config.TravelingMerchantDiamondRingIncrease);
			packet.Write((float)Config.TravelingMerchantDynastyWoodIncrease);
			packet.Write((float)Config.TravelingMerchantExtendoGripIncrease);
			packet.Write((float)Config.TravelingMerchantFancyDishesIncrease);
			packet.Write((float)Config.TravelingMerchantFezIncrease);
			packet.Write((float)Config.TravelingMerchantGatligatorIncrease);
			packet.Write((float)Config.TravelingMerchantGiIncrease);
			packet.Write((float)Config.TravelingMerchantGreenTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantGreenTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantGypsyRobeIncrease);
			packet.Write((float)Config.TravelingMerchantKatanaIncrease);
			packet.Write((float)Config.TravelingMerchantKimonoIncrease);
			packet.Write((float)Config.TravelingMerchantLeopardSkinIncrease);
			packet.Write((float)Config.TravelingMerchantLifeformAnalyzerIncrease);
			packet.Write((float)Config.TravelingMerchantMagicHatIncrease);
			packet.Write((float)Config.TravelingMerchantMartiaLisaIncrease);
			packet.Write((float)Config.TravelingMerchantMetalDetector);
			packet.Write((float)Config.TravelingMerchantMysteriousCapeIncrease);
			packet.Write((float)Config.TravelingMerchantNotAKidNorASquidIncrease);
			packet.Write((float)Config.TravelingMerchantPadThaiIncrease);
			packet.Write((float)Config.TravelingMerchantPaintSprayerIncrease);
			packet.Write((float)Config.TravelingMerchantPhoIncrease);
			packet.Write((float)Config.TravelingMerchantPinkTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantPinkTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantPortableCementMixerIncrease);
			packet.Write((float)Config.TravelingMerchantPresseratorIncrease);
			packet.Write((float)Config.TravelingMerchantPulseBowIncrease);
			packet.Write((float)Config.TravelingMerchantRedCapeIncrease);
			packet.Write((float)Config.TravelingMerchantRedDynastyShinglesIncrease);
			packet.Write((float)Config.TravelingMerchantRedTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantRedTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantRevolverIncrease);
			packet.Write((float)Config.TravelingMerchantSakeIncrease);
			packet.Write((float)Config.TravelingMerchantSittingDucksFishingPoleIncrease);
			packet.Write((float)Config.TravelingMerchantSnowfellasIncrease);
			packet.Write((float)Config.TravelingMerchantStopwatchIncrease);
			packet.Write((float)Config.TravelingMerchantTheSeasonIncrease);
			packet.Write((float)Config.TravelingMerchantTheTruthIsUpThereIncrease);
			packet.Write((float)Config.TravelingMerchantTigerSkinIncrease);
			packet.Write((float)Config.TravelingMerchantUltraBrightTorchIncrease);
			packet.Write((float)Config.TravelingMerchantWaterGunIncrease);
			packet.Write((float)Config.TravelingMerchantWhiteTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantWhiteTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantWinterCapeIncrease);
			packet.Write((float)Config.TravelingMerchantYellowCounterweightIncrease);
			packet.Write((float)Config.TravelingMerchantYellowTeamBlockIncrease);
			packet.Write((float)Config.TravelingMerchantYellowTeamPlatformIncrease);
			packet.Write((float)Config.TravelingMerchantZebraSkinIncrease);
			packet.Write((bool)Config.TravelingMerchantAlwaysXMasForConfigurations);
			packet.Write((float)Config.ChanceThatEnemyKillWillResetTravelingMerchant);
			packet.Write((bool)Config.StationaryMerchant);
			packet.Write((float)Config.StationaryMerchantStockingChance);
			packet.Write((float)Config.S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate);
			
			packet.Write((float)Config.QuestAnglerEarringIncrease);
			packet.Write((float)Config.QuestAnglerHatIncrease);
			packet.Write((float)Config.QuestAnglerPantsIncrease);
			packet.Write((float)Config.QuestAnglerVestIncrease);
			packet.Write((float)Config.QuestCoralstoneBlockIncrease);
			packet.Write((float)Config.QuestDecorativeFurnitureIncrease);
			packet.Write((float)Config.QuestFishCostumeIncrease);
			packet.Write((float)Config.QuestFishHookIncrease);
			packet.Write((float)Config.QuestFishermansGuideIncrease);
			packet.Write((float)Config.QuestGoldenBugNetIncrease);
			packet.Write((float)Config.QuestGoldenFishingRodIncrease);
			packet.Write((float)Config.QuestHardcoreBottomlessBucketIncrease);
			packet.Write((float)Config.QuestHardcoreFinWingsIncrease);
			packet.Write((float)Config.QuestHardcoreHotlineFishingHookIncrease);
			packet.Write((float)Config.QuestHardcoreSuperAbsorbantSpongeIncrease);
			packet.Write((float)Config.QuestHighTestFishingLineIncrease);
			packet.Write((float)Config.QuestMermaidCostumeIncrease);
			packet.Write((float)Config.QuestSextantIncrease);
			packet.Write((float)Config.QuestTackleBoxIncrease);
			packet.Write((float)Config.QuestTrophyIncrease);
			packet.Write((float)Config.QuestWeatherRadioIncrease);
			
			packet.Write((bool)Config.ChestSalesmanPreHardmodeChestsRequireHardmodeActivated);
			packet.Write((bool)Config.ChestSalesmanSellsBiomeChests);
			packet.Write((bool)Config.ChestSalesmanSellsGoldChest);
			packet.Write((bool)Config.ChestSalesmanSellsIceChest);
			packet.Write((bool)Config.ChestSalesmanSellsIvyChest);
			packet.Write((bool)Config.ChestSalesmanSellsLihzahrdChest);
			packet.Write((bool)Config.ChestSalesmanSellsLivingWoodChest);
			packet.Write((bool)Config.ChestSalesmanSellsOceanChest);
			packet.Write((bool)Config.ChestSalesmanSellsShadowChest);
			packet.Write((bool)Config.ChestSalesmanSellsSkywareChest);
			packet.Write((bool)Config.ChestSalesmanSellsWebCoveredChest);
			packet.Write((bool)Config.ChestSalesmanSpawnable);
			
			packet.Write((bool)Config.MechanicSellsDartTrapAfterSkeletronDefeated);
			packet.Write((bool)Config.MechanicSellsGeyserAfterWallofFleshDefeated);
			packet.Write((bool)Config.MechanicSellsLihzahrdTrapsAfterGolemDefeated);
			packet.Write((bool)Config.MechanicSellsWoodenSpikesAfterGolemDefeated);
			packet.Write((bool)Config.MerchantSellsAllMiningGear);
			packet.Write((bool)Config.MerchantSellsBlizzardInABottleWhenInSnow);
			packet.Write((bool)Config.MerchantSellsCloudInABottleWhenInSky);
			packet.Write((bool)Config.MerchantSellsFishItem);
			packet.Write((bool)Config.MerchantSellsPyramidItems);
			packet.Write((bool)Config.MerchantSellsSandstormInABottleWhenInDesert);
			packet.Write((bool)Config.WitchDoctorSellsFlowerBoots);
			packet.Write((bool)Config.WitchDoctorSellsHoneyDispenser);
			packet.Write((bool)Config.WitchDoctorSellsSeaweed);
			packet.Write((bool)Config.WitchDoctorSellsStaffofRegrowth);
			packet.Write((int)Config.TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight);
			
			packet.Write((bool)Config.GoblinTinkererSellsGoblinRetreatOrder);
			packet.Write((bool)Config.PirateSellsPirateRetreatOrder);
			packet.Write((bool)Config.WizardSellsMoonBall);
			packet.Write((float)Config.BattlePotionMaxSpawnsMultiplier);
			packet.Write((float)Config.BattlePotionSpawnrateMultiplier);
			packet.Write((float)Config.BloodZombieAndDripplerDropsBloodMoonMedallion);
			packet.Write((float)Config.ChaosPotionMaxSpawnsMultiplier);
			packet.Write((float)Config.ChaosPotionSpawnrateMultiplier);
			packet.Write((float)Config.WarPotionMaxSpawnsMultiplier);
			packet.Write((float)Config.WarPotionSpawnrateMultiplier);
			
			packet.Write((int)Config.NewCharacterBarrels);
			packet.Write((int)Config.NewCharacterCopperBars);
			packet.Write((int)Config.NewCharacterCopperCoins);
			packet.Write((int)Config.NewCharacterGoldBars);
			packet.Write((int)Config.NewCharacterGoldCoins);
			packet.Write((int)Config.NewCharacterIronBars);
			packet.Write((int)Config.NewCharacterMiningPotions);
			packet.Write((int)Config.NewCharacterPlatinumCoins);
			packet.Write((int)Config.NewCharacterSilverBars);
			packet.Write((int)Config.NewCharacterSilverCoins);
			packet.Write((int)Config.NewCharacterWarPotions);

			packet.Write((float)Config.ExtractinatorGivesAmber);
			packet.Write((float)Config.ExtractinatorGivesAmberMosquito);
			packet.Write((float)Config.ExtractinatorGivesAmethyst);
			packet.Write((float)Config.ExtractinatorGivesCopperCoin);
			packet.Write((float)Config.ExtractinatorGivesCopperOre);
			packet.Write((float)Config.ExtractinatorGivesDiamond);
			packet.Write((float)Config.ExtractinatorGivesEmerald);
			packet.Write((float)Config.ExtractinatorGivesFossilOre);
			packet.Write((float)Config.ExtractinatorGivesGoldCoin);
			packet.Write((float)Config.ExtractinatorGivesGoldOre);
			packet.Write((float)Config.ExtractinatorGivesIronOre);
			packet.Write((float)Config.ExtractinatorGivesLeadOre);
			packet.Write((float)Config.ExtractinatorGivesPlatinumCoin);
			packet.Write((float)Config.ExtractinatorGivesPlatinumOre);
			packet.Write((float)Config.ExtractinatorGivesRuby);
			packet.Write((float)Config.ExtractinatorGivesSapphire);
			packet.Write((float)Config.ExtractinatorGivesSilverCoin);
			packet.Write((float)Config.ExtractinatorGivesSilverOre);
			packet.Write((float)Config.ExtractinatorGivesTinOre);
			packet.Write((float)Config.ExtractinatorGivesTopaz);
			packet.Write((float)Config.ExtractinatorGivesTungstenOre);
			
			packet.Write((bool)Config.CrateUpgradesDependingOnFishingPower);
			packet.Write((float)Config.FishCatchBecomesBalloonPufferfish);
			packet.Write((float)Config.FishCatchBecomesBladetongue);
			packet.Write((float)Config.FishCatchBecomesBlueJellyfish);
			packet.Write((float)Config.FishCatchBecomesChaosFish);
			packet.Write((float)Config.FishCatchBecomesCorruptCrate);
			packet.Write((float)Config.FishCatchBecomesCrimsonCrate);
			packet.Write((float)Config.FishCatchBecomesCrystalSerpent);
			packet.Write((float)Config.FishCatchBecomesDungeonCrate);
			packet.Write((float)Config.FishCatchBecomesFrogLeg);
			packet.Write((float)Config.FishCatchBecomesFrostDaggerfish);
			packet.Write((float)Config.FishCatchBecomesGoldenCarp);
			packet.Write((float)Config.FishCatchBecomesGoldenCrate);
			packet.Write((float)Config.FishCatchBecomesGreenJellyfish);
			packet.Write((float)Config.FishCatchBecomesHallowedCrate);
			packet.Write((float)Config.FishCatchBecomesIronCrate);
			packet.Write((float)Config.FishCatchBecomesJungleCrate);
			packet.Write((float)Config.FishCatchBecomesPinkJellyfish);
			packet.Write((float)Config.FishCatchBecomesPurpleClubberfish);
			packet.Write((float)Config.FishCatchBecomesReaverSharkAfterAllMechBossesDowned);
			packet.Write((float)Config.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned);
			packet.Write((float)Config.FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned);
			packet.Write((float)Config.FishCatchBecomesRockfish);
			packet.Write((float)Config.FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned);
			packet.Write((float)Config.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned);
			packet.Write((float)Config.FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned);
			packet.Write((float)Config.FishCatchBecomesScalyTruffle);
			packet.Write((float)Config.FishCatchBecomesSkyCrate);
			packet.Write((float)Config.FishCatchBecomesSwordfish);
			packet.Write((float)Config.FishCatchBecomesToxikarp);
			packet.Write((float)Config.FishCatchBecomesWoodenCrate);
			packet.Write((float)Config.FishCatchBecomesZephyrFish);
			
            player.GetModPlayer<ReducedGrindingPlayer>().clientConf = new ClientConf(
				Config.DropTriesForAllEnemyDroppedLoot,
				Config.NormalModeLootMultiplierForLootWithSeperateDifficultyRates,
				
				Config.CrateDungeonBoneWelder,
				Config.CrateEnchantedSundialGoldenIncrease,
				Config.CrateEnchantedSundialIronIncrease,
				Config.CrateEnchantedSundialWoodenIncrease,
				Config.CrateJungleAnkeltOfTheWindIncrease,
				Config.CrateJungleFeralClawsIncrease,
				Config.CrateJungleFlowerBoots,
				Config.CrateJungleLeafWand,
				Config.CrateJungleLivingLoom,
				Config.CrateJungleLivingMahoganyWand,
				Config.CrateJungleLivingWoodWand,
				Config.CrateJungleRichMahoganyLeafWand,
				Config.CrateJungleSeaweed,
				Config.CrateJungleStaffOfRegrowth,
				Config.CrateSkySkyMill,
				Config.CrateFlippersGolden,
				Config.CrateFlippersIron,
				Config.CrateFlippersWooden,
				Config.CrateWaterWalkingBootsGolden,
				Config.CrateWaterWalkingBootsIron,
				Config.CrateWaterWalkingBootsWooden,
				Config.CrateWoodenAgletIncrease,
				Config.CrateWoodenClimbingClawsIncrease,
				Config.CrateWoodenRadarIncrease,
				Config.PresentCandyCaneBlock,
				Config.PresentCandyCaneHook,
				Config.PresentCandyCanePickaxe,
				Config.PresentCandyCaneSword,
				Config.PresentChristmasPudding,
				Config.PresentCoal,
				Config.PresentDogWhistle,
				Config.PresentEggnog,
				Config.PresentFruitcakeChakram,
				Config.PresentGingerbreadCookie,
				Config.PresentGreenCandyCaneBlock,
				Config.PresentHandWarmer,
				Config.PresentHardmodeSnowGlobe,
				Config.PresentHolly,
				Config.PresentMrsClausCostume,
				Config.PresentParkaOutfit,
				Config.PresentPineTreeBlock,
				Config.PresentRedRyderPlusMusketBall,
				Config.PresentReindeerAntlers,
				Config.PresentSnowHat,
				Config.PresentStarAnise,
				Config.PresentSugarCookie,
				Config.PresentToolbox ,
				Config.PresentTreeCostume,
				Config.PresentUglySweater,
				
				Config.LootBookofSkullsIncrease,
				Config.LootPicksawIncrease,
				Config.LootSeedlingIncrease,
				Config.LootSkeletronBoneKey,
				Config.LootBinocularsIncrease,
				Config.LootBoneRattleIncrease,
				Config.LootBossMaskIncrease,
				Config.LootBossTrophyIncrease,
				Config.LootEatersBoneIncrease,
				Config.LootFishronTruffleworm,
				Config.LootFishronWingsIncrease,
				Config.LootHoneyedGogglesIncrease,
				Config.LootNectarIncrease,
				Config.LootTheAxeIncrease,
				
				Config.BiomeKeyIncreaseForOneMechBossDown,
				Config.BiomeKeyIncreaseForTwoMechBossDown,
				Config.BiomeKeyIncreaseForThreeMechBossDown,
				
				Config.AllEnemiesLootBiomeMatchingFoundOnlyChestDrop,
				Config.HellBatLootMagmaStoneIncrease,
				Config.LavaBatLootMagmaStoneIncrease,
				Config.LootAdhesiveBandageIncrease,
				Config.LootAleTosser,
				Config.LootAmarokIncrease,
				Config.LootAncientClothIncrease,
				Config.LootAncientCobaltBreastplateIncrease,
				Config.LootAncientCobaltHelmetIncrease,
				Config.LootAncientCobaltLeggingsIncrease,
				Config.LootAncientGoldHelmetIncrease,
				Config.LootAncientHornIncrease,
				Config.LootAncientIronHelmetIncrease,
				Config.LootAncientNecroHelmetIncrease,
				Config.LootAncientShadowGreavesIncrease,
				Config.LootAncientShadowHelmetIncrease,
				Config.LootAncientShadowScalemailIncrease,
				Config.LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory,
				Config.LootArmorPolishIncrease,
				Config.LootBabyGrinchsMischiefWhistleIncrease,
				Config.LootBananarangIncrease,
				Config.LootBeamSwordIncrease,
				Config.LootBezoarIncrease,
				Config.LootBlackBeltIncrease,
				Config.LootBlackLensIncrease,
				Config.LootBlessedAppleIncrease,
				Config.LootBlindfoldIncrease,
				Config.LootBloodyMacheteAndBladedGlovesIncrease,
				Config.LootBoneFeatherIncrease,
				Config.LootBonePickaxeIncrease,
				Config.LootBoneSwordIncrease,
				Config.LootBoneWandIncrease,
				Config.LootBrainScramblerIncrease,
				Config.LootBrokenBatWingIncrease,
				Config.LootBunnyHoodIncrease,
				Config.LootCascadeIncrease,
				Config.LootChainGuillotinesIncrease,
				Config.LootChainKnifeIncrease,
				Config.LootClassyCane,
				Config.LootClingerStaffIncrease,
				Config.LootClothierVoodooDollIncrease,
				Config.LootCompassIncrease,
				Config.LootCrossNecklaceIncrease,
				Config.LootCrystalVileShardIncrease,
				Config.LootDaedalusStormbowIncrease,
				Config.LootDarkShardIncrease,
				Config.LootDartPistolIncrease,
				Config.LootDartRifleIncrease,
				Config.LootDeathSickleIncrease,
				Config.LootDemonScytheIncrease,
				Config.LootDepthMeterIncrease,
				Config.LootDesertSpiritLampIncrease,
				Config.LootDivingHelmetIncrease,
				Config.LootDualHookIncrease,
				Config.LootElfHatIncrease,
				Config.LootElfPantsIncrease,
				Config.LootElfShirtIncrease,
				Config.LootEskimoCoatIncrease,
				Config.LootEskimoHoodIncrease,
				Config.LootEskimoPantsIncrease,
				Config.LootExoticScimitar,
				Config.LootEyePatchIncrease,
				Config.LootEyeSpringIncrease,
				Config.LootFastClockBaseIncrease,
				Config.LootFestiveWingsIncrease,
				Config.LootFetidBaghnakhsIncrease,
				Config.LootFireFeatherIncrease,
				Config.LootFleshKnucklesIncrease,
				Config.LootFlyingKnifeIncrease,
				Config.LootFrostStaffIncrease,
				Config.LootFrozenTurtleShellIncrease,
				Config.LootGiantBowIncrease,
				Config.LootGiantHarpyFeatherIncrease,
				Config.LootGoldenFurnitureIncrease,
				Config.LootGoldenKeyIncrease,
				Config.LootGoodieBagIncrease,
				Config.LootGreenCap,
				Config.LootHappyGrenade,
				Config.LootHarpoonIncrease,
				Config.LootHelFireIncrease,
				Config.LootHookIncrease,
				Config.LootIceSickleIncrease,
				Config.LootIlluminantHookIncrease,
				Config.LootJellyfishNecklaceIncrease,
				Config.LootKOCannonIncrease,
				Config.LootKeybrandIncrease,
				Config.LootKrakenIncrease,
				Config.LootLamiaClothesIncrease,
				Config.LootLifeDrainIncrease,
				Config.LootLightShardIncrease,
				Config.LootLihzahrdPowerCellIncrease,
				Config.LootLivingFireBlockIncrease,
				Config.LootLizardEggIncrease,
				Config.LootMagicDaggerIncrease,
				Config.LootMagicQuiverIncrease,
				Config.LootMagnetSphereIncrease,
				Config.LootMandibleBladeIncrease,
				Config.LootMarrowIncrease,
				Config.LootMeatGrinderIncrease,
				Config.LootMegaphoneBaseIncrease,
				Config.LootMeteoriteIncrease,
				Config.LootMiningHelmetIncrease,
				Config.LootMiningPantsIncrease,
				Config.LootMiningShirtIncrease,
				Config.LootMoneyTroughIncrease,
				Config.LootMoonCharmIncrease,
				Config.LootMoonMaskIncrease,
				Config.LootMoonStoneIncrease,
				Config.LootMothronWingsIncrease,
				Config.LootMummyCostumeIncrease,
				Config.LootNazarIncrease,
				Config.LootNimbusRodIncrease,
				Config.LootObsidianRoseIncrease,
				Config.LootPaintballGun,
				Config.LootPaladinsShieldIncrease,
				Config.LootPedguinssuitIncrease,
				Config.LootPhilosophersStoneIncrease,
				Config.LootPirateMapIncrease,
				Config.LootPlumbersHatIncrease,
				Config.LootPocketMirrorIncrease,
				Config.LootPresentIncrease,
				Config.LootPsychoKnifeIncrease,
				Config.LootPutridScentIncrease,
				Config.LootRainArmorIncrease,
				Config.LootRainbowBlockDropMaxIncrease,
				Config.LootRainbowBlockDropMinIncrease,
				Config.LootRallyIncrease,
				Config.LootReindeerBellsIncrease,
				Config.LootRifleScopeIncrease,
				Config.LootRobotHatIncrease,
				Config.LootRocketLauncherIncrease,
				Config.LootRodofDiscordIncrease,
				Config.LootSWATHelmetIncrease,
				Config.LootSailorHatIncrease,
				Config.LootSailorPantsIncrease,
				Config.LootSailorShirtIncrease,
				Config.LootShackleIncrease,
				Config.LootSkullIncrease,
				Config.LootSlimeStaffIncrease,
				Config.LootSniperRifleIncrease,
				Config.LootSnowballLauncherIncrease,
				Config.LootSoulofLightIncrease,
				Config.LootSoulofNightIncrease,
				Config.LootStarCloakIncrease,
				Config.LootStylishScissors,
				Config.LootSunMaskIncrease,
				Config.LootTabiIncrease,
				Config.LootTacticalShotgunIncrease,
				Config.LootTallyCounterIncrease,
				Config.LootTatteredBeeWingIncrease,
				Config.LootTendonHookIncrease,
				Config.LootTitanGloveIncrease,
				Config.LootToySledIncrease,
				Config.LootTrifoldMapIncrease,
				Config.LootTurtleShellIncrease,
				Config.LootUmbrellaHatIncrease,
				Config.LootUnicornonaStickIncrease,
				Config.LootUziIncrease,
				Config.LootVikingHelmetIncrease,
				Config.LootVitaminsIncrease,
				Config.LootWhoopieCushionIncrease,
				Config.LootWispinaBottleIncrease,
				Config.LootWormHookIncrease,
				Config.LootYeletsIncrease,
				Config.LootZombieArmIncrease,
				Config.PirateLootCoinGunBaseIncrease,
				Config.PirateLootCutlassBaseIncrease,
				Config.PirateLootDiscountCardBaseIncrease,
				Config.PirateLootGoldRingBaseIncrease,
				Config.PirateLootLuckyCoinBaseIncrease,
				Config.PirateLootPirateStaffBaseIncrease,
				Config.LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense,
				Config.SlimeStaffIncreaseToSurfaceSlimes,
				Config.SlimeStaffIncreaseToUndergroundSlimes,
				Config.SlimeStaffIncreaseToCavernSlimess,
				Config.SlimeStaffIncreaseToIceSpikedSlimes,
				Config.SlimeStaffIncreaseToSpikedJungleSlimes,

				Config.CavernModdedCavernLockBoxLoot,
				Config.DungeonModdedBiomeLockBoxLoot,
				Config.DungeonFurnitureLockBoxLoot,
				Config.HardmodeModdedLockBoxDropRateModifier,
				Config.HellBiomeModdedShadowLockBoxLoot,
				Config.JungleTempleLihzahrd_Lock_Box,
				Config.NormalmodeModdedLockBoxDropRateModifier,
				Config.SandstormAndUndergroundDesertPyramidLockBoxLoot,
				Config.SkyModdedSkywareLockBoxLoot,
				Config.SpiderNestWebCoveredLockBoxLoot,
				Config.SurfaceModdedLivingWoodLockBoxLoot,
				Config.UndergroundJungleBiomeModdedIvyLockBoxLoot,
				Config.UndergroundSnowBiomeModdedIceLockBoxLoot,
				Config.WaterEnemyModdedWaterLockBoxLoot,
				
				Config.TravelingMerchantAcornsIncrease,
				Config.TravelingMerchantAmmoBoxIncrease,
				Config.TravelingMerchantAngelHaloIncrease,
				Config.TravelingMerchantArcaneRuneWallIncrease,
				Config.TravelingMerchantBlackCounterweightIncrease,
				Config.TravelingMerchantBlueDynastyShinglesIncrease,
				Config.TravelingMerchantBlueTeamBlockIncrease,
				Config.TravelingMerchantBlueTeamPlatformIncrease,
				Config.TravelingMerchantBrickLayerIncrease,
				Config.TravelingMerchantCastleMarsbergIncrease,
				Config.TravelingMerchantCelestialMagnetIncrease,
				Config.TravelingMerchantChaliceIncrease,
				Config.TravelingMerchantCode1Increase,
				Config.TravelingMerchantCode2Increase,
				Config.TravelingMerchantColdSnapIncrease,
				Config.TravelingMerchantCompanionCubeIncrease,
				Config.TravelingMerchantCrimsonCapeIncrease,
				Config.TravelingMerchantCursedSaintIncrease,
				Config.TravelingMerchantDPSMeterIncrease,
				Config.TravelingMerchantDiamondRingIncrease,
				Config.TravelingMerchantDynastyWoodIncrease,
				Config.TravelingMerchantExtendoGripIncrease,
				Config.TravelingMerchantFancyDishesIncrease,
				Config.TravelingMerchantFezIncrease,
				Config.TravelingMerchantGatligatorIncrease,
				Config.TravelingMerchantGiIncrease,
				Config.TravelingMerchantGreenTeamBlockIncrease,
				Config.TravelingMerchantGreenTeamPlatformIncrease,
				Config.TravelingMerchantGypsyRobeIncrease,
				Config.TravelingMerchantKatanaIncrease,
				Config.TravelingMerchantKimonoIncrease,
				Config.TravelingMerchantLeopardSkinIncrease,
				Config.TravelingMerchantLifeformAnalyzerIncrease,
				Config.TravelingMerchantMagicHatIncrease,
				Config.TravelingMerchantMartiaLisaIncrease,
				Config.TravelingMerchantMetalDetector,
				Config.TravelingMerchantMysteriousCapeIncrease,
				Config.TravelingMerchantNotAKidNorASquidIncrease,
				Config.TravelingMerchantPadThaiIncrease,
				Config.TravelingMerchantPaintSprayerIncrease,
				Config.TravelingMerchantPhoIncrease,
				Config.TravelingMerchantPinkTeamBlockIncrease,
				Config.TravelingMerchantPinkTeamPlatformIncrease,
				Config.TravelingMerchantPortableCementMixerIncrease,
				Config.TravelingMerchantPresseratorIncrease,
				Config.TravelingMerchantPulseBowIncrease,
				Config.TravelingMerchantRedCapeIncrease,
				Config.TravelingMerchantRedDynastyShinglesIncrease,
				Config.TravelingMerchantRedTeamBlockIncrease,
				Config.TravelingMerchantRedTeamPlatformIncrease,
				Config.TravelingMerchantRevolverIncrease,
				Config.TravelingMerchantSakeIncrease,
				Config.TravelingMerchantSittingDucksFishingPoleIncrease,
				Config.TravelingMerchantSnowfellasIncrease,
				Config.TravelingMerchantStopwatchIncrease,
				Config.TravelingMerchantTheSeasonIncrease,
				Config.TravelingMerchantTheTruthIsUpThereIncrease,
				Config.TravelingMerchantTigerSkinIncrease,
				Config.TravelingMerchantUltraBrightTorchIncrease,
				Config.TravelingMerchantWaterGunIncrease,
				Config.TravelingMerchantWhiteTeamBlockIncrease,
				Config.TravelingMerchantWhiteTeamPlatformIncrease,
				Config.TravelingMerchantWinterCapeIncrease,
				Config.TravelingMerchantYellowCounterweightIncrease,
				Config.TravelingMerchantYellowTeamBlockIncrease,
				Config.TravelingMerchantYellowTeamPlatformIncrease,
				Config.TravelingMerchantZebraSkinIncrease,
				Config.TravelingMerchantAlwaysXMasForConfigurations,
				Config.ChanceThatEnemyKillWillResetTravelingMerchant,
				Config.StationaryMerchant,
				Config.StationaryMerchantStockingChance,
				Config.S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate,
				
				Config.QuestAnglerEarringIncrease,
				Config.QuestAnglerHatIncrease,
				Config.QuestAnglerPantsIncrease,
				Config.QuestAnglerVestIncrease,
				Config.QuestCoralstoneBlockIncrease,
				Config.QuestDecorativeFurnitureIncrease,
				Config.QuestFishCostumeIncrease,
				Config.QuestFishHookIncrease,
				Config.QuestFishermansGuideIncrease,
				Config.QuestGoldenBugNetIncrease,
				Config.QuestGoldenFishingRodIncrease,
				Config.QuestHardcoreBottomlessBucketIncrease,
				Config.QuestHardcoreFinWingsIncrease,
				Config.QuestHardcoreHotlineFishingHookIncrease,
				Config.QuestHardcoreSuperAbsorbantSpongeIncrease,
				Config.QuestHighTestFishingLineIncrease,
				Config.QuestMermaidCostumeIncrease,
				Config.QuestSextantIncrease,
				Config.QuestTackleBoxIncrease,
				Config.QuestTrophyIncrease,
				Config.QuestWeatherRadioIncrease,
				
				Config.ChestSalesmanPreHardmodeChestsRequireHardmodeActivated,
				Config.ChestSalesmanSellsBiomeChests,
				Config.ChestSalesmanSellsGoldChest,
				Config.ChestSalesmanSellsIceChest,
				Config.ChestSalesmanSellsIvyChest,
				Config.ChestSalesmanSellsLihzahrdChest,
				Config.ChestSalesmanSellsLivingWoodChest,
				Config.ChestSalesmanSellsOceanChest,
				Config.ChestSalesmanSellsShadowChest,
				Config.ChestSalesmanSellsSkywareChest,
				Config.ChestSalesmanSellsWebCoveredChest,
				Config.ChestSalesmanSpawnable,
				
				Config.MechanicSellsDartTrapAfterSkeletronDefeated,
				Config.MechanicSellsGeyserAfterWallofFleshDefeated,
				Config.MechanicSellsLihzahrdTrapsAfterGolemDefeated,
				Config.MechanicSellsWoodenSpikesAfterGolemDefeated,
				Config.MerchantSellsAllMiningGear,
				Config.MerchantSellsBlizzardInABottleWhenInSnow,
				Config.MerchantSellsCloudInABottleWhenInSky,
				Config.MerchantSellsFishItem,
				Config.MerchantSellsPyramidItems,
				Config.MerchantSellsSandstormInABottleWhenInDesert,
				Config.WitchDoctorSellsFlowerBoots,
				Config.WitchDoctorSellsHoneyDispenser,
				Config.WitchDoctorSellsSeaweed,
				Config.WitchDoctorSellsStaffofRegrowth,
				Config.TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight,
				
				Config.GoblinTinkererSellsGoblinRetreatOrder,
				Config.PirateSellsPirateRetreatOrder,
				Config.WizardSellsMoonBall,
				Config.BattlePotionMaxSpawnsMultiplier,
				Config.BattlePotionSpawnrateMultiplier,
				Config.BloodZombieAndDripplerDropsBloodMoonMedallion,
				Config.ChaosPotionMaxSpawnsMultiplier,
				Config.ChaosPotionSpawnrateMultiplier,
				Config.WarPotionMaxSpawnsMultiplier,
				Config.WarPotionSpawnrateMultiplier,
				
				Config.NewCharacterBarrels,
				Config.NewCharacterCopperBars,
				Config.NewCharacterCopperCoins,
				Config.NewCharacterGoldBars,
				Config.NewCharacterGoldCoins,
				Config.NewCharacterIronBars,
				Config.NewCharacterMiningPotions,
				Config.NewCharacterPlatinumCoins,
				Config.NewCharacterSilverBars,
				Config.NewCharacterSilverCoins,
				Config.NewCharacterWarPotions,

				Config.ExtractinatorGivesAmber,
				Config.ExtractinatorGivesAmberMosquito,
				Config.ExtractinatorGivesAmethyst,
				Config.ExtractinatorGivesCopperCoin,
				Config.ExtractinatorGivesCopperOre,
				Config.ExtractinatorGivesDiamond,
				Config.ExtractinatorGivesEmerald,
				Config.ExtractinatorGivesFossilOre,
				Config.ExtractinatorGivesGoldCoin,
				Config.ExtractinatorGivesGoldOre,
				Config.ExtractinatorGivesIronOre,
				Config.ExtractinatorGivesLeadOre,
				Config.ExtractinatorGivesPlatinumCoin,
				Config.ExtractinatorGivesPlatinumOre,
				Config.ExtractinatorGivesRuby,
				Config.ExtractinatorGivesSapphire,
				Config.ExtractinatorGivesSilverCoin,
				Config.ExtractinatorGivesSilverOre,
				Config.ExtractinatorGivesTinOre,
				Config.ExtractinatorGivesTopaz,
				Config.ExtractinatorGivesTungstenOre,
				
				Config.CrateUpgradesDependingOnFishingPower,
				Config.FishCatchBecomesBalloonPufferfish,
				Config.FishCatchBecomesBladetongue,
				Config.FishCatchBecomesBlueJellyfish,
				Config.FishCatchBecomesChaosFish,
				Config.FishCatchBecomesCorruptCrate,
				Config.FishCatchBecomesCrimsonCrate,
				Config.FishCatchBecomesCrystalSerpent,
				Config.FishCatchBecomesDungeonCrate,
				Config.FishCatchBecomesFrogLeg,
				Config.FishCatchBecomesFrostDaggerfish,
				Config.FishCatchBecomesGoldenCarp,
				Config.FishCatchBecomesGoldenCrate,
				Config.FishCatchBecomesGreenJellyfish,
				Config.FishCatchBecomesHallowedCrate,
				Config.FishCatchBecomesIronCrate,
				Config.FishCatchBecomesJungleCrate,
				Config.FishCatchBecomesPinkJellyfish,
				Config.FishCatchBecomesPurpleClubberfish,
				Config.FishCatchBecomesReaverSharkAfterAllMechBossesDowned,
				Config.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				Config.FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				Config.FishCatchBecomesRockfish,
				Config.FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned,
				Config.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				Config.FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
				Config.FishCatchBecomesScalyTruffle,
				Config.FishCatchBecomesSkyCrate,
				Config.FishCatchBecomesSwordfish,
				Config.FishCatchBecomesToxikarp,
				Config.FishCatchBecomesWoodenCrate,
				Config.FishCatchBecomesZephyrFish
			);
			
            packet.Send(toWho/*, fromWho*/);
        }
		
        public override void OnEnterWorld(Player player)
        {
			if (Main.netMode == NetmodeID.SinglePlayer)
            {
                clientConf = new ClientConf(
					Config.DropTriesForAllEnemyDroppedLoot,
					Config.NormalModeLootMultiplierForLootWithSeperateDifficultyRates,
					
					Config.CrateDungeonBoneWelder,
					Config.CrateEnchantedSundialGoldenIncrease,
					Config.CrateEnchantedSundialIronIncrease,
					Config.CrateEnchantedSundialWoodenIncrease,
					Config.CrateJungleAnkeltOfTheWindIncrease,
					Config.CrateJungleFeralClawsIncrease,
					Config.CrateJungleFlowerBoots,
					Config.CrateJungleLeafWand,
					Config.CrateJungleLivingLoom,
					Config.CrateJungleLivingMahoganyWand,
					Config.CrateJungleLivingWoodWand,
					Config.CrateJungleRichMahoganyLeafWand,
					Config.CrateJungleSeaweed,
					Config.CrateJungleStaffOfRegrowth,
					Config.CrateSkySkyMill,
					Config.CrateFlippersGolden,
					Config.CrateFlippersIron,
					Config.CrateFlippersWooden,
					Config.CrateWaterWalkingBootsGolden,
					Config.CrateWaterWalkingBootsIron,
					Config.CrateWaterWalkingBootsWooden,
					Config.CrateWoodenAgletIncrease,
					Config.CrateWoodenClimbingClawsIncrease,
					Config.CrateWoodenRadarIncrease,
					Config.PresentCandyCaneBlock,
					Config.PresentCandyCaneHook,
					Config.PresentCandyCanePickaxe,
					Config.PresentCandyCaneSword,
					Config.PresentChristmasPudding,
					Config.PresentCoal,
					Config.PresentDogWhistle,
					Config.PresentEggnog,
					Config.PresentFruitcakeChakram,
					Config.PresentGingerbreadCookie,
					Config.PresentGreenCandyCaneBlock,
					Config.PresentHandWarmer,
					Config.PresentHardmodeSnowGlobe,
					Config.PresentHolly,
					Config.PresentMrsClausCostume,
					Config.PresentParkaOutfit,
					Config.PresentPineTreeBlock,
					Config.PresentRedRyderPlusMusketBall,
					Config.PresentReindeerAntlers,
					Config.PresentSnowHat,
					Config.PresentStarAnise,
					Config.PresentSugarCookie,
					Config.PresentToolbox ,
					Config.PresentTreeCostume,
					Config.PresentUglySweater,
					
					Config.LootBookofSkullsIncrease,
					Config.LootPicksawIncrease,
					Config.LootSeedlingIncrease,
					Config.LootSkeletronBoneKey,
					Config.LootBinocularsIncrease,
					Config.LootBoneRattleIncrease,
					Config.LootBossMaskIncrease,
					Config.LootBossTrophyIncrease,
					Config.LootEatersBoneIncrease,
					Config.LootFishronTruffleworm,
					Config.LootFishronWingsIncrease,
					Config.LootHoneyedGogglesIncrease,
					Config.LootNectarIncrease,
					Config.LootTheAxeIncrease,
					
					Config.BiomeKeyIncreaseForOneMechBossDown,
					Config.BiomeKeyIncreaseForTwoMechBossDown,
					Config.BiomeKeyIncreaseForThreeMechBossDown,
					
					Config.AllEnemiesLootBiomeMatchingFoundOnlyChestDrop,
					Config.HellBatLootMagmaStoneIncrease,
					Config.LavaBatLootMagmaStoneIncrease,
					Config.LootAdhesiveBandageIncrease,
					Config.LootAleTosser,
					Config.LootAmarokIncrease,
					Config.LootAncientClothIncrease,
					Config.LootAncientCobaltBreastplateIncrease,
					Config.LootAncientCobaltHelmetIncrease,
					Config.LootAncientCobaltLeggingsIncrease,
					Config.LootAncientGoldHelmetIncrease,
					Config.LootAncientHornIncrease,
					Config.LootAncientIronHelmetIncrease,
					Config.LootAncientNecroHelmetIncrease,
					Config.LootAncientShadowGreavesIncrease,
					Config.LootAncientShadowHelmetIncrease,
					Config.LootAncientShadowScalemailIncrease,
					Config.LootAnkhCharmMaterialIncreasePerAnkhCharmInInventory,
					Config.LootArmorPolishIncrease,
					Config.LootBabyGrinchsMischiefWhistleIncrease,
					Config.LootBananarangIncrease,
					Config.LootBeamSwordIncrease,
					Config.LootBezoarIncrease,
					Config.LootBlackBeltIncrease,
					Config.LootBlackLensIncrease,
					Config.LootBlessedAppleIncrease,
					Config.LootBlindfoldIncrease,
					Config.LootBloodyMacheteAndBladedGlovesIncrease,
					Config.LootBoneFeatherIncrease,
					Config.LootBonePickaxeIncrease,
					Config.LootBoneSwordIncrease,
					Config.LootBoneWandIncrease,
					Config.LootBrainScramblerIncrease,
					Config.LootBrokenBatWingIncrease,
					Config.LootBunnyHoodIncrease,
					Config.LootCascadeIncrease,
					Config.LootChainGuillotinesIncrease,
					Config.LootChainKnifeIncrease,
					Config.LootClassyCane,
					Config.LootClingerStaffIncrease,
					Config.LootClothierVoodooDollIncrease,
					Config.LootCompassIncrease,
					Config.LootCrossNecklaceIncrease,
					Config.LootCrystalVileShardIncrease,
					Config.LootDaedalusStormbowIncrease,
					Config.LootDarkShardIncrease,
					Config.LootDartPistolIncrease,
					Config.LootDartRifleIncrease,
					Config.LootDeathSickleIncrease,
					Config.LootDemonScytheIncrease,
					Config.LootDepthMeterIncrease,
					Config.LootDesertSpiritLampIncrease,
					Config.LootDivingHelmetIncrease,
					Config.LootDualHookIncrease,
					Config.LootElfHatIncrease,
					Config.LootElfPantsIncrease,
					Config.LootElfShirtIncrease,
					Config.LootEskimoCoatIncrease,
					Config.LootEskimoHoodIncrease,
					Config.LootEskimoPantsIncrease,
					Config.LootExoticScimitar,
					Config.LootEyePatchIncrease,
					Config.LootEyeSpringIncrease,
					Config.LootFastClockBaseIncrease,
					Config.LootFestiveWingsIncrease,
					Config.LootFetidBaghnakhsIncrease,
					Config.LootFireFeatherIncrease,
					Config.LootFleshKnucklesIncrease,
					Config.LootFlyingKnifeIncrease,
					Config.LootFrostStaffIncrease,
					Config.LootFrozenTurtleShellIncrease,
					Config.LootGiantBowIncrease,
					Config.LootGiantHarpyFeatherIncrease,
					Config.LootGoldenFurnitureIncrease,
					Config.LootGoldenKeyIncrease,
					Config.LootGoodieBagIncrease,
					Config.LootGreenCap,
					Config.LootHappyGrenade,
					Config.LootHarpoonIncrease,
					Config.LootHelFireIncrease,
					Config.LootHookIncrease,
					Config.LootIceSickleIncrease,
					Config.LootIlluminantHookIncrease,
					Config.LootJellyfishNecklaceIncrease,
					Config.LootKOCannonIncrease,
					Config.LootKeybrandIncrease,
					Config.LootKrakenIncrease,
					Config.LootLamiaClothesIncrease,
					Config.LootLifeDrainIncrease,
					Config.LootLightShardIncrease,
					Config.LootLihzahrdPowerCellIncrease,
					Config.LootLivingFireBlockIncrease,
					Config.LootLizardEggIncrease,
					Config.LootMagicDaggerIncrease,
					Config.LootMagicQuiverIncrease,
					Config.LootMagnetSphereIncrease,
					Config.LootMandibleBladeIncrease,
					Config.LootMarrowIncrease,
					Config.LootMeatGrinderIncrease,
					Config.LootMegaphoneBaseIncrease,
					Config.LootMeteoriteIncrease,
					Config.LootMiningHelmetIncrease,
					Config.LootMiningPantsIncrease,
					Config.LootMiningShirtIncrease,
					Config.LootMoneyTroughIncrease,
					Config.LootMoonCharmIncrease,
					Config.LootMoonMaskIncrease,
					Config.LootMoonStoneIncrease,
					Config.LootMothronWingsIncrease,
					Config.LootMummyCostumeIncrease,
					Config.LootNazarIncrease,
					Config.LootNimbusRodIncrease,
					Config.LootObsidianRoseIncrease,
					Config.LootPaintballGun,
					Config.LootPaladinsShieldIncrease,
					Config.LootPedguinssuitIncrease,
					Config.LootPhilosophersStoneIncrease,
					Config.LootPirateMapIncrease,
					Config.LootPlumbersHatIncrease,
					Config.LootPocketMirrorIncrease,
					Config.LootPresentIncrease,
					Config.LootPsychoKnifeIncrease,
					Config.LootPutridScentIncrease,
					Config.LootRainArmorIncrease,
					Config.LootRainbowBlockDropMaxIncrease,
					Config.LootRainbowBlockDropMinIncrease,
					Config.LootRallyIncrease,
					Config.LootReindeerBellsIncrease,
					Config.LootRifleScopeIncrease,
					Config.LootRobotHatIncrease,
					Config.LootRocketLauncherIncrease,
					Config.LootRodofDiscordIncrease,
					Config.LootSWATHelmetIncrease,
					Config.LootSailorHatIncrease,
					Config.LootSailorPantsIncrease,
					Config.LootSailorShirtIncrease,
					Config.LootShackleIncrease,
					Config.LootSkullIncrease,
					Config.LootSlimeStaffIncrease,
					Config.LootSniperRifleIncrease,
					Config.LootSnowballLauncherIncrease,
					Config.LootSoulofLightIncrease,
					Config.LootSoulofNightIncrease,
					Config.LootStarCloakIncrease,
					Config.LootStylishScissors,
					Config.LootSunMaskIncrease,
					Config.LootTabiIncrease,
					Config.LootTacticalShotgunIncrease,
					Config.LootTallyCounterIncrease,
					Config.LootTatteredBeeWingIncrease,
					Config.LootTendonHookIncrease,
					Config.LootTitanGloveIncrease,
					Config.LootToySledIncrease,
					Config.LootTrifoldMapIncrease,
					Config.LootTurtleShellIncrease,
					Config.LootUmbrellaHatIncrease,
					Config.LootUnicornonaStickIncrease,
					Config.LootUziIncrease,
					Config.LootVikingHelmetIncrease,
					Config.LootVitaminsIncrease,
					Config.LootWhoopieCushionIncrease,
					Config.LootWispinaBottleIncrease,
					Config.LootWormHookIncrease,
					Config.LootYeletsIncrease,
					Config.LootZombieArmIncrease,
					Config.PirateLootCoinGunBaseIncrease,
					Config.PirateLootCutlassBaseIncrease,
					Config.PirateLootDiscountCardBaseIncrease,
					Config.PirateLootGoldRingBaseIncrease,
					Config.PirateLootLuckyCoinBaseIncrease,
					Config.PirateLootPirateStaffBaseIncrease,
					Config.LootBloodyMacheteAndBladedGlovesAreNotLimitedByDamageAndDefense,
					Config.SlimeStaffIncreaseToSurfaceSlimes,
					Config.SlimeStaffIncreaseToUndergroundSlimes,
					Config.SlimeStaffIncreaseToCavernSlimess,
					Config.SlimeStaffIncreaseToIceSpikedSlimes,
					Config.SlimeStaffIncreaseToSpikedJungleSlimes,

					Config.CavernModdedCavernLockBoxLoot,
					Config.DungeonModdedBiomeLockBoxLoot,
					Config.DungeonFurnitureLockBoxLoot,
					Config.HardmodeModdedLockBoxDropRateModifier,
					Config.HellBiomeModdedShadowLockBoxLoot,
					Config.JungleTempleLihzahrd_Lock_Box,
					Config.NormalmodeModdedLockBoxDropRateModifier,
					Config.SandstormAndUndergroundDesertPyramidLockBoxLoot,
					Config.SkyModdedSkywareLockBoxLoot,
					Config.SpiderNestWebCoveredLockBoxLoot,
					Config.SurfaceModdedLivingWoodLockBoxLoot,
					Config.UndergroundJungleBiomeModdedIvyLockBoxLoot,
					Config.UndergroundSnowBiomeModdedIceLockBoxLoot,
					Config.WaterEnemyModdedWaterLockBoxLoot,
					
					Config.TravelingMerchantAcornsIncrease,
					Config.TravelingMerchantAmmoBoxIncrease,
					Config.TravelingMerchantAngelHaloIncrease,
					Config.TravelingMerchantArcaneRuneWallIncrease,
					Config.TravelingMerchantBlackCounterweightIncrease,
					Config.TravelingMerchantBlueDynastyShinglesIncrease,
					Config.TravelingMerchantBlueTeamBlockIncrease,
					Config.TravelingMerchantBlueTeamPlatformIncrease,
					Config.TravelingMerchantBrickLayerIncrease,
					Config.TravelingMerchantCastleMarsbergIncrease,
					Config.TravelingMerchantCelestialMagnetIncrease,
					Config.TravelingMerchantChaliceIncrease,
					Config.TravelingMerchantCode1Increase,
					Config.TravelingMerchantCode2Increase,
					Config.TravelingMerchantColdSnapIncrease,
					Config.TravelingMerchantCompanionCubeIncrease,
					Config.TravelingMerchantCrimsonCapeIncrease,
					Config.TravelingMerchantCursedSaintIncrease,
					Config.TravelingMerchantDPSMeterIncrease,
					Config.TravelingMerchantDiamondRingIncrease,
					Config.TravelingMerchantDynastyWoodIncrease,
					Config.TravelingMerchantExtendoGripIncrease,
					Config.TravelingMerchantFancyDishesIncrease,
					Config.TravelingMerchantFezIncrease,
					Config.TravelingMerchantGatligatorIncrease,
					Config.TravelingMerchantGiIncrease,
					Config.TravelingMerchantGreenTeamBlockIncrease,
					Config.TravelingMerchantGreenTeamPlatformIncrease,
					Config.TravelingMerchantGypsyRobeIncrease,
					Config.TravelingMerchantKatanaIncrease,
					Config.TravelingMerchantKimonoIncrease,
					Config.TravelingMerchantLeopardSkinIncrease,
					Config.TravelingMerchantLifeformAnalyzerIncrease,
					Config.TravelingMerchantMagicHatIncrease,
					Config.TravelingMerchantMartiaLisaIncrease,
					Config.TravelingMerchantMetalDetector,
					Config.TravelingMerchantMysteriousCapeIncrease,
					Config.TravelingMerchantNotAKidNorASquidIncrease,
					Config.TravelingMerchantPadThaiIncrease,
					Config.TravelingMerchantPaintSprayerIncrease,
					Config.TravelingMerchantPhoIncrease,
					Config.TravelingMerchantPinkTeamBlockIncrease,
					Config.TravelingMerchantPinkTeamPlatformIncrease,
					Config.TravelingMerchantPortableCementMixerIncrease,
					Config.TravelingMerchantPresseratorIncrease,
					Config.TravelingMerchantPulseBowIncrease,
					Config.TravelingMerchantRedCapeIncrease,
					Config.TravelingMerchantRedDynastyShinglesIncrease,
					Config.TravelingMerchantRedTeamBlockIncrease,
					Config.TravelingMerchantRedTeamPlatformIncrease,
					Config.TravelingMerchantRevolverIncrease,
					Config.TravelingMerchantSakeIncrease,
					Config.TravelingMerchantSittingDucksFishingPoleIncrease,
					Config.TravelingMerchantSnowfellasIncrease,
					Config.TravelingMerchantStopwatchIncrease,
					Config.TravelingMerchantTheSeasonIncrease,
					Config.TravelingMerchantTheTruthIsUpThereIncrease,
					Config.TravelingMerchantTigerSkinIncrease,
					Config.TravelingMerchantUltraBrightTorchIncrease,
					Config.TravelingMerchantWaterGunIncrease,
					Config.TravelingMerchantWhiteTeamBlockIncrease,
					Config.TravelingMerchantWhiteTeamPlatformIncrease,
					Config.TravelingMerchantWinterCapeIncrease,
					Config.TravelingMerchantYellowCounterweightIncrease,
					Config.TravelingMerchantYellowTeamBlockIncrease,
					Config.TravelingMerchantYellowTeamPlatformIncrease,
					Config.TravelingMerchantZebraSkinIncrease,
					Config.TravelingMerchantAlwaysXMasForConfigurations,
					Config.ChanceThatEnemyKillWillResetTravelingMerchant,
					Config.StationaryMerchant,
					Config.StationaryMerchantStockingChance,
					Config.S_MerchantStockingChanceBonusWhichWillBeMultipliedByH_ModeCompletionRate,
					
					Config.QuestAnglerEarringIncrease,
					Config.QuestAnglerHatIncrease,
					Config.QuestAnglerPantsIncrease,
					Config.QuestAnglerVestIncrease,
					Config.QuestCoralstoneBlockIncrease,
					Config.QuestDecorativeFurnitureIncrease,
					Config.QuestFishCostumeIncrease,
					Config.QuestFishHookIncrease,
					Config.QuestFishermansGuideIncrease,
					Config.QuestGoldenBugNetIncrease,
					Config.QuestGoldenFishingRodIncrease,
					Config.QuestHardcoreBottomlessBucketIncrease,
					Config.QuestHardcoreFinWingsIncrease,
					Config.QuestHardcoreHotlineFishingHookIncrease,
					Config.QuestHardcoreSuperAbsorbantSpongeIncrease,
					Config.QuestHighTestFishingLineIncrease,
					Config.QuestMermaidCostumeIncrease,
					Config.QuestSextantIncrease,
					Config.QuestTackleBoxIncrease,
					Config.QuestTrophyIncrease,
					Config.QuestWeatherRadioIncrease,
					
					Config.ChestSalesmanPreHardmodeChestsRequireHardmodeActivated,
					Config.ChestSalesmanSellsBiomeChests,
					Config.ChestSalesmanSellsGoldChest,
					Config.ChestSalesmanSellsIceChest,
					Config.ChestSalesmanSellsIvyChest,
					Config.ChestSalesmanSellsLihzahrdChest,
					Config.ChestSalesmanSellsLivingWoodChest,
					Config.ChestSalesmanSellsOceanChest,
					Config.ChestSalesmanSellsShadowChest,
					Config.ChestSalesmanSellsSkywareChest,
					Config.ChestSalesmanSellsWebCoveredChest,
					Config.ChestSalesmanSpawnable,
					
					Config.MechanicSellsDartTrapAfterSkeletronDefeated,
					Config.MechanicSellsGeyserAfterWallofFleshDefeated,
					Config.MechanicSellsLihzahrdTrapsAfterGolemDefeated,
					Config.MechanicSellsWoodenSpikesAfterGolemDefeated,
					Config.MerchantSellsAllMiningGear,
					Config.MerchantSellsBlizzardInABottleWhenInSnow,
					Config.MerchantSellsCloudInABottleWhenInSky,
					Config.MerchantSellsFishItem,
					Config.MerchantSellsPyramidItems,
					Config.MerchantSellsSandstormInABottleWhenInDesert,
					Config.WitchDoctorSellsFlowerBoots,
					Config.WitchDoctorSellsHoneyDispenser,
					Config.WitchDoctorSellsSeaweed,
					Config.WitchDoctorSellsStaffofRegrowth,
					Config.TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight,
					
					Config.GoblinTinkererSellsGoblinRetreatOrder,
					Config.PirateSellsPirateRetreatOrder,
					Config.WizardSellsMoonBall,
					Config.BattlePotionMaxSpawnsMultiplier,
					Config.BattlePotionSpawnrateMultiplier,
					Config.BloodZombieAndDripplerDropsBloodMoonMedallion,
					Config.ChaosPotionMaxSpawnsMultiplier,
					Config.ChaosPotionSpawnrateMultiplier,
					Config.WarPotionMaxSpawnsMultiplier,
					Config.WarPotionSpawnrateMultiplier,
					
					Config.NewCharacterBarrels,
					Config.NewCharacterCopperBars,
					Config.NewCharacterCopperCoins,
					Config.NewCharacterGoldBars,
					Config.NewCharacterGoldCoins,
					Config.NewCharacterIronBars,
					Config.NewCharacterMiningPotions,
					Config.NewCharacterPlatinumCoins,
					Config.NewCharacterSilverBars,
					Config.NewCharacterSilverCoins,
					Config.NewCharacterWarPotions,

					Config.ExtractinatorGivesAmber,
					Config.ExtractinatorGivesAmberMosquito,
					Config.ExtractinatorGivesAmethyst,
					Config.ExtractinatorGivesCopperCoin,
					Config.ExtractinatorGivesCopperOre,
					Config.ExtractinatorGivesDiamond,
					Config.ExtractinatorGivesEmerald,
					Config.ExtractinatorGivesFossilOre,
					Config.ExtractinatorGivesGoldCoin,
					Config.ExtractinatorGivesGoldOre,
					Config.ExtractinatorGivesIronOre,
					Config.ExtractinatorGivesLeadOre,
					Config.ExtractinatorGivesPlatinumCoin,
					Config.ExtractinatorGivesPlatinumOre,
					Config.ExtractinatorGivesRuby,
					Config.ExtractinatorGivesSapphire,
					Config.ExtractinatorGivesSilverCoin,
					Config.ExtractinatorGivesSilverOre,
					Config.ExtractinatorGivesTinOre,
					Config.ExtractinatorGivesTopaz,
					Config.ExtractinatorGivesTungstenOre,
					
					Config.CrateUpgradesDependingOnFishingPower,
					Config.FishCatchBecomesBalloonPufferfish,
					Config.FishCatchBecomesBladetongue,
					Config.FishCatchBecomesBlueJellyfish,
					Config.FishCatchBecomesChaosFish,
					Config.FishCatchBecomesCorruptCrate,
					Config.FishCatchBecomesCrimsonCrate,
					Config.FishCatchBecomesCrystalSerpent,
					Config.FishCatchBecomesDungeonCrate,
					Config.FishCatchBecomesFrogLeg,
					Config.FishCatchBecomesFrostDaggerfish,
					Config.FishCatchBecomesGoldenCarp,
					Config.FishCatchBecomesGoldenCrate,
					Config.FishCatchBecomesGreenJellyfish,
					Config.FishCatchBecomesHallowedCrate,
					Config.FishCatchBecomesIronCrate,
					Config.FishCatchBecomesJungleCrate,
					Config.FishCatchBecomesPinkJellyfish,
					Config.FishCatchBecomesPurpleClubberfish,
					Config.FishCatchBecomesReaverSharkAfterAllMechBossesDowned,
					Config.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
					Config.FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
					Config.FishCatchBecomesRockfish,
					Config.FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned,
					Config.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
					Config.FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned,
					Config.FishCatchBecomesScalyTruffle,
					Config.FishCatchBecomesSkyCrate,
					Config.FishCatchBecomesSwordfish,
					Config.FishCatchBecomesToxikarp,
					Config.FishCatchBecomesWoodenCrate,
					Config.FishCatchBecomesZephyrFish
				);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SendClientChangesPacket();
            }
        }
		
		public override void AnglerQuestReward(float quality, List<Item> rewardItems)
		{
			Player player = Main.player[Main.myPlayer];
			ReducedGrindingPlayer mPlayer = player.GetModPlayer<ReducedGrindingPlayer>(mod);
			
			if (mPlayer.clientConf.QuestFishermansGuideIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestFishermansGuideIncrease)
				{
					Item rewardItem = new Item();
					rewardItem.SetDefaults(ItemID.FishermansGuide);
					rewardItem.stack = 1;
					rewardItems.Add(rewardItem);
				}
			}
			if (mPlayer.clientConf.QuestWeatherRadioIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestWeatherRadioIncrease)
				{
					Item rewardItem2 = new Item();
					rewardItem2.SetDefaults(ItemID.WeatherRadio);
					rewardItem2.stack = 1;
					rewardItems.Add(rewardItem2);
				}
			}
			if (mPlayer.clientConf.QuestSextantIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestSextantIncrease)
				{
					Item rewardItem3 = new Item();
					rewardItem3.SetDefaults(ItemID.Sextant);
					rewardItem3.stack = 1;
					rewardItems.Add(rewardItem3);
				}
			}
			if (mPlayer.clientConf.QuestAnglerHatIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestAnglerHatIncrease)
				{
					Item rewardItem4 = new Item();
					rewardItem4.SetDefaults(ItemID.AnglerHat);
					rewardItem4.stack = 1;
					rewardItems.Add(rewardItem4);
				}
			}
			if (mPlayer.clientConf.QuestAnglerVestIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestAnglerVestIncrease)
				{
					Item rewardItem5 = new Item();
					rewardItem5.SetDefaults(ItemID.AnglerVest);
					rewardItem5.stack = 1;
					rewardItems.Add(rewardItem5);
				}
			}
			if (mPlayer.clientConf.QuestAnglerPantsIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestAnglerPantsIncrease)
				{
					Item rewardItem6 = new Item();
					rewardItem6.SetDefaults(ItemID.AnglerPants);
					rewardItem6.stack = 1;
					rewardItems.Add(rewardItem6);
				}
			}
			if (mPlayer.clientConf.QuestHighTestFishingLineIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestHighTestFishingLineIncrease)
				{
					Item rewardItem7 = new Item();
					rewardItem7.SetDefaults(ItemID.HighTestFishingLine);
					rewardItem7.stack = 1;
					rewardItems.Add(rewardItem7);
				}
			}
			if (mPlayer.clientConf.QuestAnglerEarringIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestAnglerEarringIncrease)
				{
					Item rewardItem8 = new Item();
					rewardItem8.SetDefaults(ItemID.AnglerEarring);
					rewardItem8.stack = 1;
					rewardItems.Add(rewardItem8);
				}
			}
			if (mPlayer.clientConf.QuestTackleBoxIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestTackleBoxIncrease)
				{
					Item rewardItem9 = new Item();
					rewardItem9.SetDefaults(ItemID.TackleBox);
					rewardItem9.stack = 1;
					rewardItems.Add(rewardItem9);
				}
			}
			if (mPlayer.clientConf.QuestGoldenFishingRodIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestGoldenFishingRodIncrease)
				{
					Item rewardItem10 = new Item();
					rewardItem10.SetDefaults(ItemID.GoldenFishingRod);
					rewardItem10.stack = 1;
					rewardItems.Add(rewardItem10);
				}
			}
			if (mPlayer.clientConf.QuestCoralstoneBlockIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestCoralstoneBlockIncrease)
				{
					Item rewardItem11 = new Item();
					rewardItem11.SetDefaults(ItemID.CoralstoneBlock);
					rewardItem11.stack = Main.rand.Next(50, 150);
					rewardItems.Add(rewardItem11);
				}
			}
			if (mPlayer.clientConf.QuestGoldenBugNetIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestGoldenBugNetIncrease)
				{
					Item rewardItem10 = new Item();
					rewardItem10.SetDefaults(3183);
					rewardItem10.stack = 1;
					rewardItems.Add(rewardItem10);
				}
			}
			if (mPlayer.clientConf.QuestFishHookIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestFishHookIncrease)
				{
					Item rewardItem10 = new Item();
					rewardItem10.SetDefaults(2360);
					rewardItem10.stack = 1;
					rewardItems.Add(rewardItem10);
				}
			}
			if (Main.hardMode)
			{
				if (mPlayer.clientConf.QuestHardcoreFinWingsIncrease > 0)
				{
					if (Main.rand.NextFloat() < mPlayer.clientConf.QuestHardcoreFinWingsIncrease)
					{
						Item rewardItem12 = new Item();
						rewardItem12.SetDefaults(ItemID.FinWings);
						rewardItem12.stack = 1;
						rewardItems.Add(rewardItem12);
					}
				}
				if (mPlayer.clientConf.QuestHardcoreBottomlessBucketIncrease > 0)
				{
					if (Main.rand.NextFloat() < mPlayer.clientConf.QuestHardcoreBottomlessBucketIncrease)
					{
						Item rewardItem13 = new Item();
						rewardItem13.SetDefaults(ItemID.BottomlessBucket);
						rewardItem13.stack = 1;
						rewardItems.Add(rewardItem13);
					}
				}
				if (mPlayer.clientConf.QuestHardcoreSuperAbsorbantSpongeIncrease > 0)
				{
					if (Main.rand.NextFloat() < mPlayer.clientConf.QuestHardcoreSuperAbsorbantSpongeIncrease)
					{
						Item rewardItem14 = new Item();
						rewardItem14.SetDefaults(ItemID.SuperAbsorbantSponge);
						rewardItem14.stack = 1;
						rewardItems.Add(rewardItem14);
					}
				}
				if (mPlayer.clientConf.QuestHardcoreHotlineFishingHookIncrease > 0)
				{
					if (Main.rand.NextFloat() < mPlayer.clientConf.QuestHardcoreHotlineFishingHookIncrease)
					{
						Item rewardItem15 = new Item();
						rewardItem15.SetDefaults(ItemID.HotlineFishingHook);
						rewardItem15.stack = 1;
						rewardItems.Add(rewardItem15);
					}
				}
			}
			if (mPlayer.clientConf.QuestTrophyIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestTrophyIncrease)
				{
					Item rewardItem16 = new Item();
					rewardItem16.SetDefaults(2446);
					rewardItem16.stack = 1;
					rewardItems.Add(rewardItem16);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestTrophyIncrease)
				{
					Item rewardItem17 = new Item();
					rewardItem17.SetDefaults(2447);
					rewardItem17.stack = 1;
					rewardItems.Add(rewardItem17);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestTrophyIncrease)
				{
					Item rewardItem18 = new Item();
					rewardItem18.SetDefaults(2448);
					rewardItem18.stack = 1;
					rewardItems.Add(rewardItem18);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestTrophyIncrease)
				{
					Item rewardItem19 = new Item();
					rewardItem19.SetDefaults(2449);
					rewardItem19.stack = 1;
					rewardItems.Add(rewardItem19);
				}
			}
			if (mPlayer.clientConf.QuestDecorativeFurnitureIncrease > 0)
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem20 = new Item();
					rewardItem20.SetDefaults(2496);
					rewardItem20.stack = 1;
					rewardItems.Add(rewardItem20);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem21 = new Item();
					rewardItem21.SetDefaults(2497);
					rewardItem21.stack = 1;
					rewardItems.Add(rewardItem21);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem22 = new Item();
					rewardItem22.SetDefaults(2442);
					rewardItem22.stack = 1;
					rewardItems.Add(rewardItem22);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem23 = new Item();
					rewardItem23.SetDefaults(2443);
					rewardItem23.stack = 1;
					rewardItems.Add(rewardItem23);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem24 = new Item();
					rewardItem24.SetDefaults(2444);
					rewardItem24.stack = 1;
					rewardItems.Add(rewardItem24);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem25 = new Item();
					rewardItem25.SetDefaults(2445);
					rewardItem25.stack = 1;
					rewardItems.Add(rewardItem25);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestDecorativeFurnitureIncrease)
				{
					Item rewardItem26 = new Item();
					rewardItem26.SetDefaults(2490);
					rewardItem26.stack = 1;
					rewardItems.Add(rewardItem26);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestFishCostumeIncrease)
				{
					Item rewardItem26 = new Item();
					rewardItem26.SetDefaults(2498); //Mask
					rewardItem26.stack = 1;
					rewardItems.Add(rewardItem26);
					
					Item rewardItem27 = new Item();
					rewardItem27.SetDefaults(2499); //Shirt
					rewardItem27.stack = 1;
					rewardItems.Add(rewardItem27);
					
					Item rewardItem28 = new Item();
					rewardItem28.SetDefaults(2500); //Finskirt
					rewardItem28.stack = 1;
					rewardItems.Add(rewardItem28);
				}
				if (Main.rand.NextFloat() < mPlayer.clientConf.QuestMermaidCostumeIncrease)
				{
					Item rewardItem26 = new Item();
					rewardItem26.SetDefaults(2417); //Hairpin
					rewardItem26.stack = 1;
					rewardItems.Add(rewardItem26);
					
					Item rewardItem27 = new Item();
					rewardItem27.SetDefaults(2418); //Adornment
					rewardItem27.stack = 1;
					rewardItems.Add(rewardItem27);
					
					Item rewardItem28 = new Item();
					rewardItem28.SetDefaults(2419); //Tail
					rewardItem28.stack = 1;
					rewardItems.Add(rewardItem28);
				}
			}
		}
		
		public override void SetupStartInventory(IList<Item> items)
		{
			Mod luiafk = ModLoader.GetMod("Luiafk"); //Prevent crashes when using Luiafk
			Mod recipebrowser = ModLoader.GetMod("RecipeBrowser"); //Prevent crashes when using RecipeBrowser
			if (luiafk == null && recipebrowser == null)
			{
				ReducedGrindingPlayer mPlayer = Main.LocalPlayer.GetModPlayer<ReducedGrindingPlayer>();
			
				Item item = new Item();
				item.SetDefaults(ItemID.MiningPotion);
				item.stack = mPlayer.clientConf.NewCharacterMiningPotions;
				items.Add(item);
				
				Item item2 = new Item();
				item2.SetDefaults(ItemID.CopperBar);
				item2.stack = mPlayer.clientConf.NewCharacterCopperBars;
				items.Add(item2);
				
				Item item3 = new Item();
				item3.SetDefaults(ItemID.IronBar);
				item3.stack = mPlayer.clientConf.NewCharacterIronBars;
				items.Add(item3);
				
				Item item4 = new Item();
				item4.SetDefaults(ItemID.SilverBar);
				item4.stack = mPlayer.clientConf.NewCharacterSilverBars;
				items.Add(item4);

				Item item5 = new Item();
				item5.SetDefaults(ItemID.GoldBar);
				item5.stack = mPlayer.clientConf.NewCharacterGoldBars;
				items.Add(item5);

				Item item6 = new Item();
				item6.SetDefaults(ItemID.Barrel);
				item6.stack = mPlayer.clientConf.NewCharacterBarrels;
				items.Add(item6);

				Item item7 = new Item();
				item7.SetDefaults(ItemID.CopperCoin);
				item7.stack = mPlayer.clientConf.NewCharacterCopperCoins;
				items.Add(item7);

				Item item8 = new Item();
				item8.SetDefaults(ItemID.SilverCoin);
				item8.stack = mPlayer.clientConf.NewCharacterSilverCoins;
				items.Add(item8);

				Item item9 = new Item();
				item9.SetDefaults(ItemID.GoldCoin);
				item9.stack = mPlayer.clientConf.NewCharacterGoldCoins;
				items.Add(item9);

				Item item10 = new Item();
				item10.SetDefaults(ItemID.PlatinumCoin);
				item10.stack = mPlayer.clientConf.NewCharacterPlatinumCoins;
				items.Add(item10);

				Item item11 = new Item();
				item11.SetDefaults(mod.ItemType("War_Potion"));
				item11.stack = mPlayer.clientConf.NewCharacterWarPotions;
				items.Add(item11);
			}
		}
		
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			
			ReducedGrindingPlayer mPlayer = Main.LocalPlayer.GetModPlayer<ReducedGrindingPlayer>(mod);
			
			if (junk)
			{
				return;
			}
			
			int EyeEaterSkeletronDowned = 0;
			if (NPC.downedBoss1)
				EyeEaterSkeletronDowned++;
			if (NPC.downedBoss2)
				EyeEaterSkeletronDowned++;
			if (NPC.downedBoss3)
				EyeEaterSkeletronDowned++;
			int MechBossesDowned = 0;
			if (NPC.downedMechBoss1)
				MechBossesDowned++;
			if (NPC.downedMechBoss2)
				MechBossesDowned++;
			if (NPC.downedMechBoss3)
				MechBossesDowned++;
			
			float reaverSharkRate = 0;
			if (NPC.downedMechBossAny)
				reaverSharkRate = mPlayer.clientConf.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned + (mPlayer.clientConf.FishCatchBecomesReaverSharkAfterAllMechBossesDowned - mPlayer.clientConf.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned) * MechBossesDowned / 3;
			else{
				reaverSharkRate = mPlayer.clientConf.FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned + (mPlayer.clientConf.FishCatchBecomesReaverSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned - mPlayer.clientConf.FishCatchBecomesReaverSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned) * EyeEaterSkeletronDowned / 3;
			}
			float sawtoothSharkRate = 0;
			if (NPC.downedMechBossAny)
				reaverSharkRate = mPlayer.clientConf.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned + (mPlayer.clientConf.FishCatchBecomesSawtoothSharkAfterAllMechBossesDowned - mPlayer.clientConf.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned) * MechBossesDowned / 3;
			else{
				sawtoothSharkRate = mPlayer.clientConf.FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned + (mPlayer.clientConf.FishCatchBecomesSawtoothSharkAfterEyeOfCthulhuEaterOfWorldsAndSkeletronDowned - mPlayer.clientConf.FishCatchBecomesSawtoothSharkBeforeEyeOfCthulhuEaterOfWorldsAndSkeletronDowned) * EyeEaterSkeletronDowned / 3;
			}
			
			float powerFloat = power;
			float crateUpgradeRate = 0;
			
			if ((caughtType >= 2334 && caughtType <= 2336) || (caughtType >= 3203 && caughtType <= 3208)) //Caught a crate
			{
				if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesGoldenCrate*Math.Min(1, powerFloat/257))
					caughtType = ItemID.GoldenCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesJungleCrate*Math.Min(1, powerFloat/257) && player.ZoneJungle)
					caughtType = ItemID.JungleFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesCorruptCrate*Math.Min(1, powerFloat/257) && player.ZoneCorrupt)
					caughtType = ItemID.CorruptFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesCrimsonCrate*Math.Min(1, powerFloat/257) && player.ZoneCrimson)
					caughtType = ItemID.CrimsonFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesHallowedCrate*Math.Min(1, powerFloat/257) && player.ZoneHoly)
					caughtType = ItemID.HallowedFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesDungeonCrate*Math.Min(1, powerFloat/257) && player.ZoneDungeon)
					caughtType = ItemID.DungeonFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesSkyCrate*Math.Min(1, powerFloat/257) && worldLayer == 0)
					caughtType = ItemID.FloatingIslandFishingCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesIronCrate*Math.Min(1, powerFloat/257))
					caughtType = ItemID.IronCrate;
				else if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesWoodenCrate*Math.Min(1, powerFloat/257))
					caughtType = ItemID.WoodenCrate;
				
				if (mPlayer.clientConf.CrateUpgradesDependingOnFishingPower)
				{
					if (power > 50 && caughtType == ItemID.WoodenCrate)
					{
						crateUpgradeRate = (power - 50) / 77;
						if (Main.rand.NextFloat() < 10000*crateUpgradeRate){
							caughtType = ItemID.IronCrate;
						}
					}
					if (power > 50 && caughtType == ItemID.IronCrate && player.ZoneDungeon)
					{
						crateUpgradeRate = (power - 127) / 78;
						if (Main.rand.NextFloat() < 10000*crateUpgradeRate)
							caughtType = ItemID.DungeonFishingCrate;
					}
					if (power > 127 && caughtType == ItemID.IronCrate)
					{
						crateUpgradeRate = (power - 127) / 78;
						if (Main.rand.NextFloat() < 10000*crateUpgradeRate){
							if (player.ZoneJungle)
								caughtType = ItemID.JungleFishingCrate;
							else if (worldLayer == 0)
								caughtType = ItemID.FloatingIslandFishingCrate;
							else if (player.ZoneCorrupt)
								caughtType = ItemID.CorruptFishingCrate;
							else if (player.ZoneCrimson)
								caughtType = ItemID.CrimsonFishingCrate;
							else if (player.ZoneHoly)
								caughtType = ItemID.HallowedFishingCrate;
							else if (player.ZoneDungeon)
								caughtType = ItemID.DungeonFishingCrate;
						}
					}
					if (power > 205 && (caughtType == ItemID.IronCrate || (caughtType >= 3203 && caughtType <= 3208)))
					{
						crateUpgradeRate = (power - 205) / 77;
						if (Main.rand.NextFloat() < 10000*crateUpgradeRate){
							caughtType = ItemID.GoldenCrate;
						}
					}
				}
			}
			else
			{
				for (int i = 1; i <= 18; i++)//Do 18 times
				{
					switch (Main.rand.Next(18+1))
					{
						//Worldlayers: 0, sky; 1, surface; 2, underground; 3, caverns; and 4, underworld.
						//Power affects config rate. It's mulitplied by power/257 , but capped at 1.
						case 1:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesGoldenCarp*Math.Min(1, powerFloat/257) && worldLayer >= 2)
							{
								caughtType = ItemID.GoldenCarp;
								i = 19;
							}
							break;
						case 2:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesBlueJellyfish*Math.Min(1, powerFloat/257) && worldLayer >= 3 && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHoly)
							{
								caughtType = ItemID.BlueJellyfish;
								i = 19;
							}
							break;
						case 3:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesPinkJellyfish*Math.Min(1, powerFloat/257) && worldLayer <= 1 && player.ZoneBeach)
							{
								caughtType = ItemID.PinkJellyfish;
								i = 19;
							}
							break;
						case 4:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesGreenJellyfish*Math.Min(1, powerFloat/257) && worldLayer >= 3 && Main.hardMode && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHoly)
							{
								caughtType = ItemID.GreenJellyfish;
								i = 19;
							}
							break;
						case 5:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesChaosFish*Math.Min(1, powerFloat/257) && worldLayer >= 2 && player.ZoneHoly)
							{
								caughtType = ItemID.ChaosFish;
								i = 19;
							}
							break;
						case 6:
							if (Main.rand.NextFloat() < reaverSharkRate*Math.Min(1, powerFloat/257) && worldLayer <= 1 && player.ZoneBeach)
							{
								caughtType = ItemID.ReaverShark;
								i = 19;
							}
							break;
						case 7:
							if (Main.rand.NextFloat() < sawtoothSharkRate*Math.Min(1, powerFloat/257) && worldLayer <= 1 && player.ZoneBeach)
							{
								caughtType = ItemID.SawtoothShark;
								i = 19;
							}
							break;
						case 8:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesScalyTruffle*Math.Min(1, powerFloat/257) && worldLayer == 3 && Main.hardMode && (player.ZoneSnow && (player.ZoneHoly || player.ZoneCorrupt || player.ZoneCrimson)))
							{
								caughtType = ItemID.ScalyTruffle;
								i = 19;
							}
							break;
						case 9:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesZephyrFish*Math.Min(1, powerFloat/257))
							{
								caughtType = ItemID.ZephyrFish;
								i = 19;
							}
							break;
						case 10:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesToxikarp*Math.Min(1, powerFloat/257) && Main.hardMode && player.ZoneCorrupt)
							{
								caughtType = ItemID.Toxikarp;
								i = 19;
							}
							break;
						case 11:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesBladetongue*Math.Min(1, powerFloat/257) && Main.hardMode && player.ZoneCrimson)
							{
								caughtType = ItemID.Bladetongue;
								i = 19;
							}
							break;
						case 12:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesCrystalSerpent*Math.Min(1, powerFloat/257) && Main.hardMode && player.ZoneHoly)
							{
								caughtType = ItemID.CrystalSerpent;
								i = 19;
							}
							break;
						case 13:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesPurpleClubberfish*Math.Min(1, powerFloat/257) && player.ZoneCorrupt)
							{
								caughtType = ItemID.PurpleClubberfish;
								i = 19;
							}
							break;
						case 14:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesRockfish*Math.Min(1, powerFloat/257) && worldLayer >= 2)
							{
								caughtType = ItemID.Rockfish;
								i = 19;
							}
							break;
						case 15:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesFrogLeg*Math.Min(1, powerFloat/257))
							{
								caughtType = ItemID.FrogLeg;
								i = 19;
							}
							break;
						case 16:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesSwordfish*Math.Min(1, powerFloat/257) && worldLayer <= 1 && player.ZoneBeach)
							{
								caughtType = ItemID.Swordfish;
								i = 19;
							}
							break;
						case 17:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesBalloonPufferfish*Math.Min(1, powerFloat/257))
							{
								caughtType = ItemID.BalloonPufferfish;
								i = 19;
							}
							break;
						case 18:
							if (Main.rand.NextFloat() < mPlayer.clientConf.FishCatchBecomesFrostDaggerfish*Math.Min(1, powerFloat/257) && player.ZoneSnow)
							{
								caughtType = ItemID.FrostDaggerfish;
								i = 19;
							}
							break;
					}
				}
			}
		}
		
		public override void PostUpdate()
		{
			Player player = Main.LocalPlayer;
			ReducedGrindingPlayer mPlayer = Main.LocalPlayer.GetModPlayer<ReducedGrindingPlayer>(mod);
			
			if (NPC.taxCollector && Main.time == 1.0)
			{

				if (player.taxMoney >= mPlayer.clientConf.TaxCollectorMinTaxRequiredToChatTaxEachMorningAndNight)
				{
					int taxGold = player.taxMoney;
					int taxSilver = 0;
					int taxCopper = 0;
					taxCopper = taxGold % 100;
					taxGold = (taxGold - taxCopper) / 100;
					taxSilver = taxGold % 100;
					taxGold = (taxGold - taxSilver) / 100;
					Main.NewText("Tax Money:");
					if (taxGold > 0)
						Main.NewText(taxGold.ToString() + " Gold", 255, 255, 0);
					if (taxSilver > 0)
					Main.NewText(taxSilver.ToString() + " Silver", 191, 191, 191);
					if (taxCopper > 0)
					Main.NewText(taxCopper.ToString() + " Copper", 255, 127, 0);
				}
			}
		}
    }
}