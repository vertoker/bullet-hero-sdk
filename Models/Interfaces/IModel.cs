using System;

namespace BH.SDK.Models.Interfaces
{
    // TODO add IUpdatable
    // TODO add IMoveable
    
    public interface IModel<T> : ICopyable<T>, IEquatable<T>, IResetable
    {
        
    }
}