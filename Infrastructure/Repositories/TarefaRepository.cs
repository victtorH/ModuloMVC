using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Domain.Interfaces;
using ModuloMVC.Enum;
using ModuloMVC.Infrastructure.Context;

namespace ModuloMVC.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly TEnancyDB _context;

        public TarefaRepository(TEnancyDB context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> ListarTodosAsync(string? titulo, DateTime? dataInicio, StatusTarefa? status, VisaoTarefa visao)
        {
            var query = _context.Tarefa.Include(t => t.ContatosEnvolvidos).AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(t => t.Titulo.Contains(titulo));

            if (dataInicio.HasValue)
                query = query.Where(t => t.DataInicio == dataInicio.Value.Date);

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (visao != null)
            {
                switch (visao)
                {
                    case VisaoTarefa.Hoje:
                        query = query.Where(t => t.DataInicio >= DateTime.Today);
                        break;
                    case VisaoTarefa.Atrasadas:
                        query = query.Where(t => t.DataFim < DateTime.Today && t.Status != StatusTarefa.Concluida);
                        break;
                }
            }

            return await query.ToListAsync();

        }

        public async Task<Tarefa> BuscarPorId(int id)
        {

            return await _context.Tarefa.FindAsync(id);
        }

        public async Task<Tarefa> BuscarComContatosPorId(int id)
        {
            return await _context.Tarefa
                .Include(t => t.ContatosEnvolvidos)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CriarAsync(Tarefa tarefa)
        {
            await ValidarSeJaExite(tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim);
            await _context.Tarefa.AddAsync(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync( Tarefa tarefa)
        {   
             await ValidarSeJaExite(tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim, tarefa.Id);
            _context.Tarefa.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Tarefa tarefa)
        {
                _context.Tarefa.Remove(tarefa);
                await _context.SaveChangesAsync();
            
        }

        public async Task<List<Contato>> ListarTodosContatosAsync()
        {
            return await _context.Contato.ToListAsync();
        }

                public Task<List<Contato>> BuscarContatosPorIdsAsync(List<int> ids)
        {
            return _context.Contato.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        private async Task ValidarSeJaExite(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, int IgnorarId = 0)
        {
            var validar = await _context.Tarefa.Where(t => t.Titulo == titulo && t.Descricao == descricao && t.DataInicio == dataInicio && t.DataFim == dataFim && t.Id != IgnorarId).AnyAsync();
            if (validar)
            {
                throw new InvalidOperationException("Está tarefa já foi criada, por genilezar verificar listagem de tarefas");
            }
        }


    }
}