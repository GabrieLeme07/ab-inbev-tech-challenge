using Ab.Inbev.Service.Presentation.Application.Extensions.ServiceCollectionExtensions;
using Ab.Inbev.Service.Presentation.Application.Extensions.WebApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddContext(builder.Configuration);
builder.Services.AddJWTConfig(builder.Configuration);
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddCorsConfiguration();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerInterface();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/v1/HealthCheck");
app.Run();