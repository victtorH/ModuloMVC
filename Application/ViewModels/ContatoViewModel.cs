using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloMVC.Application.ViewModels
{

    public class ContatoViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatorio")]
        public int Id {get; set;}
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatorio")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "O telefone é obrigatorio")]
        public string Telefone { get; set; } = string.Empty;
        [Required(ErrorMessage = "O Status não pode ser nulo")]
        public bool Status { get; set; }
        public string? Descricao { get; set; }

    }
}