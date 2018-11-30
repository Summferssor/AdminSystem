using AdminSystem.Common.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace AdminSystem.Test_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public TokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        //[Authorize(Policy="Token")]
        public IActionResult RequestToken()
        {
            #region 123


            //if (request != null)
            //{

            //    //验证账号密码,这里只是为了demo，正式场景应该是与DB之类的数据源比对
            //    if ("TokenTest".Equals(request.UserName) && "123456".Equals(request.PassWord))
            //    {
            //        var claims = new[] {
            //            //加入用户的名称
            //            new Claim(ClaimTypes.Name, "TokenTest")
            //        };

            //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnm"));
            //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //        var authTime = DateTime.UtcNow;
            //        var expiresAt = authTime.AddDays(7);

            //        var token = new JwtSecurityToken(

            //            issuer: "xy",
            //            audience: "xy",
            //            claims: claims,
            //            expires: expiresAt,
            //            signingCredentials: creds);

            //        return Ok(new
            //        {
            //            access_token = new JwtSecurityTokenHandler().WriteToken(token),
            //            token_type = "Bearer",
            //            profile = new
            //            {
            //                name = "TokenTset",
            //                auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
            //                expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
            //            }
            //        });
            //    }
            //}

            //return BadRequest("Could not verify username and password.Pls check your information.");
            #endregion

            Dictionary<string, object> payLoad = new Dictionary<string, object>
            {
                { "sub", "rober" },
                { "jti", Guid.NewGuid().ToString() },
                { "nbf", null },
                { "exp", null },
                { "iss", "xy" },
                { "aud", "xy" },
                { "age", 30 },
                { "path", "Token"}
            };

            var encodeJwt = TokenContext.CreateTokenByHandler(payLoad, 30);

            //var result = TokenContext.Validate(encodeJwt, (load) => { return true; });
            return Ok(encodeJwt);
        }

        [HttpPost("Auth")]
        public IActionResult AuthToken([FromHeader] string Authorization)
        {

            var result = TokenContext.Validate(Authorization, (load) => { return true; });
            if (result)
            {
                return Ok("验证成功！！！");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}