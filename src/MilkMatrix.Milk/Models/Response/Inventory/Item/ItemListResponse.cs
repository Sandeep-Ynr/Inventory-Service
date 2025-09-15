using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Inventory.Item
{
    public class ItemListResponse
    {
            public long ItemId { get; set; }
            public int BusinessId { get; set; }      
            public int CategoryId { get; set; }      
            public string? ItemCode { get; set; }   
            public string? ItemName { get; set; }   
            public string? Brand { get; set; }       
            public decimal? Mrp { get; set; }     
            public decimal? SaleRate { get; set; }    
            public bool IsActive { get; set; }        
            public int? CreatedBy { get; set; }    
        }
}
