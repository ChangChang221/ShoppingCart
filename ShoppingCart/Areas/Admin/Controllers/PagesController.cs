using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ShoppingCartContext context;

        public PagesController(ShoppingCartContext context)
        {
            this.context = context;
        }

        //GET Admin/Pages
        // GET /admin/pages
       /* public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;

            List<Page> pagesList = await pages.ToListAsync();

            return View(pagesList);
        }*/
        public async Task<IActionResult> Index()
        {
            var pages = from p in context.Pages orderby p.Sorting select p;
            return View(await pages.ToListAsync());
        }
        //GET Admin/Pages/details/5
        public async Task<IActionResult> Details(int? id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(x => x.id == id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
        [HttpGet]
        //GET Admin/Pages/details/5
        public IActionResult Create()
        {
            return View();
        }

        //POST /Admin/Pages/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists.");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added!";

                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET /Admin/Pages/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
        // POST /admin/pages/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(x => x.id != page.id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists.");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited!";

                return RedirectToAction("Edit", new { id = page.id });
            }

            return View(page);
        }

        // GET /admin/pages/delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Page page = await context.Pages.FindAsync(id);

            if (page == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been deleted!";
            }

            return RedirectToAction("Index");
        }

        // POST /admin/pages/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var pageId in id)
            {
                Page page = await context.Pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }


    }

}
