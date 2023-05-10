using Microsoft.EntityFrameworkCore;
using WebApiTestTaskFromVK.Models;

namespace WebApiTestTaskFromVK
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }
		public DbSet<UserState> UserStates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<UserGroup>().HasData(
				new UserGroup { Id = 1, Code = "Admin", Description = "The admin has all the rights" },
				new UserGroup { Id = 2, Code = "User", Description = "The user has a limited number of rights." }
				);
			modelBuilder.Entity<UserState>().HasData(
				new UserState { Id = 1, Code = "Active", Description = "The user is active." },
				new UserState { Id = 2, Code = "Blocked", Description = "The user deleted his account." }
				);
			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Login = "superuser", Password = "superuser", CreatedDate = DateTime.UtcNow, UserGroupId = 1, UserStateId = 1 }
				);
		}


	}
}
