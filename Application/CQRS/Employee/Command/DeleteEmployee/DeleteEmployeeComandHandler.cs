using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.DeleteEmployee
{
    public class DeleteEmployeeComandHandler : IRequestHandler<DeleteEmployeeCommand, Response<bool>>
    {
        private readonly IEmployeeRepository _employeeRepostiory;
        private readonly ICompanyRepository _companyRepository;
        
        public DeleteEmployeeComandHandler(IEmployeeRepository employeeRepository
            , ICompanyRepository companyRepository)
        {
            _employeeRepostiory = employeeRepository;
            _companyRepository = companyRepository;
        }
        public async Task<Response<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new();

            if(! await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(500, "We could not find company");
            }

            var employe = await _employeeRepostiory.GetEmployeeAsync(request.EmployeeId , request.CompanyId);

            if(employe == null)
            {
                return response.SetError(404 , $"We could not find employee with id : {request.EmployeeId}");
            }

            await _employeeRepostiory.DeleteEmployee(employe);

            response.Value = true;

            return response;
        }
    }
}
