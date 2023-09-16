using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;


namespace PlantsnNutritionRebalance.Scripts
{
    internal class ConfigFile
    {
        private static ConfigEntry<int> configLogLevel;
        
        //Plants
        private static ConfigEntry<float> configPlantWaterConsumptionMultiplier;
        private static ConfigEntry<float> configPlantWaterConsumptionLimit;
        private static ConfigEntry<float> configPlantWaterTranspirationPercentage;

        //Fog
        private static ConfigEntry<float> configAtmosphereFogThreshold;

        //Character
        private static ConfigEntry<float> configNutritionLossMultiplier;
        private static ConfigEntry<float> configHydrationLossMultiplier;
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

        //Foods
        private static ConfigEntry<bool> configEnableFoodChanges;

        //------------------------------ foods----------------------------------------------
        private static ConfigEntry<float> configTomatoSoupNutrition;
        private static ConfigEntry<float> configCornSoupNutrition;
        private static ConfigEntry<float> configCannedRicePuddingNutrition;
        private static ConfigEntry<float> configPumpkinSoupNutrition;
        private static ConfigEntry<float> configPumpkinPieNutrition;
        private static ConfigEntry<float> configBakedPotatoNutrition;
        private static ConfigEntry<float> configFrenchFriesNutrition;
        private static ConfigEntry<float> configCannedFrenchFriesNutrition;
        private static ConfigEntry<float> configMilkNutrition;
        private static ConfigEntry<float> configCannedCondensedMilkNutrition;
        private static ConfigEntry<float> configMuffinNutrition;
        private static ConfigEntry<float> configBreadLoafNutrition;
        private static ConfigEntry<float> configCerealBarNutrition;
        private static ConfigEntry<float> configCannedPowderedEggsNutrition;
        private static ConfigEntry<float> configCannedEdamameNutrition;
        private static ConfigEntry<float> configCondensedMilkNutrition;
        private static ConfigEntry<float> configCookedSoybeanNutrition;
        private static ConfigEntry<float> configCookedRiceNutrition;
        private static ConfigEntry<float> configCookedCornNutrition;
        private static ConfigEntry<float> configCookedPumpkinNutrition;
        private static ConfigEntry<float> configPowderedEggsNutrition;
        private static ConfigEntry<float> configCookedTomatoNutrition;

        private static ConfigEntry<float> configTomatoSoupEatSpeed;
        private static ConfigEntry<float> configCornSoupEatSpeed;
        private static ConfigEntry<float> configCannedRicePuddingEatSpeed;
        private static ConfigEntry<float> configPumpkinSoupEatSpeed;
        private static ConfigEntry<float> configPumpkinPieEatSpeed;
        private static ConfigEntry<float> configBakedPotatoEatSpeed;
        private static ConfigEntry<float> configFrenchFriesEatSpeed;
        private static ConfigEntry<float> configCannedFrenchFriesEatSpeed;
        private static ConfigEntry<float> configMilkEatSpeed;
        private static ConfigEntry<float> configCannedCondensedMilkEatSpeed;
        private static ConfigEntry<float> configMuffinEatSpeed;
        private static ConfigEntry<float> configBreadLoafEatSpeed;
        private static ConfigEntry<float> configCerealBarEatSpeed;
        private static ConfigEntry<float> configCannedPowderedEggsEatSpeed;
        private static ConfigEntry<float> configCannedEdamameEatSpeed;
        private static ConfigEntry<float> configCondensedMilkEatSpeed;
        private static ConfigEntry<float> configCookedSoybeanEatSpeed;
        private static ConfigEntry<float> configCookedRiceEatSpeed;
        private static ConfigEntry<float> configCookedCornEatSpeed;
        private static ConfigEntry<float> configCookedPumpkinEatSpeed;
        private static ConfigEntry<float> configPowderedEggsEatSpeed;
        private static ConfigEntry<float> configCookedTomatoEatSpeed;

        private static ConfigEntry<float> configWheatNutrition;
        private static ConfigEntry<float> configCornNutrition;
        private static ConfigEntry<float> configFernNutrition;
        private static ConfigEntry<float> configMushroomNutrition;
        private static ConfigEntry<float> configPotatoNutrition;
        private static ConfigEntry<float> configPumpkinNutrition;
        private static ConfigEntry<float> configRiceNutrition;
        private static ConfigEntry<float> configSoybeanNutrition;
        private static ConfigEntry<float> configTomatoNutrition;

        private static ConfigEntry<float> configWheatEatSpeed;
        private static ConfigEntry<float> configCornEatSpeed;
        private static ConfigEntry<float> configFernEatSpeed;
        private static ConfigEntry<float> configMushroomEatSpeed;
        private static ConfigEntry<float> configPotatoEatSpeed;
        private static ConfigEntry<float> configPumpkinEatSpeed;
        private static ConfigEntry<float> configRiceEatSpeed;
        private static ConfigEntry<float> configSoybeanEatSpeed;
        private static ConfigEntry<float> configTomatoEatSpeed;

