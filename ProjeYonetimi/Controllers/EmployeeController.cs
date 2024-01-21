using ProjeYonetimi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace ProjeYonetimi.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DbContextViewModel _context;

        public EmployeeController()
        {
            _context = new DbContextViewModel();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Employees model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.tc != null && _context.Employees.Any((System.Linq.Expressions.Expression<Func<Employees, bool>>)(u => u.tc == model.tc)))
                    {
                        throw new InvalidOperationException("This TC is registered.");
                    }


                    if (model.email != null && _context.Employees.Any((System.Linq.Expressions.Expression<Func<Employees, bool>>)(u => u.email == model.email)))
                    {
                        throw new InvalidOperationException("This email is already registered.");
                    }

                    var user = new Employees
                    {
                        tc = model.tc,
                        name = model.name,
                        surname = model.surname,
                        phone_num = model.phone_num,
                        email = model.email,
                        duty_id = model.duty_id,

                    };

                    _context.Employees.Add(user);
                    _context.SaveChanges();

                    ViewBag.SuccessMessage = "Registration successful. You are being redirected to the login page.";

                    return View("Register", model);
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing your request." + ex;

                    return View(model);
                }
                catch (InvalidOperationException ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(model);
                }
            }

            ViewBag.ErrorMessage = "Please enter your information correctly.";
            return View(model);
        }

        [HttpGet]
        public ActionResult ManageEmployees()
        {

            try
            {
                string ManagerUsername = Session["ManagerUsername"] as string;
                Managers Manager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);

                if (Manager != null)
                {
                    //manager filter

                    List<EmployeeViewModel> employees = (from user in _context.Employees
                                                         join duties in _context.Duties on user.duty_id equals duties.duty_id into dutyJoin
                                                         from duty in dutyJoin.DefaultIfEmpty()
                                                         join projects in _context.Projects on duty.project_id equals projects.project_id into projectJoin
                                                         from project in projectJoin.DefaultIfEmpty()
                                                         join managers in _context.Managers on project.manager_id equals managers.manager_id into managerJoin
                                                         from manager in managerJoin.DefaultIfEmpty()
                                                         select new EmployeeViewModel
                                                         {
                                                             tc = user.tc,
                                                             name = user.name,
                                                             surname = user.surname,
                                                             email = user.email,
                                                             phone_num = user.phone_num,
                                                             duty_id = user.duty_id,
                                                             duty_name = duty != null ? duty.duty_name : null,
                                                             project_name = project != null ? project.project_name : null,


                                                         }).ToList();


                    List<Duties> dutiesList = _context.Duties
                    .Where(duty => duty.duty_id != 6)
                    .ToList();

                    var viewModel = new EmployeeViewModel
                    {
                        EmployeesViewModelList = employees,
                        DutiesList = dutiesList,
                    };

                    if (employees == null || !employees.Any())
                    {
                        ViewBag.ErrorMessage = "No Employees Found";
                    }

                    return View(viewModel);
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
        public ActionResult EmployeeUpdate(string update_tc,int default_duty_id, int update_duty_id, string update_name, string update_surname, string update_email, string update_phone_num, string update_duty)
        {

            var employee = _context.Employees.Find(update_tc);

            if (employee != null)
            {
                try
                {
                    employee.name = update_name;
                    employee.surname = update_surname;
                    employee.email = update_email;
                    employee.phone_num = update_phone_num;
                    var duty = _context.Duties.Find(update_duty_id);
                    employee.duty_id = duty.duty_id;

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Employee updated successfully!";
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the employee." + e;
                }

            }
            return RedirectToAction("ManageEmployees", "Employee");
        }

        [HttpPost]
        public ActionResult FireEmployee(string tc)
        {
            try
            {
                var employee = _context.Employees.Find(tc);
                if (employee == null)
                {
                    return HttpNotFound();
                }

                employee.duty_id = 6;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "You fired selected Employee.";
                return RedirectToAction("ManageEmployees", "Employee");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while firing the employee." + ex;

                if (ex.InnerException != null)
                {
                    ViewBag.InnerErrorMessage = ex.InnerException.Message;
                }

                return View("ManageEmployees", "Employee");
            }
        }
    }
}