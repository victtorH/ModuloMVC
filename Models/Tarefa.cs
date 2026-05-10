using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ModuloMVC.Enum;

namespace ModuloMVC.Models
{
    public class Tarefa
    {
        public int Id { get; private set; }
        public string UserId {get; set;}
        public IdentityUser User {get; set;}
        public string? Titulo { get; private set; }
        public string? Descricao { get; private set; }
        public DateTime? DataInicio { get; private set; }
        public DateTime? DataFim{ get; private set; }
        public StatusTarefa Status { get; private set; }

        private readonly List<Contato> _contatosEnvolvidos;
        public IReadOnlyCollection<Contato> ContatosEnvolvidos => _contatosEnvolvidos.AsReadOnly();

        public Tarefa(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim)
        {
            Validar(titulo, descricao, dataInicio, dataFim);

            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Status = StatusTarefa.Pendente;
            _contatosEnvolvidos = new List<Contato>();
        }

        public void AtualizarTarefa(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim, StatusTarefa status)
        {
            Validar(titulo, descricao, dataInicio, dataFim);

            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Status = status;
        }


        public Tarefa()
        {
            _contatosEnvolvidos = new List<Contato>();
        }


        public void AdicionarContato(Contato contato)
        {
            if (contato == null) throw new ArgumentNullException("Os contatos passados não podem ser nulos");

            if (!_contatosEnvolvidos.Contains(contato))
            {
                _contatosEnvolvidos.Add(contato);
            }
        }

        public void AtualizarContatos(IEnumerable<Contato> novosContatos)
        {
            var contatosParaRemover = _contatosEnvolvidos.Where(c => !novosContatos.Any(nc => nc.Id == c.Id)).ToList();
            foreach (var contato in contatosParaRemover)
            {
                _contatosEnvolvidos.Remove(contato);
            }

            foreach (var contato in novosContatos)
            {
                if (!_contatosEnvolvidos.Any(c => c.Id == contato.Id))
                {
                    _contatosEnvolvidos.Add(contato);
                }
            }
        }

        private void Validar(string? titulo, string? descricao, DateTime? dataInicio, DateTime? dataFim)
        {
            if (string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("O Título ou descrição precisão ser preenchidos.");

            if(dataInicio > dataFim )
                throw new ArgumentException("A data de início não pode ser posterior à data de fim.");
            if(dataInicio < DateTime.Now)
                throw new ArgumentException("A data de início não pode ser anterior à data atual.");
        }
    }
}