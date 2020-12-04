using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.CallVote.Commands.VoteCommands
{
    public class CommandMaxSkills : IRocketCommand
    {
        public string Name => "maxskills";

        public string Help => "Maxes out all skills for everyone";

        public string Syntax => "";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

		private readonly List<UnturnedSkill> skills = new List<UnturnedSkill>()
		{
			UnturnedSkill.Agriculture, UnturnedSkill.Cardio, UnturnedSkill.Cooking, UnturnedSkill.Crafting, UnturnedSkill.Dexerity, 
			UnturnedSkill.Diving, UnturnedSkill.Engineer, UnturnedSkill.Exercise, UnturnedSkill.Fishing, UnturnedSkill.Healing, 
			UnturnedSkill.Immunity, UnturnedSkill.Mechanic,UnturnedSkill.Outdoors, UnturnedSkill.Overkill, UnturnedSkill.Parkour, 
			UnturnedSkill.Sharpshooter, UnturnedSkill.Sneakybeaky, UnturnedSkill.Strength, UnturnedSkill.Survival, 
			UnturnedSkill.Toughness, UnturnedSkill.Vitality, UnturnedSkill.Warmblooded
		};

        public void Execute(IRocketPlayer caller, string[] command)
        {
			foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
			{
				foreach (var skill in skills)
                {
					player.SetSkillLevel(skill, byte.MaxValue);
                }
			}

			Plugin.Broadcast(Plugin.Instance.Translate("MAX_SKILLS"));
		}
    }
}
