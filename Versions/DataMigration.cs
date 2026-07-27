using System;

namespace BH.SDK.Versions
{
    // Convenience base for a single migration step between two adjacent versions of one domain's
    // aggregate root (e.g. GameLevelV1 -> GameLevel). Implementers only need to write Migrate.
    public abstract class DataMigration<TFrom, TTo> : IMigration<TFrom, TTo>
    {
        public Type FromType => typeof(TFrom);
        public Type ToType => typeof(TTo);

        public abstract TTo Migrate(TFrom from);

        object IMigration.MigrateUntyped(object from) => Migrate((TFrom)from);
    }
}
