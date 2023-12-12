using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
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
using Assets.Scripts.Networking;

namespace PlantsnNutritionRebalance.Scripts
{
    internal class MaxHydrationStoragePatch
    {
        //In the Rocket update, they ditched the nice and beautiful MaxHydrationValue parameter and
        //for some reason, decided to make it a const value, whick makes the value hardcoded in all methods
        //that use the constant with the value set (5f).
        //So, what was modded in 1 beautiful line, now needs this entire file just to patch

        //1 - Entity Hydration patch: Remove the hardcoded clamp restriction in Entity.Hydration 
        [HarmonyPatch(typeof(Entity))]
        public static class HydrationPatch
        {
            [HarmonyPatch("set_Hydration")]
            [HarmonyPrefix]
            [UsedImplicitly]
            public static bool HydrationSetPatch(Thing __instance, float value, ref float ____hydration)
            {
                ____hydration = Mathf.Clamp(value, 0, ConfigFile.MaxHydrationStorage);

                if (NetworkManager.IsServer)
                {
                    __instance.NetworkUpdateFlags |= 1024;
                }
                return false; // Skip the original method
            }
        }
        // 2 - PlayerStateWindow Update patch: Change the hardcoded max Hydration value in the UI
        [HarmonyPatch(typeof(PlayerStateWindow))]
        public static class PlayerStateWindowPatches
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            [UsedImplicitly]
            static public void PlayerStateWindowPatch(PlayerStateWindow __instance, StateInstance ____hydrationState)
            {
                if (!__instance.IsVisible || !__instance.Parent)
                {
                    return;
                }
                ____hydrationState.UpdateText((int)(__instance.Parent.Hydration / ConfigFile.MaxHydrationStorage * 100f));
            }
        }

        // 3 - HydrationBase HydrateAmount patch: Change the hardcoded max Hydration value in the HydrationBase
        [HarmonyPatch(typeof(HydrationBase))]
        public static class HydrationBasePatches
        {
            [HarmonyPatch("HydrateAmount")]
            [HarmonyPrefix]
            [UsedImplicitly]
            public static bool HydrateAmountPatch(Entity consumer, HydrationBase __instance, ref float __result)
            {
                if (__instance.HydrationValue == 0f)
                {
                    __result = __instance.Quantity; // .Quantity;                    
                }
                __result = Mathf.Min((ConfigFile.MaxHydrationStorage - consumer.Hydration) / __instance.HydrationValue, __instance.Quantity);
                return false; // Skip the original method
            }

            /*
            // 4 - HydrationBase OnUseSecondary patch: Change the hardcoded max hydration value
            [HarmonyPatch("OnUseSecondary")]
            [HarmonyPrefix]
            [UsedImplicitly]
            public static bool OnUseSecondaryPatch(HydrationBase __instance, ref bool __result, bool ___doAction = false, float ___actionCompletedRatio = 1f)
            {
                Human human = __instance.RootParent as Human;
                if (human == null || Math.Abs(ConfigFile.MaxHydrationStorage - human.Hydration) < 0.005f || __instance.Quantity <= 0f)
                {
                    __result = __instance.Item.OnUseSecondary(___doAction, ___actionCompletedRatio);
                    return false; // skip the original method
                }
                return true; // run the original method
            }*/
        }
    }
}