        public static int LogLevel;
        public static float PlantWaterConsumptionMultiplier;
        public static float PlantWaterConsumptionLimit;
        public static float PlantWaterTranspirationPercentage;
        public static float AtmosphereFogThreshold;
        public static float NutritionLossMultiplier;
        public static float HydrationLossMultiplier;
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

        public static bool EnableFoodChanges;
        
        public static float TomatoSoupNutrition;
        public static float TomatoSoupEatSpeed;
        public static float CornSoupNutrition;
        public static float CornSoupEatSpeed;
        public static float CannedRicePuddingNutrition;
        public static float CannedRicePuddingEatSpeed;
        public static float PumpkinSoupNutrition;
        public static float PumpkinSoupEatSpeed;
        public static float PumpkinPieNutrition;
        public static float PumpkinPieEatSpeed;
        public static float BakedPotatoNutrition;
        public static float BakedPotatoEatSpeed;
        public static float FrenchFriesNutrition;
        public static float FrenchFriesEatSpeed;
        public static float CannedFrenchFriesNutrition;
        public static float CannedFrenchFriesEatSpeed;
        public static float MilkNutrition;
        public static float MilkEatSpeed;
        public static float CannedCondensedMilkNutrition;
        public static float CannedCondensedMilkEatSpeed;
        public static float MuffinNutrition;
        public static float MuffinEatSpeed;
        public static float BreadLoafNutrition;
        public static float BreadLoafEatSpeed;
        public static float CerealBarNutrition;
        public static float CerealBarEatSpeed;
        public static float CannedPowderedEggsNutrition;
        public static float CannedPowderedEggsEatSpeed;
        public static float CannedEdamameNutrition;
        public static float CannedEdamameEatSpeed;
        public static float CondensedMilkNutrition;
        public static float CondensedMilkEatSpeed;
        public static float CookedSoybeanNutrition;
        public static float CookedSoybeanEatSpeed;
        public static float CookedRiceNutrition;
        public static float CookedRiceEatSpeed;
        public static float CookedCornNutrition;
        public static float CookedCornEatSpeed;
        public static float CookedPumpkinNutrition;
        public static float CookedPumpkinEatSpeed;
        public static float PowderedEggsNutrition;
        public static float PowderedEggsEatSpeed;
        public static float CookedTomatoNutrition;
        public static float CookedTomatoEatSpeed;

        public static float WheatNutrition;
        public static float WheatEatSpeed;
        public static float CornNutrition;
        public static float CornEatSpeed;
        public static float FernNutrition;
        public static float FernEatSpeed;
        public static float MushroomNutrition;
        public static float MushroomEatSpeed;
        public static float PotatoNutrition;
        public static float PotatoEatSpeed;
        public static float PumpkinNutrition;
        public static float PumpkinEatSpeed;
        public static float RiceNutrition;
        public static float RiceEatSpeed;
        public static float SoybeanNutrition;
        public static float SoybeanEatSpeed;
        public static float TomatoNutrition;
        public static float TomatoEatSpeed;

        public static void HandleConfig(PlantsnNutritionRebalancePlugin PnN) // Create and manage the configuration file parameters
        {
            //Log Section
            configLogLevel = PnN.Config.Bind("0 - General configuration",
                 "LogLevel",
                 0, 
                 "Set the log level of the mod. Values can be 0 for errors only (default), 1 for informational logs or 2 for debug logs." +
                 "Mod logs can be found inside the player.log file in the path %appdata%\\..\\localLow\\rocketwerkz\\rocketstation\\" +
                 "Warning, if you set this to a value different than 0, the log files can become very large after a extended amount of time playing," +
                 "expecially when running this mod in a dedicated server.");
            LogLevel = configLogLevel.Value;

            //Plants configuration section
            configPlantWaterConsumptionMultiplier = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionMultiplier",  // The key of the configuration option in the configuration file
                 500f, // The default value
                 "By how much this mod should multiply the water consumption of plants?" +
                 "The vanilla water consumption value is aprox ~0.000006 moles per tick for most plants, quite low. For reference, 1 ice water stack has 1000 mols" +
                 "That means, in vanilla, 1 single stack of ice will keep a plant alive for more than 23148 hours of gameplay! That's why the suggested value here" +
                 "is 500, it increases the plants drinks to ~0.003 moles of water per tick. With this, 1 ice water stack will keep a plant alive for 46 hours of gameplay," +
                 "or 20 plants for 2 hours, enough to make the water management meaningful." +
                 "If you set this option to 1, you'll get the vanilla water consumption values on plants."); // Description of the option to show in the config file

            PlantWaterConsumptionMultiplier = Mathf.Clamp(configPlantWaterConsumptionMultiplier.Value, 1f, 100000f);

