using FirebaseAdmin;
using FPT.DestinyMatch.API;
using FPT.DestinyMatch.API.Middleware;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
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
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();//Jwt

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub"); //websocket
FirebaseApp.Create(new AppOptions()
{ 
    Credential = GoogleCredential.FromFile("destinymatch-70b72-firebase-adminsdk-swx0r-37919873ee.json")
});
app.Run();