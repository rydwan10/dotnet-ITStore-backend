using Microsoft.AspNetCore.Identity;
using System;

namespace ITStore.Domain
{
    public class ApplicationUsers : IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? UserAddressesId { get; set; }

        public virtual UserAddresses UserAddresses { get; set; }
    }
}
