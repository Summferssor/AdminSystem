using AdminSystem.Common.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static AdminSystem.Common.Token.TokenContext;

namespace AdminSystem.Common.Tokens
{
    public class JwtCustomerAuthorizeMiddleware
    {
        private readonly RequestDelegate next;
        public JwtCustomerAuthorizeMiddleware(RequestDelegate next, string secret)
        {
            #region   设置自定义jwt 的秘钥
            if (!string.IsNullOrEmpty(secret))
            {
                securityKey = secret;
            }
            #endregion
            this.next = next;
            //UserContext.AllowAnonymousPathList.AddRange(anonymousPathList);
        }

        public async Task Invoke(HttpContext context, IOptions<JwtOption> optionContainer)
        {
            //if (userContext.IsAllowAnonymous(context.Request.Path))
            //{
            //    await next(context);
            //    return;
            //}

            var option = optionContainer.Value;

            #region 身份验证，并设置用户Ruser值
            var path = "";
            var result = context.Request.Headers.TryGetValue("Authorization", out StringValues authStr);
            if (!result || string.IsNullOrEmpty(authStr.ToString()))
            {
                throw new UnauthorizedAccessException("未授权");
            }
            result = Validate(authStr.ToString().Substring("Bearer ".Length).Trim(), (Dictionary<string, object> payLoad) =>
            {
                var success = true;
                //可以添加一些自定义验证，用法参照测试用例
                //验证是否包含aud 并等于 roberAudience
                success = success && payLoad["aud"]?.ToString() == option.Audience;
                if (success)
                {
                    path = payLoad["path"]?.ToString();
                    //设置Ruse值,把user信息放在payLoad中，（在获取jwt的时候把当前用户存放在payLoad的ruser键中）
                    //如果用户信息比较多，建议放在缓存中，payLoad中存放缓存的Key值
                    
                }
                return success;
            });
            if (!result)
            {
                throw new UnauthorizedAccessException("未授权");
            }

            #endregion
            #region 权限验证
            //context.Request.Path
            if (!"Token".Equals(path))
            {
                throw new UnauthorizedAccessException("未授权");
            }
            #endregion

            await next(context);
        }
    }
}
