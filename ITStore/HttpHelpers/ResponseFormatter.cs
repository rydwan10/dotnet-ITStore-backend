using ITStore.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using static ITStore.Shared.Enums;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;

namespace ITStore.Helpers
{
    public class ResponseFormat
    {
        public int StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
    public class ResponseFormatter
    {
        public static ResponseFormat FormatResponse(int statusCode, string message = null, dynamic data = null)
        {
            ResponseFormat format = new ResponseFormat() {
                StatusCode = statusCode,
                StatusName = ReasonPhrases.GetReasonPhrase(statusCode),
                Message = message,
                Data = data,
            };

            return format;
        }
    }
}
