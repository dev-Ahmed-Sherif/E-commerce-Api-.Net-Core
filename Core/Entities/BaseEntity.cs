using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity
    {  
        // To make Entity Framework Understand it as Primary Key must Name Id 
        public int Id { get; set; }
    }
}
