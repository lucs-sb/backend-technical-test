using System.Collections.Concurrent;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class RepositorioStore : IRepositorioStore
{
    private readonly ConcurrentDictionary<string, Repositorio> _repositorios = new(StringComparer.OrdinalIgnoreCase);

    public void Adicionar(Repositorio repositorio)
    {
        _repositorios.TryAdd(repositorio.HtmlUrl, repositorio);
    }

    public Repositorio? BuscarPorId(Guid id)
    {
        return _repositorios.Values.FirstOrDefault(r => r.Id == id);
    }

    public Repositorio? BuscarPorHtmlUrl(string htmlUrl)
    {
        _repositorios.TryGetValue(htmlUrl, out var repositorio);
        return repositorio;
    }

    public IReadOnlyCollection<Repositorio> ListarTodos()
    {
        return _repositorios.Values.ToList();
    }

    public void RemoverPeloId(Guid id)
    {
        if (_repositorios.Values.FirstOrDefault(r => r.Id == id) is Repositorio repositorio)
        {
            _repositorios.TryRemove(repositorio.HtmlUrl, out _);
        }
    }
}
