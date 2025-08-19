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
            public const string InsupdSequenceTrans = "usp_sequence_Trans_insupd";
            public const string GetSequenceTransList = "usp_sequence_Trans_list";
            public const string GenNextSeqNoforTrans = "usp_sequence_Trans_generate_next";
            public const string GenClonefornextfyfromold = "usp_sequence_clone_fy";
            public const string sequence_clone_by_head_fy = "usp_sequence_clone_by_head_fy";

        }
    }
}
