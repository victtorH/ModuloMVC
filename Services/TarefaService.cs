using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Context;
using ModuloMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;
using Humanizer;
using ModuloMVC.ViewModels;

namespace ModuloMVC.Services
{
    public class TarefaService
    {
        protected readonly AgendaContext _context;
        public TarefaService(AgendaContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> ListarTodos()
        {
           return await _context.Tarefa
           .Include(t => t.ContatosEnvolvidos)
           .OrderBy(t => t.Vencimento )
           .ToListAsync();
        }

        public async Task<List<Contato>> ListarContatos()
        {
            return await  _context.Contato.ToListAsync();
        }



        public async Task<Tarefa> BuscarPorId(int id)
        {
            var tarefa = await _context.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                throw new ArgumentException("Tarefa solicitado não existe");
            }

            return tarefa;
        }




        // Transformamos em async Task para não travar o sistema durante a ida ao banco
        public async Task<Tarefa> CriarUmaAsync(string? titulo, string? descricao, DateTime? vencimento, List<int> Ids)
        {
            var contatos = await _context.Contato
                                         .Where(c => Ids.Contains(c.Id))
                                         .ToListAsync();

            Tarefa novaTarefa = new Tarefa(titulo, descricao, vencimento);
            foreach (var contato in contatos)
            {
                novaTarefa.AdicionarContato(contato);
            }

            await _context.Tarefa.AddAsync(novaTarefa);
            await _context.SaveChangesAsync();

            return novaTarefa;
        }


    }
}