using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Enum;
using ModuloMVC.Models;

namespace ModuloMVC.Interfaces
{
    public interface IContatoService 
    {
        Task<(int id, string nome, string email, string telefone, string descricao, bool status, List<(int id, string titulo, string descricao, DateTime? datainicio, DateTime? datafim, StatusTarefa status)>)> GetById(int id);
        Task<List<(int id, string nome, string email, string telefone)>> GetAll(string? nome, string? email, string? telefone);
        Task Create(string nome, string email, string telefone, string? descricao);
        Task Update(int id, string nome, string email, string telefone, bool status, string? descricao);
        Task Delete(int id);
        Task<List<(int id, string titulo, string descricao, DateTime? datainicio, DateTime? datafim)>> GetTarefasEnvolvidas(int contatoId);
    }
}