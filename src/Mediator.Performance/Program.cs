using Mediator.Shared.Models;
using Mediator.Shared.TestHandlers;
using SimpleMediator = Mediator.Simple;
using OfficialMediatr = MediatR.ISender;

Guid Id = Guid.NewGuid();
string Name = "TestName";
int Age = 30;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Official MediatR implementation
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<OfficialMediatrRecordCommand>();
});


// Simple Mediator implementation
builder.Services.AddTransient<SimpleMediator.Interfaces.ISender, SimpleMediator.Sender>();
builder.Services.AddTransient<SimpleMediator.Interfaces.IRequestHandler<SimpleMediatorCommand,
    TestResponse>, SimpleMediatorHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/official", async (OfficialMediatr sender) =>
{
    var request = new OfficialMediatrRecordStructCommand(Id, Name, Age);
    var result = await sender.Send(request);
    return Results.Ok(result);
})
.WithName("OfficialMediator_Send")
.WithOpenApi();


app.MapGet("/simple", async (SimpleMediator.Interfaces.ISender sender) =>
{
    var request = new SimpleMediatorCommand(Id, Name, Age);
    var result = await sender.Send(request);
    return Results.Ok(result);
})
.WithName("SimpleMediator_Send")
.WithOpenApi();


app.Run();
