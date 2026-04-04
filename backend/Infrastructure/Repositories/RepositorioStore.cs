using System.Collections.Concurrent;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class RepositorioStore : IRepositorioStore
{
    private readonly ConcurrentDictionary<Guid, Repositorio> _repositorios = new();

    public void Adicionar(Repositorio repositorio)
    {
        _repositorios.TryAdd(repositorio.Id, repositorio);
    }

    public Repositorio? BuscarPorId(Guid id)
    {
        return _repositorios.Values.FirstOrDefault(r => r.Id == id);
    }

    public IReadOnlyCollection<Repositorio> ListarTodos()
    {
        return _repositorios.Values.ToList();
    }

    public void RemoverPeloId(Guid id)
    {
        if (_repositorios.Values.FirstOrDefault(r => r.Id == id) is Repositorio repositorio)
        {
            _repositorios.TryRemove(repositorio.Id, out _);
        }
    }
}