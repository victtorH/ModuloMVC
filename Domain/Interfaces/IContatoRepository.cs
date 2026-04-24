using System.Collections.Generic;
using System.Threading.Tasks;
using ModuloMVC.Domain.Entities;

namespace ModuloMVC.Domain.Interfaces
{
    public interface IContatoRepository
    {

        Task<Contato> BuscarPorId(int id);
        Task<List<Contato>> ListarTodosAsync(string? nome, string? numero, string? email, List<bool>? status);
        Task<List<Contato>> GetAllAsync();
        Task AddAsync(Contato contato);
        Task UpdateAsync(Contato contato);
        Task DeleteAsync(int id);
        Task CriarUm(string nome, string email, string telefone, string? descricao);
        
        Task DeletarUm(int id);
        Task EditarUm(int id, string nome, string email, string telefone, bool status, string? descricao);
    }
}