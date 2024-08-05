using System;
using System.Reflection;
using HarmonyLib;

namespace selftracker
{
    public static class HarmonyPatches
    {
        private static Harmony _instance;
        private const string InstanceId = "com.Trakker.gorillatag.IhopTrakker";

        public static bool IsPatched { get; private set; }

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (_instance == null)
                {
                    _instance = new Harmony(InstanceId);
                }
                _instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (_instance != null && IsPatched)
            {
                _instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}
