using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApplicationBLL.Services.Abstract;

public abstract class BaseService
{
    private protected readonly ApplicationContext _applicationContext;
    private protected readonly IMapper _mapper;
    private protected readonly ILogger _logger;
    protected BaseService(ApplicationContext applicationContext, IMapper mapper, ILogger logger = null)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
        _logger = logger;
    }
}