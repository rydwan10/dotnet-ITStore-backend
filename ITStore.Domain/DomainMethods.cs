using System;
using ITStore.Shared;

namespace ITStore.Domain
{
    public static class DomainMethods
    {
        private static DateTime DateGMT7 => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneConverter.TZConvert.GetTimeZoneInfo("SE Asia Standard Time"));

        public static void CreatedBy(this BaseProperties baseProperties, Guid userId)
        {
            baseProperties.CreatedBy = userId;
            baseProperties.CreatedAt = DateGMT7;
            baseProperties.StatusRecord = Constants.StatusRecordInsert;
        }

        public static void ModifiedBy(this BaseProperties baseProperties, Guid userId)
        {
            baseProperties.ModifiedBy = userId;
            baseProperties.ModifiedAt = DateGMT7;
            baseProperties.StatusRecord = Constants.StatusRecordUpdate;
        }

        public static void DeletedBy(this BaseProperties baseProperties, Guid userId)
        {
            baseProperties.ModifiedBy = userId;
            baseProperties.ModifiedAt = DateGMT7;
            baseProperties.StatusRecord = Constants.StatusRecordDelete;
        }
    }
}
