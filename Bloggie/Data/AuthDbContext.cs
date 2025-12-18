using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Ignore Spelling: Bloggie

namespace Bloggie.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }

        // Overriding OnModelCreating method, in order to seed initial data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var superAdminRoleId = "b67a3bfa-eb80-42c2-b096-43925edaec5d";
            var adminRoleId = "9da61983-fbb8-48f2-8dd8-70ab98123f23";
            var userRoleId = "28e8d689-8ea8-4db0-afd0-4c84f5220f21";

            // Creating a list of Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = superAdminRoleId,
                    Name ="SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId
                }

            };

            // Inserting roles created using builder, for seeding and insertion into the database when using migration.
            builder.Entity<IdentityRole>().HasData(roles);


            // Seed/create a SuperAdminUser
            // 1. Create an Identity superAdminUser
            var superAdminId = "e6a5af26-87be-427b-8a74-0cb43a2497b1";

            var superAdminUser = new IdentityUser
            {
                Id = superAdminId,
                UserName = "superadmin@bloggie.com",
                NormalizedUserName = "SUPERADMIN@BLOGGIE.COM",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "SUPERADMIN@BLOGGIE.COM",
                ConcurrencyStamp = superAdminId,
            };

            // 2. Hashing superAdmin password
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
               .HashPassword(user: superAdminUser, password: "SuperAdmin@123");

            // 3. Insert and seed superAdminUser into the database
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add all roles to SuperAdminUser, using  IdentityUserRole.
            // IdentityUserRole requires a key of type string.

            // By adding all roles to SuperAdminUser, they can log as User, Admin, and SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
             new IdentityUserRole<string>
             {
               RoleId = superAdminRoleId ,
               UserId = superAdminId
             },
             new IdentityUserRole<string>
             {
                 RoleId = adminRoleId,
                 UserId = superAdminId
             },
             new IdentityUserRole<string>
             {
                 RoleId = userRoleId,
                 UserId = superAdminId,
             }
            };

            // Seed and add superAdmin User data into the database
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
