using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloMVC.ViewModels
{
    public class UserViewModel
    {   [Required(ErrorMessage = "O nome de usuario é obrigatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        public string nameuser {get; set;}

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        public  string email {get; set;}
        [Required(ErrorMessage = "A senha é obrigatorio")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 15 caracteres.")]
        public string password {get; set;}
        public  string avatar{get; set;} = "sdrt";
        [StringLength(50, ErrorMessage = "A bio deve conter até 50 caracteres.")]
        public string? bio {get; set;}
        public bool lembrar {get; set;} = false;



    }
}