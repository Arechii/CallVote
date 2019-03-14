using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.User;
using Rocket.Core.I18N;
using Rocket.Core.User;
using SDG.Unturned;

namespace Arechi.CallVote.Commands
{
    public class CVoteStart : IChildCommand
    {
        public string Name => "Start";

        public string[] Aliases => new[] {"s"};

        public string Summary => "Start a vote";

        public string Description => null;

        public string Syntax => "<vote name> [arguments]";

        public IChildCommand[] ChildCommands => null;

        private readonly CallVotePlugin _callVotePlugin;

        public CVoteStart(CallVotePlugin callVotePlugin)
        {
            _callVotePlugin = callVotePlugin;
        }

        public bool SupportsUser(IUser user) => true;

        public async Task ExecuteAsync(ICommandContext context)
        {
            if (context.Parameters.Length < 1)
            {
                await context.SendCommandUsage();
                return;
            }

            var voteName = await context.Parameters.GetAsync<string>(0);
            var argLine = context.Parameters.Skip(1);
            var votes = _callVotePlugin.ConfigurationInstance.Votes;
            var vote = votes.Find(v =>
                v.Name.Equals(voteName, StringComparison.OrdinalIgnoreCase) ||
                v.Alias.Equals(voteName, StringComparison.OrdinalIgnoreCase));

            if (vote == null)
            {
                await context.User.SendMessageAsync(string.Join(", ", votes.Select(v => v.Name).ToArray()), Color.Brown);
                return;
            }

            if (!_callVotePlugin.VoteManager.CanStartVote(vote, out var cooldown))
            {
                await context.User.SendLocalizedMessage(_callVotePlugin.Translations, "Cooldown", Color.Brown, 
                    vote.Name, cooldown);
                return;
            }

            if (Provider.clients.Count < vote.MinimumPlayers)
            {
                await context.User.SendLocalizedMessage(_callVotePlugin.Translations, "MinPlayers", Color.Brown,
                    vote.Name, vote.MinimumPlayers);
                return;
            }

            await _callVotePlugin.VoteManager.StartVote(vote, argLine);
        }
    }
}
