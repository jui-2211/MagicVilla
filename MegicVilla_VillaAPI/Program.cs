
using MegicVilla_VillaAPI.Data;
using MegicVilla_VillaAPI.Repository;
using MegicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace MegicVilla_VillaAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

			builder.Services.AddResponseCaching();

			builder.Services.AddAutoMapper(typeof(MappingConfig));

			builder.Services.AddApiVersioning(Options =>
			{
				Options.AssumeDefaultVersionWhenUnspecified = true;
				Options.DefaultApiVersion = new ApiVersion(1, 0);
				Options.ReportApiVersions = true;
			});

			builder.Services.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});

			var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

			builder.Services.AddAuthentication(x =>
					{
						x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
						x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					})
				.AddJwtBearer(x =>
				{
					x.RequireHttpsMetadata = false;
					x.SaveToken = true;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});

			builder.Services.AddScoped<IVillaRepository, VillaRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

			//User serilogger to log
			//Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
			//	.WriteTo.File("Log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

			//builder.Host.UseSerilog(); // serilog confuguration to use serilog logger

			builder.Services.AddControllers(options =>
			{
				options.CacheProfiles.Add("Default30",
					new CacheProfile()
					{
						Duration = 30
					});

				//options.ReturnHttpNotAcceptable = true;	//if application resoponse is not json then return error
			}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();//for patch & xml response acceptable
																			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description =
						"JWT Authorization header using the Bearer scheme. \r\n\r\n " +
						"Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
						"Example: \"Bearer 12345abcdef\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Scheme = "Bearer"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
										{
											Type = ReferenceType.SecurityScheme,
											Id = "Bearer"
										},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header
						},
						new List<string>()
					}
				});
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1.0",
					Title = "Magic Villa V1",
					Description = "API to manage Villa",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Dotnetmastery",
						Url = new Uri("https://dotnetmastery.com")
					},
					License = new OpenApiLicense
					{
						Name = "Example License",
						Url = new Uri("https://example.com/license")
					}
				});
				options.SwaggerDoc("v2", new OpenApiInfo
				{
					Version = "v2.0",
					Title = "Magic Villa V2",
					Description = "API to manage Villa",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Dotnetmastery",
						Url = new Uri("https://dotnetmastery.com")
					},
					License = new OpenApiLicense
					{
						Name = "Example License",
						Url = new Uri("https://example.com/license")
					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
					options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
				});
			}

            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
                });
            }



            app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}