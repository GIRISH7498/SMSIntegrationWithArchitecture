using MediatR;
using Microsoft.EntityFrameworkCore;
using SMSIntegration.Application.Features.Sms.Commands;
using SMSIntegration.Domain.Interface;
using SMSIntegration.Infrastructure.BackgroundServices;
using SMSIntegration.Infrastructure.Configurations;
using SMSIntegration.Infrastructure.Persistence;
using SMSIntegration.Infrastructure.Repositories;
using SMSIntegration.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register SmsSettings Configuration
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));

// Register Repositories & Services
builder.Services.AddScoped<ISmsLogRepository, SmsLogRepository>();
builder.Services.AddScoped<ISmsService, SmsService>();

// Register MediatR for CQRS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SendSmsCommand).Assembly));

// Register Background Service
builder.Services.AddHostedService<SmsBackgroundService>();
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
