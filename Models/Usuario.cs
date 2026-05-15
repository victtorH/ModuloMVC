using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ModuloMVC.Models
{
    public class Usuario
    {
    public string UserId { get; private set; } 
    public string NomeUsuario { get; private set; }
    public string Email {get; private set;}
    public string? Avatar {get; private set;}
    public string? Bio {get; private set;}
    public bool ConexaoGoogleAtiva { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime DataAtualizacao {get; private set;}



    public Usuario(string userid, string nomeUsuario, string email,string avatar)
    {
        Validar(nomeUsuario, email);
        UserId = userid;
        NomeUsuario = nomeUsuario;
        Email = email;
        Avatar = avatar;
        Bio = null;
        ConexaoGoogleAtiva = false; 
        DataCriacao = DateTime.UtcNow;

    }

    private Usuario() { }

    public void UpdateUsuario(string nameuser, string avatar, string? bio)
    {       
           Validar(nameuser, Email, bio);

            NomeUsuario = nameuser;
            Avatar = avatar;
            Bio = bio;
            DataAtualizacao = DateTime.UtcNow;

        
    }

    public void VincularGoogle()
    {
        ConexaoGoogleAtiva = true;
    }

    private void Validar(string nomeUsuario, string email, string? bio = null)
    {
            if(nomeUsuario.Length < 3 || string.IsNullOrWhiteSpace(nomeUsuario))throw new Exception("O nome precisa ter mais de 3 caracteres");
            if(string.IsNullOrWhiteSpace(email) || !email.Contains("@"))throw new Exception("O email é obrigatorio");
            if(!string.IsNullOrWhiteSpace(bio) && bio.Length > 50) throw new Exception("A bio está muito grande, o maximo de caracteres é 50");
    }

}
}