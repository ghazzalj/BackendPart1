using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Exceptions
{
    public class SuccessResponseContent<T>
    {
        public string StatusMessage = ResponseContentStatusMessages.Success;
        public T ResultData { get; set; }
    }

    public class FailedResponseContent
    {
        public string StatusMessage { get; set; }
        public Exception Error { get; set; }
    }

    public static class ResponseContentStatusMessages
    {
        public const string Success = "Success";
        public const string ExceptionEncounter = "ExceptionEncounter";
        public const string ResourceNotFound = "ResourceNotFound";
        public const string ResourceConflict = "ResourceConflict";
        public const string ValidationFailed = "ValidationFailed";
    }
}
