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
        public const string ValidateOtp = "usp_Validate_Otp";//To validate otp;
        public const string PasswordChangeRequest = "usp_User_Password_Change_Request"; //To insert password change request
        public const string VerifyPasswordChange = "usp_User_Varify_Password_Change"; //To validate and update user password
        public const string UserChangePassword = "usp_User_Change_Password";//To Change User Password
        public const string GetFinancialYear = "usp_financial_year"; //To get financial year details
        public const string GetActions = "usp_action_manager"; //For Actions List
    }

    public static class UserSpName
    {
        public const string UserUpsert = "usp_user_registration_insupd"; //For User Insert/Update
        public const string GetUsers = "usp_get_user_details"; //For getting users
        public const string UserProfileUpdate = "usp_user_profile_upd"; //For User profile Update
    }

    public static class BusinessSpName
    {
        public const string BusinessUpsert = "usp_business_details_insupd"; //For Business Insert/Update
        public const string GetBusinessDetails = "usp_business_details"; //For getting business lists
        public const string GetBusinessData = "usp_business_data"; //For getting business data
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

    public static class ModuleSpName
    {
        public const string ModuleUpsert = "usp_module_manager_insupd"; //For Role insert/update
        public const string GetModules = "usp_module_manager"; //For getting roles
    }

    public static class SubModuleSpName
    {
        public const string SubModuleUpsert = "usp_sub_module_manager_insupd"; //For Role insert/update
        public const string GetSubModules = "usp_sub_module_manager"; //For getting roles
    }

    public static class ConfigurationSettingSpName
    {
        public const string ConfigurationUpsert = "usp_configurations_insupd"; //For Configuration Insert
        public const string GetConfigurationSettings = "usp_configurations_list"; //For getting configuration settings
        public const string SmtpSettingsUpsert = "usp_mail_smtp_insupd"; //For Smtp settings Insert update
        public const string GetSmtpSettings = "usp_mail_smtp_list"; //For getting smtp settings
        public const string GetBlockedMobiles = "usp_blocked_mobiles"; // For getting blocked mobiles
        public const string BlockedMobilesUpsert = "usp_blocked_mobile_insupd"; //For blocked mobiles insert/update
        public const string SmsSettingsUpsert = "usp_sms_control_insupd"; //For Sms settings Insert update
        public const string GetSmsSettings = "usp_sms_control_list"; //For getting sms settings
    }

    public static class ApprovalSpName
    {
        public const string ApprovalUpsert = "usp_approval_level_insupd"; //For Approval Level Insert/Update
        public const string GetApprovalLevels = "usp_get_approval_level"; //For getting approval levels
        public const string ApprovalDetailsUpsert = "usp_approval_details_insupd"; //For Approval details Insert/Update
        public const string GetApprovalDetails = "usp_get_approval_details"; //For getting approval details
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
