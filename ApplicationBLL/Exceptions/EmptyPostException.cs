namespace ApplicationBLL.Exceptions;

public class EmptyPostException : Exception
{
    public EmptyPostException(string? message = "Post needs to have text or image content") : base(message)
    {
        
    }
}