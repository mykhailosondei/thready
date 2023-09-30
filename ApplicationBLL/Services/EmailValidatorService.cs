using System.Diagnostics;
using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationBLL.Services;

public class EmailValidatorService : BaseService
{
    public EmailValidatorService() : base(null, null)
    {
        
    }
    
    public EmailValidatorService(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext, mapper)
    {
        
    }

    public virtual async Task<bool> IsEmailAvailable(string email)
    {
        if (!ValidEmail(email))
        {
            Console.WriteLine("temp");
            return false;
        }
        
        return !await _applicationContext.Users.AnyAsync(u => u.Email == email); 
    }

    private static bool ValidEmail(string email)
    {
        var atSymbolPosition = email.LastIndexOf('@');
        return !(atSymbolPosition < 0 || email.LastIndexOf('.') < atSymbolPosition || email.Length - atSymbolPosition < 4);
    }
}