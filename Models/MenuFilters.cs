using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MenuFilters
    {
        public List<string> Categories { get; set; } = new();

        public bool OnlySeasonal { get; set; }

        public bool OnlyPromotional { get; set; }
    }
}