            configPlantWaterConsumptionLimit = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterConsumptionLimit",  // The key of the configuration option in the configuration file
                 0.004f, // The default value
                 "Limit the max consumption of water mols per tick a plant can drink. This is mainly to fix the behaviour of the water consumption of Winterspawn that drinks" +
                 "considerably more water than the other plants. Should be set to a positive float value. If you change the PlantWaterConsumptionMultiplier, you'll probably want" +
                 "to change this one too accordingly."); // Description of the option to show in the config file

            PlantWaterConsumptionLimit = Mathf.Clamp(configPlantWaterConsumptionLimit.Value, 0.000001f, 100000f);

            configPlantWaterTranspirationPercentage = PnN.Config.Bind("1 - Plants Configuration", // The section under which the option is shown 
                 "PlantWaterTranspirationPercentage",  // The key of the configuration option in the configuration file
                 25f, // The default value
                 "Set the percentage of the water consumed by plants that should be transpirated back to the atmosphere." +
                 "Can be a float number between 0 and 100. Set it to 0 to disable plants water transpiration."); // Description of the option to show in the config file

            PlantWaterTranspirationPercentage = Mathf.Clamp(configPlantWaterTranspirationPercentage.Value, 0f, 100f);


            //Liquid Fog Section
            configAtmosphereFogThreshold = PnN.Config.Bind("2 - Fog Configuration", // The section under which the option is shown 
                 "AtmosphereFogThreshold",  // The key of the configuration option in the configuration file
                 5f, // The default value
                 "Set the minimum amount of moles needed to start showing the fog effect in the atmosphere. The Vanilla behaviour is to show the effect when there's any" +
                 "amount of liquid in atmosphere thus making any greenhouse who have plants transpirating water to always look foggy. Also note that this setting will affect the fog" +
                 "visualization for *ALL* liquids in the atmosphere, not just water. Must be a float number between 0 and 100. Setting this to 0 will keep the Vanilla effect."); // Description of the option to show in the config file

            AtmosphereFogThreshold = Mathf.Clamp(configAtmosphereFogThreshold.Value, 0f, 1000f);


