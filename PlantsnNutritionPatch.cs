using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;
using Assets.Scripts.Objects;
using System.Collections.Generic;

namespace PlantsnNutritionRebalance.Scripts
{
    // Adjust the growth stages and water consumption from plants
    public class PlantGrowStagePatch
    {
        //plant & seed
        private static int[] cornStages = { 1, 1600, 3200, 4800, 6400, -1, 0 }; //258339687 & -1290755415 
        private static int[] tomatoStages = { 1, 1600, 1600, 3200, 6400, -1, 0 }; //-998592080 & -1922066841 
        private static int[] pumpkinStages = { 1, 1600, 4800, 6400, 6400, -1, 0 }; //1277828144 & 1423199840 
        private static int[] riceStages = { 1, 3600, 3600, 3600, -1, 0 }; //658916791 & -1691151239 
        private static int[] soyStages = { 1, 3600, 3600, 4800, -1, 0 }; //1924673028 & 1783004244 
        private static int[] fernStages = { 1, 2400, 3600, 4800, -1, 0 }; //892110467 & -1990600883 
        private static int[] potatoStages = { 1, 3200, 6400, 4800, -1, 0 }; //1929046963 & 1005571172
        private static int[] wheatStages = { 1, 4800, 11200, 3200, -1, 0 }; //-1057658015 & -654756733
        
        private static int[] flowerStages = { 1, 2400, 2400, 3600, 3600, -1, 0 }; //1712822019,-81376085,-1411986716,-1513337058,-1573623434

        private static int[] filterFernStages = { 1, 2400, 3600, 4800, 3600, -1, 0 }; //266654416
        private static int[] tropicalLilyPlantStages = { 1, 2400, 2400, 3600, 3600, -1, 0 }; //-800947386
        private static int[] peaceLilyPlantStages = { 1, 2400, 2400, 3600, 3600, -1, 0 }; //2042955224 
        private static int[] alienMushroomStages = { 1, 2400, 3600, 4800, -1, 0 }; //176446172 
        private static int[] mushroomStages = { 1, 3600, 1200, 4800, -1, 0 }; //2044798572
        private static int[] thermogenicGenepool1Stages = { 1, 2400, 2400, 3600, 3600, 4800, -1, 0 }; //-177792789 
        private static int[] thermogenicGenepool2Stages = { 1, 2400, 2400, 3600, 3600, 4800, -1, 0 }; //1819167057 
        private static int[] thermogenicCreativeStages = { 1, 2400, 2400, 3600, 3600, 4800, -1, 0 }; //-1208890208 
        private static int[] endothermicGenepool1Stages = { 1, 1200, 1200, 2400, 2400, 2400, -1, 0 }; //851290561 
        private static int[] endothermicGenepool2Stages = { 1, 2400, 2400, 3600, 4800, -1, 0 }; //-1414203269 
        private static int[] endothermicCreativeStages = { 1, 2400, 3600, 3600, 4800, -1, 0 }; //-1159179557 

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
            plantStages.Add(2044798572, mushroomStages);
            plantStages.Add(176446172, alienMushroomStages);
            plantStages.Add(-177792789, thermogenicGenepool1Stages);
            plantStages.Add(1819167057, thermogenicGenepool2Stages);
            plantStages.Add(-1208890208, thermogenicCreativeStages);
            plantStages.Add(-1159179557, endothermicCreativeStages);
            plantStages.Add(851290561, endothermicGenepool1Stages);
            plantStages.Add(-1414203269, endothermicGenepool2Stages);
        }

