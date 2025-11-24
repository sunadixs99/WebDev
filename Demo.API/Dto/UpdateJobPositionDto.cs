using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Dto
{
    public class UpdateJobPositionDto
    {
        public string name { get; init; } = string.Empty;
        public decimal beginning_salary { get; init; }
    }
}