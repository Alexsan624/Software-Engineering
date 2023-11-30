using System.Security.Claims;

using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Domain;
using DogsApp.Models;
using DogsApp.Models.Breed;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;

namespace DogsApp.Controllers
{
    [Authorize]
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        private readonly IBreedService _breedService;
        public DogController(IDogService dogService, IBreedService breedService)
        {
            _dogService = dogService;
            _breedService = breedService;
        }
        // GET: DogController

        [AllowAnonymous]
        public IActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed, searchStringName)
                .Select(item => new DogAllViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Age = item.Age,
                    BreedName = item.Breed.Name,
                    Picture = item.Picture,
                    FullName = item.Owner.FirstName + " " + item.Owner.LastName
                }).ToList();
            return View(dogs);
        }

        // GET: DogController/Details/5
        public IActionResult Details(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture,
                FullName = item.Owner.FirstName + " " + item.Owner.LastName
            };
            return View(dog);
        }

        // GET: DogController/Create
        public IActionResult Create()
        {
            var dog = new DogCreateViewModel();
            dog.Breeds = _breedService.GetBreeds().Select(
                c => new BreedPairViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
            return View(dog);


        }
        public IActionResult Success()
        {
            return this.View();
        }
        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] DogCreateViewModel dog)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var createdId = _dogService.Create(dog.Name, dog.Age,
                    dog.BreedId, dog.Picture, currentUserId);
                if (createdId)
                {
                    return this.RedirectToAction(nameof(Index));
                }
            }
            return this.View();
        }

        // GET: DogController/Edit/5
        public IActionResult Edit(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }
            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                BreedId = item.BreedId,
                Age = item.Age,
                Picture = item.Picture
            };
            dog.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DogCreateViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(id, bindingModel.Name, bindingModel.Age,
                    bindingModel.BreedId, bindingModel.Picture);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(bindingModel);
        }

        // GET: DogController/Delete/5
        public IActionResult Delete(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _dogService.RemoveById(id);
            if (deleted)
            {
                return this.RedirectToAction("Index", "Dog");
            }
            else
            {
                return View();
            }

        }

    }
}
