using System;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.HttpOverrides;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
//using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Extensions.Logging;

namespace locationtrackapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.ConfigureCors();  
            // services.ConfigureIISIntegration();
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,  
            //         ValidIssuer = Configuration["Jwt:Issuer"],  
            //         ValidAudience = Configuration["Jwt:Issuer"],
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //     };
            // });

             //Set up our configuration for jwt tokens inside an extension method
            services.ConfigureJwtAuthentication();
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });
            
    //          // Authentication
    //   services.AddAuthorization(auth =>
    //     {
    //         auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    //             .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
    //             .RequireAuthenticatedUser().Build());
    //     });

    //     services.AddAuthentication(options =>
    //     {
    //         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //     })
    //     .AddJwtBearer(options =>
    //     {
    //         options.TokenValidationParameters = new TokenValidationParameters()
    //         {
    //             IssuerSigningKey = new RsaSecurityKey(new RSACryptoServiceProvider(2048).ExportParameters(true)),
    //             ValidAudience = "Audience",
    //             ValidIssuer = "Issuer",
    //             ValidateIssuerSigningKey = true,
    //             ValidateLifetime = true,
    //             ClockSkew = TimeSpan.FromMinutes(0)
    //         };
    //     });

        
        // services.AddAuthorization(opts =>
        // {
        //     opts.AddPolicy("SurveyCreator", p =>
        //     {
        //         // Using value text for demo show, else use enum : ClaimTypes.Role
        //         p.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "SurveyCreator");
        //     });
        // });

            services.ConfigureLoggerService();
            services.AddMvc().AddJsonOptions(opt =>
                {
                    // Force all ISO8601 timestamp conversions to use UTC (ie: YYYY-MM-DDTHH:MM:SS.FFFZ)
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'";
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);                     
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              app.UseHsts();
            }

        // app.UseExceptionHandler(appBuilder =>
        //         {
        //             appBuilder.Use(async (context, next) =>
        //             {
        //                 var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

        //                 //when authorization has failed, should retrun a json message to client
        //                 if (error != null && error.Error is SecurityTokenExpiredException)
        //                 {
        //                     context.Response.StatusCode = 401;
        //                     context.Response.ContentType = "application/json";

        //                     await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        //                     {
        //                         State = "Unauthorized",
        //                         Msg = "token expired"
        //                     }));
        //                 }
        //                 //when orther error, retrun a error message json to client
        //                 else if (error != null && error.Error != null)
        //                 {
        //                     context.Response.StatusCode = 500;
        //                     context.Response.ContentType = "application/json";
        //                     await context.Response.WriteAsync(JsonConvert.SerializeObject(new
        //                     {
        //                         State = "Internal Server Error",
        //                         Msg = error.Error.Message
        //                     }));
        //                 }
        //                 //when no error, do next.
        //                 else await next();
        //             });
        //         });
        
            app.UseAuthentication();
            //app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();    
            app.UseStaticFiles();         
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }
        //     else
        //     {
        //       app.UseExceptionHandler("/Error");
        //       app.UseHsts();
        //     }
        //      app.UseCors("CorsPolicy");
 
        //     app.UseForwardedHeaders(new ForwardedHeadersOptions
        //     {
        //         ForwardedHeaders = ForwardedHeaders.All
        //     });
 
        //     app.Use(async (context, next) =>
        //     {
        //         await next();
        
        //         if (context.Response.StatusCode == 404
        //             && !Path.HasExtension(context.Request.Path.Value))
        //         {
        //             context.Request.Path = "/index.html";
        //             await next();
        //         }
        //     });
   
        //     // app.UseExceptionHandler(config =>
        //     //     {
        //     //         config.Run(async context =>
        //     //         {
        //     //             context.Response.StatusCode = 500;
        //     //             context.Response.ContentType = "application/json";
            
        //     //             var error = context.Features.Get<IExceptionHandlerFeature>();
        //     //             if (error != null)
        //     //             {
        //     //                 var ex = error.Error;
            
        //     //                 await context.Response.WriteAsync(new ErrorModel()
        //     //                 {
        //     //                     StatusCode = 500,
        //     //                     ErrorMessage = ex.Message 
        //     //                 }.ToString(); //ToString() is overridden to Serialize object
        //     //             }
        //     //         });
        //     //     });
                
        //     app.UseHttpsRedirection();
        //     app.UseStaticFiles();            
        //     app.UseMvc();
        // }

        // public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerManager logger)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }
        //     app.ConfigureCustomExceptionMiddleware();
        //     //app.ConfigureExceptionHandler(logger);        
        //     app.UseMvc();
        // }
    }
}
