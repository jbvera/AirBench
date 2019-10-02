﻿using AirBench.Data.Repositories;
using AirBench.Models.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace AirBench.Controllers
{
    public class AccountController : Controller
    {
        private IAccountRepository _repo;

        public AccountController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            //var tempPassword = BCrypt.Net.BCrypt.HashPassword("john");
            var viewModel = new AccountLoginViewModel();
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginViewModel viewModel)
        {
            if (ModelState.IsValid &&
                ModelState.IsValidField("Username") &&
                ModelState.IsValidField("Password"))
            {
                var authenticated = _repo.Authenticate(viewModel.Username, viewModel.Password);
                if (authenticated)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);
                    return RedirectToAction("Index", "Bench");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(viewModel);
        }
    }
}