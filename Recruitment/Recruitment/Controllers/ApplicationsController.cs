using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Core;
using Recruitment.Data;
using Recruitment.Utils;

namespace Recruitment.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IApplicationData _applicationData;

        public ApplicationsController(IApplicationData applicationData)
        {
            _applicationData = applicationData;
        }

        [Authorize]
        [HttpGet]
        [UserType(UserType.User)]
        [ActionName("Index")]
        public IActionResult IndexUser()
        {
            var email = User.FindFirst("emails").Value;
            var applications = _applicationData.GetUserApplications(email);
            var model = new ApplicationsViewModel();
            model.Applications = applications;
            model.UserType = UserType.User;
            return View(model);
        }

        [Authorize]
        [HttpGet]
        [UserType(UserType.Admin)]
        [ActionName("Index")]
        public IActionResult IndexAdmin()
        {
            var applications = _applicationData.GetAll();
            var model = new ApplicationsViewModel();
            model.Applications = applications;
            model.UserType = UserType.Admin;

            return View(model);
        }

        [Authorize]
        [HttpGet]
        [UserType(UserType.Hr)]
        [ActionName("Index")]
        public IActionResult IndexHr()
        {
            var email = User.FindFirst("emails").Value;
            var applications = _applicationData.GetHrMemberApplications(email);
            var model = new ApplicationsViewModel();
            model.Applications = applications;
            model.UserType = UserType.Hr;

            return View(model);
        }

        [Authorize]
        [UserType(UserType.Hr)]
        [HttpGet]
        public IActionResult Evaluate(int id)
        {
            var application = _applicationData.FindById(id);
            if (application == null)
                return RedirectToAction("Index");

            return View(application);
        }

        [Authorize]
        [HttpGet]
        [ActionName("Index")]
        public IActionResult IndexDefault()
        {
            return RedirectToAction("SignIn", "Auth");
        }

        [Authorize]
        [UserType(UserType.Hr)]
        public IActionResult Accept(int id, string accept = "true")
        {
            var application = _applicationData.GetAll().FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
                return RedirectToAction("Index");
            
            if (accept != "true")
            {
                application.State = "Deny";
            }
            else
            {
                application.State = "Accept";
            }
            _applicationData.Update(application);
            _applicationData.Commit();

            return RedirectToAction("Index");
        }
    }
}