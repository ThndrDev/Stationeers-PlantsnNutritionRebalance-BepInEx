using Assets.Scripts.Networking;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.Objects;

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
                ____hydration = Mathf.Clamp(value, 0, ConfigFile.MaxHydrationStorage * 1.75f);

                if (NetworkManager.IsServer)
                {
                    __instance.NetworkUpdateFlags |= 1024;
                }
                return false; // Skip the original method
            }

            //2 - Patch for the ENtity GetHydrationStorage multiplier. Added in the "Shelter in Space" update
            [HarmonyPatch("GetHydrationStorage")]
            [HarmonyPostfix]
            [UsedImplicitly]
            static public void GetHydrationStoragePatch(Entity __instance, ref float __result)
            {
                // Adjusts the max food of the character:
                __result = ConfigFile.MaxHydrationStorage * __instance.GetFoodQualityMultiplier();
            }
        }

        // 3 - PlayerStateWindow Update patch: Change the hardcoded max Hydration value in the UI
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
    }
}


