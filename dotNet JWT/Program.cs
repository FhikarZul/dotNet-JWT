using System.Text.Json.Serialization;
using JwtAuth;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment;

// Add services to the container.

services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
//cors policy
services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader();
}));

// configure DI for jwt
services.JwtConfigureServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );

app.UseHttpsRedirection();

//midleware
app.JwtConfigureMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

