namespace Arechi.CallVote
{
    public class Vote
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Command { get; set; }

        public int Timer { get; set; }

        public int? Cooldown { get; set; }

        public int MinimumPlayers { get; set; }

        public Vote(string name, string alias, string command, int timer = 60, int? cooldown = 300, int minimumPlayers = 1)
        {
            Name = name;
            Alias = alias;
            Command = command;
            Timer = timer;
            Cooldown = cooldown;
            MinimumPlayers = minimumPlayers;
        }
    }
}
