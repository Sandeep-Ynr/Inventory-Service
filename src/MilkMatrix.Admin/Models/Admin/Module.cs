using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Admin
{
    public class Module
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public int OrderNumber { get; set; }
        public IEnumerable<SubModule>? SubModuleList { get; set; }
    }
}
