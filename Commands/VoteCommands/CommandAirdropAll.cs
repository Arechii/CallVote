using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.CallVote.Commands.VoteCommands
{
    public class CommandAirdropAll : IRocketCommand
    {
        public string Name => "airdropall";

        public string Help => "Drops an airdrop for everyone";

        public string Syntax => "";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        private static readonly Random random = new Random();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var airdropNodeIds = LevelNodes.nodes.Where(n => n.type == ENodeType.AIRDROP).Cast<AirdropNode>().Select(n => n.id).ToArray();

            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                int index = random.Next(airdropNodeIds.Length);

                LevelManager.airdrop(player.Position, airdropNodeIds[index], 128f);
            }

            Plugin.Broadcast(Plugin.Instance.Translate("AIRDROP_ALL"));
        }
    }
}
