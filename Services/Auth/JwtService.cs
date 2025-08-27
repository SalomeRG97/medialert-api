using Model;

namespace Service.Auth
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Model.Data;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JwtService
    {
        private readonly MedsAppContext _context;
        private readonly JwtSettings _settings;

        public JwtService(MedsAppContext context, IOptions<JwtSettings> options)
        {
            _context = context;
            _settings = options.Value;
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            // 1. Buscar usuario por email
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            // 2. Validar password (aquí debes usar tu hash real, esto es un ejemplo sencillo)
            if (user.PasswordHash != password) // 🔑 Cambiar a verificación de hash segura
                return null;

            // 3. Obtener roles
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            // 4. Claims básicos
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 5. Generar token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}