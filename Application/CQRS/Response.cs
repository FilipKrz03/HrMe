using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS
{
    public class Response<TValue>
    {
        public bool IsError { get; set; } = false;
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TValue? Value { get; set; }

        public Response<TValue> SetError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            IsError = true;

            return this;
        }
    }
}
