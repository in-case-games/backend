using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit.Abstractions;

namespace InCase.IntegrationTests.Tests.ResourcesApi
{
    public class UserReviewTests: BaseApiTest, IClassFixture<WebApplicationFactory<HostResourcesApiTests>>
    {
        private readonly ResponseService _responseService;
        private readonly Dictionary<string, Guid> DependenciesGuids = new()
        {
            ["User"] = Guid.NewGuid(),
            ["Review"] = Guid.NewGuid()
        };
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<HostResourcesApiTests>()
            .Build();

        public UserReviewTests(WebApplicationFactory<HostResourcesApiTests> webApplicationFactory,
            ITestOutputHelper output) : base(output, _configuration)
        {
            _responseService = new(webApplicationFactory.CreateClient());
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_Reviews(HttpStatusCode statusCode)
        {
            // Arrange
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeUserDependency(DependenciesGuids["User"]);
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewsByAdmin(HttpStatusCode statusCode)
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/admin", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "user")]
        public async Task GET_ReviewsByAdmin_AuthErrors(HttpStatusCode statusCode, string roleName = "admin")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/admin");
            }
            else
            {
                getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/admin", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewsById(HttpStatusCode statusCode)
        {
            // Arrange
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeUserDependency(DependenciesGuids["User"]);
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/{DependenciesGuids["Review"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewsByUserId(HttpStatusCode statusCode)
        {
            // Arrange
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeUserDependency(DependenciesGuids["User"]);
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/user/{DependenciesGuids["User"]}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Fact]
        public async Task GET_ReviewsByUserId_NotFoundUser()
        {
            // Arrange

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/user/{DependenciesGuids["Review"]}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewsByUser(HttpStatusCode statusCode)
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/user", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task GET_ReviewsByUser_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode getStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/user");
            }
            else
            {
                getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/user", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewImages(HttpStatusCode statusCode)
        {
            // Arrange
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeUserDependency(DependenciesGuids["User"]);
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode("/api/user-review/images");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewImagesById(HttpStatusCode statusCode)
        {
            // Arrange
            if (statusCode == HttpStatusCode.OK)
            {
                await InitializeUserDependency(DependenciesGuids["User"]);
                await InitializeDependencies(DependenciesGuids["User"]);
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/{DependenciesGuids["Review"]}/images");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Fact]
        public async Task GET_ReviewImagesByNotExistedId_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies(DependenciesGuids["User"]);
            
            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/{Guid.NewGuid()}/images");

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, getStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task GET_ReviewImageById(HttpStatusCode statusCode)
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"]);
            await InitializeDependencies(DependenciesGuids["User"]);

            Guid guid = Guid.NewGuid();

            if (statusCode == HttpStatusCode.OK)
            {
                guid = DependenciesGuids["Review"];

                UpdateContext();

                List<ReviewImage> images = await Context.ReviewImages
                    .Where(w => w.ReviewId == guid).ToListAsync();

                guid = images[0].Id;
            }

            // Act
            HttpStatusCode getStatusCode = await _responseService
                .ResponseGetStatusCode($"/api/user-review/image/{guid}");

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, getStatusCode);
        }
        [Fact]
        public async Task POST_CreateReview_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review", review.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, postStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task POST_CreateReview_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode postStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review", review.Convert(false));
            }
            else
            {
                postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review", review.Convert(false), AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, postStatusCode);
        }
        [Fact]
        public async Task POST_CreateReviewImage_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            ReviewImage? image = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review/image", image.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, postStatusCode);
        }
        [Fact]
        public async Task POST_CreateReviewImage_NotFoundReview()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            ReviewImage? image = new()
            {
                ImageUri = $"{GenerateString()}.jpg"
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review/image", image.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, postStatusCode);
        }
        [Fact]
        public async Task POST_CreateReviewImage_Conflict()
        {
            // Arrange
            string token = await CreateFakeToken("user");
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            ReviewImage? image = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            // Act
            HttpStatusCode postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review/image", image.Convert(false), token);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.Conflict, postStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task POST_CreateImage_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            ReviewImage? image = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            // Act
            HttpStatusCode postStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review/image", image.Convert(false));
            }
            else
            {
                postStatusCode = await _responseService
                .ResponsePostStatusCode("/api/user-review/image", image.Convert(false), AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, postStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task PUT_UpdateReview_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                putStatusCode = await _responseService
                .ResponsePut("/api/user-review", review.Convert(false));
            }
            else
            {
                putStatusCode = await _responseService
                .ResponsePut("/api/user-review", review.Convert(false), AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateReview_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-review", review.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateReview_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            string fakeUserToken = await CreateFakeToken("user");
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-review", review.Convert(false), fakeUserToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, putStatusCode);
        }


        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "user")]
        public async Task PUT_UpdateReviewByAdmin_AuthErrors(HttpStatusCode statusCode, string roleName = "admin")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                putStatusCode = await _responseService
                .ResponsePut("/api/user-review/admin", review.Convert(false));
            }
            else
            {
                putStatusCode = await _responseService
                .ResponsePut("/api/user-review/admin", review.Convert(false), AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateReviewByAdmin_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-review/admin", review.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, putStatusCode);
        }
        [Fact]
        public async Task PUT_UpdateReviewByAdmin_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);
            UpdateContext();
            UserReview review = new()
            {
                Id = Guid.NewGuid(),
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = DependenciesGuids["User"]
            };

            // Act
            HttpStatusCode putStatusCode = await _responseService
                .ResponsePut("/api/user-review/admin", review.Convert(false), AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, putStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task DELETE_DeleteReview_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/{DependenciesGuids["Review"]}");
            }
            else
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/{DependenciesGuids["Review"]}", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReview_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/{DependenciesGuids["Review"]}", AccessToken);
            

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReview_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/{Guid.NewGuid()}", AccessToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReview_Conflict()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            string fakeUserToken = await CreateFakeToken("user");

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/{DependenciesGuids["Review"]}", fakeUserToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.Conflict, deleteStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "user")]
        public async Task DELETE_DeleteReviewByAdmin_AuthErrors(HttpStatusCode statusCode, string roleName = "admin")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/admin/{DependenciesGuids["Review"]}");
            }
            else
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/admin/{DependenciesGuids["Review"]}", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewByAdmin_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/admin/{DependenciesGuids["Review"]}", AccessToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewByAdmin_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/admin/{Guid.NewGuid()}", AccessToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "user")]
        public async Task DELETE_DeleteReviewImageByAdmin_AuthErrors(HttpStatusCode statusCode, string roleName = "admin")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            Guid guid = DependenciesGuids["Review"];

