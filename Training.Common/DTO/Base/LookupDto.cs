using Training.Common.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Base
{
    [ExcludeFromCodeCoverage]
    public class LookupDto<T> : IEntityDto<T>
    {
        public T Id { get; set; }

        [Required]
        public string NameEn { get; set; }

        [Required]
        public string NameAr { get; set; }

        public string Code { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
