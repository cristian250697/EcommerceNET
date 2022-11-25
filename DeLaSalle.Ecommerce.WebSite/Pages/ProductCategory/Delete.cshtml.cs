using DeLaSalle.Ecommerce.Core.Dto;
using DeLaSalle.Ecommerce.Core.Http;
using DeLaSalle.Ecommerce.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeLaSalle.Ecommerce.WebSite.Pages.ProductCategory;

public class Delete : PageModel
{
    [BindProperty] public ProductCategoryDto ProductCategoryDto { get; set; }
    private readonly IProductCategoryService _service;
    private bool deleteStatus = false;

    public Delete(IProductCategoryService service)
    {
        ProductCategoryDto = new ProductCategoryDto();
        _service = service;
    }
    
    public async Task<ActionResult> OnGet(int? id)
    {
        ProductCategoryDto = new ProductCategoryDto();
        
        if (id.HasValue)
        {
            // Get information from the service (API)
            var response = await _service.GetById(id.Value);
            
            ProductCategoryDto = response.Data;

            if (ProductCategoryDto == null)
            {
                return RedirectToPage("/Error");
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        Response<bool> response;
        
        // Delete
        response = await _service.DeleteAsync(ProductCategoryDto.Id);
        deleteStatus = response.Data;
        
        if (deleteStatus)
        {
            return RedirectToPage("./List");
        }

        return Page();

    }

}