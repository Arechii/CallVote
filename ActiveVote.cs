using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API.Plugins;
using Rocket.API.Scheduling;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Arechi.CallVote
{
    public class ActiveVote : Vote
    {
        public IEnumerable<string> Arguments { get; set; }

        public List<UnturnedUser> Voters { get; set; } = new List<UnturnedUser>();

        public bool InCooldown { get; set; }

        private ITask _cooldownTask;

        public ActiveVote(Vote vote, IEnumerable<string> arguments) : base(vote.Name, vote.Alias, vote.Command, vote.Timer, vote.Cooldown, vote.MinimumPlayers)
        {
            Arguments = new[] {vote.Command}.Concat(arguments);
        }

        public int Status() => (int)(decimal.Divide(Voters.Count, Provider.clients.Count == 0 ? 1 : Provider.clients.Count) * 100);

        public bool Success() => Status() >= 50;

        public void Execute() => CommandWindow.input.onInputText(string.Join(" ", Arguments));

        public void StartCooldown(VoteManager voteManager, IPlugin plugin, ITaskScheduler taskScheduler)
        {
            InCooldown = true;
            _cooldownTask = taskScheduler.SchedulePeriodically(plugin, () =>
            {
                if (--Cooldown > 0) return;

                voteManager.ReleaseVote(this);
                taskScheduler.CancelTask(_cooldownTask);
            }, "CooldownVote" + Name, TimeSpan.FromSeconds(1));
        }
    }
}
