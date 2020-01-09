using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.WebUI.ViewModels.Users;

namespace UserManagement.WebUI.Controllers
{
    public class UsersController : Controller
    {
        public UsersController()
        {
        }

        public IActionResult Index(string name, int? page)
        {
            IEnumerable<User> users = UsersRepository.FindByName(name);

            var vm = new UsersVm(users, name, page);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            User user = UsersRepository.SingleByExternalId(id);

            var vm = new EditVm(user);

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditVm input)
        {
            if (ModelState.IsValid == false)
                return View(nameof(Edit), input);
            
            if (UsersRepository.TryUpdate(input.ToModel()))
                TempData["Success"] = "The changes were saved.";
            else
                TempData["Error"] = "An error occured and the changes were NOT saved.";

            return RedirectToAction(nameof(Edit), new { Id = input.Id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateVm());
        }

        [HttpPost]
        public IActionResult Create(CreateVm input)
        {
            if (ModelState.IsValid == false)
                return View(nameof(Create), input);
            
            Guid createdId = UsersRepository.Create(input.ToModel());
            TempData["Success"] = "The user was created.";

            return RedirectToAction(nameof(Edit), new { Id = createdId });
        }

        public IActionResult CreateRandom()
        {
            UsersRepository.CreateRandom();

            TempData["Success"] = "A randomly generated user was created.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Load()
        {
            UsersRepository.Load();

            TempData["Success"] = "Users were loaded.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Save()
        {
            UsersRepository.Save();

            TempData["Success"] = "Users were saved.";

            return RedirectToAction(nameof(Index));
        }
    }
}