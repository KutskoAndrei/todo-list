using System.Collections.Generic;
using System.Linq;
using ToDoList.Models;
using Microsoft.AspNetCore.Http;


namespace ToDoList.AuxiliaryClasses
{
    //вспомогательный класс, чтобы не плодить одинаковый код в контроллерах и компонентах

    public class CurrentUserInfo
    {

        //получаем из кук email зарегистрированного пользователя(в Startup добавлен сервис AddHttpContextAccessor()
        private static HttpContext httpContext; //=> new HttpContextAccessor().HttpContext;
        private string currentUserIdentity; // = httpContext.User.Identity.Name;

        public CurrentUserInfo()
        {
            httpContext = new HttpContextAccessor().HttpContext;
            currentUserIdentity = httpContext.User.Identity.Name;
        }     

        //возвращаем залогиненого юзера
        public User GetUserFromDb(UserContext context)
        {   
            User user = context.Users.FirstOrDefault(u => u.Email == currentUserIdentity);
            return user;
        }
        //возвращаем UserId залогиненого юзера
        public int GetUserId(UserContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == currentUserIdentity);
            return user.UserId;
        }
        //возвращаем список будущих дел залогиненого юзера
        public List<ToDoCase> GetUserToDoList(UserContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == currentUserIdentity);
            List<ToDoCase> toDoList = context.ToDoCases.Where(c => c.UserId == user.UserId).ToList();
            return toDoList;
        }
        //возвращаем список сделанных дел залогиненого юзера
        public List<DoneCase> GetUserDoneList(UserContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == currentUserIdentity);
            List<DoneCase> doneCases = context.DoneCases.Where(c => c.UserId == user.UserId).ToList();
            return doneCases;
        }
    }
}
