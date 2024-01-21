using ProjeYonetimi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjeYonetimi.Controllers
{
    public class ManagerController : Controller
    {
        private readonly DbContextViewModel _context;

        public ManagerController()
        {
            _context = new DbContextViewModel();
        }

        [HttpGet]
        public ActionResult ManagerLoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManagerLoginPage(string username, string password)
        {

            string encryptedPassword = HomeController.PasswordEncrypt(password);
            Session["ManagerUsername"] = username;

            var Manager = _context.Managers.FirstOrDefault(m => m.username == username && m.password == encryptedPassword);


            if (Manager != null)
            {
                return RedirectToAction("ManagerMainPage");
            }

            string errorMessage = "Invalid username or password.";
            ViewData["ErrorMessage"] = errorMessage;
            return View();
        }

        public ActionResult ManagerMainPage()
        {
            if(Session["ManagerUsername"] == null)
                ViewBag.ErrorMessage = "Manager not found. Redirecting Login Page...";
            
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("ManagerUsername");
            return RedirectToAction("ManagerLoginPage", "Manager");
        }

        [HttpGet]
        public ActionResult ManagerUpdateAccount()
        {
            try
            {
                string ManagerUsername = Session["ManagerUsername"] as string;
                Managers Manager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);


                if (Manager != null)
                {
                    return View(Manager);
                }
                else
                {
                    ViewBag.ErrorMessage = "Manager not found. Redirecting Login Page...";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";

                if (ex.InnerException != null)
                {
                    ViewBag.InnerErrorMessage = ex.InnerException.Message;
                }

                return View();
            }
        }
        [HttpPost]
        public ActionResult ManagerUpdateAccount(Managers updatedManager)
        {
            string ManagerUsername = Session["ManagerUsername"] as string;
            if (ModelState.IsValid)
            {
                try
                {
                    Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);

                    currentManager.username = updatedManager.username;
                    currentManager.name = updatedManager.name;
                    currentManager.surname = updatedManager.surname;
                    currentManager.email = updatedManager.email;
                    currentManager.phone_num = updatedManager.phone_num;

                    if (updatedManager.password != null)
                    {
                        string encryptedPassword = HomeController.PasswordEncrypt(updatedManager.password);
                        currentManager.password = encryptedPassword;
                    }



                    _context.SaveChanges();
                    Session["ManagerUsername"] = currentManager.username;
                    ViewBag.SuccessMessage = "Account information updated successfully. Redirecting mainpage...";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex + "Failed to update account information. Please try again.";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    var errorMessage = error.ErrorMessage;

                }

                ViewBag.ErrorMessage = "Invalid data. Please check your inputs.";
            }

            return View(updatedManager);
        }




    }
}