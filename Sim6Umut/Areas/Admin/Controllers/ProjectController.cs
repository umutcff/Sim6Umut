using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sim6Umut.Contexts;
using Sim6Umut.Helpers;
using Sim6Umut.Models;
using Sim6Umut.ViewModels.Project;

namespace Sim6Umut.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _envoriment;
        private readonly string _folderPath;

        public ProjectController(AppDbContext context, IWebHostEnvironment envoriment)
        {
            _context = context;
            _envoriment = envoriment;
            _folderPath = Path.Combine(_envoriment.WebRootPath, "assets", "images");
        }

        public async Task<IActionResult> Index()
        {
            var project = await _context.Projects.Include(x => x.Category).Select(x => new ProjectGetVM()
            {
                CategoryName=x.Category.Name,
                ImagePath=x.ImagePath,
                Id=x.Id,
                Name=x.Name

            }).ToListAsync();

            return View(project);
           
        }


        public async Task<IActionResult> Create()
        {
            await SendCategoryViewBag();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateVM vm)
        {
            await SendCategoryViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

            if (!isExistCategory)
            {
                ModelState.AddModelError("", "Bele Category movcud deyil");
                return View(vm);
            }

            if (!vm.Image.CheckSize(3))
            {
                ModelState.AddModelError("image", "Image 3mb-dan kicik olmalidir");
                return View(vm);
            }
            if (!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("image", "Daxil etdiyiniz fayl image tipinde olmalidi");
                return View(vm);
            }

            var uniqueImageName = Path.Combine(_folderPath);

            Project project = new()
            {
                Name=vm.Name,
                ImagePath=uniqueImageName,
                CategoryId=vm.CategoryId,
            };


            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deletedProject = await _context.Projects.FindAsync(id);

            if(deletedProject is null)
            {
                return NotFound();
            }

            string deletedImagePath = Path.Combine(_folderPath, deletedProject.ImagePath);

            ExtensionMethods.DeleteFile(deletedImagePath);

             _context.Projects.Remove(deletedProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            await SendCategoryViewBag();

            var updatedProject = await _context.Projects.FindAsync(id);
            
            if(updatedProject is null)
            {
                return BadRequest();
            }

            ProjectUpdateVM vm = new()
            {
                Id=updatedProject.Id,
                CategoryId=updatedProject.CategoryId,
                Name=updatedProject.Name
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProjectUpdateVM vm)
        {
            await SendCategoryViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var isUpdatedCategory = await _context.Categories.AnyAsync(x => x.Id == vm.Id);

            if (!isUpdatedCategory)
            {
                ModelState.AddModelError("", "Bele bir category movcud deyil");
            }

            if (!vm.Image.CheckSize(3))
            {
                ModelState.AddModelError("image", "Image 3mb-dan kicik olmalidir");
                return View(vm);
            }

            if (!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("image", "Daxil etdiyiniz fayl image tipinde olmalidi");
                return View(vm);
            }

            var updatedProject = await _context.Projects.FindAsync(vm.Id);

            updatedProject.Name = vm.Name;
            updatedProject.CategoryId = vm.CategoryId;
            
            if(vm.Image is { })
            {
                string newImagePath = await vm.Image.FileUploadAsync(_folderPath);
                string oldImagePath = Path.Combine(_folderPath, updatedProject.ImagePath);
                ExtensionMethods.DeleteFile(oldImagePath);
                updatedProject.ImagePath = newImagePath;
            }

            _context.Projects.Update(updatedProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }















        private async Task SendCategoryViewBag()
        {
            var categories = await _context.Categories.Select(x => new SelectListItem()
            {
                Value=x.Id.ToString(),
                Text=x.Name

            }).ToListAsync();

            ViewBag.Categories = categories;
        }
    }
}
