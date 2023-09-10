using ApplicationCommon.Interfaces;

namespace ApplicationBLL.Logic;

public class CurrentUserIdProvider : IUserIdGetter, IUserIdSetter
{
    private int _id;
    public int CurrentId { 
        get => ValidateId(_id);
        set => _id = value;
    }

    private int ValidateId(int id)
    {
        if (_id == 0)
        {
            throw new Exception("No token passed");
        }

        return _id;
    }

    
}