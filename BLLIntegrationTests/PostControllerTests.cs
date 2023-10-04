using System.Net;
using System.Text.Json.Nodes;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentAssertions;
using Xunit;

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
        (await response.Content.ReadAsAsync<List<Post>>()).Should().NotBeEmpty();
    }

    [Fact]
    public async Task UserPostPost_ShouldIncreaseUserPostsCount()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var posts = await (await TestClient.GetAsync($"/api/Post/{userDTO.Id}/posts")).Content.ReadAsAsync<List<PostDTO>>();
        
        int initialPostCount = posts.Count;
        //Act
        var postResponse = await TestClient.PostAsJsonAsync("/api/post/", new PostCreateDTO()
        {
            AuthorId = userDTO.Id,
            TextContent = "new post",
            Images = new List<ImageDTO>()
        });

        var response = await TestClient.GetAsync($"/api/Post/{userDTO.Id}/posts");
        //Assert
        
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultPost = await response.Content.ReadAsAsync<List<PostDTO>>();
        Assert.Equal(initialPostCount, resultPost.Count-1);
    }


    
}