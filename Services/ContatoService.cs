using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ModuloMVC.Context;
using ModuloMVC.Enum;
using ModuloMVC.Interfaces;
using ModuloMVC.Models;


namespace ModuloMVC.Services
{
    public class ContatoService : IContatoService
    {

            private readonly IContatoRepository _contatoRepository;

        public ContatoService(IContatoRepository contatoRepository, ITarefasRepository tarefasRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task Create(string nome, string email, string telefone, string? descricao)
        {
           var contato = new Contato(nome, email, telefone, descricao);
           await _contatoRepository.CreateContato(contato);
        }

        public async Task Delete(int id)
        {
            await _contatoRepository.DeleteContato(id);
        }

        public async Task<List<(int id, string nome, string email, string telefone)>> GetAll(string nome, string email, string telefone)
        {
           var contatos = await _contatoRepository.GetAllContatos(nome, email, telefone);
           return contatos.Select(c => (c.Id, c.Nome, c.Email, c.Telefone)).ToList();
        }
    

        public async Task<(int id, string nome, string email, string telefone, string descricao, bool status, List<(int id, string titulo, string descricao, DateTime? datainicio, DateTime? datafim, StatusTarefa status)>)> GetById(int id)
        {
            var contato = await _contatoRepository.GetByIdContato(id);
            
            return (contato.Id, contato.Nome, contato.Email, contato.Telefone, contato.Descricao, contato.Status, contato.TarefasEnvolvidas.Select(t => (t.Id, t.Titulo, t.Descricao, t.DataInicio, t.DataFim, t.Status)).ToList());
        }

        public async Task<List<(int id, string titulo, string descricao, DateTime? datainicio, DateTime? datafim)>> GetTarefasEnvolvidas(int contatoId)
        {
            var contato = await _contatoRepository.GetByIdContato(contatoId);
            var tarefas = contato.TarefasEnvolvidas;
            return tarefas.Select(t => (t.Id, t.Titulo, t.Descricao, t.DataInicio, t.DataFim)).ToList();
        }

        public async Task Update(int id, string nome, string email, string telefone, bool status, string? descricao)
        {
            var contato = await _contatoRepository.GetByIdContato(id);
            contato.AtualizarDados(nome, email, telefone, status, descricao);
            await _contatoRepository.UpdateContato(contato);
        }
    }
}