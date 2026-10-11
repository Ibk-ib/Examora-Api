using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;
using UniversityAcademicApi.Middleware;
using UniversityAcademicApi.Repositories;
using UniversityAcademicApi.Services;
using Serilog;
using FluentValidation;
using UniversityAcademicApi.Validators;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


//Add repositories to the container.

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Add services to the container.

builder.Services.AddScoped<IStudentService, StudentService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<FluentValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<FluentValidationFilter>();
});
builder.Services.AddValidatorsFromAssemblyContaining<
    StudentCreateDtoValidator>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
