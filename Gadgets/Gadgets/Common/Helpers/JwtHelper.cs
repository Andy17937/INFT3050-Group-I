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
        /// Issuing JWT strings
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModel tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),//User Id
                new Claim("Uname", tokenModel.Uname),
                new Claim("Role", tokenModel.Role),//Role
                new Claim("Project", tokenModel.Project),
                new Claim("UNickname", tokenModel.UNickname),
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(), ClaimValueTypes.Integer64)
            };
            //secret key
            JwtAuthConfig jwtAuthConfig = ConfigHelper.GetConfig<JwtAuthConfig>("JwtAuth");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthConfig.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //expiration date
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
                claims: claims, //declarative set
                expires: dateTime.AddHours(exp),
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return "Bearer " + encodedJwt;
        }

        /// <summary>
        /// Getting a data statement from a token
        /// </summary>
        /// <param name="token">token</param>
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
        /// validate Token
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
                throw new Exception("JwtSettings failed to get");
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
                ValidateLifetime = true,//Whether to validate the Token expiration date, using the current time compared to NotBefore and Expires in the Token's Claims
                ClockSkew = TimeSpan.FromSeconds(30)
                //RequireExpirationTime = true,//expiration date
            };
            return tokenDescriptor;
        }

        /// <summary>
        /// Jwt
        /// </summary>
        public class JwtSettings
        {
            /// <summary>
            /// Who issued the token?
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// The token can be used by those clients
            /// </summary>
            public string Audience { get; set; }

            /// <summary>
            /// Encrypted key
            /// </summary>
            public string SecretKey { get; set; }

            /// <summary>
            /// Token time (minutes)
            /// </summary>
            public int Expire { get; set; } = 1440;

            /// <summary>
            /// Refresh token length
            /// </summary>
            public int RefreshTokenTime { get; set; }

            /// <summary>
            /// Token type
            /// </summary>
            public string TokenType { get; set; } = "Bearer";
        }

        /// <summary>
        /// analyze
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
