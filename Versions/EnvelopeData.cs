using System;
using System.Runtime.CompilerServices;

namespace BH.SDK.Versions
{
    public readonly struct EnvelopeData
    {
        public readonly Version Version;
        public readonly object RawPayload;

        public EnvelopeData(Version version, object rawPayload)
        {
            Version = version;
            RawPayload = rawPayload;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetPayload<TValue>() => (TValue)RawPayload;
    }
}