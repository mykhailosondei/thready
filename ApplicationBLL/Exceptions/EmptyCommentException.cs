namespace ApplicationBLL.Exceptions;

public class EmptyCommentException : NotAcceptableExceptions
{
    public EmptyCommentException(string? message = "Post needs to have text or image content") : base(message)
    {
        
    }
}