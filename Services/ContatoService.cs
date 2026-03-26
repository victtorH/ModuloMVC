using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using ModuloMVC.Context;
using ModuloMVC.Models;


namespace ModuloMVC.Services
{
    public class ContatoService
    {


        protected readonly AgendaContext _context;
        public ContatoService(AgendaContext context)
        {
            _context = context;
        }


        public List<Contato> ListarTodos()
        {
            return _context.Contato.OrderBy(c => c.Nome).ToList();

        }


        private void ValidarSeJaExiste(string email, string telefone, int idDesconsiderado = 0)
        {

            bool contatoDuplicado = _context.Contato.Any(c =>
                (c.Email == email || c.Telefone == telefone) &&
                c.Id != idDesconsiderado);

            if (contatoDuplicado)
            {
                throw new InvalidOperationException("Já existe um contato cadastrado com este E-mail ou Telefone.");
            }
        }

        public void CriarUm(string nome, string email, string telefone, string? descricao)
        {
            ValidarSeJaExiste(email, telefone);

            Contato NovoContato = new Contato(nome, email, telefone, descricao);

            _context.Contato.Add(NovoContato);
            _context.SaveChanges();
        }
 

        public Contato BuscarPorId(int id)
        {
            var contato = _context.Contato.Find(id);
            if (contato == null)
            {
                throw new ArgumentException("Contato solicitado não existe");
            }

            return contato;
        }

        public void DeletarUm(int id)
        {

            _context.Contato.Remove(BuscarPorId(id));
            _context.SaveChanges();
        }


        public void EditarUm(int id, string nome, string email, string telefone, bool status, string? descricao)
        {

            var contatodb = BuscarPorId(id);
            ValidarSeJaExiste(contatodb.Nome, contatodb.Email, id);


            contatodb.AtualizarDados(nome, email, telefone, status, descricao);
            _context.SaveChanges();
        }


    }
}