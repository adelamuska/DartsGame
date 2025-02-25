using DartsGame.Data;
using DartsGame.Repositories;
using DartsGame.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

//builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<GameService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
