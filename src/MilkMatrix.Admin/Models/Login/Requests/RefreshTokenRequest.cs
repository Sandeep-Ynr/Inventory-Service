using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Login.Requests;

public class RefreshTokenRequest
{
    public string EmailId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

