using BookStore.DTOs.authorDTO;
using BookStore.DTOs.catalogDTO;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        UnitWork unit;
        public CatalogController(UnitWork unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public IActionResult getAllCatalogs() 
        {
            var catalogs = unit.Generic_CatalogRepo.selectAll();
            if (!catalogs.Any()) { return NotFound(); }
            else {
                
                List<SelectCatalogDTO> catalogsDTO = new List<SelectCatalogDTO>();
                foreach (var catalog in catalogs)
                {
                    SelectCatalogDTO catalogDTO = new SelectCatalogDTO()
                    {
                        Description = catalog.Description,
                        Id = catalog.Id,
                        Name = catalog.Name,
                    };
                    catalogsDTO.Add(catalogDTO);

                }
                return Ok(catalogsDTO);

            }
        
        }
        [HttpGet("{id}")]
        public IActionResult getCatalogById(int id)
        {
            var catalog = unit.Generic_CatalogRepo.selectById(id);
            if (catalog == null) { return NotFound(); }
            SelectCatalogDTO catalogDTO = new SelectCatalogDTO()
            {
                Description = catalog.Description,
                Id = catalog.Id,
                Name = catalog.Name,
            };
            return Ok(catalogDTO);
        }



        [HttpPost]
        [Authorize(Roles = "admin")]

        public IActionResult createCatalog(AddCatalogDTO catalogDTO)
        {

            if (ModelState.IsValid)
            {
                Catalog catalog = new Catalog
                {
                    Name = catalogDTO.Name,
                    Description = catalogDTO.Description
                };
     
                unit.Generic_CatalogRepo.add(catalog);
                unit.Save();
                return Ok();
            }
            else { return BadRequest(ModelState); }

        }


        [HttpPut]
        [Authorize(Roles = "admin")]

        public IActionResult editCatalog(EditCatalogDTO catalogDTO)
        {
            if (ModelState.IsValid)
            {


                Catalog catalog = unit.Generic_CatalogRepo.selectById(catalogDTO.CatalogId);

                if (catalog == null)
                {
                    return NotFound();
                }
                catalog.Description = catalogDTO.Description;
                catalog.Name = catalogDTO.Name;

                unit.Generic_CatalogRepo.update(catalog);
                unit.Save();


                return Ok();

            }
            else { return BadRequest(ModelState); }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult deleteCatalog(int id)
        {
            var catalog = unit.Generic_CatalogRepo.selectById(id);
            if (catalog == null) { return NotFound(); }
            unit.Generic_CatalogRepo.remove(id);
            return Ok();

        }


    }
}
