using Akaru.Domain.Entities;

namespace Akaru.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario);
}