        public static void PatchPrefabs()
        {
            var type = typeof(Assets.Scripts.Objects.Prefab);
            var fieldInfo = type.GetFields()[0];
            Dictionary<int, Thing> allPrefabs = (Dictionary<int, Thing>)fieldInfo.GetValue(null);
            foreach (var keyValuePair in plantStages)
            {
                Plant plant = (Plant)allPrefabs[keyValuePair.Key];
                if (plant is Seed seed)
                {
                    plant = seed.PlantType;
                }
                for (var index = 0; index < plant.GrowthStates.Count; index++)
                {
                    var plantStage = plant.GrowthStates[index];
                    if (plantStage.Length > 2)
                    {
                        plantStage.Length = plantStages[plant.PrefabHash][index];
                    }
                }
                plant.WaterPerTick = 0.0034f; 
                switch (plant.DisplayName)
                { 
                    case "Tomato":
                        plant.WaterPerTick *= 0.6f;
                        break;
                    case "Wheat":
                        plant.WaterPerTick *= 1.2f;
                        break;
                    case "Rice":
                        plant.WaterPerTick *= 1.4f;
                        break;
                    case "Pumpkin":
                        plant.WaterPerTick *= 0.6f;
                        break;
                    case "Darga Fern":
                        plant.WaterPerTick *= 0.8f;
                        break;
                    case "Soybean":
                        plant.WaterPerTick *= 1.3f;
                        break;
                    case "Mushroom":
                        plant.WaterPerTick *= 1.4f;
                        break;
                } //TODO: Make water consumption to be different through growth stages, preferably with a gradient increase
                  //TODO: Find a way to change the yields of each plant, right now all plants HarvestQuantity are equal: 2
            }
            Debug.Log("Plants and Nutrition - Prefabs for plants are patched!");
        }
    }

    // Adjusts the fertilizer to be less OP:
    [HarmonyPatch(typeof(Plant))]
    public class PlantPatch
    {
        [HarmonyPatch("ApplyFertilizer")]
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool PlantApplyFertilizerPatch(Plant __instance)
        {
            __instance.HarvestQuantity += 2; // yields + 2 
            __instance.SeedQuantity = __instance.HarvestQuantity;
            __instance.FertilizerBoost = 1.15f; // 15% boost in growth time
            __instance.IsFertilized = true;
            return false;
        }
    }

        [HarmonyPatch(typeof(Human))]
    public static class HumanNutritionHydrationPatch
    {
        // Adjusts the Human hydration based on the world difficulty and the warning/critical alerts
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        [UsedImplicitly]
        static public void HydrationAndWarningsPatch(Human __instance, ref float ___MaxHydrationStorage, ref float ____hydrationLossPerTick)
        {
            ___MaxHydrationStorage = 42f;
            switch (WorldManager.CurrentWorldSetting.DifficultySetting.HydrationRate) 
            {
                case 1.5f: //stationeers difficulty, water will last 2 and half days
                    ____hydrationLossPerTick = 0.0019446f;
                    break;
                case 1f: //normal difficulty, 85% water usage of stationeers difficulty
                    ____hydrationLossPerTick = 0.00165291f;
                    break;
                case 0.5f: //easy difficulty, 70% water usage of stationeers difficulty, should be easy enough
                    ____hydrationLossPerTick = 0.00136122f;
                    break;
                case 0f: //user disabled water consumption directly on world config
                    ____hydrationLossPerTick = 0f;
                    break;
                default: // if it's none of the above, will try to calculate hydrationLossPerTick based on DifficultySetting.HydrationRate:
                    float a = Mathf.InverseLerp(0f, 3f, WorldManager.CurrentWorldSetting.DifficultySetting.HydrationRate);
                    ____hydrationLossPerTick = Mathf.Lerp(0.00106953f, 0.00281967f, a);
                break;
            }
            // Nutrition/hydration warnings:
            __instance.WarningNutrition = 1080f;
            __instance.CriticalNutrition = 540f;
            __instance.WarningHydration = 10.5f;
            __instance.CriticalHydration = 5.25f;
        }

        
        [HarmonyPatch("get_MaxNutritionStorage")]
        [HarmonyPostfix]
        [UsedImplicitly]
        static public void MaxNutritionPatch(ref float __result)
        {
            // Adjusts the max food of the character:
            __result = 5400f;
        }

        // Adjusts the HungerRate based on the world difficulty and changes the damage system for Starvation
        [HarmonyPatch("LifeNutrition")]
        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool MetabolismRatePatch(Human __instance)
        {
            float NutritionLossPerTick;
            switch (WorldManager.CurrentWorldSetting.DifficultySetting.HungerRate)
            {
                case 1.5f: //stationeers difficulty, full food will last 10 game days
                    NutritionLossPerTick = 0.1125f; 
                    break;
                case 1f: //normal difficulty, 85% food usage of stationeers difficulty
                    NutritionLossPerTick = 0.09f;
                    break;
                case 0.5f: //easy difficulty, 70% food usage of stationeers difficulty, should be easy enough
                    NutritionLossPerTick = 0.07875f;
                    break;
                case 0f: //user disabled food consumption directly on world config
                    NutritionLossPerTick = 0f;
                    break;
                default: // if it's none of the above, will try to calculate an hydrationLossPerTick based on DifficultySetting.HungerRate:                    
                    float hungerdifficulty = Mathf.InverseLerp(0f, 3f, WorldManager.CurrentWorldSetting.DifficultySetting.HungerRate);
                    NutritionLossPerTick = Mathf.Lerp(0.061875f, 0.163125f, hungerdifficulty);
                    break;
            }
            // Human.LifeNutrition
            float num = NutritionLossPerTick * (__instance.OrganBrain.IsOnline ? 1f : WorldManager.CurrentWorldSetting.DifficultySetting.LifeFunctionLoggedOut);
            __instance.Nutrition -= num;
            // Entity.LifeNutrition
            if (__instance.IsArtificial)
            {
                return false;
            }
            if (__instance.Nutrition <= 0f)
            {
                __instance.DamageState.Damage(ChangeDamageType.Increment, 0.2f, DamageUpdateType.Starvation);
                return false;
            }
            if (__instance.DamageState.Starvation > 0f && __instance.Nutrition >= 540f)
            {
                __instance.DamageState.Damage(ChangeDamageType.Decrement, 0.1f, DamageUpdateType.Starvation);
            }
            return false;
        }
    }

    // After Death/Respawn, don't give 100% food and water:
	[HarmonyPatch(typeof(Entity))]
    public static class OnLifeCreatedPatch
	{
        [HarmonyPatch("OnLifeCreated")]
        [HarmonyPostfix]
        [UsedImplicitly]
        private static void RespawnNutritionPatch(Entity __instance)
        {
            float Dayspastnorm = WorldManager.DaysPast * Settings.CurrentData.SunOrbitPeriod * 10f;
            float Foodslice = __instance.MaxNutritionStorage / 200f;
            float Hydrationslice = Human.MaxHydrationStorage / 200f;
            float Hydrationtogive;
             if (Dayspastnorm <= 195)
            {
                // Calculate the food for respawn acordingly to the days past and SunOrbit
                __instance.Nutrition = (200f - Dayspastnorm) * Foodslice;
                Hydrationtogive = (200f - Dayspastnorm) * Hydrationslice;
            }
            else
            {
                // give minimal food and water, so a respawned character have some time to eat and drink.
                __instance.Nutrition = Foodslice * 3f;
                Hydrationtogive = Hydrationslice * 5f;
            }
            Traverse.Create(__instance).Property("Hydration").SetValue(Hydrationtogive);
        }
    }

    // Update food Nutrition Values
    [HarmonyPatch(typeof(Food))]
    [HarmonyPatch("OnUseSecondary")]
    public class FoodNutritionPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static void PatchFoodNutrition(Food __instance)
        {
            __instance.EatSpeed = 0.015f;
            switch (__instance.DisplayName)
            {
                case "Tomato Soup":
                    __instance.NutritionValue = 190f;
                    break;
                case "Corn Soup":
                    __instance.NutritionValue = 280f;
                    break;
                case "Canned Rice Pudding":
                    __instance.NutritionValue = 180f;
                    break;
                case "Pumpkin Soup":
                    __instance.NutritionValue = 375f;
                    break;
                case "Pumpkin Pie":
                    __instance.NutritionValue = 1000f;
                    break;
                case "Baked Potato":
                    __instance.NutritionValue = 45f;
                    break;
                case "French Fries":
                    __instance.NutritionValue = 120f;
                    break;
                case "Canned French Fries":
                    __instance.NutritionValue = 200f;
                    break;
                case "Milk":
                    __instance.NutritionValue = 2f;
                    break;
                case "Canned Condensed Milk":
                    __instance.NutritionValue = 370f;
                    break;
                case "Muffin":
                    __instance.NutritionValue = 840f;
                    break;
                case "Bread Loaf":
                    __instance.NutritionValue = 535f;
                    break;
                case "Cereal Bar":
                    __instance.NutritionValue = 200f;
                    break;
                case "Canned Powdered Eggs":
                    __instance.NutritionValue = 500f;
                    break;
                case "Canned Edamame":
                    __instance.NutritionValue = 190f;
                    break;
            }
        }
    }
    [HarmonyPatch(typeof(StackableFood))]
    [HarmonyPatch("OnUseSecondary")]
    public class StackableFoodNutritionPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static void PatchStackableNutrition(StackableFood __instance)
        {
            __instance.EatSpeed = 0.015f;
            switch (__instance.DisplayName)
            {
                case "Condensed Milk":
                    __instance.NutritionValue = 220f;
                    break;
                case "Cooked Soybean":
                    __instance.NutritionValue = 60f;
                    break;
                case "Cooked Rice":
                    __instance.NutritionValue = 40f;
                    break;
                case "Cooked Corn":
                    __instance.NutritionValue = 65f;
                    break;
                case "Cooked Pumpkin":
                    __instance.NutritionValue = 90f;
                    break;
                case "Powdered Eggs":
                    __instance.NutritionValue = 350f;
                    break;
                case "Cooked Tomato":
                    __instance.NutritionValue = 40f;
                    break;
            }
        }
    }

    // Changes fertilized Eggs requisites to hatch:
    [HarmonyPatch(typeof(FertilizedEgg))]
    [HarmonyPatch("OnAtmosphericTick")]
    public class FertilizedEggPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool PatchFertilizedEgg(FertilizedEgg __instance)
        {
            if (__instance.HasAtmosphere == true && __instance.WorldAtmosphere.PressureGasses >= 20f && __instance.WorldAtmosphere.PressureGasses < 200f && __instance.WorldAtmosphere.Temperature > 309.15f && __instance.WorldAtmosphere.Temperature <= 311.15f)
            {
                if (__instance.ParentSlot != null && __instance.ParentSlot.Occupant)
                {
                    //Good enviroment, but in Slot so don't hatch
                    return false;
                }
                //Good enviroment, Hatching.
                __instance.HatchTime -= 0.5f;
                if (__instance.HatchTime <= 0f)
                {
                    OnServer.Interact(__instance.InteractOnOff, 1, false);
                }
            }
            else if (__instance.HasAtmosphere == true) { 
                //Bad enviroment, not hatching.
            }
            return false;
        }
    }

    // Changes the hatch time of Fertilized Eggs:
    [HarmonyPatch(typeof(FertilizedEgg))]
    [HarmonyPatch("Awake")]
    public class FertilizedEggAwakePatch
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void PatchFertilizedEggAwake(FertilizedEgg __instance)
        {
            __instance.HatchTime = 16800f; //Should hatch in 7 days
                                           //TODO: Make a patch to show the egg status, if it's hatching or not and how long it will take.
        }
    }

    // Changes the time it takes to spoil eggs:
    [HarmonyPatch(typeof(Item))]
    [HarmonyPatch("Awake")]
    public class EggSpoilPatch
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void PatchEggsSpoil(Item __instance)
        {
            if (__instance.DisplayName == "Egg" || __instance.DisplayName == "Fertilized Egg")
            {
                Debug.Log($"__instance.name {__instance.name}    __instance.CustomName {__instance.CustomName}");
                __instance.DecayRate = 0.000091f; //should spoil in ~12 game days without refrigeration with this value
                Debug.Log($"new __instance.DecayRate: {__instance.DecayRate}");
            }
        }
    }

    //TODO: Find a way to write/update stuff in Stationpedia:
    /*[HarmonyPatch(typeof(Stationpedia))]
    [HarmonyPatch("Render")]
    public class StationpediaRenderPatch
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void PatchRenderStatiopedia(StationpediaPage __instance)
        {
            Debug.Log($" Statiopedia PageTitle: {Stationpedia.Instance.PageTitle.text} ");
        }
    }*/
}