using System;

namespace BH.SDK.Models.Interfaces
{
    public interface IModel<T> : ICopyable<T>, IEquatable<T>, IResetable
    {
        
    }
}