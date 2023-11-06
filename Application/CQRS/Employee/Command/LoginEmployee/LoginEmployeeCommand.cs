using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.LoginEmployee
{
    public class LoginEmployeeCommand : IRequest<Response<string>>
    {
        public string Email {  get; set; } = string.Empty;

    }
}
