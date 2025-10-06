using Training.Domain.Enum;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Training.Domain.Enum
{
    #region Lookup Enum

    public enum Gender
    {
        [Values("Male", "ذكر", "")]
        Male = 1,
        [Values("Female", "أنثى", "")]
        Female,
        [Values("Both", "الكل", "")]
        Both
    }


    public enum Action
    {
        [Values("Approve", "موافقة", "APPROVE")]
        Approve = 1,
        [Values("Reject", "رفض", "REJECT")]
        Reject

    }
    public enum Status
    {
        [Values("Approved", "مقبول", "APPROVED")]
        Approved = 1,
        [Values("Rejected", "مرفوض", "REJECTED")]
        Rejected,
        [Values("Pending", "معلق", "PENDING")]
        Pending
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }


    #endregion


    #region Common Enum

    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
    public enum UnitType
    {
        Sector = 1,
        Directorate,
        Department,
        Section,
        Team
    }
    public enum MaritalStatus
    {
        Single = 1,
        Married,
        Divorced,
        Widow
    }

    public enum PublicationStatus
    {
        [Values("Draft", "محفوظ", "DRAFT")]
        Draft = 1,
        [Values("Published", "تم النشر", "PUBLISHED")]
        Published,
        [Values("Archived", "ارشيف", "ARCHIVED")]
        Archived
    }

    public enum PageType
    {
        [Values("Default", "افتراضي", "DEFAULT")]
        Default = 1,
        [Values("Custome", "متغير", "CUSTOME")]
        Custome
    }

    #endregion

    [ExcludeFromCodeCoverage]
    internal class Values : Attribute
    {
        public string NameEn;
        public string NameAr;
        public string Code;
        public Values(string nameEn, string nameAr, string code)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            Code = code;
        }
    }
}
[ExcludeFromCodeCoverage]
public static class Extensions
{
    public static ActionResult GetActionName(this System.Enum e)
    {
        var type = e.GetType();

        var memInfo = type.GetMember(e.ToString());

        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(Values), false);
            if (attrs.Length > 0)
            {
                var attributes = (Values)attrs[0];
                return new ActionResult
                {
                    Id = Convert.ToInt32(e),
                    NameEn = attributes.NameEn,
                    NameAr = attributes.NameAr,
                    Code = attributes.Code
                };
            }
        }

        throw new ArgumentException("Name " + e + " has no Name defined!");
    }
    public static EnumResult GetName(this Enum e)
    {
        var type = e.GetType();

        var memInfo = type.GetMember(e.ToString());

        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(Values), false);
            if (attrs.Length > 0)
            {
                var attributes = (Values)attrs[0];
                return new EnumResult
                {
                    Id = Convert.ToInt32(e),
                    NameEn = attributes.NameEn,
                    NameAr = attributes.NameAr,
                    Code = attributes.Code
                };
            }
        }

        throw new ArgumentException("Name " + e + " has no Name defined!");
    }
}

[ExcludeFromCodeCoverage]
public class EnumResult
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string Code { get; set; }
}

[ExcludeFromCodeCoverage]
public class ActionResult
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string Code { get; set; }
}

