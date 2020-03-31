using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Core;
using Recruitment.Data;
using Recruitment.Utils;

namespace Recruitment.Controllers
{
    public class HrController : Controller
    {
        private readonly IStaffMemberData _staff;

        public HrController(IStaffMemberData staff)
        {
            _staff = staff;
        }
        
        [Authorize]
        [UserType(UserType.Admin)]
        [HttpGet]
        public IActionResult Index()
        {
            var members = _staff.GetAllHr();

            return View(members);
        }
        
        [Authorize]
        [UserType(UserType.Admin)]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var member = _staff.GetAllHr().FirstOrDefault(h => h.Id == id);
            return View(member);
        }
        
        [Authorize]
        [UserType(UserType.Admin)]
        [HttpPost]
        public IActionResult Delete(HrMember member)
        {
            _staff.DeleteHr(member.Id);
            _staff.Commit();
            return RedirectToAction("Index");
        }

        [Authorize]
        [UserType(UserType.Admin)]
        [HttpGet]
        public IActionResult Create()
        {
            var member = new HrMember();
            return View(member);
        }
        
        [Authorize]
        [UserType(UserType.Admin)]
        [HttpPost]
        public IActionResult Create(HrMember member)
        {
            _staff.AddHr(member);
            _staff.Commit();
            return RedirectToAction("Index");
        }
    }
}