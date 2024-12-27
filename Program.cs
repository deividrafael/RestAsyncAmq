using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configurações do OpenShift AMQ
builder.Services.Configure<AmqSettings>(builder.Configuration.GetSection("AmqSettings"));
builder.Services.AddSingleton<IAmqProducer, AmqProducer>();

var app = builder.Build();

app.MapPost("/send-client-id", async ([FromBody] ClientIdRequest request, [FromServices] IAmqProducer producer) =>
{
    if (string.IsNullOrEmpty(request.ClientId))
    {
        return Results.BadRequest("ClientId cannot be null or empty.");
    }

    try
    {
        await producer.SendMessageAsync(request.ClientId);
        return Results.Ok($"ClientId {request.ClientId} sent successfully to AMQ.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return Results.Problem("An error occurred while sending the message.", statusCode: 500);
    }
});

app.Run();
