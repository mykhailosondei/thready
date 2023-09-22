namespace ApplicationBLL.Exceptions;

public class PostNotFoundException : Exception
{
    public PostNotFoundException(string? message = "Post with specified id does not exist") : base(message)
    {
        
    }
}