using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Login.Response
{
    public class TokenResponse
    {
        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Expiration time in seconds
        /// </summary>
        public DateTime? ExpiresIn { get; set; }
        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
