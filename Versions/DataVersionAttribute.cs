using System;

namespace BH.SDK.Versions
{
    // Marks a class as an aggregate root for the versioning system: a boundary that gets its own
    // envelope ({"version": ..., "value": ...}) and is migrated as one unit. Most model classes
    // never carry this - only the six SaveData kinds today, and optionally deeper internal
    // aggregates later, introduced lazily. See VERSION-UPDATE.md for the full rationale.
    //
    // Every historical frozen snapshot of an aggregate root also carries this attribute, with its
    // own (old) Major/Minor - VersionedTypeRegistry needs it to resolve version -> Type. Classes
    // that exist only as another snapshot's implementation detail (a nested leaf type frozen
    // alongside its container) do NOT need this attribute themselves.
    [AttributeUsage(AttributeTargets.Class)]
    public class DataVersionAttribute : Attribute
    {
        public string Domain { get; }
        public int Major { get; }
        public int Minor { get; }

        public DataVersionAttribute(string domain, int major, int minor)
        {
            Domain = domain;
            Major = major;
            Minor = minor;
        }

        public Version Version => new(Major, Minor);
    }
}
