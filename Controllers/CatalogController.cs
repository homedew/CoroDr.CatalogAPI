using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Catalog.API.Models;
using Catalog.API.ViewModels;
using CoroDr.CatalogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {

        // need to update it for decoupling
        private readonly CatalogContext _coroDrCatalogContext;

        public CatalogController(CatalogContext context)
        {
            _coroDrCatalogContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET api/v1/catalog/items[?pageSize=1&pageIndex=10]
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginationItemsViewModel<CatalogList>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 12, [FromQuery] int pageIndex = 1)
        {
            //need to rewrite
            var totalItems = await _coroDrCatalogContext.CatalogLists.LongCountAsync();
            var itemsInPage = await _coroDrCatalogContext.CatalogLists
                                                        .Skip(pageSize * pageIndex)
                                                        .Take(pageSize)
                                                        .ToListAsync();
            var model = new PaginationItemsViewModel<CatalogList>(pageIndex, pageSize, totalItems, itemsInPage);

            return Ok(model);
        }

        // GET api/v1/catalog/items/1
        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CatalogList>> ItemByIdAsync(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var item = await _coroDrCatalogContext.CatalogLists.SingleOrDefaultAsync(t => t.Id == id);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        // GET api/v1/catalog/items/withname/abc[?pageIndex=1&pageSize=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginationItemsViewModel<CatalogList>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginationItemsViewModel<CatalogList>>> ItemByNameAsync(string name, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            long totalItems = await _coroDrCatalogContext.CatalogLists.Where(i => i.Name.StartsWith(name)).LongCountAsync();

            if (totalItems == 0)
            {
                return NotFound();
            }

            var itemsInPage = await _coroDrCatalogContext.CatalogLists.Where(i => i.Name.StartsWith(name))
                                                                      .Skip((pageIndex -1) * pageSize)
                                                                      .Take(pageSize)
                                                                      .ToListAsync();

            var model = new PaginationItemsViewModel<CatalogList>(pageIndex, pageSize, totalItems, itemsInPage);

            return model;
        }
    }
}

