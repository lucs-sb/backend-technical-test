using Api.Models;
using Application.DTOs;
using Application.Interfaces;
using Application.Options;
using Infrastructure.IoC;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI();

builder.Services.Configure<HttpClientOptions>(
    builder.Configuration.GetSection(HttpClientOptions.SectionName));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/repos/me", async (string usuario, IRepositorioService service) =>
{
    var resultado = await service.ListarRepositoriosDoUsuario(usuario);
    return Results.Ok(resultado);
});

app.MapGet("/repos", async (string nome, IRepositorioService service) =>
{
    var resultado = await service.BuscarRepositoriosPeloNome(nome);
    return Results.Ok(resultado);
});

app.MapPost("/favoritos", async (RepositorioModel repositorioModel, IRepositorioService service) =>
{
    service.AdicionarFavorito(repositorioModel.Adapt<RepositorioDTO>());
    return Results.Ok();
});

app.MapGet("/favoritos", async (IRepositorioService service) =>
{
    var resultado = service.ListarFavoritos();
    return Results.Ok(resultado);
});

app.Run();