using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BarberMaterialTweaks
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "dopadream.lethalcompany.BarberMaterialTweaks", PLUGIN_NAME = "BarberMaterialTweaks", PLUGIN_VERSION = "1.1.0";
        internal static new ManualLogSource Logger;
        internal static Material clayTex;

        void Awake()
        {
            Logger = base.Logger;


            //Credits to ButteryStancakes for asset loading code!

            try
            {
                AssetBundle barberBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "BarberMaterialTweaks"));
                clayTex = barberBundle.LoadAsset("ScissorGuyTex 1", typeof(Material)) as Material;
                barberBundle.Unload(false);
            }
            catch
            {
                Logger.LogError("Encountered some error loading asset bundle. Did you install the plugin correctly?");
                return;
            }


            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }


        [HarmonyPatch]
        class BarberMaterialTweaksPatches
        {
            [HarmonyReversePatch]
            [HarmonyPatch(typeof(ClaySurgeonAI), "Awake")]
            [HarmonyPostfix]

            static void ClaySurgeonAIPostStart(ClaySurgeonAI __instance,ref Material ___thisMaterial)
            {
                ___thisMaterial = Instantiate(Plugin.clayTex);
                __instance.scissorBlades[0].sharedMaterial = ___thisMaterial;
                __instance.scissorBlades[1].sharedMaterial = ___thisMaterial;
                __instance.skin.sharedMaterial = ___thisMaterial;
            }   
        }
    }
}