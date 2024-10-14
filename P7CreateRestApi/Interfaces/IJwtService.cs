using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Services;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}