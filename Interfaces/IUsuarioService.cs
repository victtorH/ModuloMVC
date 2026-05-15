using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloMVC.Interfaces
{
    public interface IUsuarioService
    {
        Task<(string id,string nameuser, string email, string avatar, string? bio)> GetMyUser(string id);
        Task UpdateUser(string id,string nameuser, string avatar, string? bio);
        Task ResetPassword(string NewPassword, string Password);
        Task ResetEmail(string NewEmail, string Email);
        Task SingIn( string nameuser,string email,string password);
        Task Login(string email, string password, bool lembrar);
        Task Logout();

    }
}