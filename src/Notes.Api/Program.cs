using Notes.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNotesInfraestructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Use(async (context, next) => {
    await next();

    // Serving index.html for 404 non api endpoints. Necessary
    // for angular routing

    if (context.Response.StatusCode != 404 || context.Request.Path.Value!.Contains("api/")) 
    {
        return;
    }

    string indexFileName = Path.Combine("wwwroot", "index.html");
    if (!File.Exists(indexFileName)) 
    {
        await context.Response.WriteAsync($"spaclient not found on wwwroot");
        return;
    }
    string text = File.ReadAllText(indexFileName);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync(text);
});

app.Run();