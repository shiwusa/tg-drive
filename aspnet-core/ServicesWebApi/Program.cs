using MassTransit;
using TgDrive.Infrastructure.RabbitMQ;
using TgDrive.Infrastructure.Services;
using TgDrive.Web.Auth;
using TgDrive.Web.HttpApi;

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
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AuthHeaderOperationFilter>();
});

var mySqlConnectionStr =
    Environment.GetEnvironmentVariable("TGDRIVE_MYSQL_CONNECTION_STRING");
builder.Services.AddTgDriveContextPool(mySqlConnectionStr!);
builder.Services.AddFileService();
builder.Services.AddDirectoryService();
builder.Services.AddScoped<ITgFileServiceClient, TgFileServiceClient>();

const string corsAllowTgDrive = "allowTgDrive";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsAllowTgDrive,
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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
// }

app.UseHttpsRedirection();

app.UseCors(corsAllowTgDrive);

app.UseMiddleware<TgAuthMiddleware>();

app.MapControllers();

app.Run();
