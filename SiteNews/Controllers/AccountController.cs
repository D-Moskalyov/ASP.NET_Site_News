using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
//using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
//using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SiteNews.Filters;
using SiteNews.Models;
using System.Data.SqlClient;
using System.Text;

namespace SiteNews.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //InitializeSimpleMembershipAttribute u = new InitializeSimpleMembershipAttribute();
            //InitializeSimpleMembershipAttribute._initializer = new InitializeSimpleMembershipAttribute.SimpleMembershipInitializer();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                if (DataBase.GetSqlConnection().State == System.Data.ConnectionState.Open)
                {
                    string query = string.Format("select * from Users where Name = '{0}'", model.UserName);
                    SqlCommand command = new SqlCommand(query, DataBase.Sql);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Непредвиденные проблемы с БД.");
                }
            }
            else
            {
                if (DataBase.GetSqlConnection().State == System.Data.ConnectionState.Open)
                {
                    string query = string.Format("select * from Users where Name = '{0}'", model.UserName);
                    SqlCommand command = new SqlCommand(query, DataBase.Sql);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader["Pass"].ToString() == DataBase.GetMd5Hash(DataBase.Md5Hash, model.Password))
                        {
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                            WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);

                            reader.Close();

                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
                    }
                    reader.Close();
                }
                else
                {
                    ModelState.AddModelError("", "Непредвиденные проблемы с БД.");
                }
                
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (DataBase.GetSqlConnection().State == System.Data.ConnectionState.Open)
            {
                string query = string.Format("select * from Users where Name = '{0}'", model.UserName);
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    WebSecurity.Logout();
                    ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(model.UserName); // deletes record from webpages_Membership table
                    ((SimpleMembershipProvider)Membership.Provider).DeleteUser(model.UserName, true); // deletes record from UserProfile table
                    //UserManager.LoadUsers(reader);
                    reader.Close();

                    if (ModelState.IsValid)
                    {
                        // Попытка зарегистрировать пользователя
                        try
                        {
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                            WebSecurity.Login(model.UserName, model.Password);

                            string queryIns = string.Format("INSERT INTO Users (Name, Pass) VALUES ('{0}', '{1}')",
                                model.UserName, DataBase.GetMd5Hash(DataBase.Md5Hash, model.Password));
                            SqlCommand commandIns = new SqlCommand(queryIns, DataBase.Sql);
                            commandIns.ExecuteNonQuery();

                            return RedirectToAction("Index", "Home");
                        }
                        catch (MembershipCreateUserException e)
                        {
                            ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                        }
                    }
                }
                else
                {
                    reader.Close();
                    ModelState.AddModelError("", "Пользователь с таким именем уже зарегистрирован");
                }
            }
            else
            {
                ModelState.AddModelError("", "Непредвиденные проблемы с БД.");
            }
            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }


        //GET: /Account/Manage

        //////public ActionResult Manage(ManageMessageId? message)
        //////{
        //////    ViewBag.StatusMessage =
        //////        message == ManageMessageId.ChangePasswordSuccess ? "Пароль изменен."
        //////        : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
        //////        : message == ManageMessageId.RemoveLoginSuccess ? "Внешняя учетная запись удалена."
        //////        : "";
        //////    ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //////    ViewBag.ReturnUrl = Url.Action("Manage");
        //////    return View();
        //////}

        //
        // POST: /Account/Manage

        //////[HttpPost]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult Manage(LocalPasswordModel model)
        //////{
        //////    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //////    ViewBag.HasLocalPassword = hasLocalAccount;
        //////    ViewBag.ReturnUrl = Url.Action("Manage");
        //////    if (hasLocalAccount)
        //////    {
        //////        if (ModelState.IsValid)
        //////        {
        //////            // В ряде случаев при сбое ChangePassword породит исключение, а не вернет false.
        //////            bool changePasswordSucceeded;
        //////            try
        //////            {
        //////                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //////            }
        //////            catch (Exception)
        //////            {
        //////                changePasswordSucceeded = false;
        //////            }

        //////            if (changePasswordSucceeded)
        //////            {
        //////                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //////            }
        //////            else
        //////            {
        //////                ModelState.AddModelError("", "Неправильный текущий пароль или недопустимый новый пароль.");
        //////            }
        //////        }
        //////    }
        //////    else
        //////    {
        //////        // У пользователя нет локального пароля, уберите все ошибки проверки, вызванные отсутствующим
        //////        // полем OldPassword
        //////        ModelState state = ModelState["OldPassword"];
        //////        if (state != null)
        //////        {
        //////            state.Errors.Clear();
        //////        }

        //////        if (ModelState.IsValid)
        //////        {
        //////            try
        //////            {
        //////                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //////                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //////            }
        //////            catch (Exception)
        //////            {
        //////                ModelState.AddModelError("", String.Format("Не удалось создать локальную учетную запись. Возможно, учетная запись \"{0}\" уже существует.", User.Identity.Name));
        //////            }
        //////        }
        //////    }

        //////    // Появление этого сообщения означает наличие ошибки; повторное отображение формы
        //////    return View(model);
        //////}


        //////[AllowAnonymous]
        //////[ChildActionOnly]
        //////public ActionResult ExternalLoginsList(string returnUrl)
        //////{
        //////    ViewBag.ReturnUrl = returnUrl;
        //////    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //////}


        #region Вспомогательные методы
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        //////internal class ExternalLoginResult : ActionResult
        //////{
        //////    public ExternalLoginResult(string provider, string returnUrl)
        //////    {
        //////        Provider = provider;
        //////        ReturnUrl = returnUrl;
        //////    }

        //////    public string Provider { get; private set; }
        //////    public string ReturnUrl { get; private set; }

        //////    public override void ExecuteResult(ControllerContext context)
        //////    {
        //////        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //////    }
        //////}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion
    }
}
