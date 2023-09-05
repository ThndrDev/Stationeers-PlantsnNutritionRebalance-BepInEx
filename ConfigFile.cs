using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using System.Collections.Generic;
using Assets.Scripts.Genetics;
using Assets.Scripts.Objects.Structures;
using System.Collections;
using System;
using BepInEx.Logging;
using JetBrains.Annotations;

namespace PlantsnNutritionRebalance.Scripts
{
    internal class ConfigFile
    {
        private static ConfigEntry<int> configLogLevel;
        private static ConfigEntry<float> configPlantWaterConsumptionMultiplier;
        private static ConfigEntry<float> configPlantWaterConsumptionLimit;
        private static ConfigEntry<float> configPlantWaterTranspirationPercentage;
        private static ConfigEntry<float> configAtmosphereFogThreshold;

        private static ConfigEntry<float> configNutritionLossMultiplier;
        private static ConfigEntry<float> configMaxNutritionStorage;
        private static ConfigEntry<float> configMaxHydrationStorage;
        private static ConfigEntry<float> configWarningNutrition;
        private static ConfigEntry<float> configCriticalNutrition;
        private static ConfigEntry<float> configWarningHydration;
        private static ConfigEntry<float> configCriticalHydration;
        private static ConfigEntry<bool> configEnableRespawnPenaltyLogic;
        private static ConfigEntry<bool> configCustomNewPlayerRespawn;
        private static ConfigEntry<float> configCustomNewPlayerRespawnNutrition;
        private static ConfigEntry<float> configCustomNewPlayerRespawnHydration;       

        //------------------------------ foods----------------------------------------------
        private static ConfigEntry<bool> configFoods;
        private static ConfigEntry<float> configTomatoSoup;
        private static ConfigEntry<float> configCornSoup;
        private static ConfigEntry<float> configCannedRicePudding;
        private static ConfigEntry<float> configPumpkinSoup;
        private static ConfigEntry<float> configPumpkinPie;
        private static ConfigEntry<float> configBakedPotato;
        private static ConfigEntry<float> configFrenchFries;
        private static ConfigEntry<float> configCannedFrenchFries;
        private static ConfigEntry<float> configMilk;
        private static ConfigEntry<float> configCannedCondensedMilk;
        private static ConfigEntry<float> configMuffin;
        private static ConfigEntry<float> configBreadLoaf;
        private static ConfigEntry<float> configCerealBar;
        private static ConfigEntry<float> configCannedPowderedEggs;
        private static ConfigEntry<float> configCannedEdamame;
        private static ConfigEntry<float> configCondensedMilk;
        private static ConfigEntry<float> configCookedSoybean;
        private static ConfigEntry<float> configCookedRice;
        private static ConfigEntry<float> configCookedCorn;
        private static ConfigEntry<float> configCookedPumpkin;
        private static ConfigEntry<float> configPowderedEggs;
        private static ConfigEntry<float> configCookedTomato;

        private static ConfigEntry<float> configTomatoSoupES;
        private static ConfigEntry<float> configCornSoupES;
        private static ConfigEntry<float> configCannedRicePuddingES;
        private static ConfigEntry<float> configPumpkinSoupES;
        private static ConfigEntry<float> configPumpkinPieES;
        private static ConfigEntry<float> configBakedPotatoES;
        private static ConfigEntry<float> configFrenchFriesES;
        private static ConfigEntry<float> configCannedFrenchFriesES;
        private static ConfigEntry<float> configMilkES;
        private static ConfigEntry<float> configCannedCondensedMilkES;
        private static ConfigEntry<float> configMuffinES;
        private static ConfigEntry<float> configBreadLoafES;
        private static ConfigEntry<float> configCerealBarES;
        private static ConfigEntry<float> configCannedPowderedEggsES;
        private static ConfigEntry<float> configCannedEdamameES;
        private static ConfigEntry<float> configCondensedMilkES;
        private static ConfigEntry<float> configCookedSoybeanES;
        private static ConfigEntry<float> configCookedRiceES;
        private static ConfigEntry<float> configCookedCornES;
        private static ConfigEntry<float> configCookedPumpkinES;
        private static ConfigEntry<float> configPowderedEggsES;
        private static ConfigEntry<float> configCookedTomatoES;
        private static ConfigEntry<float> configmaxDaysHunger;
        

