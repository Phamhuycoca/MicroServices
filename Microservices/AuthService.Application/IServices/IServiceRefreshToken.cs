using AuthService.Application.DTO;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.IServices
{
    public interface IServiceRefreshToken
    {
        Task<TokenResponseDto> GenerateTokensAsync(nguoi_dung user,string ipAddress);
        Task<RefreshTokenDTO> GetByTokenAsync(RefreshTokenRequest refreshToken);
        Task<RefreshTokenDTO> UpdateToken(RefreshTokenDTO refreshToken);
    }
}
