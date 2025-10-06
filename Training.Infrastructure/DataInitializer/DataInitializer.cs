using Training.Common.Extensions;
using System.Collections.Generic;
using Action = Training.Domain.Entities.Lookups.Action;
using Status = Training.Domain.Entities.Lookups.Status;

namespace Training.Infrastructure.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        public IEnumerable<Action> SeedActionsAsync()
        {
            var dataText = System.IO.File.ReadAllText(@"Seed/Actions.json");
            var actions = Seeder<List<Action>>.SeedIt(dataText);
            return actions;
        }
        public IEnumerable<Status> SeedStatusesAsync()
        {
            var dataText = System.IO.File.ReadAllText(@"Seed/Statuses.json");
            var statuses = Seeder<List<Status>>.SeedIt(dataText);
            return statuses;
        }
    }
}