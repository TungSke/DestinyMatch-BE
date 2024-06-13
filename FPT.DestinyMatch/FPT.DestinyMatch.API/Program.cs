using FPT.DestinyMatch.API;
using FPT.DestinyMatch.API.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServiceRegistration.InjectServices(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

//Register Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseAuthentication();//Jwt

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub"); //websocket

app.Run();
