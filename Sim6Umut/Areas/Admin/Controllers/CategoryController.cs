using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sim6Umut.Contexts;
using Sim6Umut.Models;
using Sim6Umut.ViewModels.Category;

namespace Sim6Umut.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(AppDbContext _context) : Controller
    { 
        public async Task<IActionResult> Index()
        {
            var category = await _context.Categories.Select(x => new CategoryGetVM()
            {
                
                Id=x.Id,
                Name=x.Name

            }).ToListAsync();
            return View(category);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            Category category = new()
            {
                Name =vm.Name
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            var deletedCategory = await _context.Categories.FindAsync(id);

            if (deletedCategory == null)
            {
                return BadRequest();
            }

            _context.Categories.Remove(deletedCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var updatedCategory = await _context.Categories.FindAsync(id);

            if(updatedCategory is null)
            {
                return BadRequest();
            }

            CategoryUpdateVM vm = new()
            {
                Name = updatedCategory.Name
            };

          
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var updatedCategory = await _context.Categories.FindAsync(vm.Id);

            if(updatedCategory is null)
            {
                return BadRequest();
            }

            updatedCategory.Name = vm.Name;

            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
