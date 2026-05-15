using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ModuloMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
  
    public string NomeUsuario { get; set; }
    public string? Avatar {get; set;}
    public string? Bio {get; set;}
    public bool ConexaoGoogleAtiva { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao {get; set;}

    }
}