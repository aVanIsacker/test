using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        Employee GetEmployee(Guid employeeId, bool trackChanges);
        void DeleteEmployee(Employee employee);
        void CreateEmployeeForCompany(Guid companyId, Employee employee); //je moet onmiddellijk zeggen waar je het moet plaatsen
    }
}
