namespace ApplicationBLL.Exceptions;

public class NotAcceptableExceptions : Exception
{
    public string EntityName { get; protected set; }

    public NotAcceptableExceptions(string? message) : base(message)
    {
        EntityName = this.GetType().Name.Replace("Empty", "").Replace("Exception", "");
    }
}