using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
    
        // for Add 
        void RefreshToken(RefreshToken refreshToken);
        RefreshToken GetRefreshToken(string refreshToken);
        void Update(RefreshToken refreshToken);

    }
}
