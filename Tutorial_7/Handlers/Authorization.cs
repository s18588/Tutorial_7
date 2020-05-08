using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tutorial_5and6.Handlers
{
    public class Authorization : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public Authorization(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {


        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing authorization Header");
            }

            var autHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var creadentialBytes = Convert.FromBase64String(autHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(creadentialBytes).Split(":");
            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Incorrect authorization Header");

            }

            //check the credentials in DB

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Jan"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}