using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Common.Token
{
    public class JwtOption : IOptions<JwtOption>
    {
        public string Issuer { get; set; } = "xy";
        public string Audience { get; set; } = "xy";
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(50);
        public string SecurityKey { get; set; } = "qwertyuiopasdfghjklzxcvbnm";
        public string Name { get; set; }
        public string user_name { get; set; }

        public JwtOption Value => this;
    }
}
