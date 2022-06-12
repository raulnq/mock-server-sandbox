using mock_server_sandbox_app;
using static mock_server_sandbox_app.WeatherApiClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var setttings = builder.Configuration.GetSection("WeatherApi").Get<Settings>();

builder.Services.AddHttpClient("api", httpClient =>
{
    httpClient.BaseAddress = new Uri(setttings.Uri);
});

builder.Services.AddHttpClient("mock", httpClient =>
{
    httpClient.BaseAddress = new Uri(setttings.MockUri);
});

builder.Services.AddSingleton(setttings);

builder.Services.AddSingleton<WeatherApiClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
