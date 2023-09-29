namespace ApplicationBLL.Exceptions;

public class NotFoundException : Exception 
{
    public string EntityName { get; protected set; }

    public NotFoundException(string message) : base(message)
    {
        EntityName = this.GetType().FullName.Replace("NotFoundException", "");
    }
}