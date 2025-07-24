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
        public const string AddOrUpdateMemberBankDetails = "usp_memberbankdetails_insupd"; // Placeholder
        public const string GetMemberBankDetailsList = "usp_memberbankdetails_list"; // Placeholder

        // Milk Profile Queries
        public const string AddOrUpdateMemberMilkProfile = "usp_membermilkprofile_insupd"; // Placeholder
        public const string GetMemberMilkProfileList = "usp_membermilkprofile_list"; // Placeholder

        // Member Documents Queries
        public const string AddOrUpdateMemberDocuments = "usp_memberdocuments_insupd"; // Placeholder
        public const string GetMemberDocumentsList = "usp_memberdocuments_list"; // Placeholder
        public const string GetMemberDocumentsById = "usp_memberdocuments_getbyid"; // Placeholder
        public const string DeleteMemberDocuments = "usp_memberdocuments_delete"; // Placeholder

        // Member Address Queries
        public static string AddOrUpdateMemberAddress { get; internal set; }
        public static string GetMemberAddressList { get; internal set; }
    }
}
