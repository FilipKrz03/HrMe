using Domain.Abstractions;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PropertyMapping
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _employeeMapping =
            new(StringComparer.OrdinalIgnoreCase)
            {
                {"Id" , new(new[]{"Id"}) } ,
                {"FullName" , new(new[]{"FirstName" , "LastName"}) } ,
                {"Position" , new(new[]{"Position"})} ,
                {"Age" , new(new[]{"DateOfBirth"})}
            };

        private Dictionary<string, PropertyMappingValue> _employeeWorkDayMapping =
            new(StringComparer.OrdinalIgnoreCase)
            {
                {"Id" , new(new[]{"Id"}) } ,
                {"WorkDayDate" ,new(new[]{"WorkDayDate"})} ,
                {"StartTime" , new(new[]{"StartTimeInMinutesAfterMidnight"})} ,
                {"EndTime" , new(new[]{"EndTimeInMinutesAfterMidnight"}) } ,
            };

        private Dictionary<string, PropertyMappingValue> _employeePaymentInfoMapping =
            new(StringComparer.OrdinalIgnoreCase)
            {
                {"Id" , new(new[]{"Id"}) } ,
                {"HourlyRateBrutto" , new(new[]{"HourlyRateBrutto"})} ,
                {"StartOfContractDate" , new(new[]{"StartOfContractDate"})},
                {"EndOfContractDate" , new(new[]{"EndOfContractDate"})},
                {"ContractType" , new(new[]{"ContractType"})} ,
            };

        private readonly IList<IPropertyMapping> _propertyMapings
         = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMapings.Add
                (new PropertyMapping<Domain.Entities.Employee, EmployeeResponse>(_employeeMapping));
            _propertyMapings.Add
                (new PropertyMapping<Domain.Entities.EmployeeWorkDay, WorkDayResponse>(_employeeWorkDayMapping));
            _propertyMapings.Add
                (new PropertyMapping<Domain.Entities.EmployeePaymentInfo, PaymentInfoResponse>(_employeePaymentInfoMapping));
        }

        public bool PropertyMappingExist<TSource, TDestination>(string? fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOffWhiteSpace = trimmedField.IndexOf(' ');

                var propertyName = indexOffWhiteSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOffWhiteSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var propertyMapping
                = _propertyMapings.OfType<PropertyMapping<TSource, TDestination>>();

            if (propertyMapping.Count() == 1)
            {
                return propertyMapping.First().MapingDictionary;
            }

            throw new Exception($"Could not find exact maping from " +
                $"{typeof(TSource)} to {typeof(TDestination)}");
        }
    }
}
