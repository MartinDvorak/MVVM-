﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamsManager.DAL.DbContext;

namespace TeamsManager.DAL.Migrations
{
    [DbContext(typeof(TeamsManagerDbContext))]
    [Migration("20190404205122_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Contribution", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Contribution");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Contribution");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.ContributionUserTag", b =>
                {
                    b.Property<int?>("UserId");

                    b.Property<int?>("ContributionId");

                    b.HasKey("UserId", "ContributionId");

                    b.HasIndex("ContributionId");

                    b.ToTable("ContributionUserTags");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Files.ContributionFile", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssociatedContributionId");

                    b.Property<byte[]>("Content");

                    b.Property<int>("FileFormat");

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedContributionId");

                    b.ToTable("ContributionFiles");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Files.ProfileImage", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Content");

                    b.Property<string>("FileName");

                    b.Property<int>("PictureFormat");

                    b.HasKey("Id");

                    b.ToTable("ProfileImages");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Team", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("UserDescription");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.UserTeamMember", b =>
                {
                    b.Property<int?>("UserId");

                    b.Property<int?>("TeamId");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeamMembers");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Comment", b =>
                {
                    b.HasBaseType("TeamsManager.DAL.Entities.Contributions.Contribution");

                    b.Property<int?>("ParentContributionId");

                    b.Property<int?>("PostId");

                    b.HasIndex("ParentContributionId");

                    b.HasIndex("PostId");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Post", b =>
                {
                    b.HasBaseType("TeamsManager.DAL.Entities.Contributions.Contribution");

                    b.Property<int?>("CorrespondingTeamId");

                    b.Property<string>("Title");

                    b.HasIndex("CorrespondingTeamId");

                    b.HasDiscriminator().HasValue("Post");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Contribution", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.User", "Author")
                        .WithMany("MyContributions")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.ContributionUserTag", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Contributions.Contribution", "Contribution")
                        .WithMany("ContributionUserTags")
                        .HasForeignKey("ContributionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamsManager.DAL.Entities.User", "User")
                        .WithMany("ContributionUserTags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Files.ContributionFile", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Contributions.Contribution", "AssociatedContribution")
                        .WithMany("AssociatedFiles")
                        .HasForeignKey("AssociatedContributionId");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Team", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.User", "Admin")
                        .WithMany("AdministratedTeams")
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.User", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Files.ProfileImage", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.UserTeamMember", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Team", "Team")
                        .WithMany("TeamMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamsManager.DAL.Entities.User", "User")
                        .WithMany("UserTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Comment", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Contributions.Contribution", "ParentContribution")
                        .WithMany()
                        .HasForeignKey("ParentContributionId");

                    b.HasOne("TeamsManager.DAL.Entities.Contributions.Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("TeamsManager.DAL.Entities.Contributions.Post", b =>
                {
                    b.HasOne("TeamsManager.DAL.Entities.Team", "CorrespondingTeam")
                        .WithMany("Posts")
                        .HasForeignKey("CorrespondingTeamId");
                });
#pragma warning restore 612, 618
        }
    }
}