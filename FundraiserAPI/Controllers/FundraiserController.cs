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
        public ActionResult<SessionToken> LoginUser(LoginCredentials credentials)
        {
            try
            {
                return fundraiserBL.LoginUser(credentials);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpGet]
        public ActionResult<UserProfile> GetCurrentUser()
        {
            try
            {
                string authHeader = HttpContext.Request.Headers.Authorization;
                return fundraiserBL.GetCurrentUser(authHeader);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpGet]
        public ActionResult<List<EntityFramework.Fundraiser>> GetAllFundraisers()
        {
            try
            {
                return fundraiserBL.GetAllFundraisers();
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        public ActionResult<EntityFramework.Fundraiser> GetFundraiser(int fundraiserId)
        {
            try
            {
                return fundraiserBL.GetFundraiser(fundraiserId);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpGet]
        public ActionResult<List<Domain.Donation>> GetDonations(int fundraiserId)
        {
            try
            {
                return fundraiserBL.GetDonations(fundraiserId);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpPost]
        public ActionResult SendDonation(Domain.Donation donation)
        {
            try
            {
                string authHeader = HttpContext.Request.Headers.Authorization;
                fundraiserBL.ProcessDonation(donation, authHeader);
                return new OkObjectResult(new {Success = true});
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }

        [HttpPost]
        public ActionResult<EntityFramework.Fundraiser> CreateFundraiser(CreateFundraiser fundraiser)
        {
            try
            {
                string authHeader = HttpContext.Request.Headers.Authorization;
                return fundraiserBL.CreateFundraiser(fundraiser, authHeader);
            }
            catch (ErrorResponseException ex)
            {
                return fundraiserBL.RespondWithError(ex);
            }
        }
    }
}