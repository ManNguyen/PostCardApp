using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using WebAuthenticatePostCard.Services;

//Custom made Authentication Filter to filter out request that does not have valid token
namespace WebAuthenticatePostCard.Controllers
{
    public class TokenAuthenticateFilterAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = context.Request.Headers.Authorization;
            // If there are no credentials, set the error result.
            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }
            // If there are credentials but the filter does not recognize the Scheme, set the error result.
            if (authorization.Scheme != "Basic")
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }
            // If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }
            //Call the token service to check the token string
            var genericPrincipal = TokenService.GetPrincipleFromToken(authorization.Parameter);
            if (genericPrincipal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }
            // If the credentials are valid, set principal.
            else
            {
                context.Principal = genericPrincipal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}