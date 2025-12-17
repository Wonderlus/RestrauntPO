using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CategoryFilterItem
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }
}
