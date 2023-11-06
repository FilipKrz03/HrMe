using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.LoginEmployee
{
    public class LoginEmployeeComandHandler : IRequestHandler<LoginEmployeeCommand, Response<string>>
    {

        private IEmployeeRepository _employeeRepository;
        private IJwtProvider _jwtProvider;

        public LoginEmployeeComandHandler(IEmployeeRepository employeeRepository , 
            IJwtProvider jwtProvider)
        {
            _employeeRepository = employeeRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Response<string>> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
        {
            Response<string> response = new();

            var employee = await _employeeRepository.GetEmployeeByEmial(request.Email);

            if(employee == null)
            {
                return response.SetError(404, $"Could not find employee with email {request.Email}");
            }

            string token = _jwtProvider.Generate(request.Email, employee.CompanyId , false);

            response.Value = token;

            return response;
        }
    }
}
