using JwtAuthentication.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace JwtAuthentication
{
    // interface RefreshTokenGenerator
    public interface RefreshTokenGenerator
    {
        string GeneratorToken(string username);
    }

    
    // Class inheriting RefreshTokenGenerator
    public class RefreshToken : RefreshTokenGenerator
    {
        private readonly CustomerDbContext _context;

        public RefreshToken(CustomerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GeneratorToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshTokenn = Convert.ToBase64String(randomnumber);

                // For checking is refresh token is there if not insert one
                var _user = _context.TblRefreshtokenn.FirstOrDefault(o => o.UserId == username);
                if (_user != null)
                {
                    _user.RefreshToken = RefreshTokenn;
                    _context.SaveChanges(); 
                }
                else
                {
                    TblRefreshtoken tblRefreshToken = new TblRefreshtoken()
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = RefreshTokenn,
                        IsActive = true
                    };
                }

                return RefreshTokenn;
            }
        }
    }
}
