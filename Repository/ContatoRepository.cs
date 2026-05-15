using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Context;
using ModuloMVC.Interfaces;
using ModuloMVC.Models;
using ModuloMVC.Services;

namespace ModuloMVC.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly TEnancyDB _context;
        public ContatoRepository(TEnancyDB context)
        {
            _context = context;
        }

        public async Task CreateContato(Contato contato)
        {
            await _context.Contato.AddAsync(contato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContato(int id)
        {
            var contato = await GetByIdContato(id);
            _context.Contato.Remove(contato);
            await _context.SaveChangesAsync();

        }

public async Task<List<Contato>> GetAllContatos(string? nome, string? email, string? telefone)
{

    var query = _context.Contato.AsNoTracking().AsQueryable();

    if (!string.IsNullOrWhiteSpace(nome))
        query = query.Where(c => c.Nome.Contains(nome));

    if (!string.IsNullOrWhiteSpace(email))
        query = query.Where(c => c.Email.Contains(email));

    if (!string.IsNullOrWhiteSpace(telefone))
    {

        string telefoneLimpo = new string(telefone.Where(char.IsDigit).ToArray());
        
        if (!string.IsNullOrEmpty(telefoneLimpo))
        {
            query = query.Where(c => c.Telefone.Contains(telefoneLimpo));
        }
    }

    return await query.ToListAsync();
}

        public async Task<Contato> GetByIdContato(int id)
        {
            var contato = await _context.Contato.Include(c => c.TarefasEnvolvidas).FirstOrDefaultAsync(c => c.Id == id);
            if (contato == null)
             throw new KeyNotFoundException($"Contato não encontrado.");
            
            return contato;
        }

        public async Task<List<Contato>> GetContatosByIds(List<int> ids)
        {
            var contatos = await _context.Contato.Where(c => ids.Contains(c.Id)).ToListAsync();
            return contatos;
        }

        public async Task<List<Tarefa>> GetTarefasEnvolvidas(int contatoId)
        {
            var contato = await GetByIdContato(contatoId);
            return contato.TarefasEnvolvidas.ToList();
        }

        public async Task UpdateContato(Contato contato)
        {
            var TarefasDoContato = _context.Tarefa.Where(t => t.ContatosEnvolvidos.Any(c => c.Id == contato.Id)).ToList();

            _context.Contato.Update(contato);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exist(Contato contato, int? idcontato)
        {
         return await _context.Contato.AnyAsync(c => c.Nome == contato.Nome && c.Email == contato.Email && c.Telefone == contato.Telefone && c.Id != idcontato);
        }
    }
}