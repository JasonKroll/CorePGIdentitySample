using System;
using Microsoft.AspNetCore.Authorization;

namespace CorePGIdentityTest.Security
{

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public List<string> Scopes { get; } = new List<string>();

        public HasScopeRequirement(string scope, string issuer)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            Scopes.Add(scope);
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }

        public HasScopeRequirement(List<string> scopes, string issuer)
        {
            Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}

