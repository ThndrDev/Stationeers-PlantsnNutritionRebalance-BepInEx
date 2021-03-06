using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace PlantsnNutritionRebalance.Scripts
{
    [BepInPlugin("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts", "Plants and Nutrition Rebalance", "0.6.0.0")]
    public class PlantsnNutritionRebalancePlugin : BaseUnityPlugin
    {
        public static PlantsnNutritionRebalancePlugin Instance;


        public void Log(string line)
        {
            Debug.Log("[PlantsnNutritionRebalance]: " + line);
        }

        void Awake()
        {
            PlantsnNutritionRebalancePlugin.Instance = this;
            Log("Hello World");

            try
            {
                // Harmony.DEBUG = true;
                var harmony = new Harmony("net.ThndrDev.stationeers.PlantsnNutritionRebalance.Scripts");
                harmony.PatchAll();
                Log("Patch succeeded");
            }
            catch (Exception e)
            {
                Log("Patch Failed");
                Log(e.ToString());
            }
        }
    }
}