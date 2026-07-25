using Application.Interface;
using Application.Mapping;
using Application.Services;
using Domain.Entity;
using Domain.Interfaces;
using Employee_WebUi.Middlewares;
using Infrastructure.Data;
using Infrastructure.Repository_Implementations;
using Infrastructure.Repository_Implementations.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace Employee_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ManagementDbContext>(Options =>
            Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IEmployee, EmployeeRepository>();
            builder.Services.AddScoped<IDepartment, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
            builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserAuthentication, UserAuthenticate>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
                Options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/Json";
                        context.Response.StatusCode = 401;
                        //return context.Response.WriteAsync("Token Missing");
                        return context.Response.WriteAsync(
                            JsonSerializer.Serialize(new
                            {
                                StatusCodes = 401,
                                Message = "Unthorize Please Login"
                            }));
                    },
                    OnForbidden = context =>
                    {
                        context.Response.ContentType = "application/Json";
                        context.Response.StatusCode = 403;
                        return context.Response.WriteAsync(
                            JsonSerializer.Serialize(new
                            {
                                Message = "You don't have permission"
                            }));
                    }
                };
            });
            builder.Services.AddSession();
            // JWT Swagger ragister
            builder.Services.AddSwaggerGen(Options =>
            {
                Options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Description = "Enter JWT Token"
                });
                Options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                      Array.Empty<string>()
                  }
              });
            });

            var app = builder.Build();

            app.MapControllers();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
