using BepInEx;
using HarmonyLib;
using UnityEngine;
using Assets.Scripts.Objects;
using System;

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts", "Plants and Nutrition", "0.8.1.0")]
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
    }
}