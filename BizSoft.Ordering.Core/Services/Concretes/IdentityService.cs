using System;
using BizSoft.Ordering.Core.Services.Abstracts;
using Microsoft.AspNetCore.Http;

namespace BizSoft.Ordering.Core.Services.Concretes
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetUserIdentity()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
        }
    }
}
