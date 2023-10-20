using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class ExceptionHttpCode
    {
        public static HttpStatusCode GetHttpCodeFormException(Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ValidationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case DbUpdateException:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            return code;
        }
    }
}

