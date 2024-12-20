﻿using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HospitalManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<Category> Categories { get; set; }

        //  DbSets for the models
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BookedAppointment> BookedAppointments { get; set; }
        public DbSet<Emergency> Emergencies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Role> Roles { get; set; }
        // DbSet pour les patients
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; } // Nouvelle table pour les docteurs
        public DbSet<Shift> Shifts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed Categories, Departments, and Assistants;
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Apple", Displayorder = "1" },
                new Category { Id = 2, Name = "TV", Displayorder = "2" },
                new Category { Id = 3, Name = "Apple", Displayorder = "3" }
            );

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Pediatric Emergency" },
                new Department { DepartmentId = 2, DepartmentName = "Pediatric Intensive Care" }
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
                    DepartmentId = 1
                },
                new Assistant
                {
                    AssistantId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Phone = "555-5678",
                    Address = "456 Oak St, Springfield",
                    DepartmentId = 1
                },
                new Assistant
                {
                    AssistantId = 3,
                    FirstName = "Emily",
                    LastName = "Johnson",
                    Email = "emily.johnson@example.com",
                    Phone = "555-8765",
                    Address = "789 Pine St, Springfield",
                    DepartmentId = 2
                }
            );

            // Configure Appointment relationship with FacultyMember
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(faculty => faculty.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete for Appointment -> FacultyMember

            // Configure Assistant relationship with Department
            modelBuilder.Entity<Assistant>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Assistants)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete for Assistant -> Department

            modelBuilder.Entity<Department>()
         .HasOne(d => d.FacultyMember) // A department has one head
         .WithOne(fm => fm.DepartmentHead) // A faculty member has one department head
         .HasForeignKey<Department>(d => d.FacultyMemberId) // Foreign key in the Department
         .OnDelete(DeleteBehavior.SetNull); // Prevent cascading delete


            // modelBuilder.Entity<Doctor>()
            //.HasOne(d => d.Department)
            //.WithMany()
            //.HasForeignKey(d => d.DepartmentId)
            //.OnDelete(DeleteBehavior.Restrict);

            // Configure Doctor relationship with Department (optional if Doctor is the head of Department)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department) // A Doctor belongs to a Department
                .WithMany(d => d.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Appointment>()
                 .HasOne(a => a.Doctor)
                 .WithMany(d => d.Appointments)
                 .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);




            base.OnModelCreating(modelBuilder); // Ensure this is called at the end to apply configurations
        }





    }
}