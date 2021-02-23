using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                            c => c.MapFrom(c => c.Address + " " + c.Country));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>(); //deze is voor automappen p 82.    --> van naar, dus volgorde is hier omgekeer

            CreateMap<EmployeeForCreationDto, Employee>();

            //update
            CreateMap<EmployeeForUpdateDto, Employee>();  //ook al is het voor een update --> hier schrijf je toch CreateMap
            CreateMap<CompanyForUpdateDto, Company>();
        }
    }
}
