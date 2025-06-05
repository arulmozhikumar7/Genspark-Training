using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Repositories;
using HospitalManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using HospitalManagementAPI.Authorization.Handlers;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.SignalR;
using HospitalManagementAPI.Hubs;
var builder = WebApplication.CreateBuilder(args);

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() 
            .SetIsOriginAllowed(origin => true); 
    });
});


// Configure DbContext with PostgreSQL Server 
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register repositories
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<SpecializationService>();
builder.Services.AddScoped<AppointmentService>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ExperiencedDoctorOnly", policy =>
        policy.Requirements.Add(new MinimumExperienceRequirement(5)));

    options.AddPolicy("DoctorOwnsAppointment", policy =>
        policy.Requirements.Add(new DoctorOwnsAppointmentRequirement()));
});


builder.Services.AddScoped<IAuthorizationHandler, MinimumExperienceHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DoctorOwnsAppointmentHandler>();

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                    };
                });



builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<IFreeMedicalCampRepository, FreeMedicalCampRepository>();
builder.Services.AddScoped<FreeMedicalCampService>();
builder.Services.AddSignalR();


builder.Logging.AddLog4Net();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "Reports")),
    RequestPath = "/Reports"
});
app.UseCors("AllowAll"); 
app.UseAuthentication(); 
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationhub");


app.MapControllers();

app.Run();
