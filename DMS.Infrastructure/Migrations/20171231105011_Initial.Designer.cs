﻿// <auto-generated />
using DMS.Domain.Entities;
using DMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DMS.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20171231105011_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DMS.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DMS.Domain.Entities.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<string>("Body");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("DMS.Domain.Entities.StatusChange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChangeAuthorId");

                    b.Property<DateTime>("Created");

                    b.Property<int?>("DocumentId");

                    b.Property<string>("Message");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ChangeAuthorId");

                    b.HasIndex("DocumentId");

                    b.ToTable("StatusChanges");
                });

            modelBuilder.Entity("DMS.Domain.Entities.Document", b =>
                {
                    b.HasOne("DMS.Domain.Entities.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("DMS.Domain.Entities.StatusChange", b =>
                {
                    b.HasOne("DMS.Domain.Entities.ApplicationUser", "ChangeAuthor")
                        .WithMany()
                        .HasForeignKey("ChangeAuthorId");

                    b.HasOne("DMS.Domain.Entities.Document", "Document")
                        .WithMany("StatusChanges")
                        .HasForeignKey("DocumentId");
                });
#pragma warning restore 612, 618
        }
    }
}
