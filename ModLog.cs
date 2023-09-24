
using System;


namespace PlantsnNutritionRebalance.Scripts
{
    internal class ModLog
    {
        public static void Error(string line)
        {
            if (ConfigFile.LogLevel >= 0)
            {
                UnityEngine.Debug.LogError("[PlantsnNutritionRebalance]: " + line);
            }
        }

        public static void Error(Exception line)
        {
            UnityEngine.Debug.LogError("[PlantsnNutritionRebalance]: Exception :");
            UnityEngine.Debug.LogException(line);
        }
        
        public static void Info(string line)
        {
            if (ConfigFile.LogLevel >= 1)
            {
                UnityEngine.Debug.Log("[PlantsnNutritionRebalance]: " + line);
            }
        }

        public static void Debug(string line)
        {
            if (ConfigFile.LogLevel >= 2)
            {
                UnityEngine.Debug.Log("[PlantsnNutritionRebalance]: " + line);
            }
        }
    }
}
