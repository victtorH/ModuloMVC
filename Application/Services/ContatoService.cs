using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Application.TDOs;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Domain.Interfaces;

namespace ModuloMVC.Application.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<List<Contato>> ListarTodosAsync(string? nome, string? numero, string? email, List<bool>? status)
        {
            var contatos = await _contatoRepository.ListarTodosAsync(nome, numero, email, status);
            return contatos;
        }

        public async Task<List<ContatoTDO>> ListarTodosTDOAsync(string? nome, string? numero, string? email, List<bool>? status)
        {
            var contatos = await ListarTodosAsync(nome, numero, email, status);
            return contatos.Select(MapToTDO).ToList();
        }

        public async Task<Contato> BuscarPorId(int id)
        {
            var contato = await _contatoRepository.BuscarPorId(id);
            if (contato == null)
            {
                throw new ArgumentException("Contato solicitado não existe");
            }

            return contato;
        }

        public async Task<ContatoTDO> BuscarPorIdTDO(int id)
        {
            var contato = await BuscarPorId(id);
            return MapToTDO(contato);
        }

        public async Task CriarUm(string nome, string email, string telefone, string? descricao)
        {
            await ValidarSeJaExiste(email, telefone);
            var novoContato = new Contato(nome, email, telefone, descricao);
            await _contatoRepository.AddAsync(novoContato);
        }

        public async Task CriarUm(ContatoTDO contato)
        {
            await ValidarSeJaExiste(contato.Email ?? string.Empty, contato.Telefone ?? string.Empty);
            var novoContato = new Contato(contato.Nome, contato.Email, contato.Telefone, contato.Descricao);

            if (!contato.Status)
            {
                novoContato.AtualizarDados(contato.Nome, contato.Email, contato.Telefone, contato.Status, contato.Descricao);
            }

            await _contatoRepository.AddAsync(novoContato);
        }

        public async Task DeletarUm(int id)
        {
            await _contatoRepository.DeleteAsync(id);
        }

        public async Task EditarUm(int id, string nome, string email, string telefone, bool status, string? descricao)
        {
            var contatoDb = await BuscarPorId(id);
            await ValidarSeJaExiste(email, telefone, id);
            contatoDb.AtualizarDados(nome, email, telefone, status, descricao);
            await _contatoRepository.UpdateAsync(contatoDb);
        }

        public async Task EditarUm(ContatoTDO contato)
        {
            if (contato == null)
            {
                throw new ArgumentNullException(nameof(contato));
            }

            var contatoDb = await BuscarPorId(contato.Id);
            await ValidarSeJaExiste(contato.Email ?? string.Empty, contato.Telefone ?? string.Empty, contato.Id);
            contatoDb.AtualizarDados(contato.Nome, contato.Email, contato.Telefone, contato.Status, contato.Descricao);
            await _contatoRepository.UpdateAsync(contatoDb);
        }

        private async Task ValidarSeJaExiste(string email, string telefone, int idDesconsiderado = 0)
        {
            var contatos = await _contatoRepository.GetAllAsync();
            bool contatoDuplicado = contatos.Any(c =>
                (c.Email == email || c.Telefone == telefone) &&
                c.Id != idDesconsiderado);

            if (contatoDuplicado)
            {
                throw new InvalidOperationException("Já existe um contato cadastrado com este E-mail ou Telefone.");
            }
        }

        private ContatoTDO MapToTDO(Contato contato)
        {
            return new ContatoTDO
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Email = contato.Email,
                Telefone = contato.Telefone,
                Status = contato.Status,
                Descricao = contato.Descricao
            };
        }
    }
}