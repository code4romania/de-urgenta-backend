using DeUrgenta.Common;
using DeUrgenta.Common.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Domain.Identity
{
    public class UserDbContext : IdentityDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("identity");

            modelBuilder
                .Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    Name = ApiUserRoles.Admin,
                    NormalizedName = ApiUserRoles.Admin.ToUpper()
                });

            modelBuilder
                .Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Id = "9ac3f437-57a2-407a-b0bc-fc2d1268f4f7",
                    Name = ApiUserRoles.User,
                    NormalizedName = ApiUserRoles.User.ToUpper()
                });
        }
    }
}