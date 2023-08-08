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

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("PlantsnNutrition", "Plants and Nutrition", "0.9.5.0")]
    public class PlantsnNutritionRebalancePlugin : BaseUnityPlugin
    {
        public static PlantsnNutritionRebalancePlugin Instance;
        public void Log(string line)
        {
            Debug.Log("[PlantsnNutritionRebalance]: " + line);
        }

        private void Awake()
        {
            PlantsnNutritionRebalancePlugin.Instance = this;
            Log("Hello World");
            Handleconfig();
            var harmony = new Harmony("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts");
            harmony.PatchAll();
            Prefab.OnPrefabsLoaded += ApplyPatchesWhenPrefabsLoaded;
            Log("Patch succeeded");
        }

        private ConfigEntry<float> configPlantWaterConsumptionMultiplier;
        private ConfigEntry<float> configPlantWaterConsumptionLimit;
        private ConfigEntry<float> configPlantWaterTranspirationPercentage;
        private ConfigEntry<float> configAtmosphereFogThreshold;

        //------------------------------ foods----------------------------------------------
        private ConfigEntry<bool> configFoods;
        private ConfigEntry<float> configTomatoSoup;
        private ConfigEntry<float> configCornSoup;
        private ConfigEntry<float> configCannedRicePudding;
        private ConfigEntry<float> configPumpkinSoup;
        private ConfigEntry<float> configPumpkinPie;
        private ConfigEntry<float> configBakedPotato;
        private ConfigEntry<float> configFrenchFries;
        private ConfigEntry<float> configCannedFrenchFries;
        private ConfigEntry<float> configMilk;
        private ConfigEntry<float> configCannedCondensedMilk;
        private ConfigEntry<float> configMuffin;
        private ConfigEntry<float> configBreadLoaf;
        private ConfigEntry<float> configCerealBar;
        private ConfigEntry<float> configCannedPowderedEggs;
        private ConfigEntry<float> configCannedEdamame;
        private ConfigEntry<float> configCondensedMilk;
        private ConfigEntry<float> configCookedSoybean;
        private ConfigEntry<float> configCookedRice;
        private ConfigEntry<float> configCookedCorn;
        private ConfigEntry<float> configCookedPumpkin;
        private ConfigEntry<float> configPowderedEggs;
        private ConfigEntry<float> configCookedTomato;

        private ConfigEntry<float> configTomatoSoupES;
        private ConfigEntry<float> configCornSoupES;
        private ConfigEntry<float> configCannedRicePuddingES;
        private ConfigEntry<float> configPumpkinSoupES;
        private ConfigEntry<float> configPumpkinPieES;
        private ConfigEntry<float> configBakedPotatoES;
        private ConfigEntry<float> configFrenchFriesES;
        private ConfigEntry<float> configCannedFrenchFriesES;
        private ConfigEntry<float> configMilkES;
        private ConfigEntry<float> configCannedCondensedMilkES;
        private ConfigEntry<float> configMuffinES;
        private ConfigEntry<float> configBreadLoafES;
        private ConfigEntry<float> configCerealBarES;
        private ConfigEntry<float> configCannedPowderedEggsES;
        private ConfigEntry<float> configCannedEdamameES;
        private ConfigEntry<float> configCondensedMilkES;
        private ConfigEntry<float> configCookedSoybeanES;
        private ConfigEntry<float> configCookedRiceES;
        private ConfigEntry<float> configCookedCornES;
        private ConfigEntry<float> configCookedPumpkinES;
        private ConfigEntry<float> configPowderedEggsES;
        private ConfigEntry<float> configCookedTomatoES;
        private ConfigEntry<float> configmaxDaysHunger;
        private ConfigEntry<float> configmaxfoodPlayer;

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
        private ConfigEntry<bool> configPlants;
        private ConfigEntry<float> configWheat;
        private ConfigEntry<float> configCorn;
        private ConfigEntry<float> configFern;
        private ConfigEntry<float> configMushroom;
        private ConfigEntry<float> configPotato;
        private ConfigEntry<float> configPumpkin;
        private ConfigEntry<float> configRice;
        private ConfigEntry<float> configSoybean;
        private ConfigEntry<float> configTomato;

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



        // ---------------------------------- foods ---------------------------------------

        public static float PlantWaterConsumptionMultiplier;
        public static float PlantWaterConsumptionLimit;
        public static float PlantWaterTranspirationPercentage;
        public static float AtmosphereFogThreshold;

        private void Handleconfig() // Create and manage the configuration file parameters
        {
            configPlantWaterConsumptionMultiplier = Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionMultiplier",  // The key of the configuration option in the configuration file
                 500f, // The default value
                 "By how much this mod should multiply the water consumption of plants?\n" +
                 "The vanilla water consumption value is aprox ~0.000006 moles per tick for most plants, quite low. For reference, 1 ice water stack has 1000 mols\n" +
                 "That means, in vanilla, 1 single stack of ice will keep a plant alive for more than 23148 hours of gameplay! That's why the suggested value here\n" +
                 "is 500, it increases the plants drinks to ~0.003 moles of water per tick. With this, 1 ice water stack will keep a plant alive for 46 hours of gameplay,\n" +
                 "or 20 plants for 2 hours, enough to make the water management meaningful.\n" +
                 "Set this option to 1 to keep the vanilla water consumption on plants"); // Description of the option to show in the config file

            PlantWaterConsumptionMultiplier = Mathf.Clamp(configPlantWaterConsumptionMultiplier.Value, 1f, 100000f);

            configPlantWaterConsumptionLimit = Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionLimit",  // The key of the configuration option in the configuration file
                 0.004f, // The default value
                 "Limit the max consumption of water mols per tick a plant can drink. This is mainly to fix the behaviour of the water consumption of Winterspawn that drinks\n" +
                 "considerably more water than the other plants. Should be set to a positive float value"); // Description of the option to show in the config file

            PlantWaterConsumptionLimit = Mathf.Clamp(configPlantWaterConsumptionLimit.Value, 0.000001f, 100000f);

            configPlantWaterTranspirationPercentage = Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterTranspirationPercentage",  // The key of the configuration option in the configuration file
                 25f, // The default value
                 "This value set the percentage of the water consumed by plants that should be transpirated back to the atmosphere.\n" +
                 "Can be a float number between 0 and 100. Set it to 0 to disable plants water transpiration."); // Description of the option to show in the config file

            PlantWaterTranspirationPercentage = Mathf.Clamp(configPlantWaterTranspirationPercentage.Value, 0f, 100f);

            configAtmosphereFogThreshold = Config.Bind("2 - Fog Configuration", // The section under which the option is shown 
                 "AtmosphereFogThreshold",  // The key of the configuration option in the configuration file
                 5f, // The default value
                 "This value set the minimum amount of moles needed to start showing the fog effect in the atmosphere. The Vanilla behaviour is to show the effect when there's any\n" +
                 "amount of liquid in atmosphere thus making any greenhouse who have plants transpirating water to always look foggy. Also note that this setting will affect the fog\n" +
                 "visualization for *ALL* liquids in the atmosphere, not just water. Must be a float number between 0 and 100. Setting this to 0 will keep the Vanilla effect."); // Description of the option to show in the config file

            AtmosphereFogThreshold = Mathf.Clamp(configAtmosphereFogThreshold.Value, 0f, 1000f);

            //------------------------------------ food config-------------------------------------------

            configmaxDaysHunger = Config.Bind("3 - Foods Configuration", "Max days Hungred", 0.055555f, "Values between 0.000001f and 0.208334f \n 0.208334f = 4days and 0.055555f = 14days\n closer to 0f greater hunger time \n predefined values by game difficulty when HungerRate is at 0.5f = 12 game days, 1f = 10 game days, 1.5f = 8 game days");
            configmaxfoodPlayer = Config.Bind("3 - Foods Configuration", "Max stomach", 4000f, "Value defines how much more food you can eat.\n with 4000 you need to eat on average to fill the bar \n 4 Canned Edamame or 8 Cooked Rice ");

            fConfigsFood.Add("MDH", configmaxDaysHunger.Value);
            fConfigsFood.Add("MF", configmaxfoodPlayer.Value);

            configFoods = configPlants = Config.Bind("3 - Foods Configuration", "Enable food config", true, "Enable food config");

            configTomatoSoup = Config.Bind("3 - Foods Configuration", "Tomato Soup", 135f, "Amount of food nutrition");
            configTomatoSoupES = Config.Bind("3 - Foods Configuration", "Tomato Soup", 0.04f, "speed factor when eating the food");

            configCornSoup = Config.Bind("3 - Foods Configuration", "Corn Soup", 223f, "Amount of food nutrition");
            configCornSoupES = Config.Bind("3 - Foods Configuration", "Eat speed Corn Soup", 0.04f, "d");

            configCannedRicePudding = Config.Bind("3 - Foods Configuration", "Canned Rice Pudding", 220f, "Amount of food nutrition");
            configCannedRicePuddingES = Config.Bind("3 - Foods Configuration", "Eat speed Canned Rice Pudding", 0.04f, "Eat speed Corn Soup");

            configPumpkinSoup = Config.Bind("3 - Foods Configuration", "Pumpkin Soup", 270f, "Amount of food nutrition");
            configPumpkinSoupES = Config.Bind("3 - Foods Configuration", "Eat speed Pumpkin Soup", 0.04f, "Eat speed Corn Soup");

            configPumpkinPie = Config.Bind("3 - Foods Configuration", "Pumpkin Pie", 800f, "Amount of food nutrition");
            configPumpkinPieES = Config.Bind("3 - Foods Configuration", "Eat speed Pumpkin Pie", 0.02f, "Eat speed Corn Soup");

            configBakedPotato = Config.Bind("3 - Foods Configuration", "Baked Potato", 45f, "Amount of food nutrition");
            configBakedPotatoES = Config.Bind("3 - Foods Configuration", "Eat speed Baked Potato", 0.015f, "Eat speed Corn Soup");

            configFrenchFries = Config.Bind("3 - Foods Configuration", "French Fries", 85f, "Amount of food nutrition");
            configFrenchFriesES = Config.Bind("3 - Foods Configuration", "Eat speed French Fries", 0.02f, "Eat speed Corn Soup");

            configCannedFrenchFries = Config.Bind("3 - Foods Configuration", "Canned French Fries", 150f, "Amount of food nutrition");
            configCannedFrenchFriesES = Config.Bind("3 - Foods Configuration", "Eat speed Canned French Fries", 0.04f, "Eat speed Corn Soup");

            configMilk = Config.Bind("3 - Foods Configuration", "Milk", 2.3f, "Amount of food nutrition");
            configMilkES = Config.Bind("3 - Foods Configuration", "Eat speed Milk", 0.015f, "Eat speed Corn Soup");

            configCannedCondensedMilk = Config.Bind("3 - Foods Configuration", "Canned Condensed Milk", 400f, "Amount of food nutrition");
            configCannedCondensedMilkES = Config.Bind("3 - Foods Configuration", "Eat speed Canned Condensed Milk", 0.04f, "Eat speed Corn Soup");

            configMuffin = Config.Bind("3 - Foods Configuration", "Muffin", 570f, "Amount of food nutrition");
            configMuffinES = Config.Bind("3 - Foods Configuration", "Eat speed Muffin", 0.02f, "Eat speed Corn Soup");

            configBreadLoaf = Config.Bind("3 - Foods Configuration", "Bread Loaf", 290f, "Amount of food nutrition");
            configBreadLoafES = Config.Bind("3 - Foods Configuration", "Eat speed Bread Loaf", 0.02f, "Eat speed Corn Soup");

            configCerealBar = Config.Bind("3 - Foods Configuration", "Cereal Bar", 110f, "Amount of food nutrition");
            configCerealBarES = Config.Bind("3 - Foods Configuration", "Eat speed Cereal Bar", 0.04f, "Eat speed Corn Soup");

            configCannedPowderedEggs = Config.Bind("3 - Foods Configuration", "Canned Powdered Eggs", 550f, "Amount of food nutrition");
            configCannedPowderedEggsES = Config.Bind("3 - Foods Configuration", "Eat speed Canned Powdered Eggs", 0.04f, "Eat speed Corn Soup");

            configCannedEdamame = Config.Bind("3 - Foods Configuration", "Canned Edamame", 100f, "Amount of food nutrition");
            configCannedEdamameES = Config.Bind("3 - Foods Configuration", "Eat speed Canned Edamame", 0.04f, "Eat speed Corn Soup");

            configCondensedMilk = Config.Bind("3 - Foods Configuration", "Condensed Milk", 235f, "Amount of food nutrition");
            configCondensedMilkES = Config.Bind("3 - Foods Configuration", "Eat speed Condensed Milk", 0.015f, "Eat speed Corn Soup");

            configCookedSoybean = Config.Bind("3 - Foods Configuration", "Cooked Soybean", 38f, "Amount of food nutrition");
            configCookedSoybeanES = Config.Bind("3 - Foods Configuration", "Eat speed Cooked Soybean", 0.015f, "Eat speed Corn Soup");

            configCookedRice = Config.Bind("3 - Foods Configuration", "Cooked Rice", 50f, "Amount of food nutrition");
            configCookedRiceES = Config.Bind("3 - Foods Configuration", "Eat speed Cooked Rice", 0.015f, "Eat speed Corn Soup");

            configCookedCorn = Config.Bind("3 - Foods Configuration", "Cooked Corn", 52f, "Amount of food nutrition");
            configCookedCornES = Config.Bind("3 - Foods Configuration", "Eat speed Cooked Corn", 0.015f, "Eat speed Corn Soup");

            configCookedPumpkin = Config.Bind("3 - Foods Configuration", "Cooked Pumpkin", 60f, "Amount of food nutrition");
            configCookedPumpkinES = Config.Bind("3 - Foods Configuration", "Eat speed Cooked Pumpkin", 0.015f, "Eat speed Corn Soup");

            configPowderedEggs = Config.Bind("3 - Foods Configuration", "Powdered Eggs", 330f, "Amount of food nutrition");
            configPowderedEggsES = Config.Bind("3 - Foods Configuration", "Eat speed Powdered Eggs", 0.015f, "Eat speed Corn Soup");

            configCookedTomato = Config.Bind("3 - Foods Configuration", "Cooked Tomato", 30f, "Amount of food nutrition");
            configCookedTomatoES = Config.Bind("3 - Foods Configuration", "Eat speed Cooked Tomato", 0.015f, "Eat speed Corn Soup");

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
            configPlants = Config.Bind("3 - Foods Configuration", "Enable plant config", false, "Enable plant config");
            configWheat = Config.Bind("3 - Foods Configuration", "Wheat", 2f, "Amount of food nutrition");
            configCorn = Config.Bind("3 - Foods Configuration", "Corn", 2f, "Amount of food nutrition");
            configFern = Config.Bind("3 - Foods Configuration", "Fern", 2f, "Amount of food nutrition");
            configMushroom = Config.Bind("3 - Foods Configuration", "Mushroom", 2f, "Amount of food nutrition");
            configPotato = Config.Bind("3 - Foods Configuration", "Potato", 2f, "Amount of food nutrition");
            configPumpkin = Config.Bind("3 - Foods Configuration", "Pumpkin", 2f, "Amount of food nutrition");
            configRice = Config.Bind("3 - Foods Configuration", "Rice", 2f, "Amount of food nutrition");
            configSoybean = Config.Bind("3 - Foods Configuration", "Soybean", 2f, "Amount of food nutrition");
            configTomato = Config.Bind("3 - Foods Configuration", "Tomato", 2f, "Amount of food nutrition");

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
            configDecay = Config.Bind("3 - Foods Configuration", "Enable Decay foods config", false, "Enable Decay foods config (beta)");
            DecayFactor = Config.Bind("3 - Foods Configuration", "Debuff of Decay rate", 25.0f, "Debuff factor of Decay rate in porcentage, value 0 - 100");

            fConfigsFood.Add("DCE", configDecay.Value);
            fDecayFactor.Add("GDF", DecayFactor.Value);
            */

