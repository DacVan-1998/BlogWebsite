using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogWebsite.Models;
using BlogWebsite.EF;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BlogWebsite.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryDbContext _context;

        public CategoryController(ILogger<CategoryController> logger, CategoryDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var listItem = _context.Categories.Include(c=>c.CategoryChildren).AsEnumerable().Where(c=>c.ParentId==null).ToList();
            return View(listItem);
        }

        public IEnumerable<Category> GetItemsSelectCategorie()
        {

            var items = _context.Categories
                                .Include(c => c.CategoryChildren)
                                .Where(c => c.ParentCategory == null)
                                .ToList();

            List<Category> resultitems = new List<Category>() {
                                                new Category() {
                                                    Id = -1,
                                                    Title = "Không có danh mục cha"
                                                }
                                            };
            Action<List<Category>, int> _ChangeTitleCategory = null;
            Action<List<Category>, int> ChangeTitleCategory = (items, level) => {
                string prefix = String.Concat(Enumerable.Repeat("—", level));
                foreach (var item in items)
                {
                    item.Title = prefix + " " + item.Title;
                    resultitems.Add(item);
                    if ((item.CategoryChildren != null) && (item.CategoryChildren.Count > 0))
                    {
                        _ChangeTitleCategory(item.CategoryChildren.ToList(), level + 1);
                    }

                }
            };

            _ChangeTitleCategory = ChangeTitleCategory;
            ChangeTitleCategory(items, 0);

            return resultitems;
        }
        public IActionResult Create()
        {
           
            ViewData["ParentId"] = new SelectList(GetItemsSelectCategorie(), "Id", "Title", -1);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentId.Value == -1)
                {
                    category.ParentId = null;
                }
                _context.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Slug", category.ParentId);
         
            ViewData["ParentId"] = new SelectList(GetItemsSelectCategorie(), "Id", "Title", category.ParentId);
            return View(category);
        }

        public IActionResult Detail(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var item = _context.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _context.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList( GetItemsSelectCategorie(), "Id", "Title", item.ParentId);
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentId == -1)
                {
                    category.ParentId = null;
                }
                _context.Update(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = _context.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
