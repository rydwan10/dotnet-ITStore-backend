using System;
using Microsoft.AspNetCore.Http;

namespace ITStore.Services;
using Microsoft.Net.Http;

public class BaseService
{
    protected Guid UserId { get; }

    protected BaseService(IHttpContextAccessor httpContextAccessor)
    {
        var claimsIdentity = httpContextAccessor.HttpContext.User;
        UserId = claimsIdentity.FindFirst("userId") != null
            ? new Guid(claimsIdentity.FindFirst("userId").Value)
            : Guid.Empty;
    }
}