using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Arechi.CallVote
{
    public class Configuration : IRocketPluginConfiguration
    {
        [XmlArray(ElementName = "Votes")]
        [XmlArrayItem(ElementName = "Vote")]
        public List<VoteSettings> Votes;

        public void LoadDefaults()
        {
            Votes = new List<VoteSettings>
            {
                new VoteSettings("Day", "d", "/day"),
                new VoteSettings("Night", "n", "/night"),
                new VoteSettings("Kick", "k", "/kick", 1),
                new VoteSettings("Weather", "w", "/weather"),
                new VoteSettings("Rain", "r", "/weather storm"),
                new VoteSettings("NoRain", "nr", "/weather none"),
                new VoteSettings("Snow", "s", "/weather blizzard"),
                new VoteSettings("NoSnow", "ns", "/weather none"),
                new VoteSettings("HealAll", "ha", "/healall"),
                new VoteSettings("Airdrop", "a", "/airdrop"),
                new VoteSettings("AirdropAll", "aa", "/airdropall", requirePermission: true),
                new VoteSettings("VehicleAll", "va", "/vehicleall", requirePermission: true),
                new VoteSettings("MaxSkills", "ms", "/maxskills", requirePermission: true),
                new VoteSettings("ItemAll", "ia", "/itemall", requirePermission: true)
            };
        }
    }
}
