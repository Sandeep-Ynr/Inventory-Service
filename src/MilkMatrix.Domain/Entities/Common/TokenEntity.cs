namespace MilkMatrix.Domain.Entities.Common
{
    public class TokenEntity
    {
        public string? token { get; set; }

        public string? RefreshToken { get; set; }
        public string? userID { get; set; }
        public string? mobile { get; set; }
        public string? hostName { get; set; }
        public string? appType { get; set; }
    }
    public class ValidateTokenReq
    {
        public int ActionType { get; set; }
        public string? token { get; set; }
    }
}
