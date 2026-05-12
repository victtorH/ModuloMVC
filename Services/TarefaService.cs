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
using System.ComponentModel.DataAnnotations;
using ModuloMVC.Enum;
using ModuloMVC.Interfaces;

namespace ModuloMVC.Services
{
    public class TarefaService : ITarefaService
    {
        protected readonly ITarefasRepository _tarefasRepository;
        protected readonly IContatoRepository _contatoRepository;
        private visaotarefa visaotarefa;
        public TarefaService(ITarefasRepository tarefasRepository, IContatoRepository contatoRepository)
        {
            _tarefasRepository = tarefasRepository;
            _contatoRepository = contatoRepository;
        }


        public async Task Create(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, List<int> IdsContatos)
        {
            
            var tarefa = new Tarefa(titulo, descricao, dataInicio, dataFim);
            if(await _tarefasRepository.Exist(tarefa, null))throw new Exception("Essa tarefa já existe.");
            var contatos = await _contatoRepository.GetContatosByIds(IdsContatos);
            tarefa.AtualizarContatos(contatos);
            await _tarefasRepository.CreateTarefa(tarefa);
        }

        public async Task Delete(int id)
        {
            await _tarefasRepository.DeleteTarefa(id);
        }
        

        public async Task<List<(int id, string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<(int id, string? nome, string? email)> contatos)>> GetAll(string? titulo, DateTime? dataInicio, DateTime? dataFim, StatusTarefa? status, string? visao)
        {
            switch (visao)
            {
                case "atrasadas":
                   visaotarefa = visaotarefa.atrasadas;
                    break;
                case "hoje":
                    visaotarefa = visaotarefa.hoje;
                    break;
                case "todas":
                    visaotarefa = visaotarefa.todas;
                    break;
                
            }
            
            var tarefas = await _tarefasRepository.GetAllTarefas(titulo, dataInicio, dataFim, status, visaotarefa);
            return tarefas.Select(t => (t.Id, t.Titulo, t.Descricao, t.DataInicio, t.DataFim, t.Status, t.ContatosEnvolvidos
            .Select(c => (c.Id, c.Nome, c.Email)).ToList())).ToList();
        }

        public async Task<List<(int id, string? nome, string? email)>> GetAllContatos()
        {
            var contatos = await _contatoRepository.GetAllContatos(null, null, null);
            return contatos.Select(c => (c.Id, c.Nome, c.Email)).ToList();
        }

        public async Task<(int id, string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<(int id, string? nome, string? email)>)> GetById(int id)
        {
            var tarefa = await _tarefasRepository.GetByIdTarefa(id);
            return (tarefa.Id, tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim, tarefa.Status, tarefa.ContatosEnvolvidos
            .Select(c => (c.Id, c.Nome, c.Email)).ToList());
        }
        

        public async Task Update(int id, string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status, List<int> IdsContatos)
        {
            var tarefa = await _tarefasRepository.GetByIdTarefa(id);
            tarefa.AtualizarTarefa(titulo, descricao, dataInicio, dataFim, status);
            if(await _tarefasRepository.Exist(tarefa, id))throw new Exception("Não é possivel fazer duas tarefas serem iguais");
            var contatos = await _contatoRepository.GetContatosByIds(IdsContatos);
            tarefa.AtualizarContatos(contatos);
            await _tarefasRepository.UpdateTarefa(tarefa);
        }

    }
}