//------------------------------------ food congif-------------------------------------------
        }

        private void ApplyPatchesWhenPrefabsLoaded()
        {
            Log("Applying plants patches after prefabs are loaded");
            PlantGrowStagePatch.PatchPrefabs();
        }


        // Adjust the plants growth stages
        public class PlantGrowStagePatch
        {
            //plant & seed
            private static int[] cornStages = { 1, 1600, 2400, 3200, 2400, 2400, -1, 0 }; //258339687 & -1290755415
            private static int[] tomatoStages = { 1, 1600, 3200, 4800, 4800, 2400, -1, 0 }; //-998592080 & -1922066841
            private static int[] pumpkinStages = { 1, 2400, 4800, 4800, 9600, 2400, -1, 0 }; //1277828144 & 1423199840
            private static int[] riceStages = { 1, 2400, 3200, 4000, 2400, -1, 0 }; //658916791 & -1691151239
            private static int[] soyStages = { 1, 2400, 2400, 2400, 2400, -1, 0 }; //1924673028 & 1783004244
            private static int[] fernStages = { 1, 1200, 1600, 2000, 1200, -1, 0 }; //892110467 & -1990600883
            private static int[] potatoStages = { 1, 1200, 2400, 2400, 1200, -1, 0 }; //1929046963 & 1005571172
            private static int[] wheatStages = { 1, 2400, 4000, 3200, 2400, -1, 0 }; //-1057658015 & -654756733

            private static int[] flowerStages = { 1, 2400, 2400, 2400, -1, 0 }; //1712822019,-81376085,-1411986716,-1513337058,-1573623434

            private static int[] filterFernStages = { 1, 2400, 3600, 4800, 3600, -1, 0 }; //266654416
            private static int[] tropicalLilyPlantStages = { 1, 2400, 2400, 3600, 2400, -1, 0 }; //-800947386
            private static int[] peaceLilyPlantStages = { 1, 2400, 2400, 3600, 2400, -1, 0 }; //2042955224 
            private static int[] alienMushroomStages = { 1, 2400, 3600, 4800, -1, 0 }; //176446172 
            //private static int[] mushroomStages = { 1, 1200, 1200, 1200, 1200, -1, 0 }; //2044798572
            private static int[] thermogenicGenepool1Stages = { 1, 2400, 2400, 3600, 3600, 2400, -1, 0 }; //-177792789 
            private static int[] thermogenicGenepool2Stages = { 1, 2400, 2400, 3600, 3600, 2400, -1, 0 }; //1819167057 
            private static int[] thermogenicCreativeStages = { 1, 2400, 2400, 3600, 3600, 2400, -1, 0 }; //-1208890208 
            private static int[] endothermicGenepool1Stages = { 1, 1200, 1200, 2400, 2400, 2400, -1, 0 }; //851290561 
            private static int[] endothermicGenepool2Stages = { 1, 2400, 2400, 3600, 2400, -1, 0 }; //-1414203269 
            private static int[] endothermicCreativeStages = { 1, 2400, 3600, 3600, 2400, -1, 0 }; //-1159179557
            private static int[] switchGrassStages = { 1, 800, 800, 2400, 800, -1, 0 }; //-532672323



            private static Dictionary<int, int[]> plantStages = new Dictionary<int, int[]>();
            static PlantGrowStagePatch()
            {
                //seedbag seeds
                plantStages.Add(-1290755415, cornStages);
                plantStages.Add(-1922066841, tomatoStages);
                plantStages.Add(1423199840, pumpkinStages);
                plantStages.Add(-1691151239, riceStages);
                plantStages.Add(1783004244, soyStages);
                plantStages.Add(-1990600883, fernStages);
                plantStages.Add(1005571172, potatoStages);
                plantStages.Add(-654756733, wheatStages);
                //seedbag plants
                plantStages.Add(258339687, cornStages);
                plantStages.Add(-998592080, tomatoStages);
                plantStages.Add(1277828144, pumpkinStages);
                plantStages.Add(658916791, riceStages);
                plantStages.Add(1924673028, soyStages);
                plantStages.Add(892110467, fernStages);
                plantStages.Add(1929046963, potatoStages);
                plantStages.Add(-1057658015, wheatStages);

                //flowers
                plantStages.Add(1712822019, flowerStages);
                plantStages.Add(-81376085, flowerStages);
                plantStages.Add(-1411986716, flowerStages);
                plantStages.Add(-1513337058, flowerStages);
                plantStages.Add(-1573623434, flowerStages);

                //others
                plantStages.Add(266654416, filterFernStages);
                plantStages.Add(-800947386, tropicalLilyPlantStages);
                plantStages.Add(2042955224, peaceLilyPlantStages);
                //plantStages.Add(2044798572, mushroomStages);
                plantStages.Add(176446172, alienMushroomStages);
                plantStages.Add(-177792789, thermogenicGenepool1Stages);
                plantStages.Add(1819167057, thermogenicGenepool2Stages);
                plantStages.Add(-1208890208, thermogenicCreativeStages);
                plantStages.Add(-1159179557, endothermicCreativeStages);
                plantStages.Add(851290561, endothermicGenepool1Stages);
                plantStages.Add(-1414203269, endothermicGenepool2Stages);
                plantStages.Add(-532672323, switchGrassStages);
            }

            public static void PatchPrefabs()
            {
                List<Thing> allPrefabs = Assets.Scripts.Objects.Prefab.AllPrefabs;
                Dictionary<int, Plant> plantDict = new Dictionary<int, Plant>();

                // Create a dictionary of plants for easier access
                foreach (Thing thing in allPrefabs)
                {
                    if (thing is Plant plant)
                    {
                        plantDict[plant.PrefabHash] = plant;
                    }
                    else if (thing is Seed seed)
                    {
                        plantDict[seed.PlantType.PrefabHash] = seed.PlantType;
                    }
                }

                // Apply the changes to the growthstages of the plants
                foreach (var keyValuePair in plantStages)
                {
                    if (plantDict.TryGetValue(keyValuePair.Key, out Plant plant))
                    {
                        for (var index = 0; index < plant.GrowthStates.Count; index++)
                        {
                            var plantStage = plant.GrowthStates[index];
                            if (plantStage.Length > 2)
                            {
                                plantStage.Length = plantStages[plant.PrefabHash][index];
                            }
                        }
                    }
                }
                Debug.Log("Plants and Nutrition - Successfully applied growth stage modifications to plants.");
            }
        }
    }
}