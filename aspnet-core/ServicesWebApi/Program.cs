using System.Reflection;
using DriveServices;
using DriveServices.Clients;
using DriveServices.Implementations;
using MassTransit;
using ServicesExtensions;
using TgAuth.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add MassTransit
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq-bus",
            "/",
            h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
    
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConnectionStr =
    Environment.GetEnvironmentVariable("TGDRIVE_MYSQL_CONNECTION_STRING");
builder.Services.AddTgDriveContextPool(mySqlConnectionStr!);
builder.Services.AddFileService();
builder.Services.AddDirectoryService();
builder.Services.AddScoped<ITgFileService, TgFileServiceClient>();

string cors_allowTgDrive = "allowTgDrive";
// string cors_allowTgDriveDev = "allowTgDriveDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: cors_allowTgDrive,
        policy =>
        {
            policy.AllowCredentials();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.WithOrigins("https://tgdrive.com");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
// }

app.UseHttpsRedirection();

app.UseCors(cors_allowTgDrive);

app.UseMiddleware<TgAuthMiddleware>();

app.MapControllers();

app.Run();
