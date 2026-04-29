using CareerSphere.Data;
using CareerSphere.Repository.PostRepos;
using CareerSphere.Repository.UserRepoFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using CareerSphere.Services;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;
using CareerSphere.Manager.ChatBotManager;
using CareerSphere.Repository.MessageRepos;
using CareerSphere.Repository.ConversationRepos;
using CareerSphere.Repository.ExperienceRepos;
using CareerSphere.Manager.JserviceManager;
using CareerSphere.Manager.JobManager;
using CareerSphere.Manager.InterviewPreparationManager;

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
builder.Services.AddSingleton<IFileReader, FileReader>();
builder.Services.AddScoped<IChatBotManager, ChatBotManager>();
builder.Services.AddTransient<IMessage, MessageRepo>();
builder.Services.AddTransient<IConversation, ConversationRepo>();
builder.Services.AddScoped<IExperienceRepo, ExperienceRepo>();
builder.Services.AddScoped<IJservice, JService>();
builder.Services.AddScoped<IJobManager, JobManager>();
builder.Services.AddScoped<IInterviewPreparationManager, InterviewPreparationManager>();

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
