using Savorboard.CAP.InMemoryMessageQueue;
using DotNetCore.CAP.Dashboard;
using cap.demo.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCap(x =>
{
    //x.UseInMemoryStorage();
    //x.UseInMemoryMessageQueue();
    x.UseRabbitMQ(config =>
    {
        config.HostName = "localhost";
        config.UserName = "admin";
        config.Password = "admin";
        config.VirtualHost = "my_vhost";
        config.Port = 5672;
    });

    x.UseDashboard();

    x.UseEntityFramework<AppDbContext>();
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
