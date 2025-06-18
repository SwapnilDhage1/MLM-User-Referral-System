using DAL;
using MLMModel;
using System;
using System.Web.Mvc;

namespace MLM_task.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDAL _userDAL;

        public UserController()
        {
            _userDAL = new UserDAL();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var user = _userDAL.RegisterUser(dto);
                TempData["Message"] = "User registered successfully!";
                return RedirectToAction("Referrals", new { userId = user.SponsorId ?? user.Id });

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sponsor"))
                    ModelState.AddModelError("SponsorId", ex.Message);
                else if (ex.Message.Contains("Email"))
                    ModelState.AddModelError("Email", ex.Message);
                else
                    ModelState.AddModelError("", "Error: " + ex.Message);

                return View(dto);
            }
        }

        [HttpGet]
        public ActionResult Referrals(int userId)
        {
            if (userId <= 0)
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("Register");
            }

            try
            {
                var referralTree = _userDAL.GetReferralTree(userId);
                return View(referralTree);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Register");
            }
        }
    }
}
