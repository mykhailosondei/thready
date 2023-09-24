namespace ApplicationBLL.Exceptions;

public class EmptyCommentException : Exception
{
    public EmptyCommentException(string? message = "Post needs to have text or image content") : base(message)
    {
        
    }
}