using HarmonyLib;

namespace StreamerMod
{
    [HarmonyPatch(typeof(TextBoxTMP))]
    public static class MenuPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TextBoxTMP.SetText))]
        public static void HideMenuCode(TextBoxTMP __instance)
        {
            if (!__instance || __instance.name != "GameIdText" || !StreamerModPlugin.Enable.Value) return;
            __instance.outputText.text = new string('*', __instance.text.Length);
        }
    }
}