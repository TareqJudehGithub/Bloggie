// Ignore Spelling: bloggie

using Bloggie.Data;
using Bloggie.Models.Domain;
using Bloggie.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Controllers
{
    public class AdminTagsController : Controller
    {
        #region Fields
        private readonly BloggieDbContext _dbContext;
        #endregion

        #region Constructor
        public AdminTagsController(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public IActionResult Index()
        {
            var model = _dbContext.Tags.ToList();

            return View(model);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddTagRequestVM addTagRequest)
        {
            // 1. 'manual way' of model binding Model data, mapping AddTagRequestVM to Tag domain model       
            var model = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            // Saving to the database
            _dbContext.Tags.Add(model);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

