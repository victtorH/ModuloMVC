using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Application.TDOs;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Domain.Interfaces;
using ModuloMVC.Enum;

namespace ModuloMVC.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<List<TarefaTDO>> ListarTodosAsync(TarefaTDO tarefa, VisaoTarefa visao)
        {
                var tarefas = await _tarefaRepository.ListarTodosAsync(tarefa.Titulo, tarefa.DataInicio, tarefa.Status, visao);
                return tarefas.Select(MapToTDO).ToList();
        
        }



        public async Task<List<ContatoTDO>> ListarContatos()
        {
            var contatos = await _tarefaRepository.ListarTodosContatosAsync();
            return contatos.Select(MapContatoToTDO).ToList();
        }


        public async Task<TarefaTDO> BuscarPorId(int id)
        {
            var tarefa = await _tarefaRepository.BuscarComContatosPorId(id);
            if (tarefa == null) throw new Exception("Tarefa não encontrada");

            return MapToTDO(tarefa);
        }


        public async Task CriarUm(TarefaTDO tarefa)
        {
            var novaTarefa = new Tarefa(tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim);
            novaTarefa.AtualizarContatos(await _tarefaRepository.BuscarContatosPorIdsAsync(tarefa.ContatosSelecionadosIds));
            await _tarefaRepository.CriarAsync(novaTarefa);
        }



        public async Task AtualizarUm(TarefaTDO tarefa)
        {
            var tarefaDb = await _tarefaRepository.BuscarComContatosPorId(tarefa.Id);
            if (tarefaDb == null) throw new Exception("Tarefa não encontrada");

            tarefaDb.AtualizarTarefa(tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim, tarefa.Status.Value);
            tarefaDb.AtualizarContatos(await _tarefaRepository.BuscarContatosPorIdsAsync(tarefa.ContatosSelecionadosIds));
            await _tarefaRepository.AtualizarAsync(tarefaDb);
        }

        public async Task ExcluirUm(int id)
        {
            var tarefa = await _tarefaRepository.BuscarPorId(id);
            if (tarefa == null) throw new Exception("Tarefa não encontrada");
            await _tarefaRepository.DeletarAsync(tarefa);
        }

        private TarefaTDO MapToTDO(Tarefa tarefa)
        {
            return new TarefaTDO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataInicio = tarefa.DataInicio,
                DataFim = tarefa.DataFim,
                Status = tarefa.Status,
                ContatosSelecionadosIds = tarefa.ContatosEnvolvidos.Select(c => c.Id).ToList(),
                ContatosEnvolvidos = tarefa.ContatosEnvolvidos.Select(MapContatoToTDO).ToList()
            };
        }

        private ContatoTDO MapContatoToTDO(Contato contato)
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