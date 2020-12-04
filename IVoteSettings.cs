namespace Arechi.CallVote
{
    public interface IVoteSettings
    {
        string Name { get; set; }

        string Alias { get; set; }

        string Command { get; set; }

        int MinimumArguments { get; set; }

        string Color { get; set; }

        string Icon { get; set; }

        bool Enabled { get; set; }

        int MinimumPlayers { get; set; }

        int RequiredPercent { get; set; }

        int Timer { get; set; }

        int CooldownTime { get; set; }
    }
}
