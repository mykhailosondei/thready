using System.Text;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using Newtonsoft.Json;
using NuGet.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace BLLIntegrationTests;

public class CommentControllerTests : IntegrationTest
{
    
    private readonly ITestOutputHelper _output;
    
    public CommentControllerTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public async Task GetCommentById_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();
        
        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}";

        // Act
        var response = await TestClient.GetAsync(requestUri);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);


        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetCommentsOfPostId_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        int postId = 2;
        var requestUri = $"/api/Comment/{postId}/comments";

        // Act
        var response = await TestClient.GetAsync(requestUri);

        _output.WriteLine(response.ReasonPhrase);
        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PostComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        // Replace with your DTO
        var comment = new CommentCreateDTO()
        {
            PostId = 2,
            TextContent = "test",
            Images = new List<ImageDTO>()
        }; 
        var requestUri = "/api/Comment";
        
        // Act
        var response = await TestClient.PostAsJsonAsync(requestUri, comment);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task LikeComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}/likeComment";

        // Act
        var response = await TestClient.PostAsJsonAsync(requestUri, "");
        
        _output.WriteLine(response.ReasonPhrase);


        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PutComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        int commentId = 2;
        var comment = new CommentUpdateDTO()
        {
           TextContent = "test update",
           Images = new List<ImageDTO>()
        }; 
        var requestUri = $"/api/Comment/{commentId}";

        // Act
        var response = await TestClient.PutAsJsonAsync(requestUri, comment);
        
        _output.WriteLine(response.ReasonPhrase);


        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DeleteComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        int postId = 2;
        
        var post = TestClient.GetAsync($"/api/Post/{postId}").Result.Content.ReadAsAsync<PostDTO>().Result;
        
        int commentId = post.CommentsIds[^1] + 1;
        var requestUri = $"/api/Comment/{commentId}";

        // Act
        await TestClient.PostAsJsonAsync("/api/Comment/", new CommentCreateDTO()
        {
            PostId = postId,
            TextContent = "test",
            Images = new List<ImageDTO>()
        });
        
        var response = await TestClient.DeleteAsync(requestUri);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task UnlikeComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}/unlikeComment";

        // Act
        var response = await TestClient.PostAsync(requestUri, null);
        
        _output.WriteLine(response.ReasonPhrase);


        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task GetCommentById_ReturnsCommentWithAllCommentTree()
    {
        // Arrange
        await AuthenticateAsync();
        
        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}";

        // Act
        var response = await TestClient.GetAsync(requestUri);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        var comment = JsonConvert.DeserializeObject<CommentDTO>(response.Content.ReadAsStringAsync().Result);
        
        // Assert
        Assert.True(comment.CommentsIds.Count > 0);
    }
    
    [Fact]
    public async Task PostComment_ToComment_ReturnsSuccessfulResponse()
    {
        // Arrange
        await AuthenticateAsync();

        // Replace with your DTO
        var comment = new CommentCreateDTO()
        {
            CommentId = 2,
            TextContent = "test nested comment",
            Images = new List<ImageDTO>()
        }; 
        var requestUri = "/api/Comment";
        
        // Act
        var response = await TestClient.PostAsJsonAsync(requestUri, comment);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    
    // like comment updates comment
    [Fact]
    public async Task LikeComment_UpdatesComment()
    {
        // Arrange
        var user = await AuthenticateAsync();

        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}/likeComment";

        // Act
        var response = await TestClient.PostAsJsonAsync(requestUri, "");
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        var commentResponse = await TestClient.GetAsync($"/api/Comment/{commentId}");

        var commentDTO = await commentResponse.Content.ReadAsAsync<CommentDTO>();
        
        // Assert
        Assert.Contains(commentDTO.LikesIds, id => id == user.Id);
    }
    
    // unlike comment updates comment
    [Fact]
    public async Task UnlikeComment_UpdatesComment()
    {
        // Arrange
        var user = await AuthenticateAsync();

        int commentId = 2;
        var requestUri = $"/api/Comment/{commentId}/unlikeComment";

        // Act
        var response = await TestClient.PostAsJsonAsync(requestUri, "");
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        var commentResponse = await TestClient.GetAsync($"/api/Comment/{commentId}");

        var commentDTO = await commentResponse.Content.ReadAsAsync<CommentDTO>();
        
        // Assert
        Assert.DoesNotContain(commentDTO.LikesIds, id => id == user.Id);
    }
    
    // put comment updates comment text
    
    [Fact]
    public async Task PutComment_UpdatesCommentText()
    {
        // Arrange
        await AuthenticateAsync();

        int commentId = 2;
        
        var random = new Random();
        string textContent = String.Empty;

        for (int i = 0; i < 20; i++)
        {
            textContent += (char)random.Next(97, 122);
        }
        var comment = new CommentUpdateDTO()
        {
            TextContent = textContent,
            Images = new List<ImageDTO>()
        }; 
        var requestUri = $"/api/Comment/{commentId}";

        // Act
        var response = await TestClient.PutAsJsonAsync(requestUri, comment);
        
        _output.WriteLine(response.ReasonPhrase);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);

        var commentResponse = await TestClient.GetAsync($"/api/Comment/{commentId}");

        var commentDTO = await commentResponse.Content.ReadAsAsync<CommentDTO>();
        
        // Assert
        Assert.Equal(comment.TextContent, commentDTO.TextContent);
    }
}