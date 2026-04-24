using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuloMVC.Application.TDOs;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Enum;

namespace ModuloMVC.Domain.Interfaces
{
    public interface ITarefaService
    {
        
        Task<List<TarefaTDO>> ListarTodosAsync(TarefaTDO filtro, VisaoTarefa visao);
        Task<List<ContatoTDO>> ListarContatos();
        Task<TarefaTDO> BuscarPorId(int id);
        Task CriarUm(TarefaTDO tarefa);
        Task AtualizarUm(TarefaTDO tarefa);
        Task ExcluirUm(int id);
    }
}