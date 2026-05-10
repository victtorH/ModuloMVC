using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Enum;
using Microsoft.AspNetCore.Identity;
using Humanizer;

namespace ModuloMVC.Models
{
    public class Contato
    {
        public int Id { get; private set; }
        public string UserId {get; set;}
        public IdentityUser User {get; set;}
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public bool Status { get; private set; }
        public string? Descricao { get; private set; }
         private readonly List<Tarefa> _tarefasEnvolvidas;
        public IReadOnlyCollection<Tarefa> TarefasEnvolvidas => _tarefasEnvolvidas.AsReadOnly();

        public Contato(string nome, string email, string telefone, string? descricao)
        {

            Validar(nome, email, telefone, descricao);

            Nome = nome;
            Email = email;
            Telefone = telefone;
            Status = true;
            Descricao = descricao;

        }

        public Contato()
        {
            _tarefasEnvolvidas = new List<Tarefa>();
        }

        public void AtualizarDados(string nome, string email, string telefone, bool status, string? descricao)
        {
            Validar(nome, email, telefone, descricao, status);

            Nome = nome;
            Email = email;
            Telefone = telefone;
            Status = status;
            Descricao = descricao;
        }

        public void AtualizarListaTarefas(List<Tarefa> tarefas)
        {
           var tarefaspararemover = _tarefasEnvolvidas.Where(t => !tarefas.Any(nt => nt.Id == t.Id)).ToList();
            foreach (var tarefa in tarefaspararemover)
            {
                _tarefasEnvolvidas.Remove(tarefa);
            }

            foreach (var tarefa in tarefas)
            {
                if (!_tarefasEnvolvidas.Any(t => t.Id == tarefa.Id))
                {
                    _tarefasEnvolvidas.Add(tarefa);
                }
            }

        }


        private void Validar(string nome, string email, string telefone, string descricao = null, bool status = true)
        {

            if( status == false)
                throw new ArgumentException("Contato inativo não pode ser atualizado.");

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Nomes, E-mails e Telefones são obrigatórios.");


            if (!email.Contains("@"))
                throw new ArgumentException("Formato de E-mail inválido.");

            if (!telefone.All(char.IsDigit))
                throw new ArgumentException("Telefone deve conter apenas números.");
            
            if( telefone.Length != 11)
                throw new ArgumentException("Telefone deve conter DDD e número");

            if (!string.IsNullOrWhiteSpace(descricao) && descricao.Length > 50)
                throw new ArgumentException("A descrição não pode exceder 50 caracteres.");
        }
    }
}
