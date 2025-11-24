using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AidsController : ControllerBase
    {
        [HttpPost("LifeExpectancy")]
        public IActionResult GetLifeExpectancy(int age)
        {
            if (age <= 50)
                return Ok("Di pa mamatay");

            return Ok("Malapit ka nang mamatay");
        }

        [HttpPost("CalculateAge")]
        public IActionResult GetAge(int year, int month, int day)
        {

            var birthDate = new DateTime(year, month, day);
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age)) age--;

            return Ok(new
            {
                Age = age
            });



        }
    }
}
