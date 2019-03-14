using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Rocket.API.Plugins;
using Rocket.API.Scheduling;
using Rocket.API.User;
using Rocket.Core.I18N;
using Rocket.Core.Scheduling;

namespace Arechi.CallVote
{
    public class VoteManager
    {
        private readonly CallVotePlugin _callVotePlugin;

        private readonly ITaskScheduler _taskScheduler;

        private readonly IUserManager _userManager;

        private List<ActiveVote> _activeVotes;

        public VoteManager(IPlugin plugin, ITaskScheduler taskScheduler, IUserManager userManager)
        {
            _callVotePlugin = (CallVotePlugin)plugin;
            _taskScheduler = taskScheduler;
            _userManager = userManager;
        }

        public void Load()
        {
            _activeVotes = new List<ActiveVote>();
        }

        public ActiveVote GetActiveVote(string name) => _activeVotes.Find(v =>
            v.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
            v.Alias.Equals(name, StringComparison.OrdinalIgnoreCase));

        public async Task StartVote(Vote vote, IEnumerable<string> arguments = null)
        {
            var activeVote = new ActiveVote(vote, arguments);

            _activeVotes.Add(activeVote);
            _taskScheduler.ScheduleDelayed(_callVotePlugin, () => FinishVote(activeVote),
                "FinishVote" + activeVote.Name,
                TimeSpan.FromSeconds(activeVote.Timer));
            await BroadcastAsync("Start", activeVote.Name, activeVote.Alias, activeVote.Timer);
        }

        private void FinishVote(ActiveVote activeVote)
        {
            if (activeVote.Success())
            {
                activeVote.Execute();
                //await BroadcastAsync("Success", activeVote.Name);
            }
            else
            {
                //await BroadcastAsync("Failure", activeVote.Name);
            }

            //await BroadcastAsync("Cooldown", activeVote.Name, activeVote.Cooldown);
            activeVote.StartCooldown(this, _callVotePlugin, _taskScheduler);
        }

        public void ReleaseVote(ActiveVote activeVote)
        {
            //await BroadcastAsync("Release", activeVote.Name);
            _activeVotes.Remove(GetActiveVote(activeVote.Name));
        }

        public bool CanStartVote(Vote vote, out int? cooldown)
        {
            var activeVote = GetActiveVote(vote.Name);
            cooldown = activeVote?.Cooldown + (activeVote?.InCooldown != null && activeVote.InCooldown ? 0 : activeVote?.Timer);

            return activeVote == null;
        }

        private async Task BroadcastAsync(string key, params object[] arguments)
        {
            await _userManager.BroadcastLocalizedAsync(_callVotePlugin.Translations, key, Color.Brown, arguments);
        }
    }
}
