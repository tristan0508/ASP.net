﻿using Dogo.Models;
using Dogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnersController(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }

        // GET: OwnerController
        public ActionResult Index()
        {
            List<Owner> allOwners = _ownerRepo.GetOwners();

            return View(allOwners);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(owner);
            }
        }

        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetById(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: OwnerController/Create
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetById(id);

            return View(owner);
        }

        // POST: OwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: OwnerController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetById(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: OwnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
    }
}