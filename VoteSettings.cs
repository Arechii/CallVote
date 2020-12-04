using System.Xml.Serialization;

namespace Arechi.CallVote
{
	public class VoteSettings : IVoteSettings
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }

		[XmlAttribute("Alias")]
		public string Alias { get; set; }

		[XmlAttribute("Command")]
		public string Command { get; set; }

		[XmlAttribute("MinimumArguments")]
		public int MinimumArguments { get; set; }

		[XmlAttribute("Color")]
		public string Color { get; set; }

		[XmlAttribute("Icon")]
		public string Icon { get; set; }

		[XmlAttribute("Enabled")]
		public bool Enabled { get; set; }

		[XmlAttribute("MinimumPlayers")]
		public int MinimumPlayers { get; set; }

		[XmlAttribute("RequiredPercent")]
		public int RequiredPercent { get; set; }

		[XmlAttribute("Timer")]
		public int Timer { get; set; }

		[XmlAttribute("CooldownTime")]
		public int CooldownTime { get; set; }

		public VoteSettings() { }

		public VoteSettings(string name, string alias, string command, int minArgs = 0, string color = "#fff333", string icon = "",
			bool enabled = true, int minPlayers = 1, int reqPercent = 50, int timer = 60, int cooldown = 300)
		{
			Name = name;
			Alias = alias;
			Command = command;
			MinimumArguments = minArgs;
			Color = color;
			Icon = icon;
			Enabled = enabled;
			MinimumPlayers = minPlayers;
			RequiredPercent = reqPercent;
			Timer = timer;
			CooldownTime = cooldown;
		}
	}
}
