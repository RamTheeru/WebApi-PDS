using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class JwtMiddleware
    {

        private readonly RequestDelegate _next;
        //private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
           // _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            context.Items["userToken"] = token;
            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("PennaDeliveryServices");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                var userRole = jwtToken.Claims.FirstOrDefault(x => x.Type.ToLower() == "sub");
                var user = jwtToken.Claims.FirstOrDefault(x => x.Type.ToLower() == "email");
                var userType = jwtToken.Claims.FirstOrDefault(x => x.Type.ToLower() == "typ");
                var empId = jwtToken.Claims.FirstOrDefault(x => x.Type.ToLower() == "nameid");
                // attach user to context on successful jwt validation
                context.Items["userrole"] = userRole;
                context.Items["user"] = user;
                context.Items["usertype"] = userType;
                context.Items["empId"] = empId;
                context.Items["msg"] = null;
            }
            catch(Exception e)
            {
                context.Items["msg"] = e.Message;
                context.Items["userrole"] = null;
                context.Items["user"] = null;
                context.Items["usertype"] = null;
                context.Items["empId"] = null;
               // context.Items["userToken"] = null;
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
