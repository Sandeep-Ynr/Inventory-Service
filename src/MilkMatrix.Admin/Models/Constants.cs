using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models
{
    public static partial class Constants
    {
        public static class AuthSpName
        {
            public const string UserLogin = "usp_User_login"; //For User Login
            public const string LoginUserDetails = "usp_User_Details"; //For User Login
            public const string ValidateToken = "usp_validate_token";//To validating sekKey
            public const string ValidateRefreshToken = "usp_validate_refresh_token";//To validate secKey and refreshToken
        }

        public static class AutoMapper
        {
            public const string HostName = "HostName";
            public const string PrivateIp = "PrivateIp";
            public const string PublicIp = "PublicIp";
            public const string UserAgent = "UserAgent";
            public const string IsLoginWithOtp = "IsLoginWithOtp";
            public const string LoginId = "loginId";
            public const string ModifyUser = "modifyUserId";
            public const string ActionType = "actionType";
            public const string TermsFileId = "TermsFileId";
            public const string ProductsFileId = "ProductsFileId";
            public const string MiscFileId = "MiscFileId";
            public const string TechFileId = "TechFileId";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedBy = "ModifiedBy";
            public const string ApprovedOn = "ApprovedOn";
        }
    }
}
