using System;
using System.Collections.Generic;
using System.Linq;
using Funx.Test.Api.Controllers.Queries;
using Funx.Test.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Funx.Test.Api.Controllers
{
    
    
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]FindEmployee emp,
            [FromServices] Func<FindEmployee, Option<Employee>> lookupEmployee)
            => lookupEmployee(emp)
                .Match<IActionResult>(BadRequest, Ok);

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> SearchByLastName(string lastname,
            [FromServices] Func<string, IEnumerable<Employee>> findEmployees)
            => findEmployees(lastname).ToArray();
    }

}
