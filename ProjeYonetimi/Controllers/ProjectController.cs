using ProjeYonetimi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace ProjeYonetimi.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DbContextViewModel _context;

        public ProjectController()
        {
            _context = new DbContextViewModel();                                                //
        }
        [HttpGet]
        public ActionResult ManageProject()
        {

            string ManagerUsername = Session["ManagerUsername"] as string;
            Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);
            if (currentManager == null)
            {                                                                                                           //
                ViewBag.ErrorMessage = "Manager not found. Redirecting Login Page...";
                return View();
            }

            List<Projects> projects = _context.Projects
                                .Where(project => project.project_id != 1)
                .ToList();                                                                                  //

            var projectViewModel = new ProjectViewModel
            {
                ProjectsList = projects
            };

            return View(projectViewModel);
        }

        [HttpPost]
        public ActionResult ManageProject(ProjectViewModel model)
        {
            string ManagerUsername = Session["ManagerUsername"] as string;
            Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);

            try
            {
                var project = new Projects
                {
                    project_name = model.project_name,
                    project_description = model.project_description,
                    start_date = model.start_date,                                                              //
                    end_date = model.end_date,
                    project_status = "Waiting",
                    delay_time = 0,
                    manager_id = currentManager.manager_id,
                };
                model.ProjectsList = ProjectCreate(project);

                return View("ManageProject", model);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "An error occurred while processing your request." + ex;

                return View(model);
            }
        }
        [HttpPost]
        public List<Projects> ProjectCreate(Projects project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Projects.Add(project);
                    _context.SaveChanges();
                    ViewBag.SuccessMessage = "Project created successfully.";
                }                                                                   //
                else
                {
                    ViewBag.ErrorMessage = "Project information invalid.";
                }

                var projects = _context.Projects.Where(prj => prj.project_id != 1)
                .ToList();

                return projects;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the project." + ex;
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        [HttpPost]
        public ActionResult ProjectDelete(int project_id)
        {
            var project = _context.Projects.Find(project_id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var duties = _context.Duties.Where(duty => duty.project_id == project.project_id)
                               .ToList();
            
                                                                                            //
            foreach (var duty in duties)
            {
                DutyDelete(duty.duty_id);
            }
            _context.Projects.Remove(project);

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Project deleted.";
            return RedirectToAction("ManageProject", "Project");
        }

        [HttpPost]
        public ActionResult ProjectUpdate(Projects model)
        {

            var project = _context.Projects.Find(model.project_id);

            if (project != null && ModelState.IsValid)
            {


                                                                                                        //
                project.project_name = model.project_name;
                project.project_description = model.project_description;
                project.start_date = model.start_date;
                project.end_date = model.end_date;
                project.project_status = model.project_status;
                project.delay_time = model.delay_time;

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Project updated.";

            }
            return RedirectToAction("ManageProject", "Project");
        }

        [HttpGet]
        public ActionResult ManageDuties()
        {
            string ManagerUsername = Session["ManagerUsername"] as string;
            Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);
            if (currentManager == null)
            {
                ViewBag.ErrorMessage = "Manager not found. Redirecting Login Page...";
                return View();
            }

            //dont include empty values
            List<Projects> projects = _context.Projects
                                .Where(project => project.project_id != 1)
                .ToList();
            List<Duties> dutiesList = _context.Duties
                .Where(duty => duty.duty_id != 6)
                .ToList();

            var projectViewModel = new ProjectViewModel
            {
                ProjectsList = projects,
                DutiesList = dutiesList
            };

            return View(projectViewModel);
        }


        [HttpPost]
        public ActionResult ManageDuties(ProjectViewModel projectViewModel)
        {
            string ManagerUsername = Session["ManagerUsername"] as string;
            Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);

            try
            {
                var duty = new Duties
                {
                    project_id = projectViewModel.project_id,
                    duty_name = projectViewModel.duty_name,
                };
                projectViewModel.DutiesList = DutyCreate(duty);

                return RedirectToAction("ManageDuties", projectViewModel);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "An error occurred while processing your request." + ex;

                return View(projectViewModel);
            }
        }

        [HttpPost]
        public List<Duties> DutyCreate(Duties duty)
        {
            string ManagerUsername = Session["ManagerUsername"] as string;
            Managers currentManager = _context.Managers.FirstOrDefault(m => m.username == ManagerUsername);
            int project_id = duty.project_id;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Duties.Add(duty);
                    _context.SaveChanges();
                    ViewBag.SuccessMessage = "Duty created successfully.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Duty information invalid.";
                }

                var duties = _context.Duties
                               .Where(related_projects => related_projects.project_id == project_id)
                               .ToList();

                return duties;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the project." + ex;
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        [HttpPost]
        public ActionResult DutyDelete(int duty_id)
        {
            var duties = _context.Duties.Find(duty_id);
            var employees = _context.Employees.Where(emplooye => emplooye.duty_id == duty_id)
                               .ToList();

            foreach (var employee in employees)
            {
                employee.duty_id = 6;
            }


            if (duties == null)
            {
                return HttpNotFound();
            }
            _context.Duties.Remove(duties);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Project deleted.";
            return RedirectToAction("ManageDuties", "Project");
        }
    }
}