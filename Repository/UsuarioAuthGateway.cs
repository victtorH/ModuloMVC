using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ModuloMVC.Interfaces;
using ModuloMVC.Models;

namespace ModuloMVC.Repository
{
    public class UsuarioAuthGateway : IUsuarioAuthGateway
    {
        public readonly UserManager<ApplicationUser> _usermanager;
        public readonly SignInManager<ApplicationUser> _signinmanager;

        public UsuarioAuthGateway(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            _usermanager = usermanager;
            _signinmanager = signInManager;
        }

        public async Task Login(string email, string password, bool lembrar = false)
        {
            await _signinmanager.PasswordSignInAsync(email ,password,lembrar,false);
        }

        public async Task Logout()
        {
            await _signinmanager.SignOutAsync(); 
        }

        public async Task<string> SingIn(string nameuser,string email, string password)
        {
           var newUser = new ApplicationUser
    {
        UserName = email,
        Email = email,
        NomeUsuario = nameuser,
        Avatar = null,
        Bio = null,
        DataCriacao = DateTime.UtcNow,
        DataAtualizacao = DateTime.UtcNow,
        ConexaoGoogleAtiva = false
    };
            await _usermanager.CreateAsync(newUser,password);
            await Login(email,password);
            return  newUser.Id;

        }
    }
}