using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Repositories.MongoDb;
using Store_Management.CORE.Repositories.SQL;
using Store_Management.CORE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerDBRepository, CustomerDBRepository>();
builder.Services.AddScoped<IEmployeeDBRepository, EmployeeDBRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDBRepository, OrderDBRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDBRepository, ProductDBRepository>();
builder.Services.AddScoped<IMongoDBCache, MongoDBCache>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient("mongodb+srv://Puzzles:0JhnqCzn1toKhSTZ@cluster0.ubg9w.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"));

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
