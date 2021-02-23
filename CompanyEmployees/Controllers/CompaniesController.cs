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
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;
        public CompaniesController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _repositoryManager.Company.GetAllCompanies(false);      //als je iets niet nodig hebt --> voor performantieredenen
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            //var companiesDto = companies.Select(c => new CompanyDto
            //{ 
            //    Id = c.Id,
            //    Name = c.Name,
            //    FullAddress = c.Address + " " + c.Country               
            //});
            return Ok(companiesDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _repositoryManager.Company.GetCompany(id, trackChanges: false);
            if (company == null)
            {
                //_logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound(); //404 not found
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return Ok(companyDto); //200 ok statusboodschap
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(Guid id)
        {
            var company = _repositoryManager.Company.GetCompany(id, false); //no trackchanges
            if (company == null)
            {
                return NotFound();
            }
            _repositoryManager.Company.DeleteCompany(company);
            _repositoryManager.Save();
            return NoContent(); //204 alles dat met 2 begint is goed
        }

        //[HttpPost] //via body
        //public IActionResult CreateCompany([FromBody] Company company)
        //{
        //    if (company==null)
        //    {

        //        BadRequest("Company object is invalid");
        //    }

        //    _repositoryManager.Company.CreateCompany(company);
        //    _repositoryManager.Save();
        //    return Ok(company);
        //    //return NoContent();
        //}

        [HttpPost] //via body   //nu met DTO
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto companyDto)
        {
            if (companyDto == null)
            {

                BadRequest("Company object is invalid");
            }
            //To do : companyDto object ozetten naaar een company object
            // 2 manieren mogelijk

            //ofwel manueel code schrijven

            //Company company = new Company()
            //{
            //    Name = companyDto.Name,
            //    Address = companyDto.Address,
            //    Country = companyDto.Country
            //};
            //ofwel met automapper //altijd configuratie nodig die 1 keer wordt aangesproken bij opstarten

            Company company = _mapper.Map<Company>(companyDto); //automapper, dus dit doet hetzelfde als die manuele --> maar ook in mappingprofile nodig
            _repositoryManager.Company.CreateCompany(company);
            _repositoryManager.Save();
            return Ok(company);
            //return NoContent();
        }

        [HttpPut("{companyId}")]
        public IActionResult UpdateCompany(Guid companyId, [FromBody] CompanyForUpdateDto companyDto)
        {
            var company = _repositoryManager.Company.GetCompany(companyId, true);
            if (company == null)
            {
                NotFound(); //404
            }
            //ovwel manueel
            
            //ofwel zelf ofwel met automapper
            _mapper.Map(companyDto, company);
            _repositoryManager.Save();
            return Ok(company);
        }
    }
}
