using Famtela.Application.Interfaces.Services;
using Famtela.Domain.Application.Models.Chat;
using Famtela.Domain.Entities.Identity;
using Famtela.Domain.Contracts;
using Famtela.Domain.Entities.Catalog;
using Famtela.Domain.Entities.ExtendedAttributes;
using Famtela.Domain.Entities.Misc;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;
using Famtela.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Famtela.Infrastructure.Contexts
{
    public class FamtelaContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public FamtelaContext(DbContextOptions<FamtelaContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<ChatHistory<FamtelaUser>> ChatHistories { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<FarmProfile> FarmProfiles { get; set; }
        public DbSet<TypeofFarming> TypesofFarming { get; set; }
        public DbSet<Age> Ages { get; set; }
        public DbSet<Chick> Chicks { get; set; }
        public DbSet<ChickenExpense> ChickenExpenses { get; set; }
        public DbSet<Consumption> Consumptions { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Egg> Eggs { get; set; }
        public DbSet<Grower> Growers { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<TypeofFeed> TypesofFeed { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Cow> Cows { get; set; }
        public DbSet<DairyExpense> DairyExpenses { get; set; }
        public DbSet<Milk> Milks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WeightEstimate> WeightEstimates { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            /*builder.Entity<FarmProfile>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.FarmProfiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Chick>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Chicks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ChickenExpense>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.ChickenExpenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Egg>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Eggs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Grower>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Growers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Layer>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Layers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Cow>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Cows)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<DairyExpense>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.DairyExpenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Milk>()
                .HasOne(d => d.FamtelaUser)
                .WithMany(p => p.Milks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);*/

            builder.Entity<ChatHistory<FamtelaUser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<FamtelaUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<FamtelaRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<FamtelaRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }
}