using ApplicationDAL.Context;
using AutoMapper;

namespace ApplicationBLL.QueryRepositories.Abstract;

public class BaseQueryRepository
{
    protected readonly ApplicationContext _applicationContext;
    protected readonly IMapper _mapper;

    protected BaseQueryRepository(ApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }
}