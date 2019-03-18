using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Rocket.API.DependencyInjection;
using Rocket.API.Scheduling;
using Rocket.API.User;
using Rocket.Core.I18N;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Arechi.CallVote
{
    public class CallVotePlugin : Plugin<Config>
    {
        public readonly VoteManager VoteManager;

        private readonly IUserManager _userManager;

        public CallVotePlugin(IDependencyContainer container, ITaskScheduler taskScheduler, IUserManager userManager) : base("CallVote", container)
        {
            _userManager = userManager;
            VoteManager = new VoteManager(this, taskScheduler);
        }

        protected override async Task OnActivate(bool isFromReload)
        {
            VoteManager.Load();
            Logger.LogInformation(isFromReload ? "Reloaded!" : "Loaded!");
        }

        protected override async Task OnDeactivate()
        {
            Logger.LogInformation("Unloaded!");
        }

        internal async Task SendMessage(IUser user, string key, params object[] args)
        {
            await user.UserManager.BroadcastLocalizedAsync(Translations, key,
                Color.FromName(ConfigurationInstance.Color), args);
        }

        internal async Task AnnounceMessage(string key, params object[] arguments)
        {
            await _userManager.BroadcastLocalizedAsync(Translations, key,
                Color.FromName(ConfigurationInstance.Color), arguments);
        }

        public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
        {
            { "Dupe", "[{0}] You have already voted!" },
            { "Start", "[{0}] Vote started! Type /cvote {1} to vote! Time: {2}" },
            { "Status", "[{0}] Vote status: {1}%" },
            { "Success", "[{0}] The vote was a success!" },
            { "Failure", "[{0}] The vote has failed." },
            { "MinPlayers", "[{0}] At least {1} players are required to start this vote!" },
            { "Release", "[{0}] The vote cooldown is now over." },
            { "Error", "[{0}] Error 404: Vote not found." },
            { "Cooldown", "[{0}] The vote is in cooldown for {1} seconds." },
            { "Inactive", "This vote is not active." }
        };
    }
}
