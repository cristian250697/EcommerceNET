﻿using DeLaSalle.Ecommerce.Core.Dto;
using DeLaSalle.Ecommerce.Core.Http;
using DeLaSalle.Ecommerce.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeLaSalle.Ecommerce.WebSite.Pages.ProductCategory;

public class Edit : PageModel
{
    
    [BindProperty] public ProductCategoryDto ProductCategoryDto { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    private readonly IProductCategoryService _service;

    public Edit(IProductCategoryService service)
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

        Response<ProductCategoryDto> response;

        if (ProductCategoryDto.Id > 0)
        {
            // Update
            response = await _service.UpdateAsync(ProductCategoryDto);
            
        }
        else
        {
            // Insert
            response = await _service.SaveAsync(ProductCategoryDto);
        }

        
        Errors = response.Errors;

        if (Errors.Count > 0)
        {
            return Page();
        }
        
        ProductCategoryDto = response.Data;
        return RedirectToPage("./List");
    }
}