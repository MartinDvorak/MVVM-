using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities;
using TeamsManager.DAL.Entities.Contributions;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.DAL.Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = CreateDbContext())
            {
                ClearDatabase(dbContext);
                SeedData(dbContext);
            }
        }

        private static void SeedData(TeamsManagerDbContext dbContext)
        {
            var defaultAvatarMan = Image.FromFile(@"Images\defaultAvatar.png");
            var defaultAvatarWoman = Image.FromFile(@"Images\womanAvatar.png");
            var defaultPhoto = Image.FromFile(@"Images\defaultPhoto.png");

            ProfileImage imageToUserJames = new ProfileImage
            {
                //Id = 1,
                Content = ConvertImageToByteArray(defaultPhoto),
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            ProfileImage imageToUserJane = new ProfileImage
            {
                //Id = 2,
                Content = ConvertImageToByteArray(defaultAvatarWoman),
                FileName = "Profile Jane Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            ProfileImage imageToUserLucas = new ProfileImage
            {
                //Id = 3,
                Content = ConvertImageToByteArray(defaultAvatarMan),
                FileName = "Profile Lucas Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            ProfileImage imageToUserLisa = new ProfileImage
            {
                //Id = 4,
                Content = ConvertImageToByteArray(defaultAvatarWoman),
                FileName = "Profile Lisa Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            ProfileImage imageToUserFleur = new ProfileImage
            {
                //Id = 5,
                Content = ConvertImageToByteArray(defaultAvatarWoman),
                FileName = "Profile Fleur Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            dbContext.ProfileImages.Add(imageToUserJames);
            dbContext.ProfileImages.Add(imageToUserJane);
            dbContext.ProfileImages.Add(imageToUserLucas);
            dbContext.ProfileImages.Add(imageToUserLisa);
            dbContext.ProfileImages.Add(imageToUserFleur);

            // User

            User james = new User
            {
                //Id = 1,
                AdministratedTeams = new List<Team>(),
                ContributionUserTags = new List<ContributionUserTag>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<Contribution>(),
                Password = hashPassword("james"),
                Photo = imageToUserJames,
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMember>()
            };

            User jane = new User
            {
                //Id = 2,
                AdministratedTeams = new List<Team>(),
                ContributionUserTags = new List<ContributionUserTag>(),
                Email = "jane@gmail.com",
                FirstName = "Jane",
                LastName = "Jones",
                MyContributions = new List<Contribution>(),
                Password = hashPassword("jane"),
                Photo = imageToUserJane,
                UserDescription = "Jan work account",
                UserTeams = new List<UserTeamMember>()
            };

            User lucas = new User
            {
                //Id = 3,
                AdministratedTeams = new List<Team>(),
                ContributionUserTags = new List<ContributionUserTag>(),
                Email = "lucas@gmail.com",
                FirstName = "Lucas",
                LastName = "Collins",
                MyContributions = new List<Contribution>(),
                Password = hashPassword("lucas"),
                Photo = imageToUserLucas,
                UserDescription = "Lucas school account",
                UserTeams = new List<UserTeamMember>()
            };
            User lisa = new User
            {
                //Id = 4,
                AdministratedTeams = new List<Team>(),
                ContributionUserTags = new List<ContributionUserTag>(),
                Email = "lisa@gmail.com",
                FirstName = "Lisa",
                LastName = "Evans",
                MyContributions = new List<Contribution>(),
                Password = hashPassword("lisa"),
                Photo = imageToUserLisa,
                UserDescription = "Lisa teach account",
                UserTeams = new List<UserTeamMember>()
            };
            User fleur = new User
            {
                //Id = 5,
                AdministratedTeams = new List<Team>(),
                ContributionUserTags = new List<ContributionUserTag>(),
                Email = "fleur@gmail.com",
                FirstName = "Fleur",
                LastName = "Smith",
                MyContributions = new List<Contribution>(),
                Password = hashPassword("fleur"),
                Photo = imageToUserFleur,
                UserDescription = "Fleur school account",
                UserTeams = new List<UserTeamMember>()
            };

            dbContext.Users.Add(james);
            dbContext.Users.Add(jane);
            dbContext.Users.Add(lucas);
            dbContext.Users.Add(lisa);
            dbContext.Users.Add(fleur);

            Team teamIFJ = new Team
            {
                //Id = 1,
                Admin = james,
                Description = "Team for formal language and compilators (IFJ) project.",
                Name = "IFJ team",
                Posts = new List<Post>(),
                TeamMembers = new List<UserTeamMember>()
            };

            Team teamIAL = new Team
            {
                //Id = 2,
                Admin = jane,
                Description = "Team for algorithm (IAL) projects.",
                Name = "IAL team",
                Posts = new List<Post>(),
                TeamMembers = new List<UserTeamMember>()
            };

            Team teamWork = new Team
            {
                //Id = 3,
                Admin = lucas,
                Description = "Team for work project.",
                Name = "Work team",
                Posts = new List<Post>(),
                TeamMembers = new List<UserTeamMember>()
            };

            dbContext.Teams.Add(teamIFJ);
            dbContext.Teams.Add(teamIAL);
            dbContext.Teams.Add(teamWork);

            UserTeamMember jamesToIFJ = new UserTeamMember
            {
                Team = teamIFJ,
                TeamId = teamIFJ.Id,
                User = james,
                UserId = james.Id
            };

            UserTeamMember janeToIFJ = new UserTeamMember
            {
                Team = teamIFJ,
                TeamId = teamIFJ.Id,
                User = jane,
                UserId = jane.Id
            };

            UserTeamMember fleurToIFJ = new UserTeamMember
            {
                Team = teamIFJ,
                TeamId = teamIFJ.Id,
                User = fleur,
                UserId = fleur.Id
            };

            UserTeamMember lisaToIFJ = new UserTeamMember
            {
                Team = teamIFJ,
                TeamId = teamIFJ.Id,
                User = lisa,
                UserId = lisa.Id
            };

            UserTeamMember lucasToIFJ = new UserTeamMember
            {
                Team = teamIFJ,
                TeamId = teamIFJ.Id,
                User = lucas,
                UserId = lucas.Id
            };

            UserTeamMember janeToIAL = new UserTeamMember
            {
                Team = teamIAL,
                TeamId = teamIAL.Id,
                User = jane,
                UserId = jane.Id
            };

            UserTeamMember lisaToIAL = new UserTeamMember
            {
                Team = teamIAL,
                TeamId = teamIAL.Id,
                User = lisa,
                UserId = lisa.Id
            };

            UserTeamMember lucasToWork = new UserTeamMember
            {
                Team = teamWork,
                TeamId = teamWork.Id,
                User = lucas,
                UserId = lucas.Id
            };

            UserTeamMember lisaToWork = new UserTeamMember
            {
                Team = teamWork,
                TeamId = teamWork.Id,
                User = lisa,
                UserId = lisa.Id
            };

            UserTeamMember jamesToWork = new UserTeamMember
            {
                Team = teamWork,
                TeamId = teamWork.Id,
                User = james,
                UserId = james.Id
            };

            dbContext.UserTeamMembers.Add(jamesToIFJ);
            dbContext.UserTeamMembers.Add(janeToIFJ);
            dbContext.UserTeamMembers.Add(fleurToIFJ);
            dbContext.UserTeamMembers.Add(lisaToIFJ);
            dbContext.UserTeamMembers.Add(lucasToIFJ);
            dbContext.UserTeamMembers.Add(janeToIAL);
            dbContext.UserTeamMembers.Add(lisaToIAL);
            dbContext.UserTeamMembers.Add(lucasToWork);
            dbContext.UserTeamMembers.Add(lisaToWork);
            dbContext.UserTeamMembers.Add(jamesToWork);

            Post janePostInIFJ = new Post
            {
                //Id = 1,
                AssociatedFiles = new List<ContributionFile>(),
                Author = jane,
                Comments = new List<Comment>(),
                Content = "I am looking forward to work together!",
                ContributionUserTags = new List<ContributionUserTag>(),
                CorrespondingTeam = teamIFJ,
                Date = new DateTime(2019, 1, 1),
                Title = "Hello There!"
            };

            Post lucasPostInIFJ = new Post
            {
                //Id = 2,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lucas,
                Comments = new List<Comment>(),
                Content = "Download documentation on private web.",
                ContributionUserTags = new List<ContributionUserTag>(),
                CorrespondingTeam = teamIFJ,
                Date = new DateTime(2019, 2, 1),
                Title = "Project has been released!"
            };

            Post lisaPostInIAL = new Post
            {
                //Id = 3,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lisa,
                Comments = new List<Comment>(),
                Content = "How are you? I wish we had the recursive tree issue.",
                ContributionUserTags = new List<ContributionUserTag>(),
                CorrespondingTeam = teamIAL,
                Date = new DateTime(2019, 1, 2),
                Title = "Hello Jane!"
            };

            Post lucasPostInWork = new Post
            {
                //Id = 4,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lucas,
                Comments = new List<Comment>(),
                Content = "Am i last who is waiting for salary?",
                ContributionUserTags = new List<ContributionUserTag>(),
                CorrespondingTeam = teamWork,
                Date = new DateTime(2019, 2, 2),
                Title = "Salary!"
            };

            Post jamesPostInWork = new Post
            {
                //Id = 5,
                AssociatedFiles = new List<ContributionFile>(),
                Author = james,
                Comments = new List<Comment>(),
                Content = "I am really sorry. I am ill, i cant go to work.",
                ContributionUserTags = new List<ContributionUserTag>(),
                CorrespondingTeam = teamWork,
                Date = new DateTime(2019, 1, 3),
                Title = "My absence"
            };

            dbContext.Posts.Add(janePostInIFJ);
            dbContext.Posts.Add(lucasPostInIFJ);
            dbContext.Posts.Add(lisaPostInIAL);
            dbContext.Posts.Add(lucasPostInWork);
            dbContext.Posts.Add(jamesPostInWork);


            Comment fluerCommentJanePost = new Comment
            {
                //Id = 6,
                AssociatedFiles = new List<ContributionFile>(),
                Author = fleur,
                Content = "Hi! :)",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 1, 2),
                ParentContribution = janePostInIFJ
            };

            Comment jamesCommentJanePost = new Comment
            {
                //Id = 7,
                AssociatedFiles = new List<ContributionFile>(),
                Author = james,
                Content = "Hello!",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 1, 2),
                ParentContribution = janePostInIFJ
            };
            Comment lisaCommentJanePost = new Comment
            {
                //Id = 8,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lisa,
                Content = "Greetings!",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 1, 2),
                ParentContribution = janePostInIFJ
            };
            Comment lucasCommentJanePost = new Comment
            {
                //Id = 9,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lucas,
                Content = "Hi!",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 1, 2),
                ParentContribution = janePostInIFJ
            };
            Comment lisaCommentLucasPost1 = new Comment
            {
                //Id = 10,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lisa,
                Content = "OK",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 2, 2),
                ParentContribution = lucasPostInIFJ
            };
            Comment lisaCommentLisaPost = new Comment
            {
                //Id = 11,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lisa,
                Content = "<3",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 1, 2),
                ParentContribution = lisaPostInIAL
            };
            Comment janeCommentLisaPost = new Comment
            {
                //Id = 12,
                AssociatedFiles = new List<ContributionFile>(),
                Author = jane,
                Content = "Me too!",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 3, 2),
                ParentContribution = lisaPostInIAL
            };

            Comment lucasCommentJamesPost = new Comment
            {
                //Id = 13,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lucas,
                Content = ":(",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 3, 3),
                ParentContribution = jamesPostInWork
            };
            Comment lisaCommentLucasPost2 = new Comment
            {
                //Id = 14,
                AssociatedFiles = new List<ContributionFile>(),
                Author = lisa,
                Content = "I already have salary.",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 3, 3),
                ParentContribution = lucasPostInWork
            };
            Comment jamesCommentLucasPost = new Comment
            {
                //Id = 15,
                AssociatedFiles = new List<ContributionFile>(),
                Author = james,
                Content = "I will check it.",
                ContributionUserTags = new List<ContributionUserTag>(),
                Date = new DateTime(2019, 3, 4),
                ParentContribution = lucasPostInWork
            };

            dbContext.Comments.Add(fluerCommentJanePost);
            dbContext.Comments.Add(jamesCommentJanePost);
            dbContext.Comments.Add(lisaCommentJanePost);
            dbContext.Comments.Add(lucasCommentJanePost);
            dbContext.Comments.Add(lisaCommentLucasPost1);
            dbContext.Comments.Add(lisaCommentLisaPost);
            dbContext.Comments.Add(janeCommentLisaPost);
            dbContext.Comments.Add(lucasCommentJamesPost);
            dbContext.Comments.Add(lisaCommentLucasPost2);
            dbContext.Comments.Add(jamesCommentLucasPost);

            ContributionFile lucasTaskFile = new ContributionFile
            {
                //Id = 1,
                AssociatedContribution = lucasPostInIFJ,
                Content = new byte[300],
                FileFormat = SupportedFormatFile.Rar,
                FileName = "IFJ Basic language"
            };

            ContributionFile janeHelloFile = new ContributionFile
            {
                //Id = 2,
                AssociatedContribution = janePostInIFJ,
                Content = new byte[300],
                FileFormat = SupportedFormatFile.Zip,
                FileName = "IFJ Best practices"
            };

            ContributionFile jamesHelloFile = new ContributionFile
            {
                //Id = 3,
                AssociatedContribution = janePostInIFJ,
                Content = new byte[300],
                FileFormat = SupportedFormatFile.Rar,
                FileName = "IFJ mistakes from last years"
            };

            dbContext.ContributionFiles.Add(lucasTaskFile);
            dbContext.ContributionFiles.Add(janeHelloFile);
            dbContext.ContributionFiles.Add(jamesHelloFile);

            ContributionUserTag janeTagedJames = new ContributionUserTag
            {
                UserId = james.Id,
                User = james,
                Contribution = janePostInIFJ,
                ContributionId = janePostInIFJ.Id
            };

            ContributionUserTag lucasTagedFleur = new ContributionUserTag
            {
                UserId = fleur.Id,
                User = fleur,
                Contribution = lucasPostInIFJ,
                ContributionId = lucasPostInIFJ.Id
            };
            ContributionUserTag jamesTagedJane = new ContributionUserTag
            {
                UserId = jane.Id,
                User = jane,
                Contribution = jamesCommentJanePost,
                ContributionId = jamesCommentJanePost.Id
            };
            ContributionUserTag jamesTagedLucas = new ContributionUserTag
            {
                UserId = lucas.Id,
                User = lucas,
                Contribution = jamesCommentLucasPost,
                ContributionId = jamesCommentLucasPost.Id
            };
            ContributionUserTag janeTagedLisa = new ContributionUserTag
            {
                UserId = lisa.Id,
                User = lisa,
                Contribution = janeCommentLisaPost,
                ContributionId = janeCommentLisaPost.Id
            };
            dbContext.ContributionUserTags.Add(janeTagedJames);
            dbContext.ContributionUserTags.Add(lucasTagedFleur);
            dbContext.ContributionUserTags.Add(jamesTagedJane);
            dbContext.ContributionUserTags.Add(jamesTagedLucas);
            dbContext.ContributionUserTags.Add(janeTagedLisa);

            dbContext.SaveChanges();
        }

        private static void ClearDatabase(TeamsManagerDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.ProfileImages);
            dbContext.RemoveRange(dbContext.Users);
            dbContext.RemoveRange(dbContext.Teams);
            dbContext.RemoveRange(dbContext.UserTeamMembers);
            dbContext.RemoveRange(dbContext.Posts);
            dbContext.RemoveRange(dbContext.Comments);
            dbContext.RemoveRange(dbContext.ContributionFiles);
            dbContext.RemoveRange(dbContext.ContributionUserTags);
            dbContext.SaveChanges();
        }

        private static TeamsManagerDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TeamsManagerDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog =  TeamsManager;MultipleActiveResultSets = True;Integrated Security = True; ");
            return new TeamsManagerDbContext(optionsBuilder.Options);
        }

        private static string hashPassword(string password)
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
       private static byte[] ConvertImageToByteArray(Image image)
        {
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}
