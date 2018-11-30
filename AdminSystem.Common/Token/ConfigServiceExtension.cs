using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using static AdminSystem.Common.Token.TokenContext;

namespace AdminSystem.Common.Token
{
    public static class ConfigServiceExtension
    {
        
        public static void AddInnerAuthorize(this IServiceCollection services, IConfiguration config)
        {

            services.AddAuthorization(option =>
            {
                //自定义一些策略，原理都是基于申明key和value的值进行比较或者是否有无
                #region 键值对对比的一些验证策略
                option.AddPolicy("onlyxy", policy => policy.RequireClaim("sub", "xy"));
                option.AddPolicy("SuperAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin"));
                //多申明共同,申明中包含aud：rober或者申明中值有等于Rober的都可以通过
                option.AddPolicy("multiClaim", policy => policy.RequireAssertion(context =>
                {
                    return context.User.HasClaim("aud", config["JwtOption:Audience"]); // || context.User.HasClaim(c => c.Value == "ttt")
                }));
                #endregion

                #region 自定义验证策略
                //option.AddPolicy("ageRequire", policy => policy.Requirements.Add(new AgeRequireMent(20)));
                option.AddPolicy("common", policy => policy.Requirements.Add(new CommonAuthorize()));
                #endregion


            })
            .AddAuthentication(option =>
            {
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                if (!string.IsNullOrEmpty(config["JwtOption:SecurityKey"]))
                {
                    securityKey = config["JwtOption:SecurityKey"];
                }
                //设置需要验证的项目
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = config["JwtOption:Audience"],//Audience
                    ValidIssuer = config["JwtOption:Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))//拿到SecurityKey
                };
            });

            //自定义策略IOC添加
            //services.AddSingleton<IAuthorizationHandler, AgeRequireHandler>();
            services.AddSingleton<IAuthorizationHandler, CommonAuthorizeHandler>();
        }
    }
}
