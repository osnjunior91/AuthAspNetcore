using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ProductCatalog.Api.Security.Jwt
{
    public class JwtTokenBuilder
    {
        private SecurityKey securityKey = null;
        private string subject = string.Empty;
        private string issuer = string.Empty;
        private string audience = string.Empty;
        private string nameId = string.Empty;
        private List<Claim> claims = new List<Claim>();
        private int expiryInMinutes = 120;
        private int expiryInDays = 365;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        internal JwtTokenBuilder AddClaimsPermission(List<Mock.User.Permission> permissions)
        {
            foreach (Mock.User.Permission item in permissions)
            {
                claims.Add(new Claim(item.Controller, item.Action));
            }
            return this;
        }

        public JwtTokenBuilder AddNameId(string nameId)
        {
            this.nameId = nameId;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            var claim = new Claim(type, value);
            claims.Add(claim);
            return this;
        }

        public JwtTokenBuilder AddClaims(List<Claim> claimsList)
        {
            claims = claimsList;
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtTokenBuilder AddExpiryDays(int expiryInDays)
        {
            this.expiryInDays = expiryInDays;
            return this;
        }
        
        
        public string Build()
        {
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, this.subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim("NameId", this.nameId)
            }.Union(this.claims.Select(item => new Claim(item.Type, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: this.issuer,
                              audience: this.audience,
                              claims: claims,
                              expires: DateTime.Now.AddDays(expiryInDays),
                              signingCredentials: new SigningCredentials(
                                                        this.securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