            UpdateContext();

            List<ReviewImage> images = await Context.ReviewImages
                .Where(w => w.ReviewId == guid).ToListAsync();

            guid = images[0].Id;


            // Act
            HttpStatusCode deleteStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/admin/image/{guid}");
            }
            else
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/admin/image/{guid}", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, deleteStatusCode);
        }
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden, "admin")]
        [InlineData(HttpStatusCode.Forbidden, "owner")]
        [InlineData(HttpStatusCode.Forbidden, "bot")]
        public async Task DELETE_DeleteReviewImageByUser_AuthErrors(HttpStatusCode statusCode, string roleName = "user")
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], roleName);
            await InitializeDependencies(DependenciesGuids["User"]);

            Guid guid = DependenciesGuids["Review"];

            UpdateContext();

            List<ReviewImage> images = await Context.ReviewImages
                .Where(w => w.ReviewId == guid).ToListAsync();

            guid = images[0].Id;


            // Act
            HttpStatusCode deleteStatusCode;
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/image/{guid}");
            }
            else
            {
                deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/image/{guid}", AccessToken);
            }

            // Assert
            await RemoveDependencies();
            Assert.Equal(statusCode, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewImageByUser_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);

            Guid guid = DependenciesGuids["Review"];

            UpdateContext();

            List<ReviewImage> images = await Context.ReviewImages
                .Where(w => w.ReviewId == guid).ToListAsync();

            guid = images[0].Id;


            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/image/{guid}", AccessToken);
            

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewImageByUser_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/image/{Guid.NewGuid()}", AccessToken);

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewImageByUser_Conflict()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "user");
            await InitializeDependencies(DependenciesGuids["User"]);
            string fakeUserToken = await CreateFakeToken("user");

            Guid guid = DependenciesGuids["Review"];

            UpdateContext();

            List<ReviewImage> images = await Context.ReviewImages
                .Where(w => w.ReviewId == guid).ToListAsync();

            guid = images[0].Id;


            // Act
            HttpStatusCode deleteStatusCode = await _responseService
                .ResponseDelete($"/api/user-review/image/{guid}", fakeUserToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.Conflict, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewImageByAdmin_OK()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);

            Guid guid = DependenciesGuids["Review"];

            UpdateContext();

            List<ReviewImage> images = await Context.ReviewImages
                .Where(w => w.ReviewId == guid).ToListAsync();

            guid = images[0].Id;


            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/admin/image/{guid}", AccessToken);
            

            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.OK, deleteStatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteReviewImageByAdmin_NotFound()
        {
            // Arrange
            await InitializeUserDependency(DependenciesGuids["User"], "admin");
            await InitializeDependencies(DependenciesGuids["User"]);

            // Act
            HttpStatusCode deleteStatusCode = await _responseService
            .ResponseDelete($"/api/user-review/admin/image/{Guid.NewGuid()}", AccessToken);


            // Assert
            await RemoveDependencies();
            Assert.Equal(HttpStatusCode.NotFound, deleteStatusCode);
        }
        #region Зависимости
        private async Task InitializeDependencies(Guid userGuid)
        {
            UpdateContext();
            UserReview review = new()
            {
                Id = DependenciesGuids["Review"],
                Title = $"{GenerateString()}ReviewTest",
                Content = $"{GenerateString()}ReviewContentTest",
                IsApproved = true,
                UserId = userGuid
            };

            ReviewImage? image = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            ReviewImage? image1 = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            ReviewImage? image2 = new()
            {
                ImageUri = $"{GenerateString()}.jpg",
                ReviewId = DependenciesGuids["Review"]
            };

            await Context.UserReviews.AddAsync(review);
            await Context.ReviewImages.AddRangeAsync(image, image1, image2);
            await Context.SaveChangesAsync();
        }
        private async Task RemoveDependencies()
        {
            await ClearTableData("ReviewImage", "UserReview", "User");
        }
        #endregion
    }
}
