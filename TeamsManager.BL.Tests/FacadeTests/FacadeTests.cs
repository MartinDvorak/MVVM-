using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.DAL.Entities.Files;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.FacadeTests
{
    public class FacadeTests : IClassFixture<TeamsManagerFacadeTestsFixture>, IDisposable
    {
        private readonly TeamsManagerFacadeTestsFixture facadeTestsFixture;

        private IBusinessLogicFacade FacadeSUT => facadeTestsFixture.Facade;

        public FacadeTests(TeamsManagerFacadeTestsFixture facadeTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.facadeTestsFixture = facadeTestsFixture;

            this.facadeTestsFixture.PrepareDatabase();
        }


        [Fact]
        public void Create_NonExistingUser_IsCreated()
        {
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 7,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = 7

            };

            FacadeSUT.Create(imageToUserJames);

            UserModel james = new UserModel
            {
                Id = 10,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = 7
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            james = FacadeSUT.Create(james) as UserModel;

            var receivedUser = FacadeSUT.GetDetail(james) as UserModel;

            Assert.NotNull(receivedUser);
            FacadeSUT.Delete(receivedUser);
        }


        [Fact]
        public void FindUserByEmail_ExistingUsersEmail_DoesNotThrow()
        {
            //Arrange
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 625,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = 625

            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            UserModel userModel = new UserModel
            {
                Id = 626,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = imageToUserJames.Id
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            userModel = FacadeSUT.Create(userModel) as UserModel;

            //Act
            var userModels = FacadeSUT.FindUserByEmail("james@gmail.com");

            //Assert
            Assert.NotNull(userModels);
        }

        [Fact]
        public void DeleteUser_AddedUser_UserDeleted()
        {
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 605,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = 605

            };

            FacadeSUT.Create(imageToUserJames);

            UserModel james = new UserModel
            {
                Id = 606,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = lightPic,
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            FacadeSUT.Create(james);

            //Arrange
            var lightUser = new UserLightModel
            {
                Id = 606
            };

            //Act
            FacadeSUT.Delete(lightUser);

            var returned = FacadeSUT.GetDetail(lightUser);

            //Assert
            Assert.Null(returned);
        }

        [Fact]
        public void UpdatePicture_ToExistingUser_PictureChanged()
        {
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 650,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            var lightPic = new ProfileImageLightModel
            {
                Id = imageToUserJames.Id

            };

            UserModel james = new UserModel
            {
                Id = 651,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = lightPic,
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            james = FacadeSUT.Create(james) as UserModel;

            var lightJames = new UserLightModel
            {
                Id = james.Id
            };

            var user = FacadeSUT.GetDetail(lightJames) as UserModel;

            //Act
            var newPicture = new ProfileImageLightModel
            {
                Id = 650,
                Content = new byte[20],
                PictureFormat = SupportedFormatPicture.Jpg
            };
            user.ProfilePicture = newPicture;
            FacadeSUT.Update(user.ProfilePicture);

            //Assert
            var returnedUser = FacadeSUT.GetDetail(user) as UserModel;
            Assert.Equal(newPicture.Content.ToString(), returnedUser.ProfilePicture.Content.ToString());
        }

        [Fact]
        public void FindMyTeam_UserInTeams_IsInTeams()
        {
            //Arrange

            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 660,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            var lightPic = new ProfileImageLightModel
            {
                Id = imageToUserJames.Id

            };

            UserModel userModel = new UserModel
            {
                Id = 661,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = imageToUserJames.Id
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            userModel = FacadeSUT.Create(userModel) as UserModel;

            var userLight = new UserLightModel
            {
                Id = userModel.Id
            };

            var teamModel = new TeamModel
            {
                Id = 662,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            teamModel = FacadeSUT.Create(teamModel) as TeamModel;

            var teamLight = new TeamLightModel
            {
                Id = teamModel.Id
            };

            //Act
            FacadeSUT.JoinUserToTeam(userLight, teamLight);

            //Act
            var teams = FacadeSUT.FindMyTeams(userLight).ToList();


            //Assert
            var teamsCount = teams.Count;
            Assert.Equal(1, teamsCount);
        }

        [Fact]
        public void GetAllTeamMembers_UsersAddedToTeam_UsersAreInTeam()
        {
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 901,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = imageToUserJames.Id

            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            UserModel userModel = new UserModel
            {
                Id = 900,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = imageToUserJames.Id
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            userModel = FacadeSUT.Create(userModel) as UserModel;

            var userLight = new UserLightModel
            {
                Id = userModel.Id
            };

            var teamModel = new TeamModel
            {
                Id = 902,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            teamModel = FacadeSUT.Create(teamModel) as TeamModel;

            var teamLight = new TeamLightModel
            {
                Id = teamModel.Id
            };

            //Act
            FacadeSUT.JoinUserToTeam(userLight, teamLight);
            //Act
            var members = FacadeSUT.GetAllMembers(teamLight).ToList();

            //Assert
            Assert.Contains(members, member => member.Id == userLight.Id);
            FacadeSUT.DeleteUserFromTeam(userLight, teamLight);
        }

        [Fact]
        public void JoinUserToTeam_UserAndTeamExist_UserIsTeamMember()
        {
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 620,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = 620

            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            UserModel userModel = new UserModel
            {
                Id = 621,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = imageToUserJames.Id
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            userModel = FacadeSUT.Create(userModel) as UserModel;

            var userLight = new UserLightModel
            {
                Id = userModel.Id
            };

            var teamModel = new TeamModel
            {
                Id = 622,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            teamModel = FacadeSUT.Create(teamModel) as TeamModel;

            var teamLight = new TeamLightModel
            {
                Id = teamModel.Id
            };

            //Act
            FacadeSUT.JoinUserToTeam(userLight, teamLight);

            //Assert
            var teamsMembers = FacadeSUT.GetAllMembers(teamLight).ToList();
            Assert.Contains(teamsMembers, member => member.Id == userLight.Id);
            FacadeSUT.DeleteUserFromTeam(userLight, teamLight);
        }

        [Fact]
        public void findAllNonMemebrs_UserIsNotMember_UserIsInNonmembersList()
        {
            //Arrange
            ProfileImageModel imageToUserJames = new ProfileImageModel
            {
                Id = 670,
                Content = new byte[10],
                FileName = "Profile James Picture",
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var lightPic = new ProfileImageLightModel
            {
                Id = imageToUserJames.Id

            };

            imageToUserJames = FacadeSUT.Create(imageToUserJames) as ProfileImageModel;

            UserModel userModel = new UserModel
            {
                Id = 671,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "james@gmail.com",
                FirstName = "James",
                LastName = "Smith",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = new ProfileImageLightModel
                {
                    Id = imageToUserJames.Id
                },
                UserDescription = "James school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            userModel = FacadeSUT.Create(userModel) as UserModel;

            var userLight = new UserLightModel
            {
                Id = userModel.Id
            };

            var teamModel = new TeamModel
            {
                Id = 672,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            teamModel = FacadeSUT.Create(teamModel) as TeamModel;

            var teamLight = new TeamLightModel
            {
                Id = teamModel.Id
            };


            //Act
            var allNonUsers = FacadeSUT.GetAllNonMembers(teamLight);

            //Assert 
            Assert.Contains(allNonUsers, model => model.Id == userModel.Id);
        }

        [Fact]
        public void getAllPostsInIFJTeam_twoPosts_notThrow()
        {
            //Arrange
            var imageToUserLucas = new ProfileImageModel
            {
                Id = 810,
                Content = new byte[10],
                FileName = "Profile Lucas Picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            imageToUserLucas = FacadeSUT.Create(imageToUserLucas) as ProfileImageModel;

            var imageToUserLucasLight = new ProfileImageLightModel
            {
                Id = imageToUserLucas.Id
            };


            var lucas = new UserModel
            {
                Id = 811,
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                Email = "lucas@gmail.com",
                FirstName = "Lucas",
                LastName = "Collins",
                MyContributions = new List<ContributionLightModel>(),
                Password = "****",
                ProfilePicture = imageToUserLucasLight,
                UserDescription = "Lucas school account",
                UserTeams = new List<UserTeamMemberModel>()
            };
            lucas = FacadeSUT.Create(lucas) as UserModel;

            var lucasLight = new UserLightModel
            {
                Id = lucas.Id
            };

            var teamIFJ = new TeamModel
            {
                Id = 812,
                Admin = lucasLight,
                Description = "Team for formal language and compilators (IFJ) project.",
                Name = "IFJ team",
                Posts = new List<PostLightModel>(),
                Members = new List<UserTeamMemberModel>()
            };
            teamIFJ = FacadeSUT.Create(teamIFJ) as TeamModel;

            var teamLight = new TeamLightModel
            {
                Id = teamIFJ.Id
            };

            FacadeSUT.JoinUserToTeam(lucasLight, teamLight);

            var lucasPostInIFJ = new PostModel
            {
                Id = 813,
                AssociatedFiles = new List<ContributionFileLightModel>(),
                Author = lucasLight,
                Comments = new List<CommentModel>(),
                Content = "Download documentation on private web.",
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = teamLight,
                Date = new DateTime(2019, 2, 1),
                Title = "Project has been released!"
            };
            lucasPostInIFJ = FacadeSUT.Create(lucasPostInIFJ) as PostModel;

            teamIFJ = FacadeSUT.GetDetail(teamLight) as TeamModel;
            //Act
            var allPosts = FacadeSUT.GetAllPostsInTeam(teamIFJ).ToList();
            //Assert
            Assert.Contains(allPosts, model => model.Id == lucasPostInIFJ.Id);
        }

        [Fact]
        public void CheckEmailValidity_Invalid()
        {
            //Arrange
            const string invalidEmailAddress = "james";
            //Act
            var returnedValue = FacadeSUT.IsEmailValid(invalidEmailAddress);
            //Assert
            Assert.False(returnedValue);
        }

        [Fact]
        public void CompareHash_EqualStrings_ReturnsTrue()
        {
            //Arrange
            const string string1 = "abc";
            const string string2 = "abc";
            //Act
            var hash1 = FacadeSUT.HashPassword(string1);
            var hash2 = FacadeSUT.HashPassword(string2);
            //Assert
            Assert.Equal(hash1, hash2);

        }

        [Fact]
        public void CheckUserDataValidity_DataValid()
        {
            //Arrange
            const string firstName = "Tormund";
            const string lastName = "Giantsbane";
            const string email = "tormund@giantsbane.com";
            const string password1 = "abc";
            const string password2 = "abc";
            //Act
            var returnedValue = FacadeSUT.AreUserDataValid(firstName, lastName, email, password1, password2);
            //Assert
            Assert.True(returnedValue);
        }

        [Fact]
        public void CheckUserDataValidity_DataInvalid()
        {
            //Arrange
            const string firstName = "Tormund";
            const string lastName = "Giantsbane";
            const string email = "tormund@giantsbane.com";
            const string password1 = "abc";
            const string password2 = "abb";
            //Act
            var returnedValue = FacadeSUT.AreUserDataValid(firstName, lastName, email, password1, password2);
            //Assert
            Assert.False(returnedValue);
        }

        [Fact]
        public void ConvertFileExtensionToEnum_SupportedExtension_Returns()
        {
            //Arrange
            const string extension = "png";
            //Act
            var format = FacadeSUT.ConvertFileExtenstionToEnum(extension);
            //Assert
            Assert.Equal(SupportedFormatPicture.Png, format);
        }

        [Fact]
        public void ConvertFileExtensionToEnum_UnknownExtension_Returns()
        {
            //Arrange
            const string extension = "jpg";
            //Act
            var format = FacadeSUT.ConvertFileExtenstionToEnum(extension);
            //Assert
            Assert.Equal(SupportedFormatPicture.Jpg, format);
        }

        [Fact]
        public void ConvertTeamModelToTeamLightModel_WithExistingTeam()
        {
            //Arrange
            var teamModel = new TeamLightModel() {Id = 1};
            var existingTeam = FacadeSUT.GetDetail(teamModel) as TeamModel;
            //Act
            var existingTeamLightModel = FacadeSUT.ConvertTeamModelToTeamLightModel(existingTeam);
            //Assert
            Assert.Equal(existingTeamLightModel.Id, teamModel.Id);
        }

        [Fact]
        public void IsUserAdmin_ExistingTeam_ReturnsTrue()
        {
            //Arrange
            var teamModel = new TeamLightModel() {Id = 1};
            var user = FacadeSUT.FindUserByEmail("lucas@gmail.com");
            //Act
            var isAdmin = FacadeSUT.IsUserAdminInTeam(user, teamModel);
            //Assert
            Assert.True(isAdmin);
        }

        [Fact]
        public void DelegateAdminPosition_WithExistingTeamAndUser_AdminDelegated()
        {
            //Arrange
            var teamModel = new TeamLightModel() {Id = 1};
            var notAdmin = new UserLightModel() {Id = 3};
            //Act
            FacadeSUT.DelegateAdminPosition(notAdmin, teamModel);
            var updatedTeam = FacadeSUT.GetDetail(teamModel) as TeamModel;
            //Assert
            Debug.Assert(updatedTeam != null, nameof(updatedTeam) + " != null");
            Assert.Equal(notAdmin.Id, updatedTeam.Admin.Id);
        }

        [Fact]
        public void FindNewestNPostsForUser_NoExistingPosts_ReturnsEmpty()
        {
            //Arrange
            var user = new UserLightModel() {Id = 10};
            //Act
            var newestPosts = FacadeSUT.FindNewestNPostsForUser(user, 2);
            var numberOfFoundPosts = newestPosts.Count();
            //Assert
            Assert.Equal(0, numberOfFoundPosts);
        }
        [Fact]
        public void FindNewestNPostsInTeam_WithExistingTeam()
        {
            //Arrange
            var team = new TeamLightModel() { Id = 1 };
            var existingTeam = FacadeSUT.GetDetail(team) as TeamModel;
            //Act
            var newestPosts = FacadeSUT.FindNewestNPostsInTeam(existingTeam, 2);
            var numberOfFoundPosts = newestPosts.Count();
            //Assert
            Assert.Equal(2, numberOfFoundPosts);
        }

        [Fact]
        public void SearchInTeam_PatternExists_ReturnsContributions()
        {
            //Arrange
            var team = new TeamLightModel() {Id = 1};
            var existingTeam = FacadeSUT.GetDetail(team) as TeamModel;
            const string pattern = "salary";
            //Act
            var contributions = FacadeSUT.Search(pattern, existingTeam);
            var numberOfContributions = contributions.Count();
            //Assert
            Assert.Equal(2, numberOfContributions);
        }

        [Fact]
        public void SearchInTeam_PatternDoesNotExist_ReturnsContributions()
        {
            //Arrange
            var team = new TeamLightModel() { Id = 1 };
            var existingTeam = FacadeSUT.GetDetail(team) as TeamModel;
            const string pattern = "bogus";
            //Act
            var contributions = FacadeSUT.Search(pattern, existingTeam);
            var numberOfContributions = contributions.Count();
            //Assert
            Assert.Equal(0, numberOfContributions);
        }

        [Fact]
        public void SearchUser_NotExistingPattern()
        {
            //Arrange
            var user = new UserLightModel() {Id = 1};
            //Act
            var contributions = FacadeSUT.Search("salary", user);
            var numberOfContributions = contributions.Count();
            //Assert
            Assert.Equal(0, numberOfContributions);
        }

        [Fact]
        public void GetAllCommentsInPost_ExistingPost()
        {
            //Arrange
            var postModel = FacadeSUT.GetDetail(new PostLightModel() {Id = 1}) as PostModel;
            //Act
            var comments = FacadeSUT.GetAllCommentInPost(postModel);
            var numberOfComments = comments.Count();
            //Assert
            Assert.Equal(2, numberOfComments);
        }

        [Fact]
        public void MyRecentActivity_ReturnLastPostAdded()
        {
            //Arrange
            var newPostModel = new PostModel()
            {
                AssociatedFiles = new List<ContributionFileLightModel>(),
                Author = new UserLightModel() { Id = 3},
                Comments = new List<CommentModel>(),
                Content = "I am tired",
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel() { Id = 1},
                Date = new DateTime(),
                Id = 10,
                Title = "Desperate"
            };
            FacadeSUT.Create(newPostModel);
            //Act
            var recentActivity = FacadeSUT.GetRecentUserActivity(new UserLightModel() {Id = 3});
            var containsLast = false;
            foreach (var activity in recentActivity)
            {
                if (activity.Contribution.Content.Equals("I am tired"))
                {
                    containsLast = true;
                }
            }
            //Assert
            Assert.True(containsLast);
        }

        public void Dispose()
        {
            this.facadeTestsFixture.TearDownDatabase();
        }
    }
}

//var user = new UserModel()
//{
//    Id = 10,
//    AdministratedTeams = new List<TeamLightModel>(),
//    ContributionUserTags = new List<ContributionUserTagModel>(),
//    Email = "joe@doe.com",
//    FirstName = "Joe",
//    LastName = "Doe",
//    MyContributions = new List<ContributionLightModel>(),
//    Password = FacadeSUT.HashPassword("1234"),
//    ProfilePicture = new ProfileImageLightModel()
//        {
//            Content = new byte[20],
//            Id = 10,
//            PictureFormat = SupportedFormatPicture.Png
//        },
//    UserDescription = "aaaaaaaa"
//};
