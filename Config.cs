using System.Collections.Generic;

namespace Arechi.CallVote
{
    public class Config
    {
        public string Color = "Yellow";

        public List<Vote> Votes { get; set; } = new List<Vote>
        {
            new Vote("Day", "d", "/day"),
            new Vote("Night", "n", "/night"),
            new Vote("Wipe Map", "wm", ""),
            new Vote("Kick", "k", "/kick", 60, 300, 5),
            new Vote("Airdrop", "a", "/airdrop", 60, 600, 12)
        };
    }
}