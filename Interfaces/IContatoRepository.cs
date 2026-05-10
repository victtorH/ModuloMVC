using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Models;

namespace ModuloMVC.Interfaces
{
    public interface IContatoRepository
    {
        Task<Contato> GetByIdContato(int id);
        Task<List<Contato>> GetAllContatos(string? nome, string? email, string? telefone);
        Task<List<Contato>> GetContatosByIds(List<int> ids);
        Task CreateContato(Contato contato);
        Task UpdateContato(Contato contato);
        Task DeleteContato(int id);
        Task<List<Tarefa>> GetTarefasEnvolvidas(int contatoId);

    }
}