using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModuloMVC.Models;
using ModuloMVC.Services;
using ModuloMVC.ViewModels;

namespace ModuloMVC.Controllers
{
    [Route("[controller]")]
    public class TarefaController : Controller
    {
        private readonly TarefaService _service;

        public TarefaController(TarefaService service)
        {
            _service = service;
        }

    [HttpGet]
    public async Task<IActionResult> Index()
        {
            var ListaDeTodos = await _service.ListarTodos();
            return View(ListaDeTodos);
        }



        [HttpGet("Criar")]
        // 1. O método precisa ser async porque vamos no banco buscar os contatos
        public async Task<IActionResult> Criar()
        {
            // 2. Busca os contatos originais (Entidades ricas) lá do banco de dados
            var contatosDoBanco = await _service.ListarContatos(); // Ou _context.Contatos.ToListAsync();

            // 3. A Tradução: Transforma a lista de 'Contato' em 'ContatoViewModel'
            var contatosParaTela = contatosDoBanco.Select(c => new ContatoViewModel
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Status = c.Status
            }).ToList();

            // 4. Cria a "mala" que vai viajar para a View, já com os contatos dentro
            var viewModel = new TarefaViewModel
            {
                ContatosEnvolvidos = contatosParaTela
            };


            return View(viewModel);
        }


        [HttpPost("Criar")]
        public async Task<IActionResult> Criar(TarefaViewModel tarefa)
        {

            if (!ModelState.IsValid)
            {
                var contatosDoBanco = await _service.ListarContatos();
                tarefa.ContatosEnvolvidos = contatosDoBanco.Select(c => new ContatoViewModel { Id = c.Id, Nome = c.Nome }).ToList();

                return View(tarefa);
            }
            var ListaIds = tarefa.ContatosSelecionadosIds ?? new List<int>();

            var TarefaNova = await _service.CriarUmaAsync(tarefa.Titulo, tarefa.Descricao, tarefa.Vencimento, ListaIds);
            return RedirectToAction("Index");
        }


    }
}