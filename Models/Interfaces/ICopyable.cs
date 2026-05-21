using System;

namespace BH.SDK.Models.Interfaces
{
    public interface ICopyable<out T> : ICloneable
    {
        public T Copy();
    }
}