            //Character configuration section
            configNutritionLossMultiplier = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "NutritionLossMultiplier", // The key of the configuration option in the configuration file
                1f,
                "Multiplier for the nutrition loss per tick of the character. Can be set to a positive value between 0.1 and 10." +
                "For example, on Stationeers difficulty, by default, you'll get a nutrition loss of 0.104167 wich will give you ~8 days of hunger. If you change this value for 2," +
                "your hunger will last only 4 days and will have to eat 2x more each day. If you set this value for 0.5, your character full hunger will last for 16 days on Stationeers" +
                "Difficulty and you'll have to eat 50% less calories each day." +
                "You'll want to change this option mainly if you also change the max nutrition storage of the character." +
                "Just remember that, if you change this, it will also greatly affect the amount of plants you need to grow to keep your character alive. With default mod values you need ~15" +
                "to feed each player on stationeers difficulty (~12 on medium and ~10 on easy). If you change this to 0.5, you'll need only 7 plants growing on Stationeers difficulty," +
                "6 on normal or 5 on easy to feed each player.");

            NutritionLossMultiplier = Mathf.Clamp(configNutritionLossMultiplier.Value, 0.1f, 10f);

            configHydrationLossMultiplier = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "HydrationLossMultiplier", // The key of the configuration option in the configuration file
                1f,
                "Multiplier for the hydration loss per tick of the character. Can be set to a positive value between 0.1 and 10. The water consumtpion logic in Stationeers" +
                "is somewhat complex, it depends of the enviroment/suit temperature and other factors, so i don't really recommend changing this here, instead try to use the HydrationRate" +
                "inside the worldsettings.xml file at the save folder whenever possible. If you want to disable water consumption, it's also better to do it in worldsettings.xml (Just set" +  
                "the value to 0). If that is not enough for you and you still want to mess with this value for some reason, try to do it in small increments/decrements like 0.01 and" +
                "check ingame because small changes in this value will cause a big impact in your character water consumption.");

            HydrationLossMultiplier = Mathf.Clamp(configHydrationLossMultiplier.Value, 0.1f, 10f);

            configMaxNutritionStorage = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "MaxNutritionStorage", // The key of the configuration option in the configuration file
                4000f,
                "This set the max amount of nutrition the character can store. If you change this value, it's highly advisable to start a new save or edit the character" +
                "<Nutrition> tag directly into the save (world.xml file) to adjust your character nutrition value to be inside the max value you set here." +
                "This greatly affects the amount of filling each food gives and also how long your starting 100% nutrition will last. For example, if you change" +
                "this to 2000, potatoes will fill 2% of your food (instead of 1%) but on Stationeers difficulty your character will get only 4 days worth of it's initial nutrition," +
                "instead of 8 days.Your Nutrition will also appear to drop 2x faster (in percentage) because now your max nutrition is lower. If you adjust this value, you can also adjust" +
                "NutritionLossMultiplier to counter this effect but if you do so, it will considerably decrease the amount of plants you need to grow, making the food-plant cycle far easier." +
                "NOTE: When playing in MP, the host and the clients need to have the same value here, or the hunger percentage will not show up properly.");
            MaxNutritionStorage = Mathf.Clamp(configMaxNutritionStorage.Value, 1f, 1000000f);

            configMaxHydrationStorage = PnN.Config.Bind("3 - Character Configuration", // The section under which the option is shown 
                "MaxHydrationStorage", // The key of the configuration option in the configuration file
                42f,
                "This set the max amount of hydration the character can store. If you change this value, it's advisable to start a new save or edit the character" +
                "<Hydration> tag directly into the save (world.xml file) to move your character hydration value to be inside the max value you set here." +
                "This greatly affects the amount of filling each water canister gives you and also how long your starting 100% hydration will last. For example, if you change\n" +
                "this to 21, a full water canister will now fill 36% of your water (instead of 18%) but on Stationeers difficulty your character will get only 1¼ days worth of hydration." +
                "Your Hydration will also appear to drop faster (in percentage) because now your max hydration is lower." +
                "NOTE: When playing in MP, the host and the clients need to have the same value here, or the hydration percentage will not show up properly.");
            MaxHydrationStorage = Mathf.Clamp(configMaxHydrationStorage.Value, 1f, 1000000f);

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

            //Character Respawn Section
            configEnableRespawnPenaltyLogic = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "EnableRespawnPenaltyLogic", // The key of the configuration option in the configuration file
                true,
                "This option enable or disable the respawn penalty logic of the mod. On Vanilla, players who die are rewarded with 100% nutrition/Hydration stats." +
                "This issue is even proeminent with the Plants and Nutrition mod because the mod considerably increases the amount of Nutrition/Hydration a player " +
                "can have." +
                "If this option is set to true, characters who die will respawn with nutrition and hydration proportional to the days past in the save. The mod will take into account" +
                "the difficulty of the save, the maxNutritionStorage, the current day and other factors and try to \"Guess\" what your hydration/nutrition should be. For example, on a Normal" +
                "difficulty save, default mod values and sun/orbit in 2, your character will have 10 days worth of food, and will loose 10% food each day. If you die on day 6, the mod " +
                "will respawn the player with 40% food, because that's what your initial nutrition should approximately be. After day 10, dead characters will respawn with minimal food," +
                "just enough to run to the pantry room to eat something." +
                "If set to false, the Vanilla behaviour will be kept so, after death, you will always respawn with full nutrition/hydration.");
            EnableRespawnPenaltyLogic = configEnableRespawnPenaltyLogic.Value;

            configCustomNewPlayerRespawn = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawn", // The key of the configuration option in the configuration file
                false,
                "This option control if new players who never played on the save should join with nutrition/hydration proportional to the days past in the save or with custom food amounts set" +
                "in CustomNewPlayerRespawnNutrition and CustomNewPlayerRespawnHydration. If set to false, when new players join, they will respawn with food proportional to the days past in the" +
                "save. You'll want to enable this option mostly on dedicated servers where new players will join and start playing after the server is running for a long time, on saves where each" +
                "player make separated bases, and you want them to join with full food but still penalize them if they die." +
                "This option only work if EnableRespawnPenaltyLogic is also true, otherwise everyone will always respawn with 100% nutrition/hydration.");
            CustomNewPlayerRespawn = configCustomNewPlayerRespawn.Value;

            configCustomNewPlayerRespawnNutrition = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawnNutrition", // The key of the configuration option in the configuration file
                100f,
                "If CustomNewPlayerRespawn is true, this option set with how much nutrition, in percent, new players should join. Values can be set between 0 and 100");
            CustomNewPlayerRespawnNutrition = Mathf.Clamp(configCustomNewPlayerRespawnNutrition.Value, 0f, 100f);

            configCustomNewPlayerRespawnHydration = PnN.Config.Bind("4 - Character Respawn Configuration", // The section under which the option is shown 
                "CustomNewPlayerRespawnHydration", // The key of the configuration option in the configuration file
                100f,
                "If CustomNewPlayerRespawn is true, this option set with how much hydration, in percent, new players should join. Values can be set between 0 and 100");
            CustomNewPlayerRespawnHydration = Mathf.Clamp(configCustomNewPlayerRespawnHydration.Value, 0f, 100f);

            //Food Section
            configEnableFoodChanges = PnN.Config.Bind("5 - Foods Configuration", // The section under which the option is shown 
                "EnableFoodChanges", // The key of the configuration option in the configuration file
                true,
                "This option enable or disable the changes of the Nutrition amount of foods and the Eat time. If set to true, the nutrition and eat time of foods will be" +
                "changed to the values set for each food below. If set to false, the vanilla nutrition values for foods will be kept with all it's oddities like overpowered " +
                "tomato soup and Pumpkin pie not really worth the trouble making.");
            EnableFoodChanges = configEnableFoodChanges.Value;

            configTomatoSoupNutrition = PnN.Config.Bind("5 - Foods Configuration", 
                "TomatoSoupNutrition", 
                135f, 
                "Amount of Nutrition given by eating Tomato Soup. Needs to be a positive value between 1 and 10000. NOTE: Before changing the nutrition each food gives, you should" +
                "remember that, by changing this nutrition amount, you'll probably break the nutrition balance between the plant needed to make this food and other plants. For example," +
                "if you choose to double the nutrition given by Tomato Soup, you'll need half of the amount of tomato plants growing in your greenhouse to feed each player compared to " +
                "other plants/foods. So if you decide to change this food nutrition, you'll should change all foods to retain the nutritional balance of plants.\n" +
                "The default values of food nutrition used in this mod aims to create the need for ~15 plants growing continuoulsy to feed each player so, before changing any " +
                "food values, it's strongly advisable to test the new values in the mod spreadsheet available at <spreadsheetlink> to check how the change will impact the balance and" +
                "the amount of plants you need to grow continuoulsy.");
            TomatoSoupNutrition = Mathf.Clamp(configTomatoSoupNutrition.Value, 1f, 10000f);

            configTomatoSoupEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "TomatoSoupEatSpeed", 
                0.04f,
                "Time to eat each nutrition of Tomato Soup. Needs to be a positive value between 0.001 and 10");
            TomatoSoupEatSpeed = Mathf.Clamp(configTomatoSoupEatSpeed.Value, 0.001f, 10f);

            configCornSoupNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CornSoupNutrition", 
                223f,
                "Amount of Nutrition given by eating Corn Soup. Needs to be a positive value between 1 and 10000.");
            CornSoupNutrition = Mathf.Clamp(configCornSoupNutrition.Value, 1f, 10000f);

            configCornSoupEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CornSoupEatSpeed",
                0.04f,
                "Time to eat each nutrition of Corn Soup. Needs to be a positive value between 0.001 and 10.");
            CornSoupEatSpeed = Mathf.Clamp(configCornSoupEatSpeed.Value, 0.001f, 10f);

            configCannedRicePuddingNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CannedRicePuddingNutrition",
                220f,
                "Amount of Nutrition given by eating Canned Rice Pudding. Needs to be a positive value between 1 and 10000.");
            CannedRicePuddingNutrition = Mathf.Clamp(configCannedRicePuddingNutrition.Value, 1f, 10000f);

            configCannedRicePuddingEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CannedRicePuddingEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned Rice Pudding. Needs to be a positive value between 0.001 and 10.");
            CannedRicePuddingEatSpeed = Mathf.Clamp(configCannedRicePuddingEatSpeed.Value, 0.001f, 10f);

            configPumpkinSoupNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinSoupNutrition",
                270f,
                "Amount of Nutrition given by eating Pumpkin Soup. Needs to be a positive value between 1 and 10000.");
            PumpkinSoupNutrition = Mathf.Clamp(configPumpkinSoupNutrition.Value, 1f, 10000f);

            configPumpkinSoupEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinSoupEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned Pumpkin Soup. Needs to be a positive value between 0.001 and 10.");
            PumpkinSoupEatSpeed = Mathf.Clamp(configPumpkinSoupEatSpeed.Value, 0.001f, 10f);

            configPumpkinPieNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinPieNutrition",
                800f,
                "Amount of Nutrition given by eating Pumpkin Pie. Needs to be a positive value between 1 and 10000.");
            PumpkinPieNutrition = Mathf.Clamp(configPumpkinPieNutrition.Value, 1f, 10000f);

            configPumpkinPieEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinPieEatSpeed",
                0.005f,
                "Time to eat each nutrition of Pumpkin Pie. Needs to be a positive value between 0.001 and 10.");
            PumpkinPieEatSpeed = Mathf.Clamp(configPumpkinPieEatSpeed.Value, 0.001f, 10f);

            configBakedPotatoNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "BakedPotatoNutrition",
                45f,
                "Amount of Nutrition given by eating Baked Potato. Needs to be a positive value between 1 and 10000.");
            BakedPotatoNutrition = Mathf.Clamp(configBakedPotatoNutrition.Value, 1f, 10000f);

            configBakedPotatoEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "BakedPotatoEatSpeed",
                0.015f,
                "Time to eat each nutrition of a Baked Potato. Needs to be a positive value between 0.001 and 10.");
            BakedPotatoEatSpeed = Mathf.Clamp(configBakedPotatoEatSpeed.Value, 0.001f, 10f);

            configFrenchFriesNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "FrenchFriesNutrition",
                85f,
                "Amount of Nutrition given by eating French Fries. Needs to be a positive value between 1 and 10000.");
            FrenchFriesNutrition = Mathf.Clamp(configFrenchFriesNutrition.Value, 1f, 10000f);

            configFrenchFriesEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "FrenchFriesEatSpeed",
                0.02f,
                "Time to eat each nutrition of a French Fries. Needs to be a positive value between 0.001 and 10.");
            FrenchFriesEatSpeed = Mathf.Clamp(configFrenchFriesEatSpeed.Value, 0.001f, 10f);

            configCannedFrenchFriesNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CannedFrenchFriesNutrition",
                150f,
                "Amount of Nutrition given by eating Canned French Fries. Needs to be a positive value between 1 and 10000.");
            CannedFrenchFriesNutrition = Mathf.Clamp(configCannedFrenchFriesNutrition.Value, 1f, 10000f);

            configCannedFrenchFriesEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CannedFrenchFriesEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned French Fries. Needs to be a positive value between 0.001 and 10.");
            CannedFrenchFriesEatSpeed = Mathf.Clamp(configCannedFrenchFriesEatSpeed.Value, 0.001f, 10f);

            configMilkNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "MilkNutrition",
                2.3f,
                "Amount of Nutrition given by eating Milk. Needs to be a positive value between 1 and 10000.");
            MilkNutrition = Mathf.Clamp(configMilkNutrition.Value, 1f, 10000f);

            configMilkEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "MilkEatSpeed",
                0.015f,
                "Time to eat each nutrition of Milk. Needs to be a positive value between 0.001 and 10.");
            MilkEatSpeed = Mathf.Clamp(configMilkEatSpeed.Value, 0.001f, 10f);

            configCannedCondensedMilkNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CannedCondensedMilkNutrition",
                400f,
                "Amount of Nutrition given by eating Canned Condensed Milk. Needs to be a positive value between 1 and 10000.");
            CannedCondensedMilkNutrition = Mathf.Clamp(configCannedCondensedMilkNutrition.Value, 1f, 10000f);

            configCannedCondensedMilkEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CannedCondensedMilkEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned Condensed Milk. Needs to be a positive value between 0.001 and 10.");
            CannedCondensedMilkEatSpeed = Mathf.Clamp(configCannedCondensedMilkEatSpeed.Value, 0.001f, 10f);

            configMuffinNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "MuffinNutrition",
                570f,
                "Amount of Nutrition given by eating a Muffin. Needs to be a positive value between 1 and 10000.");
            MuffinNutrition = Mathf.Clamp(configMuffinNutrition.Value, 1f, 10000f);

            configMuffinEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "MuffinEatSpeed",
                0.02f,
                "Time to eat each nutrition of a Muffin. Needs to be a positive value between 0.001 and 10.");
            MuffinEatSpeed = Mathf.Clamp(configMuffinEatSpeed.Value, 0.001f, 10f);

            configBreadLoafNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "BreadLoafNutrition",
                290f,
                "Amount of Nutrition given by eating a Bread Loaf. Needs to be a positive value between 1 and 10000.");
            BreadLoafNutrition = Mathf.Clamp(configBreadLoafNutrition.Value, 1f, 10000f);

            configBreadLoafEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "BreadLoafEatSpeed",
                0.02f,
                "Time to eat each nutrition of a Bread Loaf. Needs to be a positive value between 0.001 and 10.");
            BreadLoafEatSpeed = Mathf.Clamp(configBreadLoafEatSpeed.Value, 0.001f, 10f);

            configCerealBarNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CerealBarNutrition",
                110f,
                "Amount of Nutrition given by eating a Cereal Bar. Needs to be a positive value between 1 and 10000.");
            CerealBarNutrition = Mathf.Clamp(configCerealBarNutrition.Value, 1f, 10000f);

            configCerealBarEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CerealBarEatSpeed",
                0.04f,
                "Time to eat each nutrition of a Cereal Bar. Needs to be a positive value between 0.001 and 10.");
            CerealBarEatSpeed = Mathf.Clamp(configCerealBarEatSpeed.Value, 0.001f, 10f);

            configCannedPowderedEggsNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CannedPowderedEggsNutrition",
                550f,
                "Amount of Nutrition given by eating Canned Powdered Eggs. Needs to be a positive value between 1 and 10000.");
            CannedPowderedEggsNutrition = Mathf.Clamp(configCannedPowderedEggsNutrition.Value, 1f, 10000f);

            configCannedPowderedEggsEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CannedPowderedEggsEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned Powdered Eggs. Needs to be a positive value between 0.001 and 10.");
            CannedPowderedEggsEatSpeed = Mathf.Clamp(configCannedPowderedEggsEatSpeed.Value, 0.001f, 10f);

            configCannedEdamameNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CannedEdamameNutrition",
                100f,
                "Amount of Nutrition given by eating Canned Edamame. Needs to be a positive value between 1 and 10000.");
            CannedEdamameNutrition = Mathf.Clamp(configCannedEdamameNutrition.Value, 1f, 10000f);

            configCannedEdamameEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CannedEdamameEatSpeed",
                0.04f,
                "Time to eat each nutrition of Canned Edamame. Needs to be a positive value between 0.001 and 10.");
            CannedEdamameEatSpeed = Mathf.Clamp(configCannedEdamameEatSpeed.Value, 0.001f, 10f);

            configCondensedMilkNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CondensedMilkNutrition",
                235f,
                "Amount of Nutrition given by eating Condensed Milk. Needs to be a positive value between 1 and 10000.");
            CondensedMilkNutrition = Mathf.Clamp(configCondensedMilkNutrition.Value, 1f, 10000f);

            configCondensedMilkEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CondensedMilkEatSpeed",
                0.015f,
                "Time to eat each nutrition of Condensed Milk. Needs to be a positive value between 0.001 and 10.");
            CondensedMilkEatSpeed = Mathf.Clamp(configCondensedMilkEatSpeed.Value, 0.001f, 10f);

            configCookedSoybeanNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CookedSoybeanNutrition",
                38f,
                "Amount of Nutrition given by eating Cooked Soybean. Needs to be a positive value between 1 and 10000.");
            CookedSoybeanNutrition = Mathf.Clamp(configCookedSoybeanNutrition.Value, 1f, 10000f);

            configCookedSoybeanEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CookedSoybeanEatSpeed",
                0.015f,
                "Time to eat each nutrition of Cooked Soybean. Needs to be a positive value between 0.001 and 10.");
            CookedSoybeanEatSpeed = Mathf.Clamp(configCookedSoybeanEatSpeed.Value, 0.001f, 10f);

            configCookedRiceNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CookedRiceNutrition",
                50f,
                "Amount of Nutrition given by eating Cooked Rice. Needs to be a positive value between 1 and 10000.");
            CookedRiceNutrition = Mathf.Clamp(configCookedRiceNutrition.Value, 1f, 10000f);

            configCookedRiceEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CookedRiceEatSpeed",
                0.015f,
                "Time to eat each nutrition of Cooked Rice. Needs to be a positive value between 0.001 and 10.");
            CookedRiceEatSpeed = Mathf.Clamp(configCookedRiceEatSpeed.Value, 0.001f, 10f);

            configCookedCornNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CookedCornNutrition",
                52f,
                "Amount of Nutrition given by eating Cooked Corn. Needs to be a positive value between 1 and 10000.");
            CookedCornNutrition = Mathf.Clamp(configCookedCornNutrition.Value, 1f, 10000f);

            configCookedCornEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CookedCornEatSpeed",
                0.015f,
                "Time to eat each nutrition of Cooked Corn. Needs to be a positive value between 0.001 and 10.");
            CookedCornEatSpeed = Mathf.Clamp(configCookedCornEatSpeed.Value, 0.001f, 10f);

            configCookedPumpkinNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CookedPumpkinNutrition",
                60f,
                "Amount of Nutrition given by eating Cooked Pumpkin. Needs to be a positive value between 1 and 10000.");
            CookedPumpkinNutrition = Mathf.Clamp(configCookedPumpkinNutrition.Value, 1f, 10000f);

            configCookedPumpkinEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CookedPumpkinEatSpeed",
                0.015f,
                "Time to eat each nutrition of Cooked Pumpkin. Needs to be a positive value between 0.001 and 10.");
            CookedPumpkinEatSpeed = Mathf.Clamp(configCookedPumpkinEatSpeed.Value, 0.001f, 10f);

            configPowderedEggsNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "PowderedEggsNutrition",
                330f,
                "Amount of Nutrition given by eating Powdered Eggs. Needs to be a positive value between 1 and 10000.");
            PowderedEggsNutrition = Mathf.Clamp(configPowderedEggsNutrition.Value, 1f, 10000f);

            configPowderedEggsEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "PowderedEggsEatSpeed",
                0.015f,
                "Time to eat each nutrition of Powdered Eggs. Needs to be a positive value between 0.001 and 10.");
            PowderedEggsEatSpeed = Mathf.Clamp(configPowderedEggsEatSpeed.Value, 0.001f, 10f);

            configCookedTomatoNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CookedTomatoNutrition",
                30f,
                "Amount of Nutrition given by eating Cooked Tomato. Needs to be a positive value between 1 and 10000.");
            CookedTomatoNutrition = Mathf.Clamp(configCookedTomatoNutrition.Value, 1f, 10000f);

            configCookedTomatoEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CookedTomatoEatSpeed",
                0.015f,
                "Time to eat each nutrition of Cooked Tomato. Needs to be a positive value between 0.001 and 10.");
            CookedTomatoEatSpeed = Mathf.Clamp(configCookedTomatoEatSpeed.Value, 0.001f, 10f);

            configWheatNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "WheatNutrition",
                5f,
                "Amount of Nutrition given by eating Wheat. Needs to be a positive value between 1 and 10000.");
            WheatNutrition = Mathf.Clamp(configWheatNutrition.Value, 1f, 10000f);

            configWheatEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "WheatEatSpeed",
                0.015f,
                "Time to eat each nutrition of Wheat. Needs to be a positive value between 0.001 and 10.");
            WheatEatSpeed = Mathf.Clamp(configWheatEatSpeed.Value, 0.001f, 10f);

            configCornNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "CornNutrition",
                10f,
                "Amount of Nutrition given by eating Corn. Needs to be a positive value between 1 and 10000.");
            CornNutrition = Mathf.Clamp(configCornNutrition.Value, 1f, 10000f);

            configCornEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "CornEatSpeed",
                0.015f,
                "Time to eat each nutrition of Corn. Needs to be a positive value between 0.001 and 10.");
            CornEatSpeed = Mathf.Clamp(configCornEatSpeed.Value, 0.001f, 10f);

            configFernNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "FernNutrition",
                1f,
                "Amount of Nutrition given by eating Fern. Needs to be a positive value between 1 and 10000.");
            FernNutrition = Mathf.Clamp(configFernNutrition.Value, 1f, 10000f);

            configFernEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "FernEatSpeed",
                0.015f,
                "Time to eat each nutrition of Fern. Needs to be a positive value between 0.001 and 10.");
            FernEatSpeed = Mathf.Clamp(configFernEatSpeed.Value, 0.001f, 10f);

            configMushroomNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "MushroomNutrition",
                10f,
                "Amount of Nutrition given by eating Mushroom. Needs to be a positive value between 1 and 10000.");
            MushroomNutrition = Mathf.Clamp(configMushroomNutrition.Value, 1f, 10000f);

            configMushroomEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "MushroomEatSpeed",
                0.015f,
                "Time to eat each nutrition of Mushroom. Needs to be a positive value between 0.001 and 10.");
            MushroomEatSpeed = Mathf.Clamp(configMushroomEatSpeed.Value, 0.001f, 10f);

            configPotatoNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "PotatoNutrition",
                10f,
                "Amount of Nutrition given by eating Potato. Needs to be a positive value between 1 and 10000.");
            PotatoNutrition = Mathf.Clamp(configPotatoNutrition.Value, 1f, 10000f);

            configPotatoEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "PotatoEatSpeed",
                0.015f,
                "Time to eat each nutrition of Potato. Needs to be a positive value between 0.001 and 10.");
            PotatoEatSpeed = Mathf.Clamp(configPotatoEatSpeed.Value, 0.001f, 10f);

            configPumpkinNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinNutrition",
                40f,
                "Amount of Nutrition given by eating Pumpkin. Needs to be a positive value between 1 and 10000.");
            PumpkinNutrition = Mathf.Clamp(configPumpkinNutrition.Value, 1f, 10000f);

            configPumpkinEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "PumpkinEatSpeed",
                0.015f,
                "Time to eat each nutrition of Pumpkin. Needs to be a positive value between 0.001 and 10.");
            PumpkinEatSpeed = Mathf.Clamp(configPumpkinEatSpeed.Value, 0.001f, 10f);

            configRiceNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "RiceNutrition",
                3f,
                "Amount of Nutrition given by eating Rice. Needs to be a positive value between 1 and 10000.");
            RiceNutrition = Mathf.Clamp(configRiceNutrition.Value, 1f, 10000f);

            configRiceEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "RiceEatSpeed",
                0.015f,
                "Time to eat each nutrition of Rice. Needs to be a positive value between 0.001 and 10.");
            RiceEatSpeed = Mathf.Clamp(configRiceEatSpeed.Value, 0.001f, 10f);

            configSoybeanNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "SoybeanNutrition",
                5f,
                "Amount of Nutrition given by eating Soybean. Needs to be a positive value between 1 and 10000.");
            SoybeanNutrition = Mathf.Clamp(configSoybeanNutrition.Value, 1f, 10000f);

            configSoybeanEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "SoybeanEatSpeed",
                0.015f,
                "Time to eat each nutrition of Soybean. Needs to be a positive value between 0.001 and 10.");
            SoybeanEatSpeed = Mathf.Clamp(configSoybeanEatSpeed.Value, 0.001f, 10f);

            configTomatoNutrition = PnN.Config.Bind("5 - Foods Configuration",
                "TomatoNutrition",
                15f,
                "Amount of Nutrition given by eating Tomato. Needs to be a positive value between 1 and 10000.");
            TomatoNutrition = Mathf.Clamp(configTomatoNutrition.Value, 1f, 10000f);

            configTomatoEatSpeed = PnN.Config.Bind("5 - Foods Configuration",
                "TomatoEatSpeed",
                0.015f,
                "Time to eat each nutrition of Tomato. Needs to be a positive value between 0.001 and 10.");
            TomatoEatSpeed = Mathf.Clamp(configTomatoEatSpeed.Value, 0.001f, 10f);

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
