using System.Net;
using ApplicationDAL.Entities;
using FluentAssertions;
using Xunit;

namespace BLLIntegrationTests;

public class PostControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAllPosts_WithPostPresented_ReturnsResponceWithAllPosts()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = await TestClient.GetAsync("https://localhost:7153/api/post/");
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<Post>>()).Should().NotBeEmpty();
    }
}