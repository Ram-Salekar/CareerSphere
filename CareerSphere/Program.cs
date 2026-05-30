using CareerSphere.Data;
using CareerSphere.Manager.CareerAgentManager;
using CareerSphere.Manager.ChatBotManager;
using CareerSphere.Manager.InterviewPreparationManager;
using CareerSphere.Manager.JobManager;
using CareerSphere.Manager.JserviceManager;
using CareerSphere.Middleware;
using CareerSphere.Repository.ConversationRepos;
using CareerSphere.Repository.ExperienceRepos;
using CareerSphere.Repository.MessageRepos;
using CareerSphere.Repository.PostRepos;
using CareerSphere.Repository.UserRepoFolder;
using CareerSphere.Services;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.CareerAgentService;
using CareerSphere.Services.CarrerAgentService;
using CareerSphere.Services.FileReader;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IPostRepo , PostRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<ITokenService, Tokenservice>();
builder.Services.AddHttpClient<IOpenRouterService, OpenRouterService>();
builder.Services.AddScoped<CareerSphere.Repository.ConnectionRepos.IConnectionRepo, CareerSphere.Repository.ConnectionRepos.ConnectionRepos>();
builder.Services.AddScoped<IFileReader, FileReader>();
builder.Services.AddScoped<IChatBotManager, ChatBotManager>();
builder.Services.AddTransient<IMessage, MessageRepo>();
builder.Services.AddTransient<IConversation, ConversationRepo>();
builder.Services.AddScoped<IExperienceRepo, ExperienceRepo>();
builder.Services.AddScoped<IJservice, JService>();
builder.Services.AddScoped<IJobManager, JobManager>();
builder.Services.AddScoped<IInterviewPreparationManager, InterviewPreparationManager>();
builder.Services.AddScoped<ICareerAgentService, CareerAgentService>();
builder.Services.AddScoped<ICareerAgentManager, CareerAgentManager>();
builder.Services.AddTransient<GlobalExceptionHandler>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

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

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey!)
        )
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddRateLimiter(options =>
{
   
    options.AddFixedWindowLimiter("AiPolicy", config =>
    {
        config.PermitLimit = 20;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });

   
    options.AddFixedWindowLimiter("AuthPolicy", config =>
    {
        config.PermitLimit = 5;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });

    
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            statusCode = 429,
            message = "Too many requests. Please wait and try again."
        }, token);
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token here. Example: eyJhbGci..."
    });
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"  
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
