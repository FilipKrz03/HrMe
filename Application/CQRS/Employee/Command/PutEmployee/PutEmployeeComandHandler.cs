using AutoMapper;
using Domain.Abstractions;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.PutEmployee
{
    public class PutEmployeeComandHandler : IRequestHandler<PutEmployeeComand, Response<EmployeeResponse>>
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _comapnyRepository;
        private readonly IMapper _mapper;

        public PutEmployeeComandHandler(IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _comapnyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<Response<EmployeeResponse>> Handle(PutEmployeeComand request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse> response = new();

            if (!await _comapnyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            var employee = await _employeeRepository.GetEmployeeAsync(request.EmployeeId, request.CompanyId);

            if (employee == null)
            {
                var employeeWithSameMail = 
                    await _employeeRepository.EmployeExistWithEmailInCompanyAsync(request.Email , request.CompanyId);

                if(employeeWithSameMail)
                {
                    return response.SetError(400, $"Employ with entered email {request.Email} already exist");
                }

                Domain.Entities.Employee createdEmployee
                = _mapper.Map<Domain.Entities.Employee>(request);

                createdEmployee.CompanyId = request.CompanyId;
                createdEmployee.Id = request.EmployeeId;

                await _employeeRepository.InsertEmployee(createdEmployee);

                var employeeToReturn = _mapper.Map<EmployeeResponse>(createdEmployee);

                response.Value = employeeToReturn;

                return response;
            }

            var employeeWithSameMailExist = await _employeeRepository.
                OtherEmployeeExistWithSameMail(request.Email, request.CompanyId, request.EmployeeId);

            if(employeeWithSameMailExist )
            {
                return response.SetError(400, $"You want to change email but " +
                    $"other employee has already this email :  {request.Email}");
            }

            _mapper.Map(request, employee);

            await _employeeRepository.SaveChangesAsync();

            return response;
        }
    }
}
