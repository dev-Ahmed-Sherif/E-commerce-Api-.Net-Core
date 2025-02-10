using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Base
    {
        // To make Entity Framework Understand it as Primary Key must Name Id 
        public int Id { get; set; }
    }
}
