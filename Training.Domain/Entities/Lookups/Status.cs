using Training.Domain.Entities.Base;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Training.Domain.Entities.Lookups
{
    [ExcludeFromCodeCoverage]
    public class Status : Lookup<Guid>
    {
        public string EntityName { get; set; }

        public string CssClass { get; set; }
    }
}
