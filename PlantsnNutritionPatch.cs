using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;
using Assets.Scripts.Objects;
using Assets.Scripts.Atmospherics;
using System.Reflection;
using System;
using Objects.Structures;

namespace PlantsnNutritionRebalance.Scripts
{
    // Make plants transpirate some of the water they drink:
    [HarmonyPatch(typeof(Plant))]
    public class PlantPatch
    {
        [HarmonyPatch("TakePlantDrink")]
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void TakePlantDrinkPatch(Plant __instance, ref float __result)
        {
            if (ConfigFile.PlantWaterTranspirationPercentage == 0)
                return;
            else
            {
                GasMixture gasMixture = GasMixtureHelper.Create();
                float watertotranspirate = (__result / 100) * ConfigFile.PlantWaterTranspirationPercentage;
                float waterenergy = watertotranspirate * Chemistry.SpecificHeat(Chemistry.GasType.Water) * __instance.ParentTray.WaterAtmosphere.Temperature;
                gasMixture.Add(new GasMixture(new Mole(Chemistry.GasType.Water, watertotranspirate, waterenergy)), AtmosphereHelper.MatterState.All);
                __instance.BreathingAtmosphere.Add(gasMixture);
            }
        }
    }

    // Adjusts the water consumption of plants:
    [HarmonyPatch(typeof(PlantLifeRequirements))]
    public class PlantLifeRequirementsPatch
    {
        [HarmonyPatch("get_WaterPerTick")]
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void WaterPerTickPatch(ref float __result)
        {
            __result *= ConfigFile.PlantWaterConsumptionMultiplier;
            if (__result > ConfigFile.PlantWaterConsumptionLimit)
                __result = ConfigFile.PlantWaterConsumptionLimit;
        }
    }

    [HarmonyPatch(typeof(Human))]
    public static class HumanPatches
    {
        // Adjusts the Human hydration based on the world difficulty and the warning/critical alerts
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        [UsedImplicitly]
        static public void HydrationAndWarningsPatch(Human __instance, ref float ____hydrationLossPerTick)
        {
            float hydrationLoss;
            switch (DifficultySetting.Current.HydrationRate.Value)
            {
                case 1.5f: //stationeers difficulty, full water will last 100 game minutes (2 and a half days)
                    hydrationLoss = 0.0019446f;
                    break;
                case 1f: //normal difficulty, full water should last ~160 game minutes (4 days in sun/orbit 2)
                    hydrationLoss = 0.001798755f;
                    break;
                case 0.5f: //easy difficulty, full water should last ~220 game minutes
                    hydrationLoss = 0.0017258325f;
                    break;
                case 0f: //user disabled water consumption directly on world config
                    hydrationLoss = 0f;
                    break;
                default: // if it's none of the above, will try to calculate hydrationLossPerTick based on DifficultySetting.HydrationRate:
                    float a = Mathf.InverseLerp(0.0001f, 3f, DifficultySetting.Current.HydrationRate.Value);
                    hydrationLoss = Mathf.Lerp(0.001507065f, 0.002382135f, a);
                    break;
            }
            ____hydrationLossPerTick = ConfigFile.HydrationLossMultiplier * hydrationLoss;

            // Nutrition/hydration warnings:
            __instance.WarningNutrition = (ConfigFile.MaxNutritionStorage / 100) * ConfigFile.WarningNutrition;
            __instance.CriticalNutrition = (ConfigFile.MaxNutritionStorage / 100) * ConfigFile.CriticalNutrition;
            __instance.WarningHydration = (ConfigFile.MaxHydrationStorage / 100) * ConfigFile.WarningHydration;
            __instance.CriticalHydration = (ConfigFile.MaxHydrationStorage / 100) * ConfigFile.CriticalHydration;
        }


        [HarmonyPatch("get_BaseNutritionStorage")]
        [HarmonyPostfix]
        [UsedImplicitly]
        static public void BaseNutritionStoragePatch(ref float __result)
        {
            // Adjusts the max food of the character:
            __result = ConfigFile.MaxNutritionStorage;
        }

        static float LastNutritionLossPerTick;
        // Adjusts the HungerRate based on the world difficulty and changes the damage system for Starvation
        [HarmonyPatch("LifeNutrition")]
        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool MetabolismRatePatch(Human __instance)
        {
            float NutritionLossPerTick;
            switch (DifficultySetting.Current.HungerRate.Value)
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
                default: // if it's none of the above, will try to calculate NutritionLossPerTick based on DifficultySetting.HungerRate, examples:
                         // Difficulty in 3 should give 0.208334f, will last 4 game days 
                         // Difficulty in 2.25 should give 0.1562505f, will last 6 game days
                         // Difficulty in 0.001 should give 0.055555f, will last 14 days
                    float hungerdifficulty = Mathf.InverseLerp(0f, 3f, DifficultySetting.Current.HungerRate.Value);
                    NutritionLossPerTick = Mathf.Lerp(0.055555f, 0.208334f, hungerdifficulty);
                    break;
            }
            // Save the last NutritionLossPerTick, to be used on the player respawn patch
            LastNutritionLossPerTick = ConfigFile.NutritionLossMultiplier * NutritionLossPerTick;

            // Rewrite of base method Human.LifeNutrition
            float num = ConfigFile.NutritionLossMultiplier * NutritionLossPerTick * ((__instance.OrganBrain != null && __instance.OrganBrain.IsOnline) ? 1f : DifficultySetting.Current.OfflineMetabolism);
            if (__instance.IsSleeping)
            {
                num *= 0.5f;
            }
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
            if (__instance.DamageState.Starvation > 0f && __instance.Nutrition >= __instance.CriticalNutrition) // Only heals the character when nutrition is over "Nutrition Critical" warning.
            {
                __instance.DamageState.Damage(ChangeDamageType.Decrement, 0.1f, DamageUpdateType.Starvation);
            }
            return false;
        }

