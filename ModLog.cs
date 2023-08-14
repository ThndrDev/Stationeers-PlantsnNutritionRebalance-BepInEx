using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using System;


namespace PlantsnNutritionRebalance.Scripts
{
    internal class ModLog
    {
        public static String loglevel = "INFO";

        private enum Logs
        {
            DEBUG = 1,
            ERROR = 2,
            INFO = 0,
        }

        public static void Info(string line)
        {
            UnityEngine.Debug.Log((int)Enum.Parse(typeof(Logs), loglevel));
            if (((int)Enum.Parse(typeof(Logs), loglevel)) >= 0 || UnityEngine.Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("[PlantsnNutritionRebalance]: " + line);
            }
        }

        public static void Debug(string line)
        {
            if (((int)Enum.Parse(typeof(Logs), loglevel)) >= 1 || UnityEngine.Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("[PlantsnNutritionRebalance]: " + line);
            }
        }

        public static void Warning(string line)
        {
            UnityEngine.Debug.LogWarning("[PlantsnNutritionRebalance]: " + line);
        }

        public static void Error(string line)
        {
            if (((int)Enum.Parse(typeof(Logs), loglevel)) >= 2)
            {
                UnityEngine.Debug.LogError("[PlantsnNutritionRebalance]: " + line);
            }
        }

        public static void Error(Exception line)
        {
            UnityEngine.Debug.LogError("[PlantsnNutritionRebalance]: Exception :");
            UnityEngine.Debug.LogException(line);
        }

    }
}
