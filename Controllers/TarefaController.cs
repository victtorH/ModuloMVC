using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ModuloMVC.Enum;
using ModuloMVC.ViewModels;
using ModuloMVC.Interfaces;

namespace ModuloMVC.Controllers
{

    public class TarefaController : Controller
    {
        private readonly ITarefaService _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? titulo, DateTime? datainicio, DateTime? datafim, List<StatusTarefa>? status, string visao = "hoje")
        {

            ViewBag.VisaoAtual = visao;


            var listaFiltrada = await _service.GetAll(titulo, datainicio, datafim, status != null && status.Count > 0 ? status[0] : (StatusTarefa?)null, visao );
             
            
            var tarefasViewModel = new List<TarefaViewModel>();
            foreach (var tarefa in listaFiltrada)
            {
                tarefasViewModel.Add(new TarefaViewModel
                {
                    Id = tarefa.id,
                    Titulo = tarefa.titulo,
                    Descricao = tarefa.descricao,
                    DataInicio = tarefa.dataInicio,
                    DataFim = tarefa.dataFim,
                    Status = tarefa.status,
                    ContatosEnvolvidos = tarefa.Item7.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome, Email = c.email }).ToList()
                });
            }
            return View(tarefasViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var contatosDoBanco = await _service.GetAllContatos();
            var contatosParaTela = contatosDoBanco.Select(c => new ContatoViewModel
            {
                Id = c.id,
                Nome = c.nome,
                Email = c.email,
            }).ToList();

            var viewModel = new TarefaViewModel
            {
                ContatosEnvolvidos = contatosParaTela
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
                    var contatosDoBanco = await _service.GetAllContatos();
                    tarefa.ContatosEnvolvidos = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome }).ToList();

                    return View(tarefa);
                }
                var ListaIds = tarefa.ContatosSelecionadosIds ?? new List<int>();
                await _service.Create(tarefa.Titulo, tarefa.Descricao, tarefa.DataInicio, tarefa.DataFim, ListaIds);
                TempData["MensagemTarefa"] = "Tarefa criada com sucesso";
                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                var contatosDoBanco = await _service.GetAllContatos();
                tarefa.ContatosEnvolvidos = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome }).ToList();
                TempData["MensagemTarefa"] = "Erro Mensagem:  " + err.Message;

                return View(tarefa);
            }


        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var tarefa = await _service.GetById(id);
            var contatosDoBanco = await _service.GetAllContatos();

            var viewModel = new TarefaEdicaoViewModel
            {
                Id = tarefa.id,

                Titulo = tarefa.titulo,
                Descricao = tarefa.descricao,
                DataInicio = tarefa.dataInicio,
                DataFim = tarefa.dataFim,
                Status = tarefa.status,
                ContatosSelecionadosIds = tarefa.Item7.Select(c => c.id).ToList(),

                TituloAtual = tarefa.titulo,
                DescricaoAtual = tarefa.descricao,
                DataInicioAtual = tarefa.dataInicio,
                DataFimAtual = tarefa.dataFim,
                StatusAtual = tarefa.status,
                ContatosEnvolvidosAtuais = tarefa.Item7.Select(c => new ContatoViewModel { Nome = c.nome }).ToList(),

                TodosContatosDisponiveis = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome, Email = c.email }).ToList()
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
                    var contatosDoBanco = await _service.GetAllContatos();
                    model.TodosContatosDisponiveis = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome, Email = c.email }).ToList();
                    return View(model);
                }

                var listaIds = model.ContatosSelecionadosIds ?? new List<int>();

                await _service.Update(id, model.Titulo, model.Descricao, model.DataInicio, model.DataFim, model.Status, listaIds);

                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");

            }
            catch (Exception err)
            {
                var contatosDoBanco = await _service.GetAllContatos();
                model.TodosContatosDisponiveis = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.id, Nome = c.nome, Email = c.email }).ToList();

                TempData["MensagemTarefa"] = "Erro Mensagem:  " + err.Message;
                return View(model);
            }

        }


        [HttpPost]
        public async Task<IActionResult> Excluir(int id, string RotaDeRetorno)
        {
            try
            {
                await _service.Delete(id);

                TempData["MensagemTarefa"] = "Tarefa apagada com sucesso!";

                if (!string.IsNullOrEmpty(RotaDeRetorno))
                    return Redirect(RotaDeRetorno);

                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                TempData["MensagemTarefa"] = "Não foi possível excluir: " + err.Message;
                return RedirectToAction("Editar", new { id = id });
            }


        }
    }
}