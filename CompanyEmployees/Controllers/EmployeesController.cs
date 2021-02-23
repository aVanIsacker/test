using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;
        public EmployeesController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _repositoryManager.Employee.GetAllEmployees(false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            //var employeesDto = employees.Select(c => new EmployeeDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Age = c.Age
            //});
            return Ok(employeesDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetEmployee(Guid id)
        {
            var employee = _repositoryManager.Employee.GetEmployee(id, trackChanges: false);
            if (employee == null)
            {
                //_logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound(); //404 not found
            }
            else
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return Ok(employeeDto); //200 ok statusboodschap
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = _repositoryManager.Employee.GetEmployee(id, false); //no trackchanges
            if (employee == null)
            {
                return NotFound();
            }
            _repositoryManager.Employee.DeleteEmployee(employee);
            _repositoryManager.Save();
            return NoContent();
        }
        [HttpPost] //via body
        public IActionResult CreateEmployeeForCOmpany(Guid companyId, [FromBody] EmployeeForCreationDto employeeDto)
        {
            if (employeeDto == null)
            {

                BadRequest("employee object is invalid");
            }

            var company = _repositoryManager.Company.GetCompany(companyId, false);
            if (companyId == null)
            {
                return NotFound();
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employee);
            _repositoryManager.Save();
            return Ok(employeeDto);
            //return NoContent();
        }

        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid employeeId, [FromBody] EmployeeForUpdateDto employeeDto)
        {
            if (employeeDto==null)
            {
                BadRequest("EmployeeForUpdateDto is empty");
            }
            var employee = _repositoryManager.Employee.GetEmployee(employeeId, true); //moet op true staan
            if (employee == null)
            {
                NotFound(); //404
            }
            //ovwel manueel
            //employee.Name = employee.Name;
            //employee.Age = employeeDto.Age;
            //employee.Position = employeeDto.Position;
            //ofwel zelf ofwel met automapper
            _mapper.Map(employeeDto, employee);
            _repositoryManager.Save();
            return Ok(employee);
        }
    }
}
