using HarmonyLib;
using InnerNet;
using UnityEngine;
using UnityEngine.Events;

namespace StreamerMod
{
    [HarmonyPatch(typeof(GameStartManager))]
    public class LobbyPatch
    {
        private static bool ShouldReplace => StreamerModPlugin.Enable.Value && AmongUsClient.Instance.GameMode != GameModes.LocalGame;
        private static readonly string TextReplacement = StreamerModPlugin.TextReplacement.Value;
        private static readonly string TextReplacementColor = StreamerModPlugin.TextReplacementColor.Value;

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void CodeCopy(GameStartManager __instance)
        {
            if (ShouldReplace && __instance.GameRoomName)
                __instance.GameRoomName.text = $"<color={TextReplacementColor}>{TextReplacement}</color>";

            var publicButton = __instance.MakePublicButton.GetComponent<PassiveButton>();
            publicButton.OnClick.AddListener((UnityAction)CopyGameCode);
            CopyGameCode();

            void CopyGameCode()
            {
                var gameCode = GameCode.IntToGameName(AmongUsClient.Instance.GameId);
                GUIUtility.systemCopyBuffer = gameCode;
            }
        }
    }
}