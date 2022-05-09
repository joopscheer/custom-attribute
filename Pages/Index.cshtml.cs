using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace custom_attribute.Pages;

[BindProperties]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [Display(Name = "Name")]
    [RequiredIfOtherPropertyTrue(nameof(NameRequired), ErrorMessage = "Name is required!")] // doesn't work
    public string Name { get; set; }

    public bool NameRequired { get; set; }

    public BikeModel Bike { get; set; }

    public bool Success { get; set; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Success = true;

        return Page();
    }

    public class BikeModel
    {
        [Required(ErrorMessage = "Brand is required!")]
        public string Brand { get; set; }

        [RequiredIfOtherPropertyTrue(nameof(RequireModel), ErrorMessage = "Model is required!")] // this one works
        public string Model { get; set; }

        public bool RequireModel { get; set; }
    }
}
