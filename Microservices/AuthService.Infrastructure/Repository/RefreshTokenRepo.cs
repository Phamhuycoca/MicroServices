using AuthService.Domain.Entities;
using AuthService.Infrastructure.AppContext;
using AuthService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class RefreshTokenRepo : Repo<refresh_token>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
