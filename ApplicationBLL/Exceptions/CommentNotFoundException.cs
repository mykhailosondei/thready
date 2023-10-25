namespace ApplicationBLL.Exceptions;

public class CommentNotFoundException : NotFoundException
{
    public string EntityName { get; protected set; }
    public CommentNotFoundException(string? message = "Comment with specified id does not exist") : base(message)
    {
    }
}