using Arechi.CallVote.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arechi.CallVote.Commands.VoteCommands
{
    public class CommandItemAll : IRocketCommand
    {
        public string Name => "itemall";

        public string Help => "Drops a (random) item for everyone";

        public string Syntax => "[itemId]";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        private static readonly Random random = new Random();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var items = Assets.find(EAssetType.ITEM).Cast<ItemAsset>().ToArray();
            ItemAsset item = null;

            if (command.Length > 0 && ushort.TryParse(command[0], out var itemId))
            {
                item = items.FirstOrDefault(i => i.id == itemId);
            }

            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                var playerItem = item ?? items[random.Next(items.Length)];

                player.GiveItem(playerItem.id, 1);
                player.SendMessage(Plugin.Instance.Translate("ITEM_ALL_RECEIVED", playerItem.itemName));
            }

            ChatUtil.Broadcast(Plugin.Instance.Translate("ITEM_ALL",
                item != null ? item.itemName : Plugin.Instance.Translate("RANDOM_ITEM")));
        }
    }
}
