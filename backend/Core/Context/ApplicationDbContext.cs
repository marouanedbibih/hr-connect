using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employee> Employees { get; set; } // Renamed to Employees for consistency
        public DbSet<Departement> Departements { get; set; } // Renamed to Departements for consistency

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }

        // Relation Tables

        public DbSet<CompanyDepartment> CompanyDepartments { get; set; }
        public DbSet<CompanyDepartementJob> CompanyDepartementJobs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .ToTable("Admins");

            modelBuilder.Entity<Employee>()
                .ToTable("Employees");

            modelBuilder.Entity<CompanyDepartment>()
                .HasKey(cd => new { cd.CompanyId, cd.DepartmentId });

            modelBuilder.Entity<CompanyDepartment>()
                .HasOne(cd => cd.Company)
                .WithMany(c => c.CompanyDepartments)
                .HasForeignKey(cd => cd.CompanyId);

            modelBuilder.Entity<CompanyDepartment>()
                .HasOne(cd => cd.Departement)
                .WithMany(d => d.CompanyDepartments)
                .HasForeignKey(cd => cd.DepartmentId);

            modelBuilder.Entity<CompanyDepartementJob>()
                .HasKey(cdj => new { cdj.DepartementId, cdj.CompanyId ,cdj.JobId});

            modelBuilder.Entity<CompanyDepartementJob>()
                .HasOne(cdj => cdj.Job)
                .WithMany(c => c.CompanyDepartementJobs)
                .HasForeignKey(cdj => cdj.CompanyId);

            modelBuilder.Entity<CompanyDepartementJob>()
                .HasOne(cdj => cdj.Departement)
                .WithMany(d => d.CompanyDepartementJobs)
                .HasForeignKey(cdj => cdj.DepartementId);

            modelBuilder.Entity<CompanyDepartementJob>()
                .HasOne(cdj => cdj.Job)
                .WithMany(j => j.CompanyDepartementJobs)
                .HasForeignKey(cdj => cdj.JobId);

            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.Job)
                .WithMany(job => job.Employees)
                .HasForeignKey(employee => employee.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.Departement)
                .WithMany(departement => departement.Employees)
                .HasForeignKey(employee => employee.DepartementId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.Company)
                .WithMany(company => company.Employees)
                .HasForeignKey(employee => employee.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne(admin => admin.User)
                .WithOne()
                .HasForeignKey<Admin>(admin => admin.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.User)
                .WithOne()
                .HasForeignKey<Employee>(employee => employee.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .Property(company => company.Size)
                .HasConversion<string>();

            modelBuilder.Entity<Employee>()
                .Property(employee => employee.Level)
                .HasConversion<string>();
        }
    }
}
