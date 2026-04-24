using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Domain.Interfaces;
using ModuloMVC.Infrastructure.Context;

namespace ModuloMVC.Infrastructure.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly TEnancyDB _context;

        public ContatoRepository(TEnancyDB context)
        {
            _context = context;
        }

        public async Task<Contato> BuscarPorId(int id)
        {
            return await _context.Contato.FindAsync(id);
        }

        public async Task<List<Contato>> ListarTodosAsync(string? nome, string? numero, string? email, List<bool>? status)
        {
            var query = _context.Contato.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(c => c.Nome.Contains(nome));

            if (!string.IsNullOrEmpty(numero))
                query = query.Where(c => c.Telefone.Contains(numero));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(c => c.Email.Contains(email));

            if (status != null && status.Count > 0)
                query = query.Where(c => status.Contains(c.Status));

            return await query.ToListAsync();
        }

        public async Task<List<Contato>> GetAllAsync()
        {
            return await _context.Contato.ToListAsync();
        }

        public async Task AddAsync(Contato contato)
        {
            await ValidarSeJaExiste(contato.Email, contato.Telefone, contato.Id);
            await _context.Contato.AddAsync(contato);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contato contato)
        {
            await ValidarSeJaExiste(contato.Email, contato.Telefone, contato.Id);
            _context.Contato.Update(contato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contato = await BuscarPorId(id);
            if (contato == null)
            {
                throw new InvalidOperationException("Contato não encontrado.");
            }
            _context.Contato.Remove(contato);
            await _context.SaveChangesAsync();
        }

        public async Task CriarUm(string nome, string email, string telefone, string? descricao)
        {
            await ValidarSeJaExiste(email, telefone);

            Contato NovoContato = new Contato(nome, email, telefone, descricao);

            await _context.Contato.AddAsync(NovoContato);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarUm(int id)
        {
            var contato = await BuscarPorId(id);
            if (contato == null)
            {
                throw new InvalidOperationException("Contato não encontrado.");
            }
            _context.Contato.Remove(contato);
            await _context.SaveChangesAsync();
        }

        public async Task EditarUm(int id, string nome, string email, string telefone, bool status, string? descricao)
        {
            var contato = await BuscarPorId(id);
            if (contato == null)
            {
                throw new InvalidOperationException("Contato não encontrado.");
            }
            contato.AtualizarDados(nome, email, telefone, status, descricao);
            await _context.SaveChangesAsync();
        }




        private async Task ValidarSeJaExiste(string email, string telefone, int idDesconsiderado = 0)
        {
            var contatos = await _context.Contato.ToListAsync();
            bool contatoDuplicado = contatos.Any(c =>
                (c.Email == email || c.Telefone == telefone) &&
                c.Id != idDesconsiderado);

            if (contatoDuplicado)
            {
                throw new InvalidOperationException("Já existe um contato cadastrado com este E-mail ou Telefone.");
            }
        }
    }
}