using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Unturned.Player;

namespace Arechi.CallVote.Commands
{
    public class CVote : ICommand
    {
        public string Name => "CVote";

        public string[] Aliases => new [] {"cv"};

        public string Summary => "Vote or start one";

        public string Description => null;

        public string Syntax => "<vote name> | start <vote name> [arguments]";

        public IChildCommand[] ChildCommands => new IChildCommand[] { new CVoteStart(_callVotePlugin) };

        private readonly CallVotePlugin _callVotePlugin;

        public CVote(IPlugin plugin)
        {
            _callVotePlugin = (CallVotePlugin)plugin;
        }

        public bool SupportsUser(IUser user) => user is UnturnedUser;

        public async Task ExecuteAsync(ICommandContext context)
        {
            if (context.Parameters.Length != 1)
            {
                throw new CommandWrongUsageException("Missing arguments");
            }

            var activeVote = _callVotePlugin.VoteManager.GetActiveVote(await context.Parameters.GetAsync<string>(0));

            if (activeVote == null)
            {
                await _callVotePlugin.SendMessage(context.User, "Inactive");
                return;
            }

            if (activeVote.InCooldown)
            {
                await _callVotePlugin.SendMessage(context.User, "Cooldown", activeVote.Name, activeVote.Cooldown);
                return;
            }

            var player = (UnturnedUser)context.User;
            
            if (activeVote.Voters.Contains(player)) return;

            activeVote.Voters.Add(player);
            await _callVotePlugin.SendMessage(context.User, "Status", activeVote.Name, activeVote.Status());
        }
    }
}
