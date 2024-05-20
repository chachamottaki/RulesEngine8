using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RulesEngine8.Models;
using RulesEngine8.Processors;
using RulesEngine8.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//SQL Server
builder.Services.AddDbContext<RulesEngineDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConfigDBConnection")));

//postgresql
/*builder.Services.AddDbContext<RulesEngineDBContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));*/

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ConfigFileParsingService>();
builder.Services.AddScoped<IRuleEngine, RuleEngine>();
builder.Services.AddScoped<IRuleNodeProcessor, FilterNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, TransformNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, FetchNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, DataRetrievalNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, ConditionCheckNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, EmailCreationNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, EmailSendingNodeProcessor>();
builder.Services.AddScoped<IRuleNodeProcessor, ListeningNodeProcessor>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();