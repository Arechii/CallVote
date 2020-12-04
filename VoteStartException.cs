using System;

namespace Arechi.CallVote
{
    public class VoteStartException : Exception
    {
        public VoteStartException(string translationKey) : this(translationKey, Array.Empty<object>())
        {

        }

        public VoteStartException(string translationKey, params object[] args) : base(Plugin.Instance.Translate(translationKey, args))
        {

        }
    }
}
