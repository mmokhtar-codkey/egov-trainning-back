using Training.Common.Services;
using Training.Infrastructure.Configuration;
using Training.Infrastructure.DataInitializer;
using Microsoft.EntityFrameworkCore;
using Action = Training.Domain.Entities.Lookups.Action;
using Status = Training.Domain.Entities.Lookups.Status;

namespace Training.Infrastructure.Context
{
    public partial class TrainingDbContext : DbContext
    {
        private readonly IDataInitializer _dataInitializer;
        private readonly IClaimService _claimService;
        public TrainingDbContext(DbContextOptions<TrainingDbContext> options, IDataInitializer dataInitializer, IClaimService claimService) : base(options)
        {
            _dataInitializer = dataInitializer;
            _claimService = claimService;
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>("TestSequence").StartsAt(1).IncrementsBy(1);

            modelBuilder.ApplyConfiguration(new CategoryConfig());

            modelBuilder.Entity<Action>().HasData(_dataInitializer.SeedActionsAsync());
            modelBuilder.Entity<Status>().HasData(_dataInitializer.SeedStatusesAsync());
            base.OnModelCreating(modelBuilder);
        }


    }
}
