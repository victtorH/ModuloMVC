using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuloMVC.Models;

namespace ModuloMVC.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetMyUser(string id);
        Task UpdateUsuario(Usuario Usuario);
        Task ExistUser(string email, string password);
   
    }
}