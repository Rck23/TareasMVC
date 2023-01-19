using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using TareasMVC.Models;

namespace TareasMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsuariosController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if(!ModelState.IsValid)
            {
                return View(modelo);   
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Name }; 
            var resutado = await _userManager.CreateAsync(usuario, password: modelo.Password);

            if (resutado.Succeeded)
            {
                 await _signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resutado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>  Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await _signInManager.PasswordSignInAsync(modelo.Email,
                modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);

            if(resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");

                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
