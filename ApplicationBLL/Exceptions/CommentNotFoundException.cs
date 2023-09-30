namespace ApplicationBLL.Exceptions;

public class CommentNotFoundException : NotFoundException
{
    public string EntityName { get; protected set; }
    public CommentNotFoundException(string? message) : base(message)
    {
    }
}