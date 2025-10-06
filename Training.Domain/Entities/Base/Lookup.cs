using System.Diagnostics.CodeAnalysis;

namespace Training.Domain.Entities.Base
{
    [ExcludeFromCodeCoverage]
    public class Lookup<TKey> : BaseEntity<TKey>
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
