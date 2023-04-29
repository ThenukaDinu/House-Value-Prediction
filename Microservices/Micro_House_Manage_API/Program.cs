using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Helper;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Repository;
using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SettingsModels;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Micro_House_Manage_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
            
            builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
            builder.Services.AddScoped<IHouseRepository, HouseRepository>();
            builder.Services.AddScoped<IInquiryRepository, InqueryRepository>();
            builder.Services.AddScoped<IListingRepository, ListingRepository>();
            builder.Services.AddScoped<IMessageProducer, MessageProducer>();
            builder.Services.AddScoped<IUserAccess, UserAccess>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = builder.Configuration["IdentityServerSettings:Authority"];
                options.RequireHttpsMetadata = bool.Parse(builder.Configuration["IdentityServerSettings:RequireHttpsMetadata"]);
                options.ApiName = builder.Configuration["IdentityServerSettings:ApiName"];
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseCors("CorsPolicy");

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