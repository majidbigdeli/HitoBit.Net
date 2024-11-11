using HitoBit.Net;
using HitoBit.Net.Interfaces.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHitoBit();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/price/{symbol}", async (IHitoBitRestClient restClient, string symbol) =>
{
    var price = await restClient.SpotApi.ExchangeData.GetPriceAsync(symbol);
    return price.Data;
})
.WithName("GetSymbolPrice");

app.Run();