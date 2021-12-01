using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ToDoList.ViewModels;
using ToDoList.AuxiliaryClasses;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        //подключаем контекст данных
        private UserContext db;
        public HomeController(UserContext context)
        {
            db = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {   
            return View();
        }

   

        [HttpPost]
        public async Task<IActionResult> AddCase(AddCaseModel addModel)
        {
            if (ModelState.IsValid)
            {
                //заводим вспомогательный класс, чтобы не писать лишний код
                CurrentUserInfo currentUser = new CurrentUserInfo();
                //получаем id пользователя
                int userId = currentUser.GetUserId(db);
                //и добавляем дело из модели в базу
                ToDoCase caseToAdd = new ToDoCase { Case = addModel.modelCase, UserId = userId };
                db.ToDoCases.Add(caseToAdd);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("Index");
        }

    

        [HttpPost]
        public async Task<IActionResult> DeleteCases(DelCasesList deleteCasesModel)
        {            
            //заводим вспомогательный класс, чтобы не писать лишний код
            CurrentUserInfo currentUser = new CurrentUserInfo();
            //получаем id пользователя
            int userId = currentUser.GetUserId(db);

            List<DoneCase> tempDoneCasesList = currentUser.GetUserDoneList(db);
            //удаляем из базы все сделанные дела, чтобы потом добавить выбранные пользователем
            db.DoneCases.RemoveRange(tempDoneCasesList);            

            //выгружаем список сделанных дел из формы
            List<DoneCase> fullDelCasesList = deleteCasesModel.delCL;            
            
            //обработка null - при пустом списке на удаление
            if (fullDelCasesList == null)
            {
                ViewBag.DoneList = "";
                return View("Index");
            }

            //заводим список для невыбранныз дел
            List<DoneCase> uncheckedCases = new List<DoneCase>();
            //добавим неывбранные дела и id пользователя в список              
            foreach (DoneCase d in fullDelCasesList)
            {
                if (d.Selected == false) 
                {
                    if(d.Case != null)
                    {
                        DoneCase tempCase = new DoneCase { Case = d.Case, UserId = userId };
                        uncheckedCases.Add(tempCase);
                    }
                }   
            }
            
            //и добавляем этот список в базу  
            db.DoneCases.AddRange(uncheckedCases);
            await db.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }

        

        [HttpPost]
        public async Task<IActionResult> SaveToDbOnSort(string doneSpans, string todDoSpans)
        {
            //заводим вспомогательный класс, чтобы не писать лишний код
            CurrentUserInfo currentUser = new CurrentUserInfo();
            //получаем id пользователя
            int userId = currentUser.GetUserId(db);

            //проверям список сделанных дел из пердставления
            if (doneSpans == "")
            {
                ViewBag.DoneList = "";
            }
            //и если он не пуст, добавляем его в базу, предварительно удалив старый
            else
            {
                List<DoneCase> doneListToRemove = currentUser.GetUserDoneList(db);
                db.DoneCases.RemoveRange(doneListToRemove);
                string[] spanStings = doneSpans.Split('|');
                foreach (string s in spanStings)
                {
                    //отбрасываем возможные пустые и неправильные значения, т.к. в атрибутах верификации значение не может быть короче двух символов
                    if (s.Length > 1)
                    {
                        DoneCase tempCase = new DoneCase { Case = s, UserId = userId };
                        db.DoneCases.Add(tempCase);
                    }
                }
            }

            //то же самое делаем со списком будущих дел из представления
            if (todDoSpans == "")
            {
                ViewBag.ToDoList = "";
            }
            else
            {
                List<ToDoCase> toDoListToRemove = currentUser.GetUserToDoList(db);
                db.ToDoCases.RemoveRange(toDoListToRemove);
                string[] spanStrings = todDoSpans.Split('|');
                foreach (string s in spanStrings)
                {
                    //отбрасываем возможные пустые и неправильные значения, т.к. в атрибутах верификации значение не может быть короче двух символов
                    if (s.Length > 1)
                    {
                        ToDoCase tempCase = new ToDoCase { Case = s, UserId = userId };
                        db.ToDoCases.Add(tempCase);
                    }
                }
            }

            //и сохраняем базу
            await db.SaveChangesAsync();

            return View("Index");
        }


        public async Task<IActionResult> Logout()
        {
            //убираем аутентификацию из кук
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //переводим на страницу логина
            return RedirectToAction("Login", "Account");
        }

    }
}
