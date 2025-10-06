using System.Collections.Generic;
using Action = Training.Domain.Entities.Lookups.Action;
using Status = Training.Domain.Entities.Lookups.Status;

namespace Training.Infrastructure.DataInitializer
{
    public interface IDataInitializer
    {
        IEnumerable<Action> SeedActionsAsync();

        IEnumerable<Status> SeedStatusesAsync();
    }
}