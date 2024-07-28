using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Gadgets.Common.Extensions;
using Gadgets.Common.Filters;
using Gadgets.Common.Helpers;
using Gadgets.Models.ConfModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.IdentityModel.Logging;

namespace Gadgets
{
    public class DatetimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    return date;
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ConfigHelper._configuration = Configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //  Endpoint Routing does not support 'IApplicationBuilder.UseMvc(...)'. To use 'IApplicationBuilder.UseMvc' set 'MvcOptions.EnableEndpointRouting = false' inside 'ConfigureServices
            services.AddMvc(opt =>
            {
                

            }).AddJsonOptions(options =>
            {
                //格式化日期时间格式，需要自己创建指定的转换类DatetimeJsonConverter
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //数据格式首字母小写
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //JsonNamingPolicy.CamelCase首字母小写（Default）,null则为不改变大小写
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //CancelUnicode编码
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
                //忽略空Value
                //options.JsonSerializerOptions.IgnoreNullValues = true;
                //允许额外符号
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性NameYesNo使用不区分大小写的比较
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddRouting(options => options.LowercaseUrls = true);

            // #region Swagger
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new Info
            //     {
            //         Version = "v1.0.0",
            //         Title = "Logistics WebAPI",
            //         Description = "框架集合",
            //         TermsOfService = "None",
            //         Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Xiezn", Email = "2521136840@qq.com", Url = "#" }
            //     });
            //     c.OperationFilter<SwaggerFileUploadFilter>();
            //     //Add读取注释服务
            //     var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            //     var xmlPath = Path.Combine(basePath, "APIHelp.xml");
            //     c.IncludeXmlComments(xmlPath, true);

            //     //Addheader验证Message
            //     //c.OperationFilter<SwaggerHeader>();
            //     var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
            //     c.AddSecurityRequirement(security);//Add一个必须的全局安全Message，和AddSecurityDefinition方法指定的方案Name要一致，这里YesBearer。
            //     c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //     {
            //         //Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
            //         //Name = "Authorization",//jwtDefault的参数Name
            //         Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Token: Bearer {token}\"",
            //         Name = "Token",//jwtDefault的参数Name
            //         In = "header",//jwtDefault存放AuthorizationMessage的位置(请求头中)
            //         Type = "apiKey"
            //     });
            // });
            // #endregion

            #region 认证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                JwtAuthConfig jwtAuthConfig = ConfigHelper.GetConfig<JwtAuthConfig>("JwtAuth");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "Gadgets",
                    ValidAudience = "MyAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAuthConfig.SecurityKey)),

                    /*TokenValidationParameters的参数DefaultValue*/
                    RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但Yes不建议这样做。
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    // YesNo要求Token的Claims中必须包含 Expires
                    RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // YesNo验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true
                };
                o.Events = new JwtBearerEvents
                {
                    //此处为权限验证失败后触发的事件
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net CoreDefault的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();

                        //自定义自己想要返回的数据结果，我这里要返回的YesJson对象，通过引用Newtonsoft.Json库进行转换
                        //var payload = JsonConvert.SerializeObject(new { Code = "401", Msg = "很抱歉，您无权访问该接口！" });
                        var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = "401", msg = "很抱歉，您无权访问该接口！" });
                        //自定义返回的数据类型
                        context.Response.ContentType = "application/json";
                        //自定义返回Status码，Default为401 我这里改成 200
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        //输出Json数据结果
                        context.Response.WriteAsync(payload);
                        return Task.FromResult(0);
                    }
                };
            });
            #endregion

            #region 授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClient", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("RequireAdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            IdentityModelEventSource.ShowPII = true;
            app.UseAuthentication();

            app.UseMiddleware<JwtAuthorizationFilter>();

            app.UseStaticFiles();

            app.UseFileServer();

            app.UseCors("AllRequests");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // #region Swagger
            // app.UseSwagger();
            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            // });
            // #endregion
        }
    }
}
