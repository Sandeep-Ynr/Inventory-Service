using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Accounts.HSN
{
    public class HSNResponse 
    {
        public long Id { get; set; }                       // hm.id
        public int Business_Id { get; set; }               // hm.business_id
        public string HSNCode { get; set; }                // hm.HSNCode
        public string Description { get; set; }            // hm.Description

        public decimal? Igst_Rate { get; set; }            // hm.igst_rate
        public decimal? Cgst_Rate { get; set; }            // hm.cgst_rate
        public decimal? Sgst_Rate { get; set; }            // hm.sgst_rate
        public decimal? Cess_Rate { get; set; }            // hm.cess_rate

        public long? Cgst_Input_Ledger_Id { get; set; }    // hm.cgst_input_ledger_id
        public long? Cgst_Output_Ledger_Id { get; set; }   // hm.cgst_output_ledger_id
        public long? Sgst_Input_Ledger_Id { get; set; }    // hm.sgst_input_ledger_id
        public long? Sgst_Output_Ledger_Id { get; set; }   // hm.sgst_output_ledger_id
        public long? Igst_Input_Ledger_Id { get; set; }    // hm.igst_input_ledger_id
        public long? Igst_Output_Ledger_Id { get; set; }   // hm.igst_output_ledger_id
        public long? Cess_Input_Ledger_Id { get; set; }    // hm.cess_input_ledger_id
        public long? Cess_Output_Ledger_Id { get; set; }   // hm.cess_output_ledger_id

        public bool Is_Active { get; set; }                // hm.is_active
        public bool Is_Deleted { get; set; }               // hm.is_deleted

        public DateTime? Wef_Date { get; set; }            // hm.wef_date

        public long Created_By { get; set; }               // hm.created_by
        public DateTime Created_On { get; set; }           // hm.created_on
        public long? Modify_By { get; set; }               // hm.modify_by
        public DateTime? Modify_On { get; set; }           // hm.modify_on


    }

   

}
