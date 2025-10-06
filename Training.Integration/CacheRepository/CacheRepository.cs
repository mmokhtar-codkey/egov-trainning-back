using Training.Common.Caching.Redis;
using Training.Common.DTO.Hr.Employee;
using System.Threading.Tasks;

namespace Training.Integration.CacheRepository
{
    public class CacheRepository : ICacheRepository
    {
        public async Task<EmployeeProfileDto> GetEmployeeAsync(string nationalId)
        {
            var employee = RedisCacheHelper.GetT<EmployeeProfileDto>(nationalId);
            return await Task.FromResult(employee);
        }
    }
}
