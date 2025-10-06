using Training.Common.Core;
using Training.Common.DTO.Hr.Unit;
using Training.Common.DTO.Identity.Role;
using Training.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Hr.Employee
{
    [ExcludeFromCodeCoverage]
    public class EmployeeProfileDto : IEntityDto<Guid?>
    {
        public Guid? Id { get; set; }

        public string PhoneNumber { get; set; }

        public string IpPhone { get; set; }

        public string Position { get; set; }

        public string FullNameEn { get; set; }

        public string FullNameAr { get; set; }

        public string NationalId { get; set; }

        public string Email { get; set; }

        public Gender? Gender { get; set; }

        public string UnitId { get; set; }

        public Guid ManagerId { get; set; }

        public long? EmployeeTypeId { get; set; }

        public bool? IsGovernmental { get; set; }

        public bool IsManager { get; set; }

        public bool IsRetired { get; set; }

        public bool IsTeamManager { get; set; }

        public long? TeamId { get; set; }

        public UnitDto Unit { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public long? GradeId { get; set; }

        public long? NationalityId { get; set; }

        public DateTime? HireDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool IsUpdated { get; set; }

        public List<RoleDto> Roles { get; set; } = new();
    }
}