using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arechi.CallVote
{
    public class Plugin : RocketPlugin<Configuration>
    {
        public static Plugin Instance;
        public List<IVote> Votes;

        protected override void Load()
        {
            Instance = this;
            Votes = Configuration.Instance.Votes.Select(v => new Vote(v)).ToList<IVote>();
        }

        protected override void Unload()
        {
            Instance = null;
        }

        public static void Broadcast(string message, string icon = null, Color? color = null)
        {
            ChatManager.serverSendMessage(message, color ?? Color.yellow, iconURL: icon, useRichTextFormatting: true);
        }

        public static void NotifyPlayer(UnturnedPlayer player, string message, Color? color = null)
        {
            ChatManager.serverSendMessage(message, color ?? Color.yellow, toPlayer: player.SteamPlayer(), useRichTextFormatting: true);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "VOTE_CHAT_FORMAT", "[Vote: {color}{vote}{/color}] {color}{text}{/color}" },
            { "START", "Started | Vote with /cv {0}" },
            { "COOLING_DOWN", "This vote is cooling down!: {0}s" },
            { "NOT_ENOUGH_ARGUMENTS", "You need at least {0} arguments to start this vote!" },
            { "NOT_ENOUGH_PLAYERS", "You need at least {0} players to start this vote!" },
            { "RESULT", "{0}% | Required: {1}%" },
            { "FAILURE", "Failed!" },
            { "SUCCESS", "Successful!" },
            { "COOLDOWN", "In cooldown for {0}s" },
            { "READY", "Ready!" },
            { "VOTE_NOT_FOUND", "Could not find vote {0}!" },

            //Custom vote commands
            { "AIRDROP_ALL", "Everyone received an airdrop!" },
            { "HEAL_ALL", "Everyone has been healed!" },
            { "ITEM_ALL", "Everyone received an item!: {0}" },
            { "MAX_SKILLS", "Everyone's skills have been maxed out!" },
            { "VEHICLE_ALL", "Everyone received a vehicle!: {0}" },
            { "ITEM_ALL_RECEIVED", "You received an item!: {0}" },
            { "VEHICLE_ALL_RECEIVED", "You received a vehicle!: {0}" },
            { "RANDOM_ITEM", "Random item" },
            { "RANDOM_VEHICLE", "Random vehicle" }
        };
    }
}
