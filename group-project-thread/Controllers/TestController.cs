using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly ApplicationContext _applicationContext;

        public TestController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        // POST: api/Test
        [HttpPost]
        public void Post()
        {
            _applicationContext.Users.Add(new User()
            {
                Id = 1,
                Email = "test",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Password = "test",
                Username = "Test",
                Bio = "test",
                Location = "test"
            });
            _applicationContext.SaveChanges();
        }
    }
}
