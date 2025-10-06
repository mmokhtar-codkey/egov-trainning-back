using Training.Common.DTO.Hr.Employee;
using System.Threading.Tasks;

namespace Training.Integration.CacheRepository
{
    public interface ICacheRepository
    {
        /// <summary>
        /// Get Employee From Cache By National Id
        /// </summary>
        /// <param name="nationalId"></param>
        /// <returns></returns>
        Task<EmployeeProfileDto> GetEmployeeAsync(string nationalId);
    }
}
