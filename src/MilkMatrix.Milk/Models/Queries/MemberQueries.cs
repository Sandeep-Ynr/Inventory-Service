using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public static class MemberQueries
    {
        public const string AddOrUpdateMember = "usp_member_insupd";
        public const string GetMemberList = "usp_member_list";


        // Bank Details Queries
        public const string AddOrUpdateMemberBankDetails = "usp_member_bankdetails_insupd";
        public const string GetMemberBankDetailsList = "usp_member_bankdetails_list"; 

        // Milk Profile Queriesusp_member_bankdetails_list
        public const string AddOrUpdateMemberMilkProfile = "usp_member_milkprofile_insupd"; 
        public const string GetMemberMilkProfileList = "usp_member_milkprofile_list"; 

        // Member Documents Queries
        public const string AddOrUpdateMemberDocuments = "usp_memberdocuments_insupd"; 
        public const string GetMemberDocumentsList = "usp_memberdocuments_list"; 
        public const string GetMemberDocumentsById = "usp_memberdocuments_getbyid";
        public const string DeleteMemberDocuments = "usp_memberdocuments_delete"; 

        // Member Address Queries

        public const string AddOrUpdateMemberAdress = "usp_member_address_insupd";
        public const string AddOrUpdateMemberList = "usp_member_address_list";

    }
}
