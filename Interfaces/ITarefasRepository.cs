using System;
using System.Collections.Generic;
using System.Linq;
using ModuloMVC.Enum;
using System.Threading.Tasks;
using ModuloMVC.Models;

namespace ModuloMVC.Interfaces
{
    public interface ITarefasRepository
    {
        Task<Tarefa> GetByIdTarefa(int id);
        Task<List<Tarefa>> GetAllTarefas(string? titulo, DateTime? dataInicio, DateTime? dataFim, StatusTarefa? status, visaotarefa? visao);
        Task<List<Tarefa>> GetTarefasByIds(List<int> ids);
        Task CreateTarefa(Tarefa tarefa);
        Task UpdateTarefa(Tarefa tarefa);
        Task DeleteTarefa(int id);
        Task<List<Contato>> GetContatosEnvolvidos(int tarefaId);
        Task<bool> Exist(Tarefa tarefa, int? idtarefa);

    }
}