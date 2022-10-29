using FundraiserAPI.BL;
using FundraiserAPI.Domain;
using FundraiserAPI.EntityFramework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace FundraiserAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_allowAnyDomain")]
    public class FundraiserController : ControllerBase
    {
        private readonly ILogger<FundraiserController> _logger;

        private FundraiserBL fundraiserBL = new FundraiserBL();

        public FundraiserController(ILogger<FundraiserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public SessionToken AddUser(UserProfile user)
        {
            return fundraiserBL.AddUser(user);

        }
        [HttpPost]
        public SessionToken LoginUser()
        {
            string authHeader = HttpContext.Request.Headers.Authorization;
            return fundraiserBL.LoginUser(authHeader);
        }
    }
}