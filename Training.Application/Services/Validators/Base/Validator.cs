using Training.Common.Infrastructure.UnitOfWork;
using System.Threading.Tasks;

namespace Training.Application.Services.Validators.Base
{
    public class Validator<T> : IValidator<T> where T : class
    {
        protected readonly IUnitOfWork<T> UnitOfWork;

        protected internal Validator(IUnitOfWork<T> unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public virtual async Task<(bool, string)> Validate(T entity)
        {
            return await Task.FromResult((false, string.Empty));
        }
    }
}
