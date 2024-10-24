﻿// <auto-generated />
using System;
using HospitalManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HospitalManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<int>("AvailableBeds")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientCount")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentId = 1,
                            AvailableBeds = 0,
                            DepartmentName = "Pediatric Emergency",
                            PatientCount = 0
                        },
                        new
                        {
                            DepartmentId = 2,
                            AvailableBeds = 0,
                            DepartmentName = "Pediatric Intensive Care",
                            PatientCount = 0
                        });
                });

            modelBuilder.Entity("HospitalManagement.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("HospitalManagement.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AssistantId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FacultyMemberId")
                        .HasColumnType("int");

                    b.HasKey("AppointmentId");

                    b.HasIndex("AssistantId");

                    b.HasIndex("FacultyMemberId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HospitalManagement.Models.Assistant", b =>
                {
                    b.Property<int>("AssistantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssistantId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShiftEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShiftStartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("AssistantId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Assistants");

                    b.HasData(
                        new
                        {
                            AssistantId = 1,
                            Address = "123 Elm St, Springfield",
                            DepartmentId = 1,
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Phone = "555-1234",
                            ShiftEndTime = new DateTime(2023, 10, 21, 16, 0, 0, 0, DateTimeKind.Unspecified),
                            ShiftStartTime = new DateTime(2023, 10, 21, 8, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            AssistantId = 2,
                            Address = "456 Oak St, Springfield",
                            DepartmentId = 1,
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            Phone = "555-5678",
                            ShiftEndTime = new DateTime(2023, 10, 21, 17, 0, 0, 0, DateTimeKind.Unspecified),
                            ShiftStartTime = new DateTime(2023, 10, 21, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            AssistantId = 3,
                            Address = "789 Pine St, Springfield",
                            DepartmentId = 2,
                            Email = "emily.johnson@example.com",
                            FirstName = "Emily",
                            LastName = "Johnson",
                            Phone = "555-8765",
                            ShiftEndTime = new DateTime(2023, 10, 21, 18, 0, 0, 0, DateTimeKind.Unspecified),
                            ShiftStartTime = new DateTime(2023, 10, 21, 10, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("HospitalManagement.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Displayorder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Displayorder = "1",
                            Name = "Apple"
                        },
                        new
                        {
                            Id = 2,
                            Displayorder = "2",
                            Name = "TV"
                        },
                        new
                        {
                            Id = 3,
                            Displayorder = "3",
                            Name = "Apple"
                        });
                });

            modelBuilder.Entity("HospitalManagement.Models.Emergency", b =>
                {
                    b.Property<int>("EmergencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmergencyId"));

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmergencyId");

                    b.ToTable("Emergencies");
                });

            modelBuilder.Entity("HospitalManagement.Models.HospitalManagement.Models.FacultyMember", b =>
                {
                    b.Property<int>("FacultyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacultyId"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FacultyId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("FacultyMembers");
                });

            modelBuilder.Entity("HospitalManagement.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HospitalManagement.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"));

                    b.Property<int>("AssistantId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShiftEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShiftStartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ScheduleId");

                    b.HasIndex("AssistantId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("HospitalManagement.Models.Appointment", b =>
                {
                    b.HasOne("HospitalManagement.Models.Assistant", "Assistant")
                        .WithMany("Appointments")
                        .HasForeignKey("AssistantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalManagement.Models.HospitalManagement.Models.FacultyMember", "FacultyMember")
                        .WithMany("Appointments")
                        .HasForeignKey("FacultyMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Assistant");

                    b.Navigation("FacultyMember");
                });

            modelBuilder.Entity("HospitalManagement.Models.Assistant", b =>
                {
                    b.HasOne("Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("HospitalManagement.Models.HospitalManagement.Models.FacultyMember", b =>
                {
                    b.HasOne("Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("HospitalManagement.Models.Schedule", b =>
                {
                    b.HasOne("HospitalManagement.Models.Assistant", "Assistant")
                        .WithMany("Schedules")
                        .HasForeignKey("AssistantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Department", "Department")
                        .WithMany("Schedules")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Assistant");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Department", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("HospitalManagement.Models.Assistant", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("HospitalManagement.Models.HospitalManagement.Models.FacultyMember", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
