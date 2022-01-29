﻿// <auto-generated />
using System;
using DeUrgenta.RecurringJobs.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DeUrgenta.RecurringJobs.Domain.Migrations
{
    using DeUrgenta.Domain.RecurringJobs;

    [DbContext(typeof(JobsContext))]
    [Migration("20211004124646_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("jobs")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DeUrgenta.RecurringJobs.Domain.Entities.CertificationDetails", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CertificationId")
                        .HasColumnType("uuid");

                    b.HasKey("NotificationId")
                        .HasName("PK_Certification");

                    b.ToTable("CertificationDetails");
                });

            modelBuilder.Entity("DeUrgenta.RecurringJobs.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id")
                        .HasName("PK_Notification");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("DeUrgenta.RecurringJobs.Domain.Entities.CertificationDetails", b =>
                {
                    b.HasOne("DeUrgenta.RecurringJobs.Domain.Entities.Notification", "Notification")
                        .WithOne("CertificationDetails")
                        .HasForeignKey("DeUrgenta.RecurringJobs.Domain.Entities.CertificationDetails", "NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("DeUrgenta.RecurringJobs.Domain.Entities.Notification", b =>
                {
                    b.Navigation("CertificationDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
