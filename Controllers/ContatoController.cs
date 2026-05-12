using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModuloMVC.ViewModels;
using ModuloMVC.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace ModuloMVC.Controllers
{
    [Authorize]
    public class ContatoController : Controller
    {

        private readonly IContatoService _service;
        public ContatoController(IContatoService service)
        {
            _service = service;
        }


        [HttpGet]

        public async Task<IActionResult> Index(string? nome, string? email, string? telefone)
        {

            var contatosFiltrados = await _service.GetAll(nome, email, telefone);

            var listacontatos = new List<ContatoViewModel>();
            foreach (var contato in contatosFiltrados)
            {
                listacontatos.Add(new ContatoViewModel
                {
                    Id = contato.id,
                    Nome = contato.nome,
                    Email = contato.email,
                    Telefone = contato.telefone
                });
            }
            return View(listacontatos);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(ContatoViewModel contato)
        {
            try
            {

                await _service.Create(contato.Nome, contato.Email, contato.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""), contato.Descricao);
                TempData["SucesseContato"] = "Contato criado com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                TempData["ErrorCotato"] = "Mensagem de Erro: " + err.Message;
                return View(contato);
            }


        }


        [HttpPost]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                await _service.Delete(id);
                TempData["SucesseContato"] = "Contato apagado com sucesso";
                return RedirectToAction("index");
            }
            catch (Exception err)
            {
                TempData["ErrorCotato"] = "Mensagem de Erro: " + err.Message;
                return RedirectToAction("Detalhes", new { id = id });
            }


        }



        [HttpPost]
        public async Task<IActionResult> Editar(int id, ContatoViewModel contato)
        {

            try
            {
                await _service.Update(id, contato.Nome, contato.Email, contato.Telefone, contato.Status, contato.Descricao);
                TempData["SucesseContato"] = "Autualizado com sucesso";
                return RedirectToAction("Detalhes", new { id = id });
            }
            catch (Exception err)
            {
                TempData["ErrorCotato"] = "Mensagem de erro: " + err.Message;
                return RedirectToAction("Detalhes", new { id = id });
            }



        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {

            var contato = await _service.GetById(id);
            var contatoViewModel = new ContatoViewModel
            {
                Id = contato.id,
                Nome = contato.nome,
                Email = contato.email,
                Telefone = contato.telefone,
                Descricao = contato.descricao,
                Status = contato.status,
                Tarefas = contato.Item7.Select(t => new TarefaViewModel
                {
                    Id = t.id,
                    Titulo = t.titulo,
                    DataInicio = t.datainicio,
                    DataFim = t.datafim,
                    Status = t.status

                }).ToList()
            };
            return View(contatoViewModel);

        }
    }
}