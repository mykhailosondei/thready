using System.Net;
using System.Text.Json.Nodes;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BLLIntegrationTests;

public class PostControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAllPosts_WithPostPresented_ReturnsResponseWithAllPosts()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = TestClient.GetAsync("/api/post/").Result;
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
    }

    [Fact]
    public async Task PostPost_ThenGetPost_ShouldReturnPostedPost()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        int lastPostId = TestClient.GetAsync("api/Post/").Result.Content.ReadAsAsync<List<Post>>().Result.Count;
        var postResponse = await TestClient.PostAsJsonAsync("/api/post/", new PostCreateDTO()
        {
            TextContent = "new post",
            Images = new List<ImageDTO>()
        });

        var response = await TestClient.GetAsync($"/api/Post/{lastPostId+1}");
        //Assert
        
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultPost = await response.Content.ReadAsAsync<Post>();
        Assert.Equal("new post", resultPost.TextContent);
    }
}