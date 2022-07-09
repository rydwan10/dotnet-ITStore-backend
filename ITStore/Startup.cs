using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITStore.Persistence;
using ITStore.Helpers;
using ITStore.Service.Interfaces;
using AutoMapper;
using ITStore.Services;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ITStore.Domain;
using ITStore.Shared;
using System.Security.Claims;
using ITStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IT Store Backend", Version = "v1" });
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                //c.EnableAnnotations();
            });
            services.AddApiVersioning(opts => { 
                opts.ReportApiVersions = true;
            });

            services.AddIdentity<ApplicationUsers, IdentityRole<Guid>>(opts => {
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin") );
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero,
                    };
                });
                    
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDbContext<AppDbContext>();
            services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
            // Register Interface. It will mapped to service when it called in controller
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IWishlistsService, WishlistsService>();
            services.AddScoped<ICartsService, CartsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IDataInitializerService, DataInitializerService>();
            //services.AddScoped<IDataInitializerService, DataInitializerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
