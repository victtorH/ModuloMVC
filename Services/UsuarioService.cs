using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Interfaces;
using ModuloMVC.Models;

namespace ModuloMVC.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUsuarioAuthGateway _authgateway;
        public readonly IUsuarioRepository _usuario;

        public UsuarioService(IUsuarioAuthGateway authgateway, IUsuarioRepository usuario)
        {
            _authgateway = authgateway;
            _usuario = usuario;
        }

        public async Task<(string id,string nameuser, string email, string avatar, string? bio)> GetMyUser(string id)
        {
            var user = await _usuario.GetMyUser(id);
            return(user.UserId,user.NomeUsuario,user.Email,user.Avatar,user.Bio);
        }

        public async Task Login(string email, string password, bool lembrar)
        {
            await _authgateway.Login(email, password, lembrar);
        }

        public async Task Logout()
        {
            await _authgateway.Logout();
        }

        public Task ResetEmail(string NewEmail, string Email)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(string NewPassword, string Password)
        {
            throw new NotImplementedException();
        }

        public async Task SingIn(string nameuser, string email,string password)
        {
            await _usuario.ExistUser(email,password);
            await _authgateway.SingIn(nameuser,email, password);
            
        }

        public async Task UpdateUser(string id,string nameuser, string avatar, string? bio)
        {
            var user = await _usuario.GetMyUser(id);
            user.UpdateUsuario(nameuser,avatar,bio);
            await _usuario.UpdateUsuario(user);
        }
    }
}