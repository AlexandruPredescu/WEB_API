using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Repository;
using PredescuAlexandru_API.Repository.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace PredescuAlexandru_API
{
    public class Program
    {
        public static object JWTBearerDefault { get; private set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            /* builder.Services.AddSwaggerGen(option =>
             {
                 option.AddSecurityDefinition("oauth", new OpenApiSecurityScheme
                 {
                     Name = "Autentication",
                     Type = SecuritySchemeType.ApiKey,
                     Scheme = "Bearer",
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     Description = "JWT Authorization header using schema",
                 });
             });*/

            /*builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "oauth"
            }
        },
        new string[] {}
    }
});
            });*/

            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
    " Enter 'Bearer' [space] and then your token in the text input below." +
    "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });



                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            var clubLibraConnectionString = builder.Configuration.GetConnectionString("clubLibraConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ClubLibraDataContext>(options =>
                options.UseSqlServer(clubLibraConnectionString));


            //register repository
            builder.Services.AddTransient<IAnnouncementsRepository, AnnoucementsRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ICodesnippetsRepository, CodesnippetsRepository>();
            builder.Services.AddTransient<IMembersRepository, MembersRepository>();
            builder.Services.AddTransient<IMembershipsRepository, MembershipsRepository>();
            builder.Services.AddTransient<IMembershipTypesRepository, MembershipTypesRepository>();

            builder.Logging.AddLog4Net("log4net.config");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Autentication:Domain"],
                        ValidAudience = builder.Configuration["Autentication:Audience"],
                        IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Autentication:Secret"]))
                    };
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}