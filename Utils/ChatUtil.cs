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

        public static void SendMessage(this UnturnedPlayer player, string message, Color? color = null)
        {
            ChatManager.serverSendMessage(message, color ?? Color.yellow, toPlayer: player.SteamPlayer(), useRichTextFormatting: true);
        }
    }
}
