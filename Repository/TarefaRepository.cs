using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Enum;
using ModuloMVC.Interfaces;
using ModuloMVC.Context;
using ModuloMVC.Models;

namespace ModuloMVC.Repository
{
    public class TarefaRepository : ITarefasRepository
    {
        private readonly TEnancyDB _context;
        public TarefaRepository(TEnancyDB context)
        {
            _context = context;
        }

        public async Task CreateTarefa(Tarefa tarefa)
        {
            await _context.Tarefa.AddAsync(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTarefa(int id)
        {
            var tarefa = await GetByIdTarefa(id);
            _context.Tarefa.Remove(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exist(Tarefa tarefa, int? idtarefa)
        {
         return await _context.Tarefa.AnyAsync(t => t.Titulo == tarefa.Titulo && t.Descricao == tarefa.Descricao && t.Id != idtarefa);
        }

        public async Task<List<Tarefa>> GetAllTarefas(string? titulo, DateTime? dataInicio, DateTime? dataFim, StatusTarefa? status, visaotarefa? visao)
        {
            var query = _context.Tarefa.Include(t => t.ContatosEnvolvidos).AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(t => t.Titulo.Contains(titulo));

            if (dataInicio.HasValue)
                query = query.Where(t => t.DataInicio == dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(t => t.DataFim == dataFim.Value);

            if (status != null)
                query = query.Where(t => t.Status == status);

                if (visao != null)
            {
                var hoje = DateTime.Now.AddMinutes(-1);
                switch (visao)
                {
                    case visaotarefa.hoje:
                        query = query.Where(t =>  t.DataFim >= hoje);
                        break;
                    case visaotarefa.atrasadas:
                        query = query.Where(t => t.DataFim < hoje);
                        break;

                }
            }

            return await query.ToListAsync();
        }

        public async Task<Tarefa> GetByIdTarefa(int id)
        {
            var tarefa = await _context.Tarefa.Include(t => t.ContatosEnvolvidos).FirstOrDefaultAsync(t => t.Id == id);
            if (tarefa == null)
                throw new KeyNotFoundException($"Tarefa não encontrada.");
                
            return tarefa;
        }


        public async Task<List<Contato>> GetContatosEnvolvidos(int tarefaId)
        {
            var tarefa = await GetByIdTarefa(tarefaId);
            return tarefa.ContatosEnvolvidos.ToList();
        }

        public async Task<List<Tarefa>> GetTarefasByIds(List<int> ids)
        {
            var tarefas = await _context.Tarefa.Where(t => ids.Contains(t.Id)).ToListAsync();
            return tarefas;
        }

        public async Task UpdateTarefa(Tarefa tarefa)
        {

            _context.Tarefa.Update(tarefa);
            await _context.SaveChangesAsync();
        }




    }
}