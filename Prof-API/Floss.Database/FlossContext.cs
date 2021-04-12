using System;
using Floss.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Floss.Database.Context
{
    public partial class FlossContext : DbContext, IDesignTimeDbContextFactory<FlossContext>
    {
        public FlossContext()
        {
        }

        public FlossContext(DbContextOptions<FlossContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Assignment> Assignment { get; set; }
        public virtual DbSet<AssignmentSubmission> AssignmentSubmission { get; set; }
        public virtual DbSet<AssignmentExempt> AssignmentExempt { get; set; }
        public virtual DbSet<AuthorizationRoleType> AuthorizationRoleType { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<Supervision> Supervision { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
				//optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Floss;Trusted_Connection=True;");
				var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
				Microsoft.Extensions.Configuration.IConfigurationRoot configuration = builder.Build();

				optionsBuilder.UseSqlServer(configuration.GetConnectionString("floss"));
            }
        }

		public FlossContext CreateDbContext(string[] args)
		{
			var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
			.SetBasePath(System.IO.Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
			Microsoft.Extensions.Configuration.IConfigurationRoot configuration = builder.Build();

			var optionsBuilder = new DbContextOptionsBuilder<FlossContext>();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("floss"));
			Console.WriteLine(string.Format("* Using Connectionstring {0}", configuration.GetConnectionString("floss")));

			FlossContext ctx = new FlossContext(optionsBuilder.Options);
			return ctx;
		}

		public static FlossContext GetContext()
		{
			var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
			.SetBasePath(System.IO.Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
			Microsoft.Extensions.Configuration.IConfigurationRoot configuration = builder.Build();

			var optionsBuilder = new DbContextOptionsBuilder<FlossContext>();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("floss"));
			Console.WriteLine(string.Format("* Using Connectionstring {0}", configuration.GetConnectionString("floss")));
			FlossContext ctx = new FlossContext(optionsBuilder.Options);
			return ctx;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.AssId);

                entity.Property(e => e.AssignmentName).HasMaxLength(100);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignments_Courses");
            });

            modelBuilder.Entity<AssignmentSubmission>(entity =>
            {
                entity.HasKey(e => new { e.AssId, e.UserId });

                entity.Property(e => e.FileType).HasMaxLength(10);

                entity.Property(e => e.FilePath).HasMaxLength(200);

                entity.Property(e => e.StrippedFilePath).HasMaxLength(200);

                entity.Property(e => e.EvalFilePath).HasMaxLength(200);
            });

            // Foreign key relationships for AssignmentSubmission
            modelBuilder.Entity<User>()
                .HasMany(usr => usr.AssignmentSubmissions)
                .WithOne(ass => ass.User)
                .HasForeignKey(ass => ass.UserId);
            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.AssignmentSubmissions)
                .WithOne(ass => ass.Assignment)
                .HasForeignKey(ass => ass.AssId);


            modelBuilder.Entity<AssignmentExempt>(entity =>
            {
                entity.HasKey(e => e.AssId);

                entity.Property(e => e.FileType).HasMaxLength(10);

                entity.Property(e => e.FilePath).HasMaxLength(200);

                entity.Property(e => e.StrippedFilePath).HasMaxLength(200);
            });

            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.AssignmentExempts)
                .WithOne(ae => ae.Assignment)
                .HasForeignKey(ae => ae.AssId);

            modelBuilder.Entity<AuthorizationRoleType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(50);

                //entity.HasOne(d => d.IdNavigation)
                //    .WithOne(p => p.AuthorizationRoleType)
                //    .HasForeignKey<AuthorizationRoleType>(d => d.Id)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_AuthorizationRoleTypes_User");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Duration).HasMaxLength(10);

                entity.Property(e => e.DepartmentName).HasMaxLength(4);

                entity.Property(e => e.CourseCode).HasMaxLength(4);

                entity.Property(e => e.Duration).HasMaxLength(2);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.Property(e => e.Until).HasColumnType("date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Courses");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_User");
            });

            modelBuilder.Entity<Supervision>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Supervision)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supervisions_Courses");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Supervision)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supervisions_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.Domain).HasMaxLength(10);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                //entity.HasOne(d => d.UserRole)
                //    .WithMany(p => p.User)
                //    .HasForeignKey(d => d.UserRoleId)
                //    .HasConstraintName("FK_User_UserRole");
            });

			modelBuilder.Entity<UserRole>().HasKey(er => new { er.Id });
			modelBuilder.Entity<UserRole>().HasOne(er => er.AuthorizationRoleType).WithMany(art => art.EmployeeRoles).HasForeignKey(ert => ert.AuthorizationRoleTypeId);
			modelBuilder.Entity<User>().HasMany(e => e.UserRoles).WithOne(er => er.User).HasForeignKey(er => er.UserId);

			Seed(modelBuilder);
        }


		protected void Seed(ModelBuilder modelBuilder)
		{
			SeedUsers(modelBuilder);
			SeedUserRoles(modelBuilder);
			SeedAuthorizationRoleTypes(modelBuilder);
            SeedCourses(modelBuilder);
            SeedEnrollments(modelBuilder);
            SeedAssignments(modelBuilder);
            SeedAssignmentSubmissions(modelBuilder);
		}

		private async void SeedUsers(ModelBuilder modelbuilder)
		{
			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 1,
				AccountName = "jp14fg",
				DisplayName = "Jordan Prada",
				Domain = "brocku.ca",
				Email = "jp14fg@brocku.ca",
				FullName = "Jordan Prada"
			});
			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 2,
				AccountName = "dd14wm",
				DisplayName = "Dana Darmohray",
				Domain = "brocku.ca",
				Email = "dd14wm@brocku.ca",
				FullName = "Dana Darmohray"
			});

			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 3,
				AccountName = "tn16jv",
				DisplayName = "ThaiBinh Nguyen",
				Domain = "brocku.ca",
				Email = "tn16jv@brocku.ca",
				FullName = "ThaiBinh Nguyen"
			});
			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 4,
				AccountName = "dp14hx",
				DisplayName = "Dylan Pavao",
				Domain = "brocku.ca",
				Email = "dp14hx@brocku.ca",
				FullName = "Dylan Pavao"
			});
			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 5,
				AccountName = "tk14rs",
				DisplayName = "Tyler Kisac",
				Domain = "brocku.ca",
				Email = "tk14rs@brocku.ca",
				FullName = "Tyler Kisac"
			});
			modelbuilder.Entity<User>().HasData(new User()
			{
				Id = 6,
				AccountName = "ma15om",
				DisplayName = "Michael Ahle",
				Domain = "brocku.ca",
				Email = "ma15om@brocku.ca",
				FullName = "Michael Ahle"
			});
		}


		private async void SeedUserRoles(ModelBuilder modelbuilder)
		{
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 1,
				UserId = 1,
				AuthorizationRoleTypeId = 1,
			});
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 2,
				UserId = 2,
				AuthorizationRoleTypeId = 1,
			});
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 3,
				UserId = 3,
				AuthorizationRoleTypeId = 1,
			});
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 4,
				UserId = 4,
				AuthorizationRoleTypeId = 1,
			});
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 5,
				UserId = 5,
				AuthorizationRoleTypeId = 1,
			});
			modelbuilder.Entity<UserRole>().HasData(new UserRole()
			{
				Id = 6,
				UserId = 6,
				AuthorizationRoleTypeId = 1,
			});
		}

        private async void SeedCourses(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Course>().HasData(new Course()
            {
                Id = 1,
                ProfId = 1,
                DepartmentName = "COSC",
                CourseCode = "4F00",
                Year = 2018,
                Duration = "D3"
            });
            modelbuilder.Entity<Course>().HasData(new Course()
            {
                Id = 2,
                ProfId = 1,
                DepartmentName = "COSC",
                CourseCode = "4P80",
                Year = 2018,
                Duration = "D4"
            });
            modelbuilder.Entity<Course>().HasData(new Course()
            {
                Id = 3,
                ProfId = 1,
                DepartmentName = "MATH",
                CourseCode = "1p06",
                Year = 2018,
                Duration = "D2"
            });
        }

        private async void SeedEnrollments(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 1,
                CourseId = 1,
                Until = new DateTime(2020, 1, 1)
            });
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 1,
                CourseId = 2,
                Until = new DateTime(2020, 1, 1)
            });
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 1,
                CourseId = 3,
                Until = new DateTime(2020, 1, 1)
            });
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 4,
                CourseId = 1,
                Until = new DateTime(2020, 1, 1)
            });
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 4,
                CourseId = 2,
                Until = new DateTime(2020, 1, 1)
            });
            modelbuilder.Entity<Enrollment>().HasData(new Enrollment()
            {
                UserId = 4,
                CourseId = 3,
                Until = new DateTime(2020, 1, 1)
            });
        }

        private async void SeedAssignments(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Assignment>().HasData(new Assignment()
            {
                AssId = 1,
                CourseId = 1,
                AssignmentName = "First Assignment",
                DueDate = new DateTime(2019, 4, 1),
                LateDueDate = new DateTime(2019, 4, 4)
            });
            modelbuilder.Entity<Assignment>().HasData(new Assignment()
            {
                AssId = 2,
                CourseId = 1,
                AssignmentName = "Second Assignment",
                DueDate = new DateTime(2019, 4, 1),
                LateDueDate = new DateTime(2019, 4, 4)
            });
        }

        private async void SeedAssignmentSubmissions(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<AssignmentSubmission>().HasData(new AssignmentSubmission()
            {
                AssId = 1,
                UserId = 1,
                FilePath = "MyMentalState1/reee.cpp",
                StrippedFilePath = "MyMentalState1/reee.txt",
                FileType = "cpp",
                SubmittedDate = new DateTime(2019, 4, 1)
            });
            modelbuilder.Entity<AssignmentSubmission>().HasData(new AssignmentSubmission()
            {
                AssId = 1,
                UserId = 4,
                FilePath = "MyMentalState/reee.java",
                StrippedFilePath = "MyMentalState2/reee.txt",
                FileType = "java",
                SubmittedDate = new DateTime(2019, 4, 1)
            });
            modelbuilder.Entity<AssignmentSubmission>().HasData(new AssignmentSubmission()
            {
                AssId = 2,
                UserId = 1,
                FilePath = "MyMentalState/reee.c",
                StrippedFilePath = "MyMentalState3/reee.txt",
                FileType = "c",
                SubmittedDate = new DateTime(2019, 4, 1)
            });
        }

        private async void SeedAuthorizationRoleTypes(ModelBuilder modelbuilder)
		{
			modelbuilder.Entity<AuthorizationRoleType>().HasData(new AuthorizationRoleType()
			{
				Id = 1,
				RoleName = "Professor"
			});
			modelbuilder.Entity<AuthorizationRoleType>().HasData(new AuthorizationRoleType()
			{
				Id = 2,
				RoleName = "System Admin"
			});
			modelbuilder.Entity<AuthorizationRoleType>().HasData(new AuthorizationRoleType()
			{
				Id = 3,
				RoleName = "Student"
			});
			modelbuilder.Entity<AuthorizationRoleType>().HasData(new AuthorizationRoleType()
			{
				Id = 4,
				RoleName = "Group Coordinator"
			});
		}

	}
}
