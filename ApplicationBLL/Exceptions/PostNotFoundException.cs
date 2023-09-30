namespace ApplicationBLL.Exceptions;

public class PostNotFoundException : NotFoundException
{
    public PostNotFoundException(string? message = "Post with specified id does not exist") : base(message)
    {
    }
}