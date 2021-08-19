using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace StreamerMod
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
#if REACTOR
    [BepInDependency(Reactor.ReactorPlugin.Id)]
    [Reactor.ReactorPluginSide(Reactor.PluginSide.ClientOnly)]
#endif
    public class StreamerModPlugin : BasePlugin
    {
        private const string Id = "com.tbh.streamermod";
        private Harmony Harmony { get; } = new(Id);

        public static ConfigEntry<bool> Enable { get; private set; }
        public static ConfigEntry<string> TextReplacement { get; private set; }
        public static ConfigEntry<string> TextReplacementColor { get; private set; }

        public override void Load()
        {
            const string options = "Options";
            
            Enable = Config.Bind(options, "Enable Hide Code", true, "Enable/Disable code hider.");
            TextReplacement = Config.Bind(options, "Text Replacement", "Code\r\nCopied!",
                "Text that will be used for lobby code replacement.");
            TextReplacementColor = Config.Bind(options, "Text Replacement Color", "green",
                "Change text replacement's color.");

            Harmony.PatchAll();
        }
    }
}
