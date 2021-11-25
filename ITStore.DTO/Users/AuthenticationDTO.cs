using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.DTOs.Users
{
    public class AuthenticationDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
