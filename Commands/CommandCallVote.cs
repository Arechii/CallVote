using Arechi.CallVote.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arechi.CallVote.Commands
{
    public class CommandCallVote : IRocketCommand
    {
        public string Name => "callvote";

        public string Help => "Call a vote or vote for one";

        public string Syntax => "<vote> [arguments]";

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public List<string> Aliases => new List<string>() { "cvote", "cv" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                player.SendMessage($"/{Name} {Syntax}", Color.red);
                return;
            }

            var voteName = command[0];
            var vote = Plugin.Instance.Votes.FirstOrDefault(v =>
                v.Settings.Name.Equals(voteName, StringComparison.CurrentCultureIgnoreCase) ||
                v.Settings.Alias.Equals(voteName, StringComparison.CurrentCultureIgnoreCase)
            );

            if (vote == null)
            {
                player.SendMessage(Plugin.Instance.Translate("VOTE_NOT_FOUND", voteName), Color.red);
                return;
            }

            if (vote.Status == VoteStatus.Ongoing)
            {
                vote.AddVote(player);
                return;
            }

            try
            {
                var arguments = command.Skip(1).ToList();

                vote.Start(arguments);
                vote.AddVote(player);
            }
            catch (VoteStartException ex)
            {
                player.SendMessage(ex.Message, Color.red);
            }
        }
    }
}
