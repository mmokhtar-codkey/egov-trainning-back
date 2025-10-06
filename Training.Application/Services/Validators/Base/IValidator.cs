using System.Threading.Tasks;

namespace Training.Application.Services.Validators.Base
{
    public interface IValidator<in T> where T : class
    {
        Task<(bool, string)> Validate(T entity);
    }
}
