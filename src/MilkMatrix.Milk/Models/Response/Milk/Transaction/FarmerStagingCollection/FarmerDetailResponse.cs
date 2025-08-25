using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerStagingCollection
{
        public class FarmerDetailResponse
        {
            public long DetailId { get; set; }
            public long FarmerId { get; set; }
            public string FarmerName { get; set; }
            public string Mobile { get; set; }
            public decimal? Fat { get; set; }
            public decimal? Snf { get; set; }
            public decimal? LR { get; set; }
            public decimal? WeightLiter { get; set; }
            public string Type { get; set; }
            public decimal? Rtpl { get; set; }
            public decimal? TotalAmount { get; set; }
            public int? SampleId { get; set; }
            public int? Can { get; set; }
            public long? ReferenceId { get; set; }
        }

    }
