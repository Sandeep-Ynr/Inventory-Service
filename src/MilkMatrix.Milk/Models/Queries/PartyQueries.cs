using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public static class PartyQueries
    {
        public const string AddParty = "usp_party_insupd_new";
        public const string GetPartyList = "usp_party_list";

        public const string AddPartyGroup = "usp_partygroup_insupd";
        public const string GetPartyGroupList = "usp_partygroup_list";
    }
}
