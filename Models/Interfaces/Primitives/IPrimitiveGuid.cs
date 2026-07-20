using System;

namespace BH.SDK.Models.Interfaces.Primitives
{
    public interface IPrimitiveGuid : IResetable
    {
        public Guid Value { get; }
    }
}
