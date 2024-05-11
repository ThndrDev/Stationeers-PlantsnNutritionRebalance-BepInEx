using BepInEx;
using HarmonyLib;
using UnityEngine;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using System.Collections.Generic;

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("PlantsnNutrition", "Plants and Nutrition", "1.1.9.0")]
    public class PlantsnNutritionRebalancePlugin : BaseUnityPlugin
    {
        public static PlantsnNutritionRebalancePlugin Instance;

        private void Awake()
        {
            PlantsnNutritionRebalancePlugin.Instance = this;
            ModLog.Info("Start Patch");
            ConfigFile.HandleConfig(this);     // read (or create) the configuration file parameters
            var harmony = new Harmony("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts");
            harmony.PatchAll();   // Apply harmony patches
            Prefab.OnPrefabsLoaded += ApplyPatchesWhenPrefabsLoaded; // Change the plants growthstages in the prefab
            ModLog.Info("Patch succeeded");
        }


        private void ApplyPatchesWhenPrefabsLoaded()
        {
            ModLog.Info("Applying plants patches after prefabs are loaded");
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