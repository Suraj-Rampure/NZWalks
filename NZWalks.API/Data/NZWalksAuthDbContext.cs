using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ReaderRoleId = "cf3b78ac-d650-4be2-b1f6-8f2b04ecf8ad";
            var WriteRoleId = "d16f268b-28f2-4a68-a636-33eeba26cdac";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = ReaderRoleId,
                    ConcurrencyStamp =  ReaderRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id = WriteRoleId,
                    Name = "Writer",
                    ConcurrencyStamp = WriteRoleId,
                    NormalizedName = "Writer".ToUpper(),
                    }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
