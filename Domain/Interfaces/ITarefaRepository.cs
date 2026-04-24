using System.Collections.Generic;
using System.Threading.Tasks;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Enum;

namespace ModuloMVC.Domain.Interfaces
{
    public interface ITarefaRepository
    {
        Task<List<Tarefa>> ListarTodosAsync(string? titulo, DateTime? dataInicio, StatusTarefa? status, VisaoTarefa visao);
        Task<Tarefa> BuscarPorId(int id);
        Task<Tarefa> BuscarComContatosPorId(int id);
        Task CriarAsync(Tarefa tarefa);
        Task AtualizarAsync( Tarefa tarefa);
        Task DeletarAsync(Tarefa tarefa);
        Task<List<Contato>> ListarTodosContatosAsync();
        Task<List<Contato>> BuscarContatosPorIdsAsync(List<int> ids);
         
    }
}