using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rocket.API.Scheduling;
using Rocket.API.User;
using Rocket.Core.Scheduling;

namespace Arechi.CallVote
{
    public class VoteManager
    {
        private readonly CallVotePlugin _callVotePlugin;
        private readonly ITaskScheduler _taskScheduler;
        private readonly IUserManager _userManager;

        private List<ActiveVote> _activeVotes;

        public VoteManager(CallVotePlugin callVotePlugin, ITaskScheduler taskScheduler, IUserManager userManager)
        {
            _callVotePlugin = callVotePlugin;
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
            await _callVotePlugin.AnnounceMessage("Start", activeVote.Name, activeVote.Alias, activeVote.Timer);
        }

        private void FinishVote(ActiveVote activeVote)
        {
            if (activeVote.Success())
            {
                activeVote.Execute();
                //await _callVotePlugin.AnnounceMessage("Success", activeVote.Name);
            }
            else
            {
                //await _callVotePlugin.AnnounceMessage("Failure", activeVote.Name);
            }

            activeVote.StartCooldown(this, _callVotePlugin, _taskScheduler);
            //await _callVotePlugin.AnnounceMessage("Cooldown", activeVote.Name, activeVote.Cooldown);
        }

        public void ReleaseVote(ActiveVote activeVote)
        {
            _activeVotes.Remove(GetActiveVote(activeVote.Name));
            //await _callVotePlugin.AnnounceMessage("Release", activeVote.Name);
        }

        public bool CanStartVote(Vote vote, out int? cooldown)
        {
            var activeVote = GetActiveVote(vote.Name);
            cooldown = activeVote?.Cooldown + (activeVote?.InCooldown != null && activeVote.InCooldown ? 0 : activeVote?.Timer);

            return activeVote == null;
        }
    }
}
