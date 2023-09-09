using ApplicationDAL.Context;

namespace ApplicationBLL.Services.Abstract;

public abstract class BaseService
{
    private protected readonly ApplicationContext _applicationContext;
    protected BaseService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
}