namespace MilkMatrix.Admin.Models;

public static partial class Constants
{
    public static class AuthSpName
    {
        public const string UserLogin = "usp_User_login"; //For User Login
        public const string LoginUserDetails = "usp_User_Details"; //For User Login
        public const string ValidateToken = "usp_validate_token";//To validating sekKey
        public const string ValidateRefreshToken = "usp_validate_refresh_token";//To validate secKey and refreshToken
        public const string GetUserIdFromEmailId = "usp_user_with_email_id"; //For getting userid using email or mobilenumber
        public const string GetCommonDetails = "usp_user_common_details";
        public const string RoleList = "usp_User_RoleList";//To retriving user role list
        public const string PageMenuList = "usp_user_page_menu_list";//To retriving user menu list
        public const string ActionList = "usp_User_ActionList";//To retriving user acton list
        public const string GetUserId = "usp_get_user_id";
    }

    public static class UserSpName
    {
        public const string UserUpsert = "usp_user_registration_insupd"; //For User Insert/Update
        public const string GetUsers = "usp_get_user_details"; //For getting users
    }

    public static class RoleSpName
    {
        public const string RoleUpsert = "usp_role_manager_insupd"; //For Role insert/update
        public const string GetRoles = "usp_role_manager"; //For getting roles
    }

    public static class PageSpName
    {
        public const string PageUpsert = "usp_page_manager_insupd"; //For page insert/update
        public const string GetPages = "usp_page_manager"; //For getting pages
    }

    public static class RolePageSpName
    {
        public const string RolePageUpsert = "usp_page_role_permission_manager_ins"; //For rolepage insert/update
        public const string GetRolePages = "usp_page_role_permission_manager"; //For getting rolepages
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
