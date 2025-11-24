using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Models
{
    public class JobPosition
    {
        public int Id { get; init; }
        public string name { get; init; } = string.Empty;
        public decimal beginning_salary { get; init; }
        
    }
}