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
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type.ToLower() == "sub");

                // attach user to context on successful jwt validation
                context.Items["userallow"] = userId;
                context.Items["msg"] = null;
            }
            catch(Exception e)
            {
                context.Items["msg"] = e.Message;
                context.Items["userallow"] = null;
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
