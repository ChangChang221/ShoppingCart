
using ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly ShoppingCartContext context;

        public MainMenuViewComponent(ShoppingCartContext context)
        {
            this.context = context;
        }
        // phuong thuc goi khong dong bo neu chung khong dong bo
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }


        private Task<List<Page>> GetPagesAsync()
        {
            return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
