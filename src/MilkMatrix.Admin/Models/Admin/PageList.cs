using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Admin
{
    public class PageList
    {
        public string? ActionId { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public string? PageName { get; set; }
        public string? PageURL { get; set; }
        public string? PageIcon { get; set; }
        public int PageOrder { get; set; }
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleIcon { get; set; }
        public int ModuleOrderNumber { get; set; }
        public int SubModuleId { get; set; }
        public string? SubModuleName { get; set; }
        public int SubModuleOrderNumber { get; set; }
        public IEnumerable<Actions>? ActionList { get; set; }
    }
}
