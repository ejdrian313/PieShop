using Microsoft.AspNetCore.Mvc;
using PieShop.Models;
using PieShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(ICategoryRepository categoryRepository, IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        [Route("Pie/List/{category?}")]
        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory = string.Empty;


            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.Pies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.Pies.Where(p => p.Category.CategoryName == category)
                .OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryName == category).CategoryName;
            }

            return View(new PiesListViewModel
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
                return NotFound();

            return View(pie);
        }

        private bool IsCategoryExist(string category) => _pieRepository
            .Pies
            .Any(p => p.Category.CategoryName == category);
    }
}