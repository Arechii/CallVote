using Arechi.CallVote.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.CallVote.Commands.VoteCommands
{
    public class CommandHealAll : IRocketCommand
    {
        public string Name => "healall";

        public string Help => "Heals every player";

        public string Syntax => "";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                player.Heal(100, true, true);
                player.Hunger = 0;
                player.Thirst = 0;
                player.Infection = 0;
            }

            ChatUtil.Broadcast(Plugin.Instance.Translate("HEAL_ALL"));
        }
    }
}
