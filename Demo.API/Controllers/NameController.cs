using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NameController : ControllerBase
    {
        [HttpPost("reverse")]
        public IActionResult InsertName(string firstname, string lastname)
        {
            if (string.IsNullOrEmpty(firstname))
                return BadRequest("First Name Is Empty");

            if (string.IsNullOrEmpty(lastname))
                return BadRequest("Last Name Is Empty");

            char[] charArray = firstname.ToCharArray();
            Array.Reverse(charArray);
            firstname = new string(charArray);

            charArray = lastname.ToCharArray();
            Array.Reverse(charArray);
            lastname = new string(charArray);

            return Ok(new
            {
                FirstName = firstname,
                LastName = lastname,
            });
        }

        [HttpPost("isPalindrome")]
        public IActionResult GetName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                return BadRequest("First Name Is Empty");

            bool isPalindrome = IsPalindrome(firstName);

            return Ok(new
            {
                FirstName = firstName,
                IsPalindrome = isPalindrome
            });
        }

        private bool IsPalindrome(string input)
        {
            string reversed = new string(input.Reverse().ToArray());
            return string.Equals(input, reversed, StringComparison.OrdinalIgnoreCase);
        }
    }
}
