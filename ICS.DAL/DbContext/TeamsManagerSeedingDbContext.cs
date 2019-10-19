using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeamsManager.DAL.Entities;
using TeamsManager.DAL.Entities.Contributions;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.DAL.DbContext
{
    public class TeamsManagerSeedingDbContext : TeamsManagerDbContext
    {

        public static ProfileImage ImageToUserJames = new ProfileImage
        {
            Id = 1,
            Content = new byte[10],
            FileName = "Profile James Picture",
            PictureFormat = SupportedFormatPicture.Jpg
        };
        public static ProfileImage ImageToUserLucas = new ProfileImage
        {
            Id = 2,
            Content = new byte[10],
            FileName = "Profile Lucas Picture",
            PictureFormat = SupportedFormatPicture.Png
        };
        public static ProfileImage ImageToUserLisa = new ProfileImage
        {
            Id = 3,
            Content = new byte[10],
            FileName = "Profile Lisa Picture",
            PictureFormat = SupportedFormatPicture.Png
        };

        public static User James = new User
        {
            Id = 1,
            Email = "james@gmail.com",
            FirstName = "James",
            LastName = "Smith",
            Password = HashPassword("james"),
            Photo = ImageToUserJames,
            UserDescription = "James school account",
        };
        public static User Lucas = new User
        {
            Id = 2,
            Email = "lucas@gmail.com",
            FirstName = "Lucas",
            LastName = "Collins",
            Password = HashPassword("lucas"),
            Photo = ImageToUserLucas,
            UserDescription = "Lucas school account",
        };
        public static User Lisa = new User
        {
            Id = 3,
            Email = "lisa@gmail.com",
            FirstName = "Lisa",
            LastName = "Evans",
            Password = HashPassword("lisa"),
            Photo = ImageToUserLisa,
            UserDescription = "Lisa teach account",
        };

        public static Team TeamWork = new Team
        {
            Id = 1,
            Admin = Lucas,
            Description = "Team for work project.",
            Name = "Work team",
        };

        public static UserTeamMember LucasToWork = new UserTeamMember
        {
            Team = TeamWork,
            TeamId = TeamWork.Id,
            User = Lucas,
            UserId = Lucas.Id
        };
        public static UserTeamMember LisaToWork = new UserTeamMember
        {
            Team = TeamWork,
            TeamId = TeamWork.Id,
            User = Lisa,
            UserId = Lisa.Id
        };
        public static UserTeamMember JamesToWork = new UserTeamMember
        {
            Team = TeamWork,
            TeamId = TeamWork.Id,
            User = James,
            UserId = James.Id
        };

        public static Post LucasPostInWork = new Post
        {
            Id = 1,
            Author = Lucas,
            Content = "Am i last who is waiting for salary?",
            CorrespondingTeam = TeamWork,
            Date = new DateTime(2019, 2, 2),
            Title = "Salary!"
        };
        public static Post JamesPostInWork = new Post
        {
            Id = 2,
            Author = James,
            Content = "I am really sorry. I am ill, i cant go to work.",
            CorrespondingTeam = TeamWork,
            Date = new DateTime(2019, 1, 3),
            Title = "My absence"
        };

        public static Comment LucasCommentJamesPost = new Comment
        {
            Id = 3,
            Author = Lucas,
            Content = ":(",
            Date = new DateTime(2019, 3, 3),
            ParentContribution = JamesPostInWork
        };
        public static Comment LisaCommentLucasPost = new Comment
        {
            Id = 4,
            Author = Lisa,
            Content = "I already have salary.",
            ContributionUserTags = new List<ContributionUserTag>(),
            Date = new DateTime(2019, 3, 3),
            ParentContribution = LucasPostInWork
        };
        public static Comment JamesCommentLucasPost = new Comment
        {
            Id = 5,
            Author = James,
            Content = "I will check it.",
            Date = new DateTime(2019, 3, 4),
            ParentContribution = LucasPostInWork
        };

        public static ContributionUserTag JamesTaggedLucas = new ContributionUserTag
        {
            UserId = Lucas.Id,
            User = Lucas,
            Contribution = JamesCommentLucasPost,
            ContributionId = JamesCommentLucasPost.Id
        };


        static TeamsManagerSeedingDbContext()
        {
            James.Photo = ImageToUserJames;
            James.AdministratedTeams = new List<Team>();
            James.ContributionUserTags = new List<ContributionUserTag>();
            James.MyContributions = new List<Contribution>() {JamesPostInWork, JamesCommentLucasPost};
            James.UserTeams = new List<UserTeamMember>() {JamesToWork};

            Lucas.Photo = ImageToUserLucas;
            Lucas.AdministratedTeams = new List<Team>() {TeamWork};
            Lucas.ContributionUserTags = new List<ContributionUserTag>() {JamesTaggedLucas};
            Lucas.MyContributions = new List<Contribution>() { LucasPostInWork, LucasCommentJamesPost };
            Lucas.UserTeams = new List<UserTeamMember>() { LucasToWork };

            Lisa.Photo = ImageToUserLisa;
            Lisa.AdministratedTeams = new List<Team>() { TeamWork };
            Lisa.ContributionUserTags = new List<ContributionUserTag>();
            Lisa.MyContributions = new List<Contribution>() { LisaCommentLucasPost };
            Lisa.UserTeams = new List<UserTeamMember>() { LisaToWork };

            TeamWork.Admin = Lucas;
            TeamWork.Posts = new List<Post>() {LucasPostInWork, JamesPostInWork};
            TeamWork.TeamMembers = new List<UserTeamMember>() { LucasToWork, JamesToWork, LisaToWork };

            LucasToWork.Team = TeamWork;
            LucasToWork.User = Lucas;

            LisaToWork.Team = TeamWork;
            LisaToWork.User = Lisa;

            JamesToWork.Team = TeamWork;
            JamesToWork.User = James;

            LucasPostInWork.Author = Lucas;
            LucasPostInWork.CorrespondingTeam = TeamWork;
            LucasPostInWork.AssociatedFiles = new List<ContributionFile>();
            LucasPostInWork.Comments = new List<Comment>() { LisaCommentLucasPost, JamesCommentLucasPost};
            LucasPostInWork.ContributionUserTags = new List<ContributionUserTag>();

            JamesPostInWork.Author = James;
            JamesPostInWork.CorrespondingTeam = TeamWork;
            JamesPostInWork.AssociatedFiles = new List<ContributionFile>();
            JamesPostInWork.Comments = new List<Comment>() { LucasCommentJamesPost };
            JamesPostInWork.ContributionUserTags = new List<ContributionUserTag>();

            LucasCommentJamesPost.Author = Lucas;
            LucasCommentJamesPost.AssociatedFiles = new List<ContributionFile>();
            LucasCommentJamesPost.ContributionUserTags = new List<ContributionUserTag>();
            LucasCommentJamesPost.ParentContribution = JamesPostInWork;

            JamesCommentLucasPost.Author = James;
            JamesCommentLucasPost.AssociatedFiles = new List<ContributionFile>();
            JamesCommentLucasPost.ContributionUserTags = new List<ContributionUserTag>() {JamesTaggedLucas};
            JamesCommentLucasPost.ParentContribution = LucasPostInWork;

            LisaCommentLucasPost.Author = Lucas;
            LisaCommentLucasPost.AssociatedFiles = new List<ContributionFile>();
            LisaCommentLucasPost.ContributionUserTags = new List<ContributionUserTag>();
            LisaCommentLucasPost.ParentContribution = LucasPostInWork;

            JamesTaggedLucas.User = Lucas;
            JamesTaggedLucas.Contribution = JamesCommentLucasPost;

        }
        public TeamsManagerSeedingDbContext(DbContextOptions<TeamsManagerDbContext> optionsBuilderOptions) : base(optionsBuilderOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfileImage>().HasData(
                new
                {
                    ImageToUserJames.Id,
                    ImageToUserJames.Content,
                    ImageToUserJames.FileName,
                    ImageToUserJames.PictureFormat

                },
                new
                {
                    ImageToUserLucas.Id,
                    ImageToUserLucas.Content,
                    ImageToUserLucas.FileName,
                    ImageToUserLucas.PictureFormat
                },
                new
                {
                    ImageToUserLisa.Id,
                    ImageToUserLisa.Content,
                    ImageToUserLisa.FileName,
                    ImageToUserLisa.PictureFormat
                });

            modelBuilder.Entity<User>().HasData(
                new
                {
                    James.Id,
                    James.Email,
                    James.FirstName,
                    James.LastName,
                    James.Password,
                    PhotoId = ImageToUserJames.Id,
                    James.UserDescription
                },
                new
                {
                    Lucas.Id,
                    Lucas.Email,
                    Lucas.FirstName,
                    Lucas.LastName,
                    Lucas.Password,
                    PhotoId = ImageToUserLucas.Id,
                    Lucas.UserDescription
                },
                new
                {
                    Lisa.Id,
                    Lisa.Email,
                    Lisa.FirstName,
                    Lisa.LastName,
                    Lisa.Password,
                    PhotoId = ImageToUserLisa.Id,
                    Lisa.UserDescription
                }
            );

            modelBuilder.Entity<Team>().HasData(
                new
                {
                    TeamWork.Id,
                    AdminId = Lucas.Id,
                    TeamWork.Description,
                    TeamWork.Name
                }
            );

            modelBuilder.Entity<UserTeamMember>().HasData(
                new
                {
                    TeamId = TeamWork.Id,
                    UserId = Lucas.Id
                },
                new
                {
                    TeamId = TeamWork.Id,
                    UserId = Lisa.Id
                },
                new
                {
                    TeamId = TeamWork.Id,
                    UserId = James.Id
                }
            );

            modelBuilder.Entity<Post>().HasData(
                new
                {
                    LucasPostInWork.Id,
                    AuthorId = Lucas.Id,
                    LucasPostInWork.Content,
                    CorrespondingTeamId = TeamWork.Id,
                    LucasPostInWork.Date,
                    LucasPostInWork.Title
                },
                new
                {
                    JamesPostInWork.Id,
                    AuthorId = James.Id,
                    JamesPostInWork.Content,
                    CorrespondingTeamId = TeamWork.Id,
                    JamesPostInWork.Date,
                    JamesPostInWork.Title
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new
                {
                    LucasCommentJamesPost.Id,
                    LucasCommentJamesPost.Content,
                    LucasCommentJamesPost.Date,
                    AuthorId = Lucas.Id,
                    ParentContributionId = JamesPostInWork.Id
                },
                new
                {
                    JamesCommentLucasPost.Id,
                    JamesCommentLucasPost.Content,
                    JamesCommentLucasPost.Date,
                    AuthorId = James.Id,
                    ParentContributionId = LucasPostInWork.Id
                },
                new
                {
                    LisaCommentLucasPost.Id,
                    LisaCommentLucasPost.Content,
                    LisaCommentLucasPost.Date,
                    AuthorId = Lisa.Id,
                    ParentContributionId = LucasPostInWork.Id
                }
            );

            modelBuilder.Entity<ContributionUserTag>().HasData(
                new
                {
                    UserId = Lucas.Id,
                    ContributionId = JamesCommentLucasPost.Id
                }    
            );
        }

        private static string HashPassword(string password)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var builder = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
