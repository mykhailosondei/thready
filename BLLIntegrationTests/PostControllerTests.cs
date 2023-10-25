using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Nodes;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        var postResponse = await TestClient.PostAsJsonAsync("/api/post/", new PostCreateDTO()
        {
            AuthorId = userDTO.Id,
            TextContent = $"new post {Guid.NewGuid().ToString()}",
            Images = new List<ImageDTO>()
        });
        //Act & Assert
        
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdPost =  await postResponse.Content.ReadAsAsync<PostDTO>();
        var response = await TestClient.GetAsync($"/api/Post/{createdPost.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var receivedPost = await response.Content.ReadAsAsync<PostDTO>();
        Assert.Equal(createdPost.Id, receivedPost.Id);
        Assert.Equal(createdPost.TextContent, receivedPost.TextContent);

    }

    [Fact]
    public async Task UserBookmarkPostAndUndoBookmark_ShouldChangeUsersBookmarksAndPostBookmarks()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var expectedBookmarksCount = userDTO.BookmarkedPostsIds.Count;
        var response = await TestClient.GetAsync($"/api/Post/2");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postBookmarksExpected = postDTO.Bookmarks;
        //Act
        HttpResponseMessage? responseMessage;
        if (userDTO.BookmarkedPostsIds.Contains(2))
        {
            expectedBookmarksCount--;
            postBookmarksExpected--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/2/removeFromBookmarks", 2);
        }
        else
        {
            expectedBookmarksCount++;
            postBookmarksExpected++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/2/bookmarkPost", 2);
        }

        var actualBookmarksCount = (await (await TestClient.GetAsync($"/api/User/{userDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).BookmarkedPostsIds.Count;
        var postBookmarksActual = (await (await TestClient.GetAsync($"/api/Post/2/")).Content.ReadAsAsync<PostDTO>()).Bookmarks;
        
        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(expectedBookmarksCount, actualBookmarksCount);
        Assert.Equal(postBookmarksExpected, postBookmarksActual);
    }

    [Fact]
    public async Task UpdatePost_ReturnsPostWithUpdatedText()
    {
        // Arrange
        await AuthenticateAsync();

        int postId = 2;
        
        string textContent = "updated Post";

        var post = new PostUpdateDTO()
        {
            TextContent = textContent,
            Images = new List<ImageDTO>()
        }; 
        var requestUri = $"/api/Post/{postId}";

        // Act
        var response = await TestClient.PutAsJsonAsync(requestUri, post);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var postResponse = await TestClient.GetAsync($"/api/Post/{postId}");
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var postDTO = await postResponse.Content.ReadAsAsync<PostDTO>();
        
        // Assert
        Assert.Equal(postDTO.TextContent, post.TextContent);
    }
    
    [Fact]
    public async Task UserRepostPostAndUndoRepost_ShouldChangeUsersRepostsAndPostReposts()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var expectedRepostsCount = userDTO.RepostsIds.Count;
        var response = await TestClient.GetAsync($"/api/Post/2");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postRepostersExpectedCount = postDTO.RepostersIds.Count;
        //Act
        HttpResponseMessage? responseMessage;
        if (userDTO.RepostsIds.Contains(2) && postDTO.RepostersIds.Contains(1))
        {
            expectedRepostsCount--;
            postRepostersExpectedCount--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/2/undoRepost", 2);
        }
        else
        {
            expectedRepostsCount++;
            postRepostersExpectedCount++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/2/repost", 2);

        }

        var actualRepostsCount = (await (await TestClient.GetAsync($"/api/User/{userDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).RepostsIds.Count;
        var postRepostsActual = (await (await TestClient.GetAsync($"/api/Post/2/")).Content.ReadAsAsync<PostDTO>()).RepostersIds.Count;
        
        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(expectedRepostsCount, actualRepostsCount);
        Assert.Equal(postRepostersExpectedCount, postRepostsActual);
    }
    
    [Fact]
    public async Task PostLikes_ShouldChangePostLikes()
    {
        //Arrange
        await AuthenticateAsync();
        var response = await TestClient.GetAsync($"/api/Post/2");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postLikesExpectedCount = postDTO.LikesIds.Count;
        //Act
        HttpResponseMessage? responseMessage;
        if (postDTO.LikesIds.Contains(1))
        {
            postLikesExpectedCount--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/2/unlikePost", 2);
        }
        else
        {
            postLikesExpectedCount++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/2/likePost", 2);

        }

        var postLikesActual = (await (await TestClient.GetAsync($"/api/Post/2/")).Content.ReadAsAsync<PostDTO>()).LikesIds.Count;
        
        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(postLikesExpectedCount, postLikesActual);
    }

    [Fact]
    public async Task DeletePost_ShouldDeletePost()
    {
        // Arrange
        var userDTO = await AuthenticateAsync();
        
        var postResponse = await TestClient.PostAsJsonAsync("/api/post/", new PostCreateDTO()
        {
            AuthorId = userDTO.Id,
            TextContent = " post for delete",
            Images = new List<ImageDTO>()
        });
        
        // Act
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdPost =  await postResponse.Content.ReadAsAsync<PostDTO>();
        
        var response = await TestClient.DeleteAsync($"api/post/{createdPost.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response = await TestClient.GetAsync($"/api/Post/{createdPost.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    
}