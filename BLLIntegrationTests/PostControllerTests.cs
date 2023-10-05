using System.Net;
using System.Text.Json.Nodes;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
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

    [Fact]
    public async Task UserBookmarkPostAndUndoBookmark_ShouldChangeUsersBookmarksAndPostBookmarks()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var expectedBookmarksCount = userDTO.BookmarkedPostsIds.Count;
        var response = await TestClient.GetAsync($"/api/Post/1");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postBookmarksExpected = postDTO.Bookmarks;
        //Act
        HttpResponseMessage? responseMessage;
        if (userDTO.BookmarkedPostsIds.Contains(1))
        {
            expectedBookmarksCount--;
            postBookmarksExpected--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/1/removeFromBookmarks", 1);
        }
        else
        {
            expectedBookmarksCount++;
            postBookmarksExpected++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/1/bookmarkPost", 1);

        }

        var actualBookmarksCount = (await (await TestClient.GetAsync($"/api/User/{userDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).BookmarkedPostsIds.Count;
        var postBookmarksActual = (await (await TestClient.GetAsync($"/api/Post/1/")).Content.ReadAsAsync<PostDTO>()).Bookmarks;
        
        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(expectedBookmarksCount, actualBookmarksCount);
        Assert.Equal(postBookmarksExpected, postBookmarksActual);
    }
    
    [Fact]
    public async Task UserRepostPostAndUndoRepost_ShouldChangeUsersRepostsAndPostReposts()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var expectedRepostsCount = userDTO.RepostsIds.Count;
        var response = await TestClient.GetAsync($"/api/Post/1");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postRepostersExpectedCount = postDTO.RepostersIds.Count;
        //Act
        HttpResponseMessage? responseMessage;
        if (userDTO.RepostsIds.Contains(1) && postDTO.RepostersIds.Contains(1))
        {
            expectedRepostsCount--;
            postRepostersExpectedCount--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/1/undoRepost", 1);
        }
        else
        {
            expectedRepostsCount++;
            postRepostersExpectedCount++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/1/repost", 1);

        }

        var actualRepostsCount = (await (await TestClient.GetAsync($"/api/User/{userDTO.Id}/")).Content.ReadAsAsync<UserDTO>()).RepostsIds.Count;
        var postRepostsActual = (await (await TestClient.GetAsync($"/api/Post/1/")).Content.ReadAsAsync<PostDTO>()).RepostersIds.Count;
        
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
        var response = await TestClient.GetAsync($"/api/Post/1");
        var postDTO = await response.Content.ReadAsAsync<PostDTO>();
        var postLikesExpectedCount = postDTO.LikesIds.Count;
        //Act
        HttpResponseMessage? responseMessage;
        if (postDTO.LikesIds.Contains(1))
        {
            postLikesExpectedCount--;
            responseMessage = await TestClient.PostAsJsonAsync("/api/Post/1/unlikePost", 1);
        }
        else
        {
            postLikesExpectedCount++;
            responseMessage = await TestClient.PostAsJsonAsync($"/api/Post/1/likePost", 1);

        }

        var postLikesActual = (await (await TestClient.GetAsync($"/api/Post/1/")).Content.ReadAsAsync<PostDTO>()).LikesIds.Count;
        
        //Assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        Assert.Equal(postLikesExpectedCount, postLikesActual);
    }

    [Fact]
    public async Task PutPostAndDelete_ShouldUpdateInfoAboutPost()
    {
        //Arrange
        var userDTO = await AuthenticateAsync();
        var posts = await (await TestClient.GetAsync($"/api/Post/{userDTO.Id}/posts")).Content.ReadAsAsync<List<PostDTO>>();
        var postResponse = await TestClient.PostAsJsonAsync("/api/post/", new PostCreateDTO()
        {
            AuthorId = userDTO.Id,
            TextContent = "new post",
            Images = new List<ImageDTO>()
        });
        var postId = posts[posts.Count - 1].Id;
        string expectedText = "updated post";
        //Act && assert
        var responce = await TestClient.PutAsJsonAsync($"/api/Post/{postId}", new PostCreateDTO()
        {
            TextContent = "updated post",
        });
        if (responce.StatusCode == HttpStatusCode.OK)
        {
            var response = await TestClient.GetAsync($"/api/Post/{postId}");
            var actualPostText = (await response.Content.ReadAsAsync<PostDTO>()).TextContent;
            Assert.Equal(expectedText, actualPostText);
            var deleteResponce = await TestClient.DeleteAsync($"/api/Post/{postId}");
            deleteResponce.StatusCode.Should().Be(HttpStatusCode.OK);
            response = await TestClient.GetAsync($"/api/Post/{postId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
    
    
    
}