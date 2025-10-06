using Training.Common.DTO.Identity.Sts;
using System.Threading.Tasks;

namespace Training.Integration.TokenRepository
{
    public interface ITokenRepository
    {
        Task<TokenResponse> GenerateTokenAsync();
    }
}
