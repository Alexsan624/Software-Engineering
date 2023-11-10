using System.ComponentModel.DataAnnotations;

namespace DogsApp.Models
{
    public class DogDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Breed")]
        public string Breed { get; set; }

        [Display(Name = "Dog Picture")]
        public string? Picture { get; set; }
    }
}
