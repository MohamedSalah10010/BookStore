using BookStore.DTOs.authorDTO;
using BookStore.DTOs.catalogDTO;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all catalogs", Description = "This endpoint retrieves all catalogs available in the store. Accessible by authenticated users.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of catalogs retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]

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
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get catalog by ID", Description = "This endpoint retrieves a catalog by its ID. Accessible by authenticated users.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Catalog details retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Catalog not found.")]

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
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new catalog", Description = "This endpoint creates a new catalog. Accessible only by users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Catalog created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request - Invalid data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
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
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Edit an existing catalog", Description = "This endpoint updates an existing catalog. Accessible only by users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Catalog updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request - Invalid data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Catalog not found.")]
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
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete a catalog by ID", Description = "This endpoint allows an admin to delete a catalog from the store by its ID. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Catalog deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Catalog not found.")]
        public IActionResult deleteCatalog(int id)
        {
            var catalog = unit.Generic_CatalogRepo.selectById(id);
            if (catalog == null) { return NotFound(); }
            unit.Generic_CatalogRepo.remove(id);
            return Ok();

        }


    }
}
