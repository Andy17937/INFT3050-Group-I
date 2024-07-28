using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gadgets.Common.Helpers;
using Gadgets.Models;
using Gadgets.Models.ConfModel;
using NetTaste;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gadgets.Common.Helpers
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModel tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),//用户Id
                new Claim("Uname", tokenModel.Uname),
                new Claim("Role", tokenModel.Role),//身份
                new Claim("Project", tokenModel.Project),
                new Claim("UNickname", tokenModel.UNickname),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(), ClaimValueTypes.Integer64)
            };
            //秘钥
            JwtAuthConfig jwtAuthConfig = ConfigHelper.GetConfig<JwtAuthConfig>("JwtAuth");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthConfig.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //过期时间
            double exp = 0;
            switch (tokenModel.TokenType)
            {
                case "Web":
                    exp = jwtAuthConfig.WebExp;
                    break;
                case "App":
                    exp = jwtAuthConfig.AppExp;
                    break;
                case "MiniProgram":
                    exp = jwtAuthConfig.MiniProgramExp;
                    break;
                case "Other":
                    exp = jwtAuthConfig.OtherExp;
                    break;
            }
            var jwt = new JwtSecurityToken(
                issuer: jwtAuthConfig.Issuer,
                claims: claims, //声明集合
                expires: dateTime.AddHours(exp),
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return "Bearer " + encodedJwt;
        }

        /// <summary>
        /// 从令牌中获取数据声明
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public static JwtSecurityToken? ParseToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validateParameter = ValidParameters();
            token = token.Replace("Bearer ", "");
            try
            {

                //var authToken = tokenHandler.ReadToken(token);// as JwtSecurityToken;

                //tokenHandler.ValidateToken(token, validateParameter, out SecurityToken validatedToken);

                //string Base64Decode(string value)
                //{
                //    return Encoding.Default.GetString(Convert.FromBase64String(value));
                //}

                // Get the first two segments into an enumerable:
                //var pieces = token.Split('.').Take(2);
                //// Pad them with equals signs to a length that is a multiple of four:
                //var paddedPieces = pieces.Select(x => x.PadRight(x.Length + (x.Length % 4), '='));
                //// Base64 decode the pieces:
                //var decodedPieces = paddedPieces.Select(x => Base64Decode(x));
                //// Join it all back into one string with .Aggregate:
                //var aa = decodedPieces;// (decodedPieces.Aggregate((s1, s2) => s1 + Environment.NewLine + s2));

                return tokenHandler.ReadJwtToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return null if validation fails
                return null;
            }

        }


        /// <summary>
        /// 验证Token
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters ValidParameters()
        {
            JwtAuthConfig jwtSettings = new JwtAuthConfig
            {
                AppExp = 1440,
                Issuer = "Gadgets",
                Audience = "MyAudience",
                SecurityKey = "SecretKey-huawei-20240707-123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                MiniProgramExp = 1440,
                OtherExp = 1,
                WebExp = 12
            };

            //          "JwtAuth": {
            //              "Issuer": "Gadgets",
            //  "Audience": "MyAudience",
            //  //"SecurityKey": "0a1b2c3d4e5f6g7h8i9j",
            //  "SecurityKey": "SecretKey-huawei-20240707-123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
            //  "WebExp": 12,
            //  "AppExp": 1440,
            //  "MiniProgramExp": 1440,
            //  "OtherExp": 1
            //}

            if (jwtSettings == null || jwtSettings.SecurityKey.IsNullOrEmpty())
            {
                throw new Exception("JwtSettings获取失败");
            }
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecurityKey);

            var tokenDescriptor = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,//YesNo验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                ClockSkew = TimeSpan.FromSeconds(30)
                //RequireExpirationTime = true,//过期时间
            };
            return tokenDescriptor;
        }

        /// <summary>
        /// Jwt
        /// </summary>
        public class JwtSettings
        {
            /// <summary>
            /// tokenYes谁颁发的
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// token可以给那些客户端使用
            /// </summary>
            public string Audience { get; set; }

            /// <summary>
            /// 加密的key（SecretKey必须大于16个,Yes大于，不Yes大于等于）
            /// </summary>
            public string SecretKey { get; set; }

            /// <summary>
            /// token时间（分）
            /// </summary>
            public int Expire { get; set; } = 1440;

            /// <summary>
            /// 刷新token时长
            /// </summary>
            public int RefreshTokenTime { get; set; }

            /// <summary>
            /// token类型
            /// </summary>
            public string TokenType { get; set; } = "Bearer";
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJWT(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            try
            {
                JwtSecurityToken jwtToken = ParseToken(jwtStr);
                object uname = new object();
                object role = new object();
                object project = new object();
                object uNickname = new object();
                if (jwtToken != null)
                {
                    jwtToken.Payload.TryGetValue("Uname", out uname);
                    jwtToken.Payload.TryGetValue("Role", out role);
                    jwtToken.Payload.TryGetValue("Project", out project);
                    jwtToken.Payload.TryGetValue("UNickname", out uNickname);
                }
                var tm = new TokenModel
                {
                    Uid = jwtToken == null ? 0 : long.Parse(jwtToken.Id),
                    Uname = uname.ToString(),
                    Role = role.ToString(),
                    Project = project.ToString(),
                    UNickname = uNickname.ToString()
                };
                return tm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
