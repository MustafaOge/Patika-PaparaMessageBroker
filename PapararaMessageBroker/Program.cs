using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaparaMessageBroker.Entity;
using PaparaMessageBroker.Messaging;
using PapararaMessageBroker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AppDbContext>(options =>
                                                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<OrderMessageConsumer>();

    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        cfg.UseMessageRetry(r => r.Immediate(5));
        cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(45)));

        cfg.ReceiveEndpoint("order-created", e =>
        {
            e.ConfigureConsumer<OrderMessageConsumer>(context);
        });
    });
       
});

builder.Services.AddOptions<MassTransitHostOptions>()
    .Configure(options =>
    {
        options.WaitUntilStarted = true;
        options.StartTimeout = TimeSpan.FromSeconds(30);
        options.StopTimeout = TimeSpan.FromSeconds(30);
    });

builder.Services.AddScoped<OrderMessagePublisher>();


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
