using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using backend.Config;
using backend.Core.Modules.User;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly string _secretKey;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        _secretKey = jwtSettings.SecretKey;
        _expirationMinutes = jwtSettings.ExpirationMinutes;
    }

    public string GenerateToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("User", JsonSerializer.Serialize(user))
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ExtractUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, GetValidationParameters(), out _);
            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }

    public string? ExtractSubject(string token)
    {
        var principal = ExtractUserFromToken(token);
        return principal?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
    }

    public DateTime? ExtractExpirationDate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return jwtToken?.ValidTo;
    }

    // ✅ 5. Validate Token
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, GetValidationParameters(), out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string? ExtractRole(string token)
    {
        var principal = ExtractUserFromToken(token);
        return principal?.FindFirst(ClaimTypes.Role)?.Value;
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
        };
    }

    public UserDTO? ExtractUser(string token)
    {
        var principal = ExtractUserFromToken(token);
        var userClaim = principal?.FindFirst("User")?.Value;
        if (userClaim != null)
        {
            return JsonSerializer.Deserialize<UserDTO>(userClaim);
        }
        return null;
    }
}
