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

        public async Task<IActionResult> Index(string? nome, string? numero, string? email)
        {

            var contatosFiltrados = await _service.GetAll(nome, numero, email);

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

            await _service.Create(contato.Nome, contato.Email, contato.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""), contato.Descricao);
            return RedirectToAction("Index");


        }


        [HttpPost]
        public async Task<IActionResult> Deletar(int id)
        {

            await _service.Delete(id);
            return RedirectToAction("index");

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {

            var contato = await _service.GetById(id);
            var contatoViewModel = new ContatoViewModel
            {
                Id = contato.id,
                Nome = contato.nome,
                Email = contato.email,
                Telefone = contato.telefone,
                Descricao = contato.descricao,
                Status = contato.status
            };
            return View(contatoViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, ContatoViewModel contato)
        {

            await _service.Update(id, contato.Nome, contato.Email, contato.Telefone, contato.Status, contato.Descricao);
            return RedirectToAction("Detalhes", new { id = id });


        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            try
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
            catch (Exception err)
            {
                ModelState.AddModelError(string.Empty, err.Message);
                return RedirectToAction("Index");
            }
        }
    }
}