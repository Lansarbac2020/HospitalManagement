using HospitalManagement.Models;
using HospitalManagement.Models.HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        
        public DbSet<Category> Categories { get; set; }

        //  DbSets for the models
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Emergency> Emergencies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Apple", Displayorder = "1" },
                new Category { Id = 2, Name = "TV", Displayorder = "2" },
                new Category { Id = 3, Name = "Apple", Displayorder = "3" }
                );
            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Pediatric Emergency" },
                new Department { DepartmentId = 2, DepartmentName = "Pediatric Intensive Care" }
            // Add more departments as needed
            );

            // Seed Assistants
            modelBuilder.Entity<Assistant>().HasData(
                new Assistant
                {
                    AssistantId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Phone = "555-1234",
                    Address = "123 Elm St, Springfield",
                    DepartmentId = 1, // Matches existing department
                    ShiftStartTime = new DateTime(2023, 10, 21, 8, 0, 0), // 8 AM
                    ShiftEndTime = new DateTime(2023, 10, 21, 16, 0, 0) // 4 PM
                },
                new Assistant
                {
                    AssistantId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Phone = "555-5678",
                    Address = "456 Oak St, Springfield",
                    DepartmentId = 1, // Matches existing department
                    ShiftStartTime = new DateTime(2023, 10, 21, 9, 0, 0), // 9 AM
                    ShiftEndTime = new DateTime(2023, 10, 21, 17, 0, 0) // 5 PM
                },
                new Assistant
                {
                    AssistantId = 3,
                    FirstName = "Emily",
                    LastName = "Johnson",
                    Email = "emily.johnson@example.com",
                    Phone = "555-8765",
                    Address = "789 Pine St, Springfield",
                    DepartmentId = 2, // Matches existing department
                    ShiftStartTime = new DateTime(2023, 10, 21, 10, 0, 0), // 10 AM
                    ShiftEndTime = new DateTime(2023, 10, 21, 18, 0, 0) // 6 PM
                }
            );

            // Define relationships with NoAction or Restrict for cascading deletes
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Assistant)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.AssistantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Update for Appointments
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Assistant)
                .WithMany(assistant => assistant.Appointments)
                .HasForeignKey(a => a.AssistantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.FacultyMember)
                .WithMany(faculty => faculty.Appointments)
                .HasForeignKey(a => a.FacultyMemberId) // Use FacultyMemberId here
                .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths

            base.OnModelCreating(modelBuilder);
        }




    }
}
