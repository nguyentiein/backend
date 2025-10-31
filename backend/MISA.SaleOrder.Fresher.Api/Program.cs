using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SalesManagement.BusinessLogic.Exceptions;
using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.Interfaces.Service;
using SalesManagement.BusinessLogic.Mapper;
using SalesManagement.BusinessLogic.Mapper.Customers;
using SalesManagement.BusinessLogic.Result;
using SalesManagement.BusinessLogic.Services;
using SalesManagement.DataAccess.Repositories;



var builder = WebApplication.CreateBuilder(args);
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;


// Cấu hình DI:
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<ICustomerTypeRepo, CustomerTypeRepo>();
builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();

builder.Services.AddAutoMapper(typeof(ModelToResourseProfile).Assembly);
builder.Configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "ResponseMessage.json"), optional: true, reloadOnChange: true);

builder.Services.Configure<ResponseMessage>(
    builder.Configuration.GetSection("ResponseMessage"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()      // Cho phép tất cả domain (hoặc dùng .WithOrigins("https://example.com") để giới hạn)
            .AllowAnyMethod()      // GET, POST, PUT, DELETE...
            .AllowAnyHeader();     // Cho phép tất cả header
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Message = e.Value.Errors.First().ErrorMessage
            })
            .ToList();

        var message = errors.Count > 1
            ? string.Join("; ", errors.Select(e => $"{e.Field}: {e.Message}"))
            : errors.FirstOrDefault()?.Message ?? "Dữ liệu không hợp lệ";

        var result = new BaseResult<object>
        {
            Data = null,
            Meta = null,
            Status = StatusEnum.Failed,
            Error = new ErrorData
            {
                Code = "400",
                Message = message
            }
        };

        return new BadRequestObjectResult(result);
    };
});



var app = builder.Build();

app.UseMiddleware<ValidateExceptionMiddware>();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
