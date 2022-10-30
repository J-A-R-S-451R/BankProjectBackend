using FundraiserAPI.Domain;
using FundraiserAPI.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Net;

namespace FundraiserAPI.BL
{
    public class FundraiserBL
    {
        public SessionToken AddUser(UserProfile user)
        {
            if (!IsValidUsername(user.Username))
            {
                throw new ErrorResponseException("Your username isn't valid.", ErrorResponseCodes.USERNAME_NOT_VALID, HttpStatusCode.BadRequest);
            }

            if (!IsPasswordStrong(user.Password))
            {
                throw new ErrorResponseException("Your password isn't strong enough.", ErrorResponseCodes.PASSWORD_TOO_WEAK, HttpStatusCode.BadRequest);
            }

            EntityEntry<Login> userLogin = null;

            using (var fdb = new FundraiserProjectContext())
            {
                if (fdb.Logins.Any(x => x.Username == user.Username))
                {
                    throw new ErrorResponseException("There is already an existing user with this username.", ErrorResponseCodes.USER_ALREADY_EXISTS, HttpStatusCode.BadRequest);
                }

                userLogin = fdb.Logins.Add(new Login()
                {
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                });

                fdb.SaveChanges();
            }

            SessionToken token = CreateSessionTokenForUser(userLogin.Entity.Id);
            return token;
        }

        public SessionToken CreateSessionTokenForUser(int userId)
        {
            SessionToken sessionToken = new SessionToken()
            {
                UserId = userId,
                ExpiresOn = DateTime.UtcNow.AddHours(5),
                SessionId = GenerateRandomBase64(16),
            };

            using (var fdb = new FundraiserProjectContext())
            {
                fdb.SessionTokens.Add(sessionToken);
                fdb.SaveChanges();
            }

            return sessionToken;
        }

        public string GenerateRandomBase64(int lengthInBytes)
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(lengthInBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public bool IsPasswordStrong(string password)
        {
            if (String.IsNullOrEmpty(password)) return false;
            if (password.Length < 8) return false;
            if (!password.Any(x => Char.IsUpper(x))) return false;
            if (!password.Any(x => Char.IsNumber(x))) return false;
            if (!password.Any(x => !Char.IsLetterOrDigit(x))) return false;
            return true;
        }

        public bool IsValidUsername(string username)
        {
            if (String.IsNullOrEmpty(username)) return false;
            if (!Regex.IsMatch(username, "^[A-Za-z0-9]{6,32}$")) return false;
            return true;
        }

        public SessionToken LoginUser(UserProfile user)
        {
            using (var fdb = new FundraiserProjectContext())
            {
 
                Login currentUser = fdb.Logins.Where(x => x.Username == user.Username).SingleOrDefault();
                if(currentUser == null)
                {
                    throw new ErrorResponseException("The username does not exist", ErrorResponseCodes.USER_DNE, HttpStatusCode.BadRequest);
                }

                if(currentUser.Password == user.Password)
                {
                    return CreateSessionTokenForUser(currentUser.Id);
                }
                else
                {
                    throw new ErrorResponseException("The password is not correct", ErrorResponseCodes.PASSWORD_INCORRECT, HttpStatusCode.BadRequest);
                }
            }

        }

        public SessionToken LoginUser(string authSlug)
        {
            UserProfile? profile = new();
            string[]? authParts = authSlug?.Split(" ");

            if (authParts == null || authParts?.Length != 2)
            {
                throw new ErrorResponseException("Invalid Authorization header.");
            }

            byte[] credentialsJsonBytes = Convert.FromBase64String(authParts[1]);
            string credentialsJson = Encoding.UTF8.GetString(credentialsJsonBytes);

            try
            {
                profile = JsonSerializer.Deserialize<UserProfile>(credentialsJson);
            }
            catch
            {
                throw new ErrorResponseException("Malformed credentials JSON.");
            }

            return LoginUser(profile ?? new UserProfile());
        }

        public UserProfile? SessionTokenToProfile(SessionToken sessionToken)
        {
            using (var fdb = new FundraiserProjectContext())
            {
                SessionToken? dbToken = fdb.SessionTokens.Where(x => x.SessionId == sessionToken.SessionId).SingleOrDefault();
                if (dbToken == null) return null;

                if (dbToken.ExpiresOn >= DateTime.UtcNow) return null;

                Login? currentUser = fdb.Logins.Where(x => x.Id == dbToken.UserId).SingleOrDefault();
                if (currentUser == null) return null;


                UserProfile profile = new UserProfile
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Username = currentUser.Username,
                    Password = currentUser.Password
                };

                return profile;

            }
        }

        public ObjectResult RespondWithError(ErrorResponseException exception)
        {
            return new ObjectResult(exception.ErrorData)
            {
                StatusCode = (int)(exception?.ErrorData?.HttpCode ?? System.Net.HttpStatusCode.InternalServerError)
            };
        }

        public void DonateToFundraiser(UserProfile profile)
        {

        }

        public void DonateToFundraiser()
        {

        }
    }
}
