using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ModuloMVC.ViewModels;
using ModuloMVC.Interfaces;


namespace ModuloMVC.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : Controller
    {

        public readonly IUsuarioService _usuario;

        public UsuarioController(IUsuarioService usuario)
        {
            _usuario = usuario;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SingIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SingIn(UserViewModel user)
        {
             await _usuario.SingIn(user.nameuser, user.email, user.password);
              return RedirectToAction("Index", "Tarefa");
            
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user)
        {
                
        
            await _usuario.Login(user.email, user.password, user.lembrar);
            return RedirectToAction("Index", "Tarefa");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}