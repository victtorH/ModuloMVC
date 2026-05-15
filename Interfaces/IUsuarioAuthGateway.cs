using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ModuloMVC.Interfaces
{
    public interface IUsuarioAuthGateway 
    {
        Task<string> SingIn(string nameuser,string email, string password);
        Task Login(string email, string password, bool lembrar = false );
        Task Logout();
    }
}