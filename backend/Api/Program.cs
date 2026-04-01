using Application.DTOs;
using Application.Interfaces;
using Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddApplicationDI();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/repos/me", async (IRepositorioService service) =>
{
    var result = await service.ListarRepositoriosDoUsuario("octocat");
    return Results.Ok(result);
});

app.MapPost("/favoritos", async (FavoritoDTO favorito, IRepositorioService service) =>
{
    await service.AdicionarFavorito(favorito);
    return Results.Ok();
});

app.MapGet("/favoritos", async (IRepositorioService service) =>
{
    var result = await service.ListarFavoritos();
    return Results.Ok(result);
});

app.Run();