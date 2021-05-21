using Arechi.CallVote.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.CallVote.Commands.VoteCommands
{
    public class CommandMaxSkills : IRocketCommand
    {
        public string Name => "maxskills";

        public string Help => "Maxes out all skills for everyone";

        public string Syntax => "";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                player.MaxSkills();
            }

            ChatUtil.Broadcast(Plugin.Instance.Translate("MAX_SKILLS"));
        }
    }
}
