using System.Net;
using System.Net.Http.Headers;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using FluentAssertions;
using Xunit;

namespace BLLIntegrationTests;

public class UserControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAllUsers_WithUsersPresented_ReturnsResponseWithAllUsers()
    {
        //Act
        var response = TestClient.GetAsync("/api/user/").Result;
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<UserDTO>>()).Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetUserById_ReturnsSuccessfulResponseAndUser()
    {
        // Arrange
        
        int userId = 1;
        var requestUri = $"/api/User/{userId}";

        // Act
        var response = await TestClient.GetAsync(requestUri);
        var user = response.Content.ReadAsAsync<UserDTO>().Result;

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(userId, user.Id);
    }
    
    [Fact]
    public async Task GetCurrentUser_ReturnsSuccessfulResponseAndUser()
    {
        // Arrange
        await AuthenticateAsync();
        var requestUri = $"/api/User/currentUser";

        // Act
        var response = await TestClient.GetAsync(requestUri);
        var user = response.Content.ReadAsAsync<UserDTO>().Result;

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(1, user.Id);
    }
    
    [Fact]
    public async Task UserFollow_ShouldFollowUserIfNotAlreadyFollowing_IfFollowingShouldUnfollow()
    {
        
        //Arrange
        var userDTO = await AuthenticateAsync();
        var expectedFollowingCount = userDTO.FollowingIds.Count;
        var response = await TestClient.GetAsync($"/api/User/2");
        var userToFollowDTO = await response.Content.ReadAsAsync<UserDTO>();
        var expetedFollowersCount = userToFollowDTO.FollowersIds.Count;
        //Act
        HttpResponseMessage? responseMessage;
        if (userDTO.FollowingIds.Contains(2))
        {
            expectedFollowingCount--;
            expetedFollowersCount--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/User/2/unfollow", 2);
        }
        else
        {
            expectedFollowingCount++;
            expetedFollowersCount++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/User/2/follow", 2);
        }

        var actualFollowingCount = (await (await TestClient.GetAsync($"/api/User/{userDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).FollowingIds.Count;
        var actualFollowersCount = (await (await TestClient.GetAsync($"/api/User/{userToFollowDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).FollowersIds.Count;

        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(expectedFollowingCount, actualFollowingCount);
        Assert.Equal(expetedFollowersCount, actualFollowersCount);
    }

    [Fact]
    public async Task PutUserAndDelete_ShouldUpdateInfoAboutUserAndThenDeleteHim()
    {
        //Arrange
        var userDTO = await RegisterNewUser("putanddelete@gmail.com", "putanddelete", "putanddelete");
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDTO.Token);
        var users = await (await TestClient.GetAsync($"/api/user/")).Content.ReadAsAsync<List<UserDTO>>();
        var userId = users[users.Count - 1].Id;
        string expectedText = "updated bio";
        //Act && assert
        var updateRequest = await TestClient.PutAsJsonAsync($"/api/User/{userId}", new UpdateUserDTO()
        {
            Location = "New Location",
            Bio = "updated bio",
            Avatar = new ImageDTO()
            {
                Url = "test14"
            }
        });
        updateRequest.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await TestClient.GetAsync($"/api/User/currentUser");
        var actualBioText = (await response.Content.ReadAsAsync<UserDTO>()).Bio;
        Assert.Equal(expectedText, actualBioText);
        var deleteResponse = TestClient.DeleteAsync($"/api/User/{userDTO.User.Id}").Result;
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        response = await TestClient.GetAsync($"/api/User/{userDTO.User.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}