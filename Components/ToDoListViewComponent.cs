using System.Collections.Generic;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;
using ToDoList.AuxiliaryClasses;


namespace ToDoList.Components
{
    public class ToDoListViewComponent : ViewComponent
    {
        //подключаем контекст данных
        private UserContext db;
        public ToDoListViewComponent(UserContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            //заводим вспомогательный класс, чтобы не писать лишний код
            CurrentUserInfo currentUser = new CurrentUserInfo();

            //получаем список будущих дел авторизированного пользователя 
            List<ToDoCase> toDoList = currentUser.GetUserToDoList(db);
            //и проверяем выборку на null - если новый пользователь или у него нет дел
            if (toDoList == null)
                ViewBag.ToDoList = "";
            else
                ViewBag.ToDoList = toDoList;

            return View();
        }
    }
}
