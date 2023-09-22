namespace ApplicationBLL.Exceptions;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException(string? message) : base(message)
    {
        
    }
}