        private static ConfigEntry<float> ddm;
        private static ConfigEntry<float> mfe;
        private static ConfigEntry<float> mhe;
        private static ConfigEntry<float> mfd;
        private static ConfigEntry<float> mhd;

        public static Dictionary<String, System.Object> fConfigsFood = new Dictionary<string, object>();

        public static Dictionary<String, System.Object> fTomatoSoup = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCornSoup = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCannedRicePudding = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fPumpkinSoup = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fPumpkinPie = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fBakedPotato = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fFrenchFries = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCannedFrenchFries = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fMilk = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCannedCondensedMilk = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fMuffin = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fBreadLoaf = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCerealBar = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCannedPowderedEggs = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCannedEdamame = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCondensedMilk = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCookedSoybean = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCookedRice = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCookedCorn = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCookedPumpkin = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fPowderedEggs = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCookedTomato = new Dictionary<string, object>();

        //plants
        private static ConfigEntry<bool> configPlants;
        private static ConfigEntry<float> configWheat;
        private static ConfigEntry<float> configCorn;
        private static ConfigEntry<float> configFern;
        private static ConfigEntry<float> configMushroom;
        private static ConfigEntry<float> configPotato;
        private static ConfigEntry<float> configPumpkin;
        private static ConfigEntry<float> configRice;
        private static ConfigEntry<float> configSoybean;
        private static ConfigEntry<float> configTomato;

        public static Dictionary<String, System.Object> fWheat = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fCorn = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fFern = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fMushroom = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fPotato = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fPumpkin = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fRice = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fSoybean = new Dictionary<string, object>();
        public static Dictionary<String, System.Object> fTomato = new Dictionary<string, object>();

        //decay
        /* not implemented
        private ConfigEntry<bool> configDecay;
        private ConfigEntry<float> DecayFactor;
        public static Dictionary<String, System.Object> fDecayFactor = new Dictionary<string, object>();
        */

        public static int LogLevel;
        public static float PlantWaterConsumptionMultiplier;
        public static float PlantWaterConsumptionLimit;
        public static float PlantWaterTranspirationPercentage;
        public static float AtmosphereFogThreshold;
        public static float NutritionLossMultiplier;
        public static float MaxNutritionStorage;
        public static float MaxHydrationStorage;
        public static float WarningNutrition;
        public static float CriticalNutrition;
        public static float WarningHydration;
        public static float CriticalHydration;
        public static bool EnableRespawnPenaltyLogic;
        public static bool CustomNewPlayerRespawn;
        public static float CustomNewPlayerRespawnNutrition;
        public static float CustomNewPlayerRespawnHydration;

        public static void HandleConfig(PlantsnNutritionRebalancePlugin PnN) // Create and manage the configuration file parameters
        {
            configLogLevel = PnN.Config.Bind("0 - General configuration",
                 "Log Level",
                 0, 
                 "Set the log level of the mod. Values can be 0 for errors only (default), 1 for informational logs or 2 for debug logs.\n" +
                 "Logs can be found inside %appdata%\\..\\localLow\\rocketwerkz\\rocketstation\\player.log file");
            LogLevel = configLogLevel.Value;

            configPlantWaterConsumptionMultiplier = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionMultiplier",  // The key of the configuration option in the configuration file
                 500f, // The default value
                 "By how much this mod should multiply the water consumption of plants?\n" +
                 "The vanilla water consumption value is aprox ~0.000006 moles per tick for most plants, quite low. For reference, 1 ice water stack has 1000 mols\n" +
                 "That means, in vanilla, 1 single stack of ice will keep a plant alive for more than 23148 hours of gameplay! That's why the suggested value here\n" +
                 "is 500, it increases the plants drinks to ~0.003 moles of water per tick. With this, 1 ice water stack will keep a plant alive for 46 hours of gameplay,\n" +
                 "or 20 plants for 2 hours, enough to make the water management meaningful.\n" +
                 "Set this option to 1 to keep the vanilla water consumption on plants"); // Description of the option to show in the config file

            PlantWaterConsumptionMultiplier = Mathf.Clamp(configPlantWaterConsumptionMultiplier.Value, 1f, 100000f);

            configPlantWaterConsumptionLimit = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionLimit",  // The key of the configuration option in the configuration file
                 0.004f, // The default value
                 "Limit the max consumption of water mols per tick a plant can drink. This is mainly to fix the behaviour of the water consumption of Winterspawn that drinks\n" +
                 "considerably more water than the other plants. Should be set to a positive float value"); // Description of the option to show in the config file

            PlantWaterConsumptionLimit = Mathf.Clamp(configPlantWaterConsumptionLimit.Value, 0.000001f, 100000f);

            configPlantWaterTranspirationPercentage = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterTranspirationPercentage",  // The key of the configuration option in the configuration file
                 25f, // The default value
                 "This value set the percentage of the water consumed by plants that should be transpirated back to the atmosphere.\n" +
                 "Can be a float number between 0 and 100. Set it to 0 to disable plants water transpiration."); // Description of the option to show in the config file

            PlantWaterTranspirationPercentage = Mathf.Clamp(configPlantWaterTranspirationPercentage.Value, 0f, 100f);

            configAtmosphereFogThreshold = PnN.Config.Bind("2 - Fog Configuration", // The section under which the option is shown 
                 "AtmosphereFogThreshold",  // The key of the configuration option in the configuration file
                 5f, // The default value
                 "This value set the minimum amount of moles needed to start showing the fog effect in the atmosphere. The Vanilla behaviour is to show the effect when there's any\n" +
                 "amount of liquid in atmosphere thus making any greenhouse who have plants transpirating water to always look foggy. Also note that this setting will affect the fog\n" +
                 "visualization for *ALL* liquids in the atmosphere, not just water. Must be a float number between 0 and 100. Setting this to 0 will keep the Vanilla effect."); // Description of the option to show in the config file

            AtmosphereFogThreshold = Mathf.Clamp(configAtmosphereFogThreshold.Value, 0f, 1000f);

            configNutritionLossMultiplier = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "NutritionLossMultiplier", // The key of the configuration option in the configuration file
                1f,
                "This value is a multiplier for the nutrition loss per tick of the character.\n" +
                "For example, on Stationeers difficulty, by default, you'll get a nutrition loss of 0.104167 wich will give you ~8 days of hunger. If you change this value for 2,\n" +
                "your hunger will last only 4 days or if you set this value for 0.5, your character full hunger will last for 16 days on Stationeers Difficulty.\n" +
                "You'll want to change this option mainly if you also change the max nutrition of the character.\n" +
                "Just remember that, if you change this, it will also greatly affect the amount of plants you need to grow to keep your character alive.");

            NutritionLossMultiplier = configNutritionLossMultiplier.Value;


            configMaxNutritionStorage = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "MaxNutritionStorage", // The key of the configuration option in the configuration file
                4000f,
                "This set the max amount of nutrition the character can store. If you change this value, it's highly advisable to start a new save or edit the character\n" +
                "<Nutrition> tag directly into the save (world.xml file) to adjust your character nutrition value to be inside the max value you set here.\n" +
                "This greatly affects the amount of filling each food gives and also how long your starting 100% nutrition will last. For example, if you change\n" +
                "this to 2000, potatoes will fill 2% of your food (instead of 1%) but on Stationeers difficulty your character will get only 4 days worth of it's initial nutrition," +
                "instead of 8 days.Your Nutrition will also appear to drop 2x faster (in percentage) because now your max nutrition is lower.");
            MaxNutritionStorage = configMaxNutritionStorage.Value;

            configMaxHydrationStorage = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "MaxHydrationStorage", // The key of the configuration option in the configuration file
                42f,
                "This set the max amount of hydration the character can store. If you change this value, it's advisable to start a new save or edit the character\n" +
                "<Hydration> tag directly into the save (world.xml file) to move your character hydration value to be inside the max value you set here.\n" +
                "This greatly affects the amount of filling each water canister gives you and also how long your starting 100% hydration will last. For example, if you change\n" +
                "this to 21, a full water canister will now fill 36% of your water (instead of 18%) but on Stationeers difficulty your character will get only 1¼ days worth of hydration." +
                "Your Hydration will also appear to drop faster (in percentage) because now your max hydration is lower.");
            MaxHydrationStorage = configMaxHydrationStorage.Value;

            configWarningNutrition = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "WarningNutrition", // The key of the configuration option in the configuration file
                20f,
                "This sets after what percentage of hunger the \"Hunger Warning\" alert should show up. Values can be set between 0 and 50");
            WarningNutrition = Mathf.Clamp(configWarningNutrition.Value, 0f, 50f);

            configCriticalNutrition = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "CriticalNutrition", // The key of the configuration option in the configuration file
                10f,
                "This sets after what percentage of hunger the \"Hunger Critical\" alert should show up. Values can be set between 0 and 50");
            CriticalNutrition = Mathf.Clamp(configCriticalNutrition.Value, 0f, 50f);

            configWarningHydration = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "WarningHydration", // The key of the configuration option in the configuration file
                25f,
                "This sets after what percentage of hydration the \"Hydration Warning\" alert should show up. Values can be set between 0 and 50");
            WarningHydration = Mathf.Clamp(configWarningHydration.Value, 0f, 50f);

            configCriticalHydration = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "CriticalHydration", // The key of the configuration option in the configuration file
                12.5f,
                "This sets after what percentage of hydration the \"Hydration Critical\" alert should show up. Values can be set between 0 and 50");
            CriticalHydration = Mathf.Clamp(configCriticalHydration.Value, 0f, 50f);


            configEnableRespawnPenaltyLogic = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "EnableRespawnPenaltyLogic", // The key of the configuration option in the configuration file
                true,
                "This option enable or disable the respawn penalty logic of the mod. On Vanilla, players who die are rewarded with 100% nutrition/Hydration stats." +
                "This issue is even proeminent with the Plants and Nutrition mod because the mod considerably increases the amount of Nutrition/Hydration a player " +
                "can have.\n" +
                "If this option is set to true, characters who die will respawn with nutrition and hydration proportional to the days past in the save. The mod will take into account" +
                "the difficulty of the save, the maxNutritionStorage, the current day and other factors and try to \"Guess\" what your hydration/nutrition should be. For example, on a Normal" +
                "difficulty save, default mod values and sun/orbit in 2, your character will have 10 days worth of food, and will loose 10% food each day. If you die on day 6, the mod " +
                "will respawn the player with 40% food, because that's what your initial nutrition should approximately be. After day 10, dead characters will respawn with minimal food," +
                "just enough to run to the pantry room to eat something.\n" +
                "If set to false, the Vanilla behaviour will be kept so, after death, you will always respawn with full nutrition/hydration.");
            EnableRespawnPenaltyLogic = configEnableRespawnPenaltyLogic.Value;

            configCustomNewPlayerRespawn = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawn", // The key of the configuration option in the configuration file
                false,
                "This option control if new players who never played on the save should join with nutrition/hydration proportional to the days past in the save or with custom food amounts set" +
                "in CustomNewPlayerRespawnNutrition and CustomNewPlayerRespawnHydration. If set to false, when new players join, they will respawn with food proportional to the days past in the" +
                "save. You'll want to enable this option mostly on dedicated servers where new players will join and start playing after the server is running for a long time, on saves where each" +
                "player make separated bases, and you want them to join with full food but still penalize them if they die.\n" +
                "This option only work if EnableRespawnPenaltyLogic is also true, otherwise everyone will always respawn with 100% nutrition/hydration.");
            CustomNewPlayerRespawn = configCustomNewPlayerRespawn.Value;

            configCustomNewPlayerRespawnNutrition = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawnNutrition", // The key of the configuration option in the configuration file
                100f,
                "If CustomNewPlayerRespawn is true, this option set with how much nutrition, in percent, new players should join.");
            CustomNewPlayerRespawnNutrition = configCustomNewPlayerRespawnNutrition.Value;

            configCustomNewPlayerRespawnHydration = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawnHydration", // The key of the configuration option in the configuration file
                100f,
                "If CustomNewPlayerRespawn is true, this option set with how much hydration, in percent, new players should join.");
            CustomNewPlayerRespawnHydration = configCustomNewPlayerRespawnHydration.Value;




            mfe = PnN.Config.Bind("3 - Foods Configuration", "food Enter", 0f, "Sets the initial game difficulty multiplier(how much food you`ll strated).\n values between 0 and [Max stomach]. \n 0 disable this configuration and put like the death system. ");
            mhe = PnN.Config.Bind("3 - Foods Configuration", "Hidration Enter", 0f, "Sets the initial game difficulty multiplier(how much food you`ll strated).\n values between 0 and 42. \n 0 disable this configuration and put like the death system. ");

            mfd = PnN.Config.Bind("3 - Foods Configuration", "Min food die", 0f, "Sets the initial game difficulty how much food minimum you get when you die.\n values between 0 and 1. \n minimmun 1%, 0 disable this function and come with predefination in mod. ");
            mhd = PnN.Config.Bind("3 - Foods Configuration", "Min Hidration die", 0f, "Sets the initial game difficulty how much Hidration minimum you get when you die.\n values between 0 and 1. \n minimmun 1%, 0 disable this function and come with predefination in mod. ");


            ddm = PnN.Config.Bind("3 - Foods Configuration", "Days Death multiplier", 10f, "Sets the initial game difficulty multiplier.\n Defines the proportion of drop in days of food in case you die. \n The default is 10 which gives 20 game days. ");



            fConfigsFood.Add("DDM", ddm.Value);
            fConfigsFood.Add("MFE", mfe.Value);
            fConfigsFood.Add("MHE", mhe.Value);
            fConfigsFood.Add("MFD", mfd.Value);
            fConfigsFood.Add("MHD", mhd.Value);

            configFoods = configPlants = PnN.Config.Bind("3 - Foods Configuration", "Enable food config", true, "Enable food config");

            configTomatoSoup = PnN.Config.Bind("3 - Foods Configuration", "Tomato Soup", 135f, "Amount of food nutrition");
            configTomatoSoupES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Tomato Soup", 0.04f, "speed factor when eating the food");

            configCornSoup = PnN.Config.Bind("3 - Foods Configuration", "Corn Soup", 223f, "Amount of food nutrition");
            configCornSoupES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Corn Soup", 0.04f, "d");

            configCannedRicePudding = PnN.Config.Bind("3 - Foods Configuration", "Canned Rice Pudding", 220f, "Amount of food nutrition");
            configCannedRicePuddingES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Canned Rice Pudding", 0.04f, "Eat speed Corn Soup");

            configPumpkinSoup = PnN.Config.Bind("3 - Foods Configuration", "Pumpkin Soup", 270f, "Amount of food nutrition");
            configPumpkinSoupES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Pumpkin Soup", 0.04f, "Eat speed Corn Soup");

            configPumpkinPie = PnN.Config.Bind("3 - Foods Configuration", "Pumpkin Pie", 800f, "Amount of food nutrition");
            configPumpkinPieES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Pumpkin Pie", 0.02f, "Eat speed Corn Soup");

            configBakedPotato = PnN.Config.Bind("3 - Foods Configuration", "Baked Potato", 45f, "Amount of food nutrition");
            configBakedPotatoES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Baked Potato", 0.015f, "Eat speed Corn Soup");

            configFrenchFries = PnN.Config.Bind("3 - Foods Configuration", "French Fries", 85f, "Amount of food nutrition");
            configFrenchFriesES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed French Fries", 0.02f, "Eat speed Corn Soup");

            configCannedFrenchFries = PnN.Config.Bind("3 - Foods Configuration", "Canned French Fries", 150f, "Amount of food nutrition");
            configCannedFrenchFriesES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Canned French Fries", 0.04f, "Eat speed Corn Soup");

            configMilk = PnN.Config.Bind("3 - Foods Configuration", "Milk", 2.3f, "Amount of food nutrition");
            configMilkES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Milk", 0.015f, "Eat speed Corn Soup");

            configCannedCondensedMilk = PnN.Config.Bind("3 - Foods Configuration", "Canned Condensed Milk", 400f, "Amount of food nutrition");
            configCannedCondensedMilkES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Canned Condensed Milk", 0.04f, "Eat speed Corn Soup");

            configMuffin = PnN.Config.Bind("3 - Foods Configuration", "Muffin", 570f, "Amount of food nutrition");
            configMuffinES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Muffin", 0.02f, "Eat speed Corn Soup");

            configBreadLoaf = PnN.Config.Bind("3 - Foods Configuration", "Bread Loaf", 290f, "Amount of food nutrition");
            configBreadLoafES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Bread Loaf", 0.02f, "Eat speed Corn Soup");

            configCerealBar = PnN.Config.Bind("3 - Foods Configuration", "Cereal Bar", 110f, "Amount of food nutrition");
            configCerealBarES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cereal Bar", 0.04f, "Eat speed Corn Soup");

            configCannedPowderedEggs = PnN.Config.Bind("3 - Foods Configuration", "Canned Powdered Eggs", 550f, "Amount of food nutrition");
            configCannedPowderedEggsES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Canned Powdered Eggs", 0.04f, "Eat speed Corn Soup");

            configCannedEdamame = PnN.Config.Bind("3 - Foods Configuration", "Canned Edamame", 100f, "Amount of food nutrition");
            configCannedEdamameES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Canned Edamame", 0.04f, "Eat speed Corn Soup");

            configCondensedMilk = PnN.Config.Bind("3 - Foods Configuration", "Condensed Milk", 235f, "Amount of food nutrition");
            configCondensedMilkES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Condensed Milk", 0.015f, "Eat speed Corn Soup");

            configCookedSoybean = PnN.Config.Bind("3 - Foods Configuration", "Cooked Soybean", 38f, "Amount of food nutrition");
            configCookedSoybeanES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cooked Soybean", 0.015f, "Eat speed Corn Soup");

            configCookedRice = PnN.Config.Bind("3 - Foods Configuration", "Cooked Rice", 50f, "Amount of food nutrition");
            configCookedRiceES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cooked Rice", 0.015f, "Eat speed Corn Soup");

            configCookedCorn = PnN.Config.Bind("3 - Foods Configuration", "Cooked Corn", 52f, "Amount of food nutrition");
            configCookedCornES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cooked Corn", 0.015f, "Eat speed Corn Soup");

            configCookedPumpkin = PnN.Config.Bind("3 - Foods Configuration", "Cooked Pumpkin", 60f, "Amount of food nutrition");
            configCookedPumpkinES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cooked Pumpkin", 0.015f, "Eat speed Corn Soup");

            configPowderedEggs = PnN.Config.Bind("3 - Foods Configuration", "Powdered Eggs", 330f, "Amount of food nutrition");
            configPowderedEggsES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Powdered Eggs", 0.015f, "Eat speed Corn Soup");

            configCookedTomato = PnN.Config.Bind("3 - Foods Configuration", "Cooked Tomato", 30f, "Amount of food nutrition");
            configCookedTomatoES = PnN.Config.Bind("3 - Foods Configuration", "Eat speed Cooked Tomato", 0.015f, "Eat speed Corn Soup");

            fTomatoSoup.Add("NUTV", configTomatoSoup.Value);
            fTomatoSoup.Add("SEAT", configTomatoSoupES.Value);

            fCornSoup.Add("NUTV", configCornSoup.Value);
            fCornSoup.Add("SEAT", configCornSoupES.Value);

            fCannedRicePudding.Add("NUTV", configCannedRicePudding.Value);
            fCannedRicePudding.Add("SEAT", configCannedRicePuddingES.Value);

            fPumpkinSoup.Add("NUTV", configPumpkinSoup.Value);
            fPumpkinSoup.Add("SEAT", configPumpkinSoupES.Value);

            fPumpkinPie.Add("NUTV", configPumpkinPie.Value);
            fPumpkinPie.Add("SEAT", configPumpkinPieES.Value);

            fBakedPotato.Add("NUTV", configBakedPotato.Value);
            fBakedPotato.Add("SEAT", configBakedPotatoES.Value);

            fFrenchFries.Add("NUTV", configFrenchFries.Value);
            fFrenchFries.Add("SEAT", configFrenchFriesES.Value);

            fCannedFrenchFries.Add("NUTV", configCannedFrenchFries.Value);
            fCannedFrenchFries.Add("SEAT", configCannedFrenchFriesES.Value);

            fMilk.Add("NUTV", configMilk.Value);
            fMilk.Add("SEAT", configMilkES.Value);

            fCannedCondensedMilk.Add("NUTV", configCannedCondensedMilk.Value);
            fCannedCondensedMilk.Add("SEAT", configCannedCondensedMilkES.Value);

            fMuffin.Add("NUTV", configMuffin.Value);
            fMuffin.Add("SEAT", configMuffinES.Value);

            fBreadLoaf.Add("NUTV", configBreadLoaf.Value);
            fBreadLoaf.Add("SEAT", configBreadLoafES.Value);

            fCerealBar.Add("NUTV", configCerealBar.Value);
            fCerealBar.Add("SEAT", configCerealBarES.Value);

            fCannedPowderedEggs.Add("NUTV", configCannedPowderedEggs.Value);
            fCannedPowderedEggs.Add("SEAT", configCannedPowderedEggsES.Value);

            fCannedEdamame.Add("NUTV", configCannedEdamame.Value);
            fCannedEdamame.Add("SEAT", configCannedEdamameES.Value);

            fCondensedMilk.Add("NUTV", configCondensedMilk.Value);
            fCondensedMilk.Add("SEAT", configCondensedMilkES.Value);

            fCookedSoybean.Add("NUTV", configCookedSoybean.Value);
            fCookedSoybean.Add("SEAT", configCookedSoybeanES.Value);

            fCookedRice.Add("NUTV", configCookedRice.Value);
            fCookedRice.Add("SEAT", configCookedRiceES.Value);

            fCookedCorn.Add("NUTV", configCookedCorn.Value);
            fCookedCorn.Add("SEAT", configCookedCornES.Value);

            fCookedPumpkin.Add("NUTV", configCookedPumpkin.Value);
            fCookedPumpkin.Add("SEAT", configCookedPumpkinES.Value);

            fPowderedEggs.Add("NUTV", configPowderedEggs.Value);
            fPowderedEggs.Add("SEAT", configPowderedEggsES.Value);

            fCookedTomato.Add("NUTV", configCookedTomato.Value);
            fCookedTomato.Add("SEAT", configCookedTomatoES.Value);

            //plants
            configPlants = PnN.Config.Bind("3 - Foods Configuration", "Enable plant config", false, "Enable plant config");
            configWheat = PnN.Config.Bind("3 - Foods Configuration", "Wheat", 2f, "Amount of food nutrition");
            configCorn = PnN.Config.Bind("3 - Foods Configuration", "Corn", 2f, "Amount of food nutrition");
            configFern = PnN.Config.Bind("3 - Foods Configuration", "Fern", 2f, "Amount of food nutrition");
            configMushroom = PnN.Config.Bind("3 - Foods Configuration", "Mushroom", 2f, "Amount of food nutrition");
            configPotato = PnN.Config.Bind("3 - Foods Configuration", "Potato", 2f, "Amount of food nutrition");
            configPumpkin = PnN.Config.Bind("3 - Foods Configuration", "Pumpkin", 2f, "Amount of food nutrition");
            configRice = PnN.Config.Bind("3 - Foods Configuration", "Rice", 2f, "Amount of food nutrition");
            configSoybean = PnN.Config.Bind("3 - Foods Configuration", "Soybean", 2f, "Amount of food nutrition");
            configTomato = PnN.Config.Bind("3 - Foods Configuration", "Tomato", 2f, "Amount of food nutrition");

            fConfigsFood.Add("PCE", configPlants.Value);
            fWheat.Add("NUTV", configWheat.Value);
            fCorn.Add("NUTV", configCorn.Value);
            fFern.Add("NUTV", configFern.Value);
            fMushroom.Add("NUTV", configMushroom.Value);
            fPotato.Add("NUTV", configPotato.Value);
            fPumpkin.Add("NUTV", configPumpkin.Value);
            fRice.Add("NUTV", configRice.Value);
            fSoybean.Add("NUTV", configSoybean.Value);
            fTomato.Add("NUTV", configTomato.Value);
            fConfigsFood.Add("FCE", configFoods.Value);

            //decay
            /* not implemented
            configDecay = PnN.Config.Bind("3 - Foods Configuration", "Enable Decay foods config", false, "Enable Decay foods config (beta)");
            DecayFactor = PnN.Config.Bind("3 - Foods Configuration", "Debuff of Decay rate", 25.0f, "Debuff factor of Decay rate in porcentage, value 0 - 100");

            fConfigsFood.Add("DCE", configDecay.Value);
            fDecayFactor.Add("GDF", DecayFactor.Value);
            */
        }
    }
}
