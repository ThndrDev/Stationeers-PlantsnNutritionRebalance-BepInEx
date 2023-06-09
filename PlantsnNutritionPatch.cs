using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;
using Assets.Scripts.Objects;


namespace PlantsnNutritionRebalance.Scripts
{
    // Adjusts the water consumption of plants:
    [HarmonyPatch(typeof(PlantLifeRequirements))]
    public class PlantLifeRequirementsPatch
    {
        [HarmonyPatch("get_WaterPerTick")]
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void WaterPerTickPatch(ref float __result)
        {
            __result *= 566.67f; //increase most plants water consumtpion from 6E-05f to 0.0034f with max consumption limited to this value.
            if (__result > 0.0034f)
                __result = 0.0034f;
            Debug.Log($"Water consumption: {__result}");
            //TODO: Transfer the water consumption boost to the prefab load, so it also shows the correct modded values in Stationpedia.
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
            __instance.WarningNutrition = 800f;
            __instance.CriticalNutrition = 400f;
            __instance.WarningHydration = 10.5f;
            __instance.CriticalHydration = 5.25f;
            // Fix for the character nutrition if it's bigger than 4000 (due to the 20% reduction of maxnutrition made in v0.9)
            if (__instance.Nutrition > 4000f)
                __instance.Nutrition *= 0.8f;
        }


        [HarmonyPatch("get_MaxNutritionStorage")]
        [HarmonyPostfix]
        [UsedImplicitly]
        static public void MaxNutritionPatch(ref float __result)
        {
            // Adjusts the max food of the character:
            __result = 4000f;
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
                case 1.5f: //stationeers difficulty, character food will last 8 game days
                    NutritionLossPerTick = 0.104167f;
                    break;
                case 1f: //normal difficulty, will last 10 game days
                    NutritionLossPerTick = 0.083333f;
                    break;
                case 0.5f: //easy difficulty, will last 12 game days
                    NutritionLossPerTick = 0.069444f;
                    break;
                case 0f: //user disabled food consumption directly on world config
                    NutritionLossPerTick = 0f;
                    break;
                default: // if it's none of the above, will try to calculate an hydrationLossPerTick based on DifficultySetting.HungerRate, examples:
                         // Difficulty in 3 should give 0.208334f, will last 4 game days 
                         // Difficulty in 2.25 should give 0.1562505f, will last 6 game days
                         // Difficulty in 0.001 should give 0.055555f, will last 14 days
                    float hungerdifficulty = Mathf.InverseLerp(0f, 3f, WorldManager.CurrentWorldSetting.DifficultySetting.HungerRate);
                    NutritionLossPerTick = Mathf.Lerp(0.055555f, 0.208334f, hungerdifficulty);
                    break;
            }
            // Complete rewrite of base method Human.LifeNutrition
            float num = NutritionLossPerTick * (__instance.OrganBrain.IsOnline ? 1f : WorldManager.CurrentWorldSetting.DifficultySetting.LifeFunctionLoggedOut);
            __instance.Nutrition -= num;
            // Entity.LifeNutrition
            if (__instance.IsArtificial)
            {
                return false;
            }
            if (__instance.Nutrition <= 0f) // increase the damage when nutrition reaches 0
            {
                __instance.DamageState.Damage(ChangeDamageType.Increment, 0.2f, DamageUpdateType.Starvation);
                return false;
            }
            if (__instance.DamageState.Starvation > 0f && __instance.Nutrition >= 400f) // Only heals the character when nutrition is over 400 (after "Nutrition Critical" warning)
            {
                __instance.DamageState.Damage(ChangeDamageType.Decrement, 0.1f, DamageUpdateType.Starvation);
            }
            return false;
        }
    }

    // Calculates the Food and Nutrition to give to the player based on the dayspast in the save, so we don't reward a character who dies with 100% stats
    [HarmonyPatch(typeof(Entity))]
    public static class OnLifeCreatedPatch
    {
        [HarmonyPatch("OnLifeCreated")]
        [HarmonyPostfix]
        [UsedImplicitly]
        private static void RespawnPatch(Entity __instance)
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
            //TODO: Make it to calculate the food and hydration based also on the difficulty setting
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
                    __instance.NutritionValue = 135f;
                    break;
                case "Corn Soup":
                    __instance.NutritionValue = 223f;
                    break;
                case "Canned Rice Pudding":
                    __instance.NutritionValue = 220f;
                    break;
                case "Pumpkin Soup":
                    __instance.NutritionValue = 270f;
                    break;
                case "Pumpkin Pie":
                    __instance.NutritionValue = 800f;
                    break;
                case "Baked Potato":
                    __instance.NutritionValue = 45f;
                    break;
                case "French Fries":
                    __instance.NutritionValue = 85f;
                    break;
                case "Canned French Fries":
                    __instance.NutritionValue = 150f;
                    break;
                case "Milk":
                    __instance.NutritionValue = 2.3f;
                    break;
                case "Canned Condensed Milk":
                    __instance.NutritionValue = 400f;
                    break;
                case "Muffin":
                    __instance.NutritionValue = 570f;
                    break;
                case "Bread Loaf":
                    __instance.NutritionValue = 290f;
                    break;
                case "Cereal Bar":
                    __instance.NutritionValue = 110f;
                    break;
                case "Canned Powdered Eggs":
                    __instance.NutritionValue = 550f;
                    break;
                case "Canned Edamame":
                    __instance.NutritionValue = 100f;
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
                    __instance.NutritionValue = 235f;
                    break;
                case "Cooked Soybean":
                    __instance.NutritionValue = 38f;
                    break;
                case "Cooked Rice":
                    __instance.NutritionValue = 50f;
                    break;
                case "Cooked Corn":
                    __instance.NutritionValue = 52f;
                    break;
                case "Cooked Pumpkin":
                    __instance.NutritionValue = 60f;
                    break;
                case "Powdered Eggs":
                    __instance.NutritionValue = 330f;
                    break;
                case "Cooked Tomato":
                    __instance.NutritionValue = 30f;
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
            if (__instance.HasAtmosphere == true && __instance.WorldAtmosphere.PressureGasses >= 50f && __instance.WorldAtmosphere.PressureGasses <= 120.5f && __instance.WorldAtmosphere.Temperature >= 309.15f && __instance.WorldAtmosphere.Temperature < 311.65f)
            {
                if (__instance.ParentSlot != null && __instance.ParentSlot.Occupant)
                {
                    //Good enviroment, but in Slot so don't hatch
                    return false;
                }
                //Good enviroment, Hatching.
                __instance.HatchTime -= 0.5f;
                //If it's near hatching (1 day left), then randomly move the egg to simulate the chick trying to pip the shell.
                if (__instance.HatchTime < 2400){
                    if (UnityEngine.Random.Range(0f, 10f) > 9)
                        __instance.RigidBody.AddForce(new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f)));
                }
                if (__instance.HatchTime <= 0f)
                {
                    OnServer.Interact(__instance.InteractOnOff, 1, false);
                }
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
                __instance.DecayRate = 0.000091f; //should spoil in ~12 game days without refrigeration
            }
        }
    }

    [HarmonyPatch(typeof(DynamicThing))]
    [HarmonyPatch("GetPassiveTooltip")]
    public class FertilizedeggSTooltipPatch
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static PassiveTooltip PatchFertilizedEggTooltip(PassiveTooltip __result, DynamicThing __instance)
        {
            if (__instance is FertilizedEgg fertilizedEgg)
            {
                __result.Extended = getTooltipText(fertilizedEgg);
            }
            return __result;
        }

        private static string getTooltipText(FertilizedEgg fertilizedEgg)
        {
            string text = "";
            if (fertilizedEgg.HasAtmosphere == true && fertilizedEgg.WorldAtmosphere.PressureGasses >= 50f && fertilizedEgg.WorldAtmosphere.PressureGasses < 120.5f && fertilizedEgg.WorldAtmosphere.Temperature >= 309.15f && fertilizedEgg.WorldAtmosphere.Temperature < 311.65f)
            {
                if (fertilizedEgg.ParentSlot != null && fertilizedEgg.ParentSlot.Occupant)
                {
                    //Good enviroment, but inside a Slot.
                    return text;
                }
                if (fertilizedEgg.HatchTime >= 2400f)
                    text = string.Format("The Egg <color=green>is hatching</color>\n");
                else
                    text = string.Format("The Egg <color=green>is almost hatching </color>\nThe chick <color=green>is trying to pip the shell</color>");
            }
            else
            {
                text = string.Format("The Egg <color=red>is not hatching</color>\n");
                if (fertilizedEgg.HasAtmosphere == false)
                {
                    text += string.Format("The Egg <color=red>needs an atmosphere</color>\n");
                }
                else
                {
                    if (fertilizedEgg.WorldAtmosphere.PressureGasses < 50f)
                        text += string.Format("The Egg <color=red>is in a low pressure environment</color> (P < 50kpa)\n");
                    else if (fertilizedEgg.WorldAtmosphere.PressureGasses >= 120.5f)
                        text += string.Format("The Egg <color=red>is in a high pressure environment</color> (P > 120kpa)\n");
                    if (fertilizedEgg.WorldAtmosphere.Temperature < 309.15f)
                        text += string.Format("The Egg temperature <color=red>is too cold</color> (T < 36°C)\n");
                    else if (fertilizedEgg.WorldAtmosphere.Temperature >= 311.65f)
                        text += string.Format("The Egg temperature <color=red>is too hot</color> (T > 38°C)\n");
                }
            }           
            return text;
        }
    }
}