using System;
using System.ComponentModel.DataAnnotations;
using static ITStore.Shared.Enums;

namespace ITStore.Helpers
{
    public class MetaValue
    {
        public EnumStatusCodes Status { get; set; }
        public string StatusName
        {
            get
            {
                try
                {
                    return EnumFunction.GetAttribute<DisplayAttribute>(Status).Name;
                }
                catch (Exception)
                {
                    return "Enum display name is not found";
                }
            }
        }
        public string Message { get; set; }
    }
    public class ResponseFormat
    {
        public MetaValue Meta { get; set; }
        public dynamic Data { get; set; }
    }
    public class ResponseFormatter
    {
        public static ResponseFormat FormatResponse(EnumStatusCodes statusCode, string message = null, dynamic data = null)
        {
            ResponseFormat format = new ResponseFormat() {
                Meta = new MetaValue()
                {
                    Status = statusCode,
                    Message = message,
                },
                Data = data,
            };

            return format;
        }
    }
}
