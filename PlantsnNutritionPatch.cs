using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;

namespace PlantsnNutritionRebalance.Scripts
{
    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("Awake")]
    public class PlantGrowStagePatch
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void PlantStagePatch(Plant __instance)
        {
            for (int i = 1; i < __instance.GrowthStates.Count; i++)
            {
                if (i < __instance.GrowthStates.Count - 2)
                {
                    __instance.GrowthStates[i].Length = 3600f;
                }
                else
                {
                    if (i == __instance.GrowthStates.Count - 2)
                    {
                        __instance.GrowthStates[i].Length = -1f;
                    }
                    else
                    { 
                        if (i == __instance.GrowthStates.Count - 1)
                         {
                                __instance.GrowthStates[i].Length = 0f;
                         }
                    }
                }
            }
            __instance.WaterPerTick = 0.0034f;
        }
    }
    [HarmonyPatch(typeof(Human))]
    public static class NutritionHydrationPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static public void HungerHydrationPatch(Human __instance)
        {
            __instance.MaxNutritionStorage = 5400f;
            __instance.MetabolismRate = 0.1125f;
            __instance.WarningNutrition = 1080f;
            __instance.CriticalNutrition = 540f;
            Human.MaxHydrationStorage = 42f;
            Human.BaseDehydrationRate = 0.0063f;
            __instance.WarningHydration = 10.5f;
            __instance.CriticalHydration = 5.25f;
        }
            
        [HarmonyPatch("OnLifeCreated")]
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void RespawnNutritionPatch(Human __instance)
        {
            float Dayspastnorm = WorldManager.DaysPast * Settings.CurrentData.SunOrbitPeriod * 10f;
            float Foodslice = __instance.MaxNutritionStorage / 200f;
            float Hydrationslice = Human.MaxHydrationStorage / 200f;

            if (Dayspastnorm <= 195)
            {
                // Calculate the food for respawn acordingly to the days past and SunOrbit
                __instance.Nutrition = (200f - Dayspastnorm) * Foodslice;
                __instance.Hydration = (200f - Dayspastnorm) * Hydrationslice;
            }
            else
            {
                __instance.Nutrition = Foodslice * 3f;
                __instance.Hydration = Hydrationslice * 5f;
            }
        }
    }
    [HarmonyPatch(typeof(Food))]
    [HarmonyPatch("OnUseSecondary")]
    public class FoodNutritionPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static void PatchFoodNutrition(Food __instance)
        {
            __instance.EatSpeed = 0.015f;
            if (__instance.DisplayName == "Tomato Soup")
            {
                __instance.NutritionValue = 210f;
            }
            if (__instance.DisplayName == "Corn Soup")
            {
                __instance.NutritionValue = 280f;
            }
            if (__instance.DisplayName == "Canned Rice Pudding")
            {
                __instance.NutritionValue = 180f;
            }
            if (__instance.DisplayName == "Pumpkin Soup")
            {
                __instance.NutritionValue = 290f;
            }
            if (__instance.DisplayName == "Pumpkin Pie")
            {
                __instance.NutritionValue = 950f;
            }
            if (__instance.DisplayName == "Baked Potato")
            {
                __instance.NutritionValue = 45f;
            }
            if (__instance.DisplayName == "French Fries")
            {
                __instance.NutritionValue = 120f;
            }
            if (__instance.DisplayName == "Canned French Fries")
            {
                __instance.NutritionValue = 200f;
            }
            if (__instance.DisplayName == "Milk")
            {
                __instance.NutritionValue = 2f;
            }
            if (__instance.DisplayName == "Canned Condensed Milk")
            {
                __instance.NutritionValue = 370f;
            }
            if (__instance.DisplayName == "Muffin")
            {
                __instance.NutritionValue = 650f;
            }
            if (__instance.DisplayName == "Bread Loaf")
            {
                __instance.NutritionValue = 380f;
            }
            if (__instance.DisplayName == "Cereal Bar")
            {
                __instance.NutritionValue = 140f;
            }
            if (__instance.DisplayName == "Canned Powdered Eggs")
            {
                __instance.NutritionValue = 500f;
            }
            if (__instance.DisplayName == "Canned Edamame")
            {
                __instance.NutritionValue = 190f;
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
            if (__instance.DisplayName == "Condensed Milk")
            {
                __instance.NutritionValue = 220f;
            }
            if (__instance.DisplayName == "Cooked Soybean")
            {
                __instance.NutritionValue = 60f;
            }
            if (__instance.DisplayName == "Cooked Rice")
            {
                __instance.NutritionValue = 40f;
            }
            if (__instance.DisplayName == "Cooked Corn")
            {
                __instance.NutritionValue = 65f;
            }
            if (__instance.DisplayName == "Cooked Pumpkin")
            {
                __instance.NutritionValue = 70f;
            }
            if (__instance.DisplayName == "Powdered Eggs")
            {
                __instance.NutritionValue = 350f;
            }
            if (__instance.DisplayName == "Cooked Tomato")
            {
                __instance.NutritionValue = 45f;
            }
        }
    }
}