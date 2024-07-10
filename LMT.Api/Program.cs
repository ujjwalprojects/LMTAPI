using Asp.Versioning;
using LMT.Application.Interfaces;
using LMT.Application.MappingProfiles;
using LMT.Infrastructure.Data;
using LMT.Infrastructure.ExceptionHandling;
using LMT.Infrastructure.Repositories;
using LMT.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//SWAGGER WITH AUTHORIZATION
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    { Version = "v1" });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },Array.Empty<string>()
        }
    });
});

//GET CONNECTION STRING FROM CONFIGURATION
string sqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

//CONFIGURE EFDBContext FOR OTHER TABLES
builder.Services.AddDbContext<EFDBContext>(options =>
    options.UseSqlServer(sqlConnectionStr));

//CONFIGURE AppDBContext FOR IDENTITY
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(sqlConnectionStr));

//AUTHENTICATION SERVICE
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

//REGISTER SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlockMunicipalRepository, BlockMunicipalRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IEstablishmentRegistrationRepository, EstablishmentRegistrationRepository>();
builder.Services.AddScoped<IJobRoleRepository, JobRoleRepository>();
builder.Services.AddScoped<IRegistrationActRepository, RegistrationActRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ITaskAllocationFormRepository, TaskAllocationFormRepository>();
builder.Services.AddScoped<ITaskAllocationSiteImageRepository, TaskAllocationSiteImageRepository>();
builder.Services.AddScoped<ITaskPurposeRepository, TaskPurposeRepository>();
builder.Services.AddScoped<IWorkerRegistrationRepository, WorkerRegistrationRepository>();
builder.Services.AddScoped<IWorkerTypeRepository, WorkerTypeRepository>();

//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(BlockMunicipalProfile));
builder.Services.AddAutoMapper(typeof(DistrictProfile));
builder.Services.AddAutoMapper(typeof(EstablishmentRegistrationProfile));
builder.Services.AddAutoMapper(typeof(JobRoleProfile));
builder.Services.AddAutoMapper(typeof(RegistrationActProfile));
builder.Services.AddAutoMapper(typeof(StateProfile));
builder.Services.AddAutoMapper(typeof(TaskAllocationFormProfile));
builder.Services.AddAutoMapper(typeof(TaskAllocationSiteImageProfile));
builder.Services.AddAutoMapper(typeof(TaskPurposeProfile));
builder.Services.AddAutoMapper(typeof(WorkerRegistrationProfile));
builder.Services.AddAutoMapper(typeof(WorkerTypeProfile));

//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

//ADD SUPPORT TO LOGGING WITH SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//GLOBAL EXCEPTION HANDLER
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//API VERSIONING
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseExceptionHandler(opt => { });
app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await EnsureRolesAsync(roleManager);
}

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

//ensure the Admin role exists when your application starts
async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
{
    var roleName = "ADMIN";
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}
