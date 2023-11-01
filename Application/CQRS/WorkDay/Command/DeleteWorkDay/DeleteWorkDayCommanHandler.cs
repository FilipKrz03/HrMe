using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.DeleteWorkDay
{
    public class DeleteWorkDayCommanHandler : IRequestHandler<DeleteWorkDayCommand, Response<bool>>
    {

        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkDayReposiotry _workDayReposiotry;

        public DeleteWorkDayCommanHandler(ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IWorkDayReposiotry workDayReposiotry)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _workDayReposiotry = workDayReposiotry;
        }

        public async Task<Response<bool>> Handle(DeleteWorkDayCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find company");
            }

            if (!await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId, request.CompanyId))
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDay = _workDayReposiotry.GetWorkDayAsync(request.WorkDayId, request.EmployeeId);

            if (workDay == null)
            {
                return response.SetError(404, $"We could not find work day with id {request.WorkDayId}");
            }

            response.Value = true;

            return response;
        }
    }
}
