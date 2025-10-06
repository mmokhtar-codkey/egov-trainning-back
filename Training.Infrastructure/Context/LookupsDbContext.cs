using Training.Domain.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace Training.Infrastructure.Context
{
    public partial class TrainingDbContext
    {
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }

    }
}
