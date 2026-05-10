using System;
using System.Collections.Generic;
using System.Linq;
using ModuloMVC.Enum;
using ModuloMVC.Models;
using System.Threading.Tasks;

namespace ModuloMVC.Interfaces
{
    public interface ITarefaService
    {
        Task<(int id, string titulo, string descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<(int id, string? nome, string? email)>)> GetById(int id);
       Task<List<(int id, string titulo,string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<(int id, string? nome, string? email)> contatos)>> GetAll(string? titulo, DateTime? dataInicio, DateTime? dataFim, StatusTarefa? status, string? visao);
       Task<List<(int id, string? nome, string? email)>> GetAllContatos();
        Task Create(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, List<int> IdsContatos);
        Task Update(int id, string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<int> IdsContatos);
        Task Delete(int id);
    }
}