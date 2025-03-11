using DartsGame.Data;
using DartsGame.Middleware;
using DartsGame.Repositories;
using DartsGame.Repositories.Statistics;
using DartsGame.Services;
using DartsGame.Services.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<LegStatsService>();
builder.Services.AddTransient<SetStatsService>();
builder.Services.AddTransient<MatchStatsService>();
builder.Services.AddTransient<PlayerStatsService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<PlayerService>();

builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<MatchService>();

builder.Services.AddScoped<SetRepository>();
builder.Services.AddScoped<SetService>();

builder.Services.AddScoped<LegRepository>();
builder.Services.AddScoped<LegService>();

builder.Services.AddScoped<TurnRepository>();
builder.Services.AddScoped<TurnService>();

builder.Services.AddScoped<TurnThrowRepository>();
builder.Services.AddScoped<TurnThrowService>();

builder.Services.AddScoped<SetResultRepository>();
builder.Services.AddScoped<LegScoreRepository>();

builder.Services.AddScoped<LegStatsRepository>();
builder.Services.AddScoped<SetStatsRepository>();
builder.Services.AddScoped<MatchStatsRepository>();

builder.Services.AddScoped<PlayerStatsRepository>();

builder.Services.AddScoped<StatisticsService>();

// Game service
builder.Services.AddScoped<GameFlowService>();

// AutoMapper setup
builder.Services.AddAutoMapper(typeof(Program));

// Adding JSON options for circular references handling
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

// HTTPS Redirection and Authorization
app.UseHttpsRedirection();
app.UseAuthorization();

// Map the controllers to the routes
app.MapControllers();

app.Run();
