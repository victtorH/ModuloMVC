using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ModuloMVC.Enum;
using ModuloMVC.Application.TDOs;
using ModuloMVC.Domain.Entities;
using ModuloMVC.Application.Services;
using ModuloMVC.Application.ViewModels;
using ModuloMVC.Domain.Interfaces;

namespace ModuloMVC.Application.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly ITarefaService  _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? titulo, DateTime? data, List<StatusTarefa>? status, string? visao = "hoje")
        {
            var visaoEnum = System.Enum.TryParse<VisaoTarefa>(visao, true, out var resultado)
                ? resultado
                : VisaoTarefa.Hoje;

            ViewBag.VisaoAtual = visao;

            var filtro = new TarefaTDO
            {
                Titulo = titulo,
                DataInicio = data,
                // Melhoria: Se a lista de status for vazia, não atribuímos nada para não filtrar por default(0)
                Status = (status != null && status.Any()) ? status.First() : (StatusTarefa?)null 
            };

            // Nota: O TDO precisa aceitar Status como Nullable para o filtro funcionar corretamente

            var listaFiltrada = await _service.ListarTodosAsync(filtro, visaoEnum);
            return View(listaFiltrada);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var viewModel = new TarefaViewModel
            {
                ContatosEnvolvidos = await ObterContatosParaViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(string RotaDeRetorno, TarefaViewModel tarefa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    tarefa.ContatosEnvolvidos = await ObterContatosParaViewModel();
                    return View(tarefa);
                }

                var tarefaDto = new TarefaTDO
                {
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    DataInicio = tarefa.DataInicio,
                    DataFim = tarefa.DataFim,
                    ContatosSelecionadosIds = tarefa.ContatosSelecionadosIds ?? new List<int>()
                };

                await _service.CriarUm(tarefaDto);

                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                tarefa.ContatosEnvolvidos = await ObterContatosParaViewModel();
                TempData["CriarDublicado"] = "Erro Mensagem:  " + err.Message;
                return View(tarefa);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var tarefa = await _service.BuscarPorId(id);

            var viewModel = new TarefaEdicaoViewModel
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataInicio = tarefa.DataInicio,
                DataFim = tarefa.DataFim,
                Status = tarefa.Status.Value,
                ContatosSelecionadosIds = tarefa.ContatosSelecionadosIds,
                TituloAtual = tarefa.Titulo,
                DescricaoAtual = tarefa.Descricao,
                DataInicioAtual = tarefa.DataInicio,
                DataFimAtual = tarefa.DataFim,
                StatusAtual = tarefa.Status.Value,
                ContatosEnvolvidosAtuais = tarefa.ContatosEnvolvidos.Select(c => new ContatoViewModel { Id = c.Id, Nome = c.Nome ?? string.Empty, Email = c.Email ?? string.Empty, Telefone = c.Telefone ?? string.Empty, Status = c.Status }).ToList(),
                TodosContatosDisponiveis = await ObterContatosParaViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, TarefaEdicaoViewModel model, string RotaDeRetorno)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.TodosContatosDisponiveis = await ObterContatosParaViewModel();
                    return View(model);
                }

                var tarefaDto = new TarefaTDO
                {
                    Id = id,
                    Titulo = model.Titulo,
                    Descricao = model.Descricao,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Status = model.Status,
                    ContatosSelecionadosIds = model.ContatosSelecionadosIds ?? new List<int>()
                };

                await _service.AtualizarUm(tarefaDto);

                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                model.TodosContatosDisponiveis = await ObterContatosParaViewModel();
                TempData["ErroEditar"] = "Erro ao atualizar: " + err.Message;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id, string RotaDeRetorno)
        {
            try
            {
                await _service.ExcluirUm(id);

                TempData["ExcluirTarefa"] = "1";

                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                TempData["ExcluirTarefa"] = "Não foi possível excluir: " + err.Message;
                return RedirectToAction("Editar", new { id = id });
            }


        }

        private async Task<List<ContatoViewModel>> ObterContatosParaViewModel()
        {
            var contatos = await _service.ListarContatos();
            return contatos.Select(c => new ContatoViewModel
            {
                Id = c.Id,
                Nome = c.Nome ?? string.Empty,
                Email = c.Email ?? string.Empty,
                Telefone = c.Telefone ?? string.Empty,
                Status = c.Status
            }).ToList();
        }
    }
}