        // Calculates the Food and Nutrition to give to the player based on the dayspast in the save, so we don't reward a character who dies with 100% stats
        [HarmonyPatch("OnLifeCreated")]
        [HarmonyPostfix]
        [UsedImplicitly]
        private static void RespawnPatch(Human __instance, ref float ____hydrationLossPerTick, ref bool isRespawn)
        {
            float HydrationToGive;

            if (ConfigFile.EnableRespawnPenaltyLogic)
            {
                // Get how long in days the max nutrition should last with the current configuration parameters
                int NormalizedMaxHungerDays = Mathf.RoundToInt(ConfigFile.MaxNutritionStorage / (LastNutritionLossPerTick==0f ? 0.083333f : LastNutritionLossPerTick) / 2 / 60 / 
                    ((float)Settings.CurrentData.DayLength));
                ModLog.Debug("Human-OnLifeCreated: NormalizedMaxHungerDays: " + NormalizedMaxHungerDays + " ConfigFile.MaxNutritionStorage: " + ConfigFile.MaxNutritionStorage +
                             " LastNutritionLossPerTick:" + LastNutritionLossPerTick + " Settings.CurrentData.DayLength: " + (float)Settings.CurrentData.DayLength);
                // Then, calculate a nutrition slice for each day
                float NutritionSlicePerDay = ConfigFile.MaxNutritionStorage / NormalizedMaxHungerDays;
                ModLog.Debug("Human-OnLifeCreated: NutritionSlicePerDay: " + NutritionSlicePerDay);
                // Get the normalized days
                //float DaysPastNorm = WorldManager.DaysPast * Settings.CurrentData.SunOrbitPeriod;
                ModLog.Debug("Human-OnLifeCreated: Current day: " + WorldManager.DaysPast);

                // If the character is a new player joining and not a old character who died, and the configfile is set to modify the amount of food to give to the new player
                // apply the desired amount of food for the new character
                if (!isRespawn && ConfigFile.CustomNewPlayerRespawn)
                {
                    __instance.Nutrition = ConfigFile.CustomNewPlayerRespawnNutrition * (ConfigFile.MaxNutritionStorage / 100f);
                    ModLog.Info("Human-OnLifeCreated: Nutrition given because CustomNewPlayerRespawn is true and a new player joined: " + __instance.Nutrition);
                }
                // If it's a respawn and the DaysPastNorm is lower than the NormalizedMaxHungerDays, that means we should calculate the amount of food to give to the respawning character
                else if (WorldManager.DaysPast < NormalizedMaxHungerDays)
                {
                    __instance.Nutrition = NutritionSlicePerDay * (NormalizedMaxHungerDays - WorldManager.DaysPast);
                    ModLog.Info("Human-OnLifeCreated: Nutrition given for an player who died and are respawning: " + __instance.Nutrition);
                }
                //if DaysPastNorm is equal or bigger than NormalizedMaxHungerDays, that means we should give a minimal amount of food, just enough for the character to go eat something
                else
                {
                    __instance.Nutrition = ConfigFile.MaxNutritionStorage / 95;
                    ModLog.Info("Human-OnLifeCreated: Minimal Nutrition given for respawing player after "+ WorldManager.DaysPast + " days passed: " + __instance.Nutrition);
                }
                // Now do the same logic, but for Hydration:
                //Make sure we don't get values with 0 so they don't break the division from NormalizedMaxHydrationDays
                //for hydrationLossPerTick
                float hydrationLossPerTick = (____hydrationLossPerTick == 0f ? 0.001798755f : ____hydrationLossPerTick);
                float HydrationRate = (DifficultySetting.Current.HydrationRate.Value == 0f ? 1f : DifficultySetting.Current.HydrationRate.Value);
                int NormalizedMaxHydrationDays = Mathf.RoundToInt(ConfigFile.MaxHydrationStorage / ((hydrationLossPerTick + hydrationLossPerTick * 0.2f) * HydrationRate)  / 2 / 60 /
                    ((float)Settings.CurrentData.DayLength));
                ModLog.Debug("Human-OnLifeCreated: NormalizedMaxHydrationDays: " + NormalizedMaxHydrationDays + " hydrationLossPerTick: " + hydrationLossPerTick + " HydrationRate: " +
                    HydrationRate);
                float HydrationSlicePerDay = ConfigFile.MaxHydrationStorage / NormalizedMaxHydrationDays;
                ModLog.Debug("Human-OnLifeCreated: HydrationSlicePerDay: " + HydrationSlicePerDay);
                if (!isRespawn && ConfigFile.CustomNewPlayerRespawn)
                {
                    HydrationToGive = ConfigFile.CustomNewPlayerRespawnHydration * (ConfigFile.MaxHydrationStorage / 100f);
                    ModLog.Info("Human-OnLifeCreated: Hydration given because CustomNewPlayerRespawn is true and a new player joined: " + __instance.Nutrition);
                }
                else if (WorldManager.DaysPast < NormalizedMaxHydrationDays)
                {
                    HydrationToGive = HydrationSlicePerDay * (NormalizedMaxHydrationDays - WorldManager.DaysPast);
                    ModLog.Info("Human-OnLifeCreated: Hydration given because a player who died are respawning: " + HydrationToGive);
                }
                //if DaysPastNorm is equal or bigger than NormalizedMaxHydrationDays, that means we should give a minimal amount of hydration, just enough for the character to go drink something
                else
                {
                    HydrationToGive = ConfigFile.MaxHydrationStorage / 93; //give a little more than 1% water
                    ModLog.Info("Human-OnLifeCreated: Minimal Hydration given for respawing player after " + WorldManager.DaysPast + " days passed: " + HydrationToGive);
                }
            }
            else
            {
                //Respawn Penalty logic disabled, give full nutrition and hydration to the player:
                __instance.Nutrition = ConfigFile.MaxNutritionStorage;
                HydrationToGive = ConfigFile.MaxHydrationStorage;
            }
            Traverse.Create(__instance).Property("Hydration").SetValue(HydrationToGive);
        }
    }

    // Adjusts the time taken to drink in the fountain
    [HarmonyPatch(typeof(StructureDrinkingFountain))]
    public static class StructureDrinkingFountainPatch
    {
        [HarmonyPatch("HydrateTime")]
        [HarmonyPostfix]
        [UsedImplicitly]
        public static void HydrationSetPatch(ref float __result)
        {
            __result = __result * 0.1f;
        }

    }

    public static class FoodsValues
    {
        public static float getFoodNutrition(String FoodDisplayName)
        {
            ModLog.Info("FoodValues: Checking if there's a nutritional value set for: " + FoodDisplayName);
            if (typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "Nutrition") != null)
            {
                return (float)typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "Nutrition").GetValue(null);
            }
            return -1f;
        }

        public static float getFoodEatSpeed(String FoodDisplayName)
        {
            ModLog.Info("FoodValues: Checking if there's an EatSpeed value for: " + FoodDisplayName );
            if (typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "EatSpeed") != null)
            {                
                return (float)typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "EatSpeed").GetValue(null);
            }
            return -1f;
        }

        public static float getFoodHydration(String FoodDisplayName)
        {
            ModLog.Info("FoodValues: Checking if there's an Hydration value for: " + FoodDisplayName);
            if (typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "Hydration") != null)
            {
                return (float)typeof(ConfigFile).GetField(FoodDisplayName.Trim().Replace(" ", "") + "Hydration").GetValue(null);
            }
            return 0f;
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
            if (ConfigFile.EnableFoodChanges)
            {
                try
                {
                    float EatSpeed = FoodsValues.getFoodEatSpeed(__instance.DisplayName);
                    float NutritionValue = FoodsValues.getFoodNutrition(__instance.DisplayName);
                    if (EatSpeed > 0f)
                    {
                        ModLog.Debug("Food-OnUseSecondary: Changing EatSpeed for food " + __instance.DisplayName + " From: "+ __instance.EatSpeed + " To: " + EatSpeed);
                        __instance.EatSpeed = EatSpeed;
                    }
                    if (NutritionValue > 0f)
                    {
                        ModLog.Debug("Food-OnUseSecondary: Changing NutritionValue for food " + __instance.DisplayName + " From: " + __instance.NutritionValue + " To: " + NutritionValue);
                        __instance.NutritionValue = NutritionValue;
                    }
                }
                catch (Exception ex)
                {
                    ModLog.Error(ex);
                }
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
            if (ConfigFile.EnableFoodChanges)
            {
                try
                {
                    float EatSpeed = FoodsValues.getFoodEatSpeed(__instance.DisplayName);
                    float NutritionValue = FoodsValues.getFoodNutrition(__instance.DisplayName);
                    if (EatSpeed >= 0f)
                    {
                        ModLog.Debug("StackableFood-OnUseSecondary: Changing EatSpeed for StackableFood " + __instance.DisplayName + " From: " + __instance.EatSpeed + " To: " + EatSpeed);
                        __instance.EatSpeed = EatSpeed;
                    }
                    if (NutritionValue >= 0f)
                    {
                        ModLog.Debug("StackableFood-OnUseSecondary: Changing NutritionValue for StackableFood " + __instance.DisplayName + " From: " + __instance.NutritionValue + " To: " + NutritionValue);
                        __instance.NutritionValue = NutritionValue;
                    }
                }
                catch (Exception ex)
                {
                    ModLog.Error(ex);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("OnUseSecondary")]
    public class PlantNutritionPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static void PatchPlantNutrition(Plant __instance)
        {
            if (ConfigFile.EnableFoodChanges)
            {
                try
                {
                    float NutritionValue = FoodsValues.getFoodNutrition(__instance.DisplayName);
                    if (NutritionValue >= 0f)
                    {
                        ModLog.Debug("Plant-OnUseSecondary: Changing NutritionValue for Plant " + __instance.DisplayName + " From: " + __instance.NutritionValue + " To: " + NutritionValue);
                        __instance.NutritionValue = NutritionValue;
                    }
                }
                catch (Exception ex)
                {
                    ModLog.Error(ex);
                }
            }
        }
    }
    
    // Update food Nutrition Values
    [HarmonyPatch(typeof(Stationpedia))]
    [HarmonyPatch("AddNutrition")]
    public class StationpediaPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static bool AddNutrition(ref Thing prefab, ref StationpediaPage page)
        {
            if (ConfigFile.EnableFoodChanges)
            {
                Food food = prefab as Food;
                if (food != null)
                {
                    float NutritionValue = FoodsValues.getFoodNutrition(food.DisplayName);
                    if (NutritionValue >= 0f)
                    {
                        food.NutritionValue = NutritionValue;
                        ModLog.Debug("Stationpedia-AddNutrition: Changed nutritional value of food: " + prefab.DisplayName + " to: " + NutritionValue);
                    }

                }
                StackableFood stackableFood = prefab as StackableFood;
                if (stackableFood != null)
                {
                    float NutritionValue = FoodsValues.getFoodNutrition(stackableFood.DisplayName);
                    if (NutritionValue >= 0f)
                    {
                        stackableFood.NutritionValue = NutritionValue;
                        ModLog.Debug("Stationpedia-AddNutrition: Changed nutritional value of stackableFood: " + prefab.DisplayName + " to: " + NutritionValue);
                    }
                }

                Plant Plantfood = prefab as Plant;
                if (Plantfood != null)
                {
                    float NutritionValue = FoodsValues.getFoodNutrition(Plantfood.DisplayName);
                    if (NutritionValue >= 0f)
                    {
                        Plantfood.NutritionValue = NutritionValue;
                        ModLog.Debug("Stationpedia-AddNutrition: Changed nutritional value of stackableFood: " + prefab.DisplayName + " to: " + NutritionValue);
                    }
                }
                return true;
            }
            else
            {
                return false; 
            }
        }
    }

    //Hydrating by food
    [HarmonyPatch(typeof(Item), "OnUseItem")] //Plants, Food and Eggs is different consumable Items
    internal class HydratingFoodPatch
    {
        [HarmonyPrefix] //unnamed patching errors on game load
        [UsedImplicitly]
        private static void HydratingByConsumedQuantity(Item __instance, ref float quantity, ref Thing useOnThing)
        {
            if (ConfigFile.EnableFoodHydration)
            {
                if (__instance is INutrition)
                {
                    try
                    {
                        float hydrationValue = FoodsValues.getFoodHydration(__instance.DisplayName);
                        ModLog.Debug("Item-OnUseItem: item Name: " + __instance.DisplayName + " Hydration rate received from config file: " + hydrationValue);
                        Human human = useOnThing as Human;
                        if (human && hydrationValue != 0f)
                        {
                            float hydrate = quantity * hydrationValue;
                            ModLog.Debug("Item-OnUseItem: item Name: " + __instance.DisplayName + " total Hydrate value got when considering the food quantity: " + hydrate);
                            if (hydrate > 0f)
                            {
                                float humanHydrationtoFill = ConfigFile.MaxHydrationStorage - human.Hydration;
                                if (hydrate > humanHydrationtoFill)
                                {
                                    hydrate = Mathf.Clamp(hydrate, 0f, humanHydrationtoFill);
                                    ModLog.Debug("Item-OnUseItem: Clamped hydration amount because it's positive and bigger than the amount available to fill 100%. hydrate: " + hydrate);
                                }
                            }
                            else if (hydrate < 0f)
                            {
                                if (hydrate * -1f > human.Hydration)
                                {
                                    hydrate = Mathf.Clamp(hydrate, human.Hydration * -1f, 0f);
                                    ModLog.Debug("Item-OnUseItem: Clamped hydration amount because it's negative and the hydration avaliable in the character is lower than the amount the food will remove. dehydrate amount: " + hydrate);
                                }
                            }
                            ModLog.Info("Item-OnUseItem: Final hydration got/lost from eating " + __instance.DisplayName + ": " + hydrate);
                            human.Hydrate(hydrate);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModLog.Error(ex);
                    }
                }
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
        public static bool PatchFertilizedEgg(FertilizedEgg __instance, bool ____viable)
        {
            if (____viable && __instance.HasAtmosphere == true && __instance.WorldAtmosphere.PressureGasses >= ConfigFile.EggMinimumPressureToHatch && __instance.WorldAtmosphere.PressureGasses <= ConfigFile.EggMaximumPressureToHatch && __instance.WorldAtmosphere.Temperature >= ConfigFile.EggMinimumTemperatureToHatch && __instance.WorldAtmosphere.Temperature < ConfigFile.EggMaximumTemperatureToHatch)
            {
                if (__instance.ParentSlot != null && __instance.ParentSlot.Occupant)
                {
                    //Good enviroment, but in Slot so don't hatch
                    return false;
                }
                //Good enviroment, Hatching.
                __instance.HatchTime -= 0.5f;
                //If it's near hatching, then randomly move the egg to simulate the chick trying to pip the shell.
                if (__instance.HatchTime < ConfigFile.EggNearHatching)
                {
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
            __instance.HatchTime = ConfigFile.EggHatchTime;
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
                __instance.DecayRate = ConfigFile.EggDecayRate;
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
                if (Traverse.Create(__instance).Field("_viable").GetValue<bool>())
                __result.Extended += getTooltipText(fertilizedEgg);
            }
            return __result;
        }

        private static string getTooltipText(FertilizedEgg fertilizedEgg)
        {
            string text = "";
            if (fertilizedEgg.HasAtmosphere == true && fertilizedEgg.WorldAtmosphere.PressureGasses >= ConfigFile.EggMinimumPressureToHatch && fertilizedEgg.WorldAtmosphere.PressureGasses < ConfigFile.EggMaximumPressureToHatch && fertilizedEgg.WorldAtmosphere.Temperature >= ConfigFile.EggMinimumTemperatureToHatch && fertilizedEgg.WorldAtmosphere.Temperature < ConfigFile.EggMaximumTemperatureToHatch)
            {
                if (fertilizedEgg.ParentSlot != null && fertilizedEgg.ParentSlot.Occupant)
                {
                    //Good enviroment, but inside a Slot.
                    return text;
                }
                if (fertilizedEgg.HatchTime >= ConfigFile.EggNearHatching)
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
                    if (fertilizedEgg.WorldAtmosphere.PressureGasses < ConfigFile.EggMinimumPressureToHatch)
                        text += string.Format("The Egg <color=red>is in a low pressure environment</color> (P < " + ConfigFile.EggMinimumPressureToHatch + ")\n");
                    else if (fertilizedEgg.WorldAtmosphere.PressureGasses >= 120.5f)
                        text += string.Format("The Egg <color=red>is in a high pressure environment</color> (P > " + ConfigFile.EggMaximumPressureToHatch + ")\n");
                    if (fertilizedEgg.WorldAtmosphere.Temperature < 309.15f)
                        text += string.Format("The Egg temperature <color=red>is too cold</color> (T < " + Mathf.RoundToInt(ConfigFile.EggMinimumTemperatureToHatch-273.15f) + "°C)\n");
                    else if (fertilizedEgg.WorldAtmosphere.Temperature >= 311.65f)
                        text += string.Format("The Egg temperature <color=red>is too hot</color> (T > " + Mathf.RoundToInt(ConfigFile.EggMaximumTemperatureToHatch-273.15f) + "°C)\n");
                }
            }
            return text;
        }
    }
}