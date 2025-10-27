using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("PlantsnNutrition", "Plants and Nutrition", "1.3.4.0")]
    public class PlantsnNutritionRebalancePlugin : BaseUnityPlugin
    {
        public static PlantsnNutritionRebalancePlugin Instance;

        private void Awake()
        {
            PlantsnNutritionRebalancePlugin.Instance = this;
            Debug.Log("Start Patch");
            ConfigFile.HandleConfig(this);     // read (or create) the configuration file parameters
            var harmony = new Harmony("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts");
            harmony.PatchAll();   // Apply harmony patches
            Debug.Log("Patch succeeded");
        }
    }
}