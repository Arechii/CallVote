using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace Arechi.CallVote.Utils
{
    public static class ChatUtil
    {
        public static void Broadcast(string message, string icon = null, Color? color = null)
        {
            ChatManager.serverSendMessage(message, color ?? Color.yellow, iconURL: icon, useRichTextFormatting: true);
        }

        public static void SendMessage(this IRocketPlayer caller, string message, Color? color = null)
        {
            if (caller is ConsolePlayer)
                Logger.Log(message);
            else
                ChatManager.serverSendMessage(message, color ?? Color.yellow, toPlayer: ((UnturnedPlayer)caller).SteamPlayer(), useRichTextFormatting: true);
        }
    }
}
