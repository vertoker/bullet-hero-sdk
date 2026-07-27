using System;

namespace BH.SDK.Versions
{
    // Untyped handle so VersionedTypeRegistry can hold and chain migrators for any (TFrom, TTo)
    // pair without knowing every pair at compile time.
    public interface IMigration
    {
        Type FromType { get; }
        Type ToType { get; }
        object MigrateUntyped(object from);
    }

    public interface IMigration<in TFrom, out TTo> : IMigration
    {
        TTo Migrate(TFrom from);
    }
}
