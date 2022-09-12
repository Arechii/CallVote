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

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>() { "cvote", "cv" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                var voteGroups = Plugin.Instance.Votes.GroupBy(v => v.Status);

                caller.SendMessage(Plugin.Instance.Translate("VOTE_HELP"));

                foreach (var voteGroup in voteGroups)
                {
                    var text = voteGroup.Select(v => $"{v.Settings.Name} ({v.Settings.Alias}){(voteGroup.Key == VoteStatus.CoolingDown ? $" [{v.CooldownTime}s]" : "")}");
                    caller.SendMessage(Plugin.Instance.Translate($"VOTE_HELP_{voteGroup.Key.ToString().ToUpper()}", string.Join(", ", text)));
                }

                return;
            }

            var voteName = command[0];
            var vote = Plugin.Instance.Votes.FirstOrDefault(v =>
                v.Settings.Name.Equals(voteName, StringComparison.CurrentCultureIgnoreCase) ||
                v.Settings.Alias.Equals(voteName, StringComparison.CurrentCultureIgnoreCase)
            );

            if (vote == null)
            {
                caller.SendMessage(Plugin.Instance.Translate("VOTE_NOT_FOUND", voteName), Color.red);
                return;
            }

            if (vote.Status == VoteStatus.Ongoing)
            {
                if (caller is UnturnedPlayer player) 
                    vote.AddVote(player);

                return;
            }

            try
            {
                if (vote.Settings.RequirePermission && !caller.HasPermission($"callvote.{vote.Settings.Name}"))
                    throw new VoteStartException("NO_PERMISSION");

                var arguments = command.Skip(1).ToList();

                vote.Start(arguments);
                
                if (caller is UnturnedPlayer player)
                    vote.AddVote(player);
            }
            catch (VoteStartException ex)
            {
                caller.SendMessage(ex.Message, Color.red);
            }
        }
    }
}
