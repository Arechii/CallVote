using Rocket.Unturned.Player;
using UnityEngine;
using System.Collections.Generic;

namespace Arechi.CallVote
{
    public interface IVote
    {
        VoteSettings Settings { get; set; }

        List<string> Arguments { get; set; }

        List<ulong> Voters { get; set; }

        VoteStatus Status { get; set; }

        // Cyphierion Edits
        Coroutine thisCoroutine1 { get; set; }

        Coroutine thisCoroutine2 { get; set; }

        VoteResult GetResult();

        void Start(List<string> arguments);

        void AddVote(UnturnedPlayer player);

        void Stop();

        void Cooldown();
    }
}
