using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.IRepository
{
    public interface IRefreshTokenRepo:IRepo<refresh_token>
    {
    }
}
