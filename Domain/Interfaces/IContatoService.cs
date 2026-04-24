using System.Collections.Generic;
using System.Threading.Tasks;
using ModuloMVC.Application.TDOs;
using ModuloMVC.Domain.Entities;

namespace ModuloMVC.Domain.Interfaces
{
    public interface IContatoService
    {
        Task<List<Contato>> ListarTodosAsync(string? nome, string? numero, string? email, List<bool>? status);
        Task<List<ContatoTDO>> ListarTodosTDOAsync(string? nome, string? numero, string? email, List<bool>? status);
        Task CriarUm(string nome, string email, string telefone, string? descricao);
        Task CriarUm(ContatoTDO contato);
        Task<Contato> BuscarPorId(int id);
        Task<ContatoTDO> BuscarPorIdTDO(int id);
        Task DeletarUm(int id);
        Task EditarUm(int id, string nome, string email, string telefone, bool status, string? descricao);
        Task EditarUm(ContatoTDO contato);
    }
}