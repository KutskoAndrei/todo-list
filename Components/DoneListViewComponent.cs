using System.Collections.Generic;
using ToDoList.Models;
using Microsoft.AspNetCore.Mvc;
using ToDoList.ViewModels;
using ToDoList.AuxiliaryClasses;


namespace ToDoList.Components
{
    public class DoneListViewComponent : ViewComponent
    {
        //подключаем контекст данных
        private UserContext db;
        public DoneListViewComponent(UserContext context)
        {
            db = context;            
        }      

        public IViewComponentResult Invoke()
        {            
            //заводим вспомогательный класс, чтобы не писать лишний код
            CurrentUserInfo currentUser = new CurrentUserInfo();

            //получаем список сделанных дел авторизированного пользователя 
            List<DoneCase> doneList = currentUser.GetUserDoneList(db);
          
            //DCL - DeleteCasesList
            DelCasesList currentDCL = new DelCasesList { delCL = doneList };

            //и проверяем выборку на null - если новый пользователь или у него нет дел
            if (doneList == null)
                ViewBag.DoneList = "";
            else
                ViewBag.DoneList = doneList;

            return View(currentDCL);
        }
    }
}
