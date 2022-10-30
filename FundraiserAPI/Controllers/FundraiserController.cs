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
        public ActionResult<SessionToken> AddUser(UserProfile user)
        {
            try
            {
                return Ok(fundraiserBL.AddUser(user));
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpPost]
        public ActionResult<SessionToken> LoginUser()
        {
            try
            {
                string authHeader = HttpContext.Request.Headers.Authorization;
                return fundraiserBL.LoginUser(authHeader);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }
    }
}