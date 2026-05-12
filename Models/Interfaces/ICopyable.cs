using System;

namespace BHSDK.Models.Interfaces
{
    public interface ICopyable<out T> : ICloneable
    {
        public T Copy();
    }
}