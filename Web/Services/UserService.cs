﻿using Domain.Abstractions;
using System.Security.Claims;

namespace Web.Services
{
    public class UserService : IUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCompanyId()
        {
            Guid result = Guid.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                result =
                    Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid));
            }

            return result;
        }

        public Guid GetEmployeeId()
        {
            Guid result = Guid.Empty;

            if (_httpContextAccessor.HttpContext is not null)
            {
                result =
                    Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Anonymous));
            }

            return result;
        }
    }
}
