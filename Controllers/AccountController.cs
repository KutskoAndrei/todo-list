using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace ToDoList.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                //если email есть в базе
                if (user != null)
                {
                    if(user.Password != model.Password)
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                        return View(model);
                    }
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }
                //если пароля нет в базе, то заводим нового пользователя(вместо регистрации)
                await Register(model);
                await Authenticate(model.Email);
                return RedirectToAction("Index", "Home");
                
            }
            return View(model);
        }


        //Дополнительные методы:

        //добавление в базу нового пользователя(вместо регистраии)
        private async Task Register(LoginModel newUser)
        {
            db.Users.Add(new Models.User { Email = newUser.Email, Password = newUser.Password });
            await db.SaveChangesAsync();
        }

        //аутентификация
        private async Task Authenticate(string userName)
        {
            //создаем claim
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, userName));
            //cоздаем объект ClaimIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            //помещение аутентификации в куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
