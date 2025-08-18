using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public class SequenceQueries
    {
        public static class SequenceQuery
        {
            public const string GetSequenceList = "usp_sequence_list_new";
            public const string InsupdSequence = "usp_sequence_insupd";
            public const string GenNextSeqNo = "usp_sequence_generate_next";
        }
    }
}
