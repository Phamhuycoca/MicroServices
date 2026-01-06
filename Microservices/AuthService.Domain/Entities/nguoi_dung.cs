using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class nguoi_dung : IdentityUser<Guid>
    {
        public string? ho { get; set; }
        public string? ten { get; set; }
        public string? ho_ten { get; set; }
        public Guid? Session_id { get; set; }
        public ICollection<refresh_token> refresh_Tokens { get; set; }
    }
}
