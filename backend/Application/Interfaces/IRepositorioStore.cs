using Domain.Entities;

namespace Application.Interfaces;

public interface IRepositorioStore
{
    void Adicionar(Repositorio repositorio);
    Repositorio? BuscarPorId(Guid id);
    IReadOnlyCollection<Repositorio> ListarTodos();
    void RemoverPeloId(Guid id);
}
