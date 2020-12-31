using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arechi.CallVote
{
    public class Vote : IVote
    {
        public VoteSettings Settings { get; set; }

        public List<string> Arguments { get; set; }

        public List<ulong> Voters { get; set; } = new List<ulong>();

        public VoteStatus Status { get; set; } = VoteStatus.Ready;

        public Coroutine thisCoroutine1 { get; set; }

        public Coroutine thisCoroutine2 { get; set; }

        public int CooldownTime { get; protected set; }

        public Vote(VoteSettings settings)
        {
            Settings = settings;
        }

        public int GetPercentage()
        {
            return (int)(decimal.Divide(Voters.Count, Provider.clients.Count == 0 ? 1 : Provider.clients.Count) * 100);
        }

        public VoteResult GetResult()
        {
            return GetPercentage() >= Settings.RequiredPercent ? VoteResult.Success : VoteResult.Failure;
        }

        public void Start(List<string> arguments)
        {
            if (!CanStart(arguments)) return;

            Status = VoteStatus.Ongoing;
            Arguments = arguments;

            SendMessage("START", Settings.Alias);

            thisCoroutine1 = Plugin.Instance.StartCoroutine(Start());
        }

        protected IEnumerator Start()
        {
            var time = Settings.Timer;

            while (time != 0)
            {
                yield return new WaitForSeconds(1f);
                time--;
            }

            Stop();
        }

        protected bool CanStart(List<string> arguments)
        {
            if (Status == VoteStatus.CoolingDown)
                throw new VoteStartException("COOLING_DOWN", CooldownTime);

            if (arguments.Count < Settings.MinimumArguments)
                throw new VoteStartException("NOT_ENOUGH_ARGUMENTS", Settings.MinimumArguments);

            if (Provider.clients.Count < Settings.MinimumPlayers)
                throw new VoteStartException("NOT_ENOUGH_PLAYERS", Settings.MinimumPlayers);

            return true;
        }

        public void AddVote(UnturnedPlayer player)
        {
            if (Voters.Contains(player.CSteamID.m_SteamID)) return;

            Voters.Add(player.CSteamID.m_SteamID);
            SendMessage("RESULT", GetPercentage(), Settings.RequiredPercent);

            if (GetResult() != VoteResult.Success) return;

            Plugin.Instance.StopCoroutine(thisCoroutine1);
            Stop();
        }

        public void Stop()
        {
            if (GetResult() == VoteResult.Failure)
            {
                SendMessage("FAILURE");
                Cooldown();
                return;
            }

            var command = new List<string> { Settings.Command };

            if (Arguments != null)
                command.AddRange(Arguments);

            SendMessage("SUCCESS");
            R.Commands.Execute(new ConsolePlayer(), string.Join(" ", command));
            Cooldown();
        }

        public void Cooldown()
        {
            Status = VoteStatus.CoolingDown;

            SendMessage("COOLDOWN", Settings.CooldownTime);
            thisCoroutine2 = Plugin.Instance.StartCoroutine(Cooldown(Settings.CooldownTime));

            Plugin.Instance.StopCoroutine(thisCoroutine1);
        }

        protected IEnumerator Cooldown(int cooldown)
        {
            CooldownTime = cooldown;

            while (CooldownTime != 0)
            {
                yield return new WaitForSeconds(1f);
                CooldownTime--;
            }

            Voters.Clear();
            Status = VoteStatus.Ready;
            SendMessage("READY");
        }

        protected void SendMessage(string translationKey, params object[] args)
        {
            var message = Plugin.Instance.Translate("VOTE_CHAT_FORMAT")
                .Replace("{color}", $"<color={Settings.Color}>")
                .Replace("{/color}", "</color>")
                .Replace("{vote}", $"{Settings.Name}{(Arguments.Any() ? " " + string.Join(" ", Arguments) : "")}")
                .Replace("{text}", Plugin.Instance.Translate(translationKey, args));

            Plugin.Broadcast(message, Settings.Icon, Color.white);
        }
    }
}
