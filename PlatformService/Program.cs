using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;
using System.IO;
var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.

Console.WriteLine($"environment: {builder.Environment}");

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
     builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}

 else
{
    Console.WriteLine("--> Using InMem Db");
    Console.WriteLine("--> Not in production");
    builder.Services.AddDbContext<AppDbContext>(option =>
   option.UseInMemoryDatabase("InMem"));
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;

Console.WriteLine("CommandService Endpoint"); 
Console.WriteLine($"--> CommandService Endpoint {configuration["CommandService"]}");

// app.UseHttpsRedirection();

app.UseAuthorization();

PrepDb.PrepPopulation(app, true);

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context => {
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

app.Run();
