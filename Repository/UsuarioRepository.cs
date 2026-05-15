using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModuloMVC.Context;
using ModuloMVC.Interfaces;
using ModuloMVC.Models;

namespace ModuloMVC.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly TEnancyDB _context;

        public UsuarioRepository(TEnancyDB context)
        {
            _context = context;
        }

        public async Task ExistUser(string email, string password)
        {
         if( await _context.Usuario.AnyAsync(u => u.Email == email)) throw new Exception("Este e-mail já está cadastrado. Que tal fazer login ou recuperar sua senha?");

        }

        public async Task<Usuario> GetMyUser(string id)
        {
            
           var user = await _context.Usuario.FirstOrDefaultAsync(u => u.UserId == id);
           if(user == null) throw new Exception("Usuario não encontrado");
           return user;

        }



        public async Task UpdateUsuario(Usuario Usuario)
        {
           _context.Usuario.Update(Usuario);
           await _context.SaveChangesAsync();
        }
    }
}