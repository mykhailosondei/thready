using ApplicationDAL.Context;
using AutoMapper;

namespace ApplicationBLL.Services.Abstract;

public abstract class BaseService
{
    private protected readonly ApplicationContext _applicationContext;
    private protected readonly IMapper _mapper;
    protected BaseService(ApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }
}