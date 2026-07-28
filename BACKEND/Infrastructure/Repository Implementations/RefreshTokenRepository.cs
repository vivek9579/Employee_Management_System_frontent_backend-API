using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository_Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ManagementDbContext _context;

        public RefreshTokenRepository(ManagementDbContext context)
        {
            _context = context;
        }
        public RefreshToken GetRefreshToken(string refreshToken)
        {
            return _context.RefereshTokens.FirstOrDefault(x => x.ReToken == refreshToken);
        }

        public void RefreshToken(RefreshToken refreshToken)
        {
            _context.RefereshTokens.Add(refreshToken);
            _context.SaveChanges();
        }

        public void Update(RefreshToken refreshToken)
        {
            _context.RefereshTokens.Update(refreshToken);
            _context.SaveChanges();
        }
    }
}
