using Mediator.Shared.Models;
using Mediator.Shared.TestHandlers;
using SimpleMediator = Mediator.Impl.Simple;
using OfficialMediatr = MediatR.ISender;
using Mediator.Impl.NickChapsas;


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
builder.Services.AddSingleton<SimpleMediator.Interfaces.ISender, SimpleMediator.Sender>();
builder.Services.AddTransient<SimpleMediator.Interfaces.IRequestHandler<SimpleMediatorCommand,
    TestResponse>, SimpleMediatorHandler>();


// Nick Chapsas Mediator Implementation
builder.Services.AddNickMediator(ServiceLifetime.Transient, typeof(NickChapsasMediatorHandler));


// Another simple Mediator implementation
builder.Services.AddSingleton<Mediator.Impl.AnotherSimple.Interfaces.IMediator, Mediator.Impl.AnotherSimple.Mediator>();
builder.Services.AddTransient<Mediator.Impl.AnotherSimple.Interfaces.IRequestHandler<AnotherSimpleMediatorCommand, TestResponse>, AnotherSimpleMediatorCommandHandler>();


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


app.MapGet("/nickchapsas", async (Mediator.Impl.NickChapsas.Interfaces.IMediator mediator) =>
{
    var request = new NickChapsasMediatorCommand(Id, Name, Age);
    var result = await mediator.SendAsync(request);
    return Results.Ok(result);
})
.WithName("NickChapsasMediator_Send")
.WithOpenApi();

app.MapGet("/anothersimple", async (Mediator.Impl.AnotherSimple.Interfaces.IMediator mediator) =>
{
    var request = new AnotherSimpleMediatorCommand(Id, Name, Age);
    var result = await mediator.Send<AnotherSimpleMediatorCommand, TestResponse>(request);
    return Results.Ok(result);
})
.WithName("AnotherSimpleMediator_Send")
.WithOpenApi();

app.Run();
