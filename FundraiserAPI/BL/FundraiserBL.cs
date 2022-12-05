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
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

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

            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName))
                throw new ErrorResponseException("Your name must not be empty.", ErrorResponseCodes.NAME_IS_EMPTY, HttpStatusCode.BadRequest);

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

        public void UpdateUser(UserProfile userInfo, string authToken)
        {
            UserProfile user = GetCurrentUser(authToken);

            if (String.IsNullOrEmpty(userInfo.FirstName) || String.IsNullOrEmpty(userInfo.LastName))
                throw new ErrorResponseException("Your name must not be empty.", ErrorResponseCodes.NAME_IS_EMPTY, HttpStatusCode.BadRequest);

            using (var fdb = new FundraiserProjectContext())
            {
                Login dbUser = fdb.Logins.Where(x => x.Id == user.UserId).Single();
                dbUser.FirstName = userInfo.FirstName;
                dbUser.LastName = userInfo.LastName;
                dbUser.AddressZip = userInfo.AddressZip;
                dbUser.AddressStreet1 = userInfo.AddressStreet1;
                dbUser.AddressStreet2 = userInfo.AddressStreet2;
                dbUser.AddressCity = userInfo.AddressCity;
                dbUser.AddressState = userInfo.AddressState;
                dbUser.AddressCountry = userInfo.AddressCountry;
                fdb.SaveChanges();
            }
        }

        public void UpdateUserPassword(PasswordChange passwordChange, string authToken)
        {
            UserProfile user = GetCurrentUser(authToken);

            if (!IsPasswordStrong(passwordChange.Password))
            {
                throw new ErrorResponseException("Your password isn't strong enough.", ErrorResponseCodes.PASSWORD_TOO_WEAK, HttpStatusCode.BadRequest);
            }

            using (var fdb = new FundraiserProjectContext())
            {
                Login dbUser = fdb.Logins.Where(x => x.Id == user.UserId).Single();
                dbUser.Password = passwordChange.Password;
                fdb.SaveChanges();
            }
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

        public SessionToken LoginUser(LoginCredentials credentials)
        {
            using (var fdb = new FundraiserProjectContext())
            {
 
                Login currentUser = fdb.Logins.Where(x => x.Username == credentials.Username).SingleOrDefault();
                if (currentUser == null)
                {
                    throw new ErrorResponseException("The username does not exist", ErrorResponseCodes.USER_DNE, HttpStatusCode.BadRequest);
                }

                if (currentUser.Password == credentials.Password)
                {
                    return CreateSessionTokenForUser(currentUser.Id);
                }
                else
                {
                    throw new ErrorResponseException("The password is not correct", ErrorResponseCodes.PASSWORD_INCORRECT, HttpStatusCode.BadRequest);
                }
            }
        }

        public UserProfile? AuthTokenToProfile(string authToken)
        {
            if (String.IsNullOrEmpty(authToken))
                return null;

            if (authToken.StartsWith("Bearer "))
                authToken = authToken.Substring("Bearer ".Length);

            using (var fdb = new FundraiserProjectContext())
            {
                SessionToken? dbToken = fdb.SessionTokens.Where(x => x.SessionId == authToken).SingleOrDefault();
                if (dbToken == null) return null;

                if (dbToken.ExpiresOn <= DateTime.UtcNow) return null;

                Login? currentUser = fdb.Logins.Where(x => x.Id == dbToken.UserId).SingleOrDefault();
                if (currentUser == null) return null;


                UserProfile profile = new UserProfile
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Username = currentUser.Username,
                    AddressCity = currentUser.AddressCity,
                    AddressCountry = currentUser.AddressCountry,
                    AddressState = currentUser.AddressState,
                    AddressStreet1 = currentUser.AddressStreet1,
                    AddressStreet2 = currentUser.AddressStreet2,
                    AddressZip = currentUser.AddressZip,
                    UserId = currentUser.Id
                };

                return profile;

            }
        }

        public UserProfile GetCurrentUser(string authToken)
        {
            UserProfile? profile = AuthTokenToProfile(authToken);
            if (profile == null)
            {
                throw new ErrorResponseException("Current auth token is invalid.", ErrorResponseCodes.INVALID_AUTH_TOKEN, HttpStatusCode.Unauthorized);
            }

            return profile;
        }

        public ObjectResult RespondWithError(ErrorResponseException exception)
        {
            return new ObjectResult(exception.ErrorData)
            {
                StatusCode = (int)(exception?.ErrorData?.HttpCode ?? System.Net.HttpStatusCode.InternalServerError)
            };
        }

        public List<EntityFramework.Fundraiser> GetAllFundraisers()
        {
            using (var fdb = new FundraiserProjectContext())
            {
                return (
                    from fundraiser in fdb.Fundraisers
                    select fundraiser
                ).ToList();
            }
        }

        public List<EntityFramework.Fundraiser> GetCurrentUserFundraisers(string authToken)
        {
            UserProfile userProfile = GetCurrentUser(authToken);

            using (var fdb = new FundraiserProjectContext())
            {
                return (
                    from fundraiser in fdb.Fundraisers
                    where fundraiser.UserId == userProfile.UserId
                    select fundraiser
                ).ToList();
            }
        }

        public EntityFramework.Fundraiser GetFundraiser(int id)
        {
            EntityFramework.Fundraiser? fundraiser = null;
            using (var fdb = new FundraiserProjectContext())
            {
                fundraiser = (
                    from fr in fdb.Fundraisers
                    where fr.Id == id
                    select fr
                ).FirstOrDefault();
            }

            if (fundraiser == null)
                throw new ErrorResponseException("Fundraiser not found.", ErrorResponseCodes.FUNDRAISER_NOT_FOUND, HttpStatusCode.NotFound);

            return fundraiser;
        }
        
        public List<Domain.Donation> GetDonations(int fundraiserId)
        {
            // This will handle if the fundraiser doesn't exist by throwing.
            GetFundraiser(fundraiserId);

            List<Domain.Donation> donations = new();
            using (var fdb = new FundraiserProjectContext())
            {
                donations = (
                    from donation in fdb.Donations
                    where donation.FundraiserId == fundraiserId
                    select new Domain.Donation
                    {
                        Id = donation.Id,
                        FirstName = donation.FirstName,
                        LastName = donation.LastName,
                        Amount = donation.Amount,
                        Note = donation.Note,
                        Date = donation.Date
                    }
                ).ToList();
            }

            return donations;
        }

        public bool ProcessDonation(Domain.Donation donation, string authHeader)
        {
            // This will handle if the fundraiser doesn't exist by throwing.
            GetFundraiser(donation.FundraiserId);

            UserProfile? profile = AuthTokenToProfile(authHeader);

            if (String.IsNullOrEmpty(donation.FirstName) || String.IsNullOrEmpty(donation.LastName))
                throw new ErrorResponseException("Invalid name.", ErrorResponseCodes.DONATION_NAME_INVALID, HttpStatusCode.BadRequest);

            if (String.IsNullOrEmpty(donation.AddressCountry)
                || String.IsNullOrEmpty(donation.AddressState)
                || String.IsNullOrEmpty(donation.AddressCity)
                || String.IsNullOrEmpty(donation.AddressStreet1)
                || String.IsNullOrEmpty(donation.AddressZip))
            {
                throw new ErrorResponseException("Invalid address.", ErrorResponseCodes.DONATION_ADDRESS_INVALID, HttpStatusCode.BadRequest);
            }

            if (donation.Amount < 0)
                throw new ErrorResponseException("Donation amount must be greater than 0.", ErrorResponseCodes.DONATION_AMOUNT_INVALID, HttpStatusCode.BadRequest);

            if (donation.PaymentType == "credit_card")
            {
                if (String.IsNullOrEmpty(donation.CreditCardNumber) || String.IsNullOrEmpty(donation.Cvv))
                    throw new ErrorResponseException("Credit card information is invalid", ErrorResponseCodes.DONATION_CREDIT_CARD_INVALID, HttpStatusCode.BadRequest);

                if (donation.BankAccountNumber != null)
                    throw new ErrorResponseException("Bank account number shouldn't be specified.", ErrorResponseCodes.DONATION_BANK_ACCOUNT_NUMBER_INVALID, HttpStatusCode.BadRequest);
            }
            else if (donation.PaymentType == "bank_account")
            {
                if (String.IsNullOrEmpty(donation.BankAccountNumber))
                    throw new ErrorResponseException("Bank account number is invalid", ErrorResponseCodes.DONATION_BANK_ACCOUNT_NUMBER_INVALID, HttpStatusCode.BadRequest);

                if (donation.CreditCardNumber != null || donation.Cvv != null)
                    throw new ErrorResponseException("Credit card number shouldn't be specified", ErrorResponseCodes.DONATION_CREDIT_CARD_INVALID, HttpStatusCode.BadRequest);
            }
            else
                throw new ErrorResponseException("Payment type is invalid.", ErrorResponseCodes.DONATION_PAYMENT_TYPE_INVALID, HttpStatusCode.BadRequest);


            using (var fdb = new FundraiserProjectContext())
            {
                fdb.Donations.Add(new EntityFramework.Donation
                {
                    UserId = profile?.UserId,
                    Amount = donation.Amount,
                    FirstName = donation.FirstName,
                    LastName = donation.LastName,
                    PaymentType = donation.PaymentType,
                    CreditCardNumber = donation.CreditCardNumber,
                    Cvv = donation.Cvv,
                    BankAccountNumber = donation.BankAccountNumber,
                    AddressCountry = donation.AddressCountry,
                    AddressState = donation.AddressState,
                    AddressCity = donation.AddressCity,
                    AddressStreet1 = donation.AddressStreet1,
                    AddressStreet2 = donation.AddressStreet2,
                    AddressZip = donation.AddressZip,
                    FundraiserId = donation.FundraiserId,
                    Note = donation.Note,
                    Date = DateTime.Now
                });

                fdb.Fundraisers.Where(x => x.Id == donation.FundraiserId)
                               .Single()
                               .DonationTotal += donation.Amount;

                fdb.SaveChanges();
            }

            return true;
        }

        public EntityFramework.Fundraiser CreateFundraiser(CreateFundraiser fundraiser, string authHeader)
        {
            const string DEFAULT_IMAGE_URL = "https://thsblog.s3.amazonaws.com/wp-content/uploads/2019/10/13222121/how_to_sell_fundraiser_tickets_online.jpg";

            UserProfile profile = GetCurrentUser(authHeader);

            if (profile == null)
                throw new ErrorResponseException("You must be signed in to create a fundraiser.", ErrorResponseCodes.NOT_SIGNED_IN, HttpStatusCode.BadRequest);

            if (String.IsNullOrEmpty(fundraiser.Name))
                throw new ErrorResponseException("You must have a title.", ErrorResponseCodes.FUNDRAISER_NO_TITLE, HttpStatusCode.BadRequest);

            if (String.IsNullOrEmpty(fundraiser.Description))
                throw new ErrorResponseException("You must have a description.", ErrorResponseCodes.FUNDRAISER_NO_DESCRIPTION, HttpStatusCode.BadRequest);

            if (fundraiser.Goal < 10)
                throw new ErrorResponseException("Your fundraiser goal must be at least $10.", ErrorResponseCodes.FUNDRAISER_GOAL_TOO_SMALL, HttpStatusCode.BadRequest);

            fundraiser.ImageUrl = Regex.Replace(fundraiser.ImageUrl, "[^a-zA-Z0-9_\\./\\:\\?\\&=\\+%-]+", "");
            if (String.IsNullOrEmpty(fundraiser.ImageUrl))
                fundraiser.ImageUrl = DEFAULT_IMAGE_URL;

            using (var fdb = new FundraiserProjectContext())
            {
                var dbFundraiser = fdb.Fundraisers.Add(new EntityFramework.Fundraiser
                {
                    UserId = profile.UserId,
                    Name = fundraiser.Name,
                    Description = fundraiser.Description,
                    Goal = fundraiser.Goal,
                    ImageUrl = fundraiser.ImageUrl,
                    DonationTotal = 0
                });

                fdb.SaveChanges();

                return dbFundraiser.Entity;
            }
        }

        public List<Domain.Donation> GetCurrentUserDonations(string authToken)
        {
            UserProfile user = GetCurrentUser(authToken);

            List<Domain.Donation> donations = new();
            using (var fdb = new FundraiserProjectContext())
            {
                donations = (
                    from donation in fdb.Donations
                    where donation.UserId == user.UserId
                    join fundraiser in fdb.Fundraisers on donation.FundraiserId equals fundraiser.Id
                    select new Domain.Donation
                    {
                        Id = donation.Id,
                        FirstName = donation.FirstName,
                        LastName = donation.LastName,
                        Amount = donation.Amount,
                        Note = donation.Note,
                        Date = donation.Date,
                        FundraiserId = fundraiser.Id,
                        FundraiserName = fundraiser.Name
                    }
                ).ToList();
            }

            return donations;
        }
    }
}
