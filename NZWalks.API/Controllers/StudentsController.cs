using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {


      //GET:  https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {

            string[] StudentsName = new string[] { "Jhon", "Jane", "Mark", "Emily", "David" };

            return Ok(StudentsName);

        }

    }
}
