using BepInEx;
using HarmonyLib;
using UnityEngine;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using System.Collections.Generic;
using System;

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts", "Plants and Nutrition", "0.8.4.0")]
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
            var harmony = new Harmony("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts");
            harmony.PatchAll();
            Action a = () => PlantGrowStagePatch.PatchPrefabs();
            Prefab.OnPrefabsLoaded += a;
            Log("Patch succeeded");
        }
        // Adjust the plants growth stages
        public class PlantGrowStagePatch
        {
            private static int[] fernStages = { 1, 1200, 1200, 1200, 1200, -1, 0 }; //892110467 & -1990600883 
            private static int[] potatoStages = { 1, 1200, 1200, 1200, 1200, -1, 0 }; //1929046963 & 1005571172


            private static Dictionary<int, int[]> plantStages = new Dictionary<int, int[]>();
            static PlantGrowStagePatch()
            {
                //seedbag seeds
                plantStages.Add(-1990600883, fernStages);
                plantStages.Add(1005571172, potatoStages);

                //seedbag plants
                plantStages.Add(892110467, fernStages);
                plantStages.Add(1929046963, potatoStages);
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
                }
                Debug.Log("Plants and Nutrition - Prefabs for selected plants are patched!");
            }
        }
    }
}