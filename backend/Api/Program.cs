using Api.Middlewares;
using Api.Models;
using Api.Validators;
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.Options;
using FluentValidation;
using Infrastructure.IoC;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

MapsterConfiguration.Register();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI();

builder.Services.AddScoped<IValidator<RepositorioModel>, RepositorioModelValidator>();

builder.Services.Configure<HttpClientOptions>(
    builder.Configuration.GetSection(HttpClientOptions.SectionName));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapGet("/repos/me", async (string usuario, IRepositorioService service) =>
{
    var resultado = await service.ListarRepositoriosDoUsuario(usuario);
    return Results.Ok(resultado);
});

app.MapGet("/repos", async (
    [AsParameters] BuscarRepositoriosQueryModel query,
    IRepositorioService service) =>
{
    var resultado = await service.BuscarRepositoriosPeloNome(query.Nome, query.Pagina!.Value, query.TamanhoPagina!.Value);

    return Results.Ok(resultado);
});

app.MapPost("/favoritos", async (
    RepositorioModel repositorioModel,
    IValidator<RepositorioModel> validator,
    IRepositorioService service) =>
{
    var validationResult = await validator.ValidateAsync(repositorioModel);

    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).Distinct().ToArray());

        return Results.ValidationProblem(errors);
    }

    service.AdicionarFavorito(repositorioModel.Adapt<RepositorioDTO>());
    return Results.Ok();
});

app.MapGet("/favoritos", async (IRepositorioService service) =>
{
    var resultado = service.ListarFavoritos();
    return Results.Ok(resultado);
});

app.Run();
