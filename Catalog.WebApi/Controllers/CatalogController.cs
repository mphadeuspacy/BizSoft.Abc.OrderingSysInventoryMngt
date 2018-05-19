using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.WebApi.Entities;
using Catalog.WebApi.Extensions;
using Catalog.WebApi.Infrastructure;
using Catalog.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.WebApi.Controllers
{
    [Route( "api/v1/[controller]" )]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogDbContext _catalogDbContext;
        private readonly CatalogSettings _settings;

        public CatalogController( CatalogDbContext dbContext, IOptionsSnapshot<CatalogSettings> settings )
        {
            _catalogDbContext = dbContext ?? throw new ArgumentNullException( nameof( dbContext ) );

            _settings = settings.Value;

            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route( "items" )]
        [ProducesResponseType( typeof( PaginatedItemsViewModel<CatalogItem> ), (int)HttpStatusCode.OK )]
        [ProducesResponseType( typeof( IEnumerable<CatalogItem> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> Items( [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0, [FromQuery] string ids = null )
        {
            if (!string.IsNullOrEmpty( ids ))
            {
                return GetItemsByIds( ids );
            }

            var totalItems = await _catalogDbContext.CatalogItems.LongCountAsync();

            var itemsOnPage = await _catalogDbContext.CatalogItems
                .OrderBy( c => c.Name )
                .Skip( pageSize * pageIndex )
                .Take( pageSize )
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder( itemsOnPage );

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage );

            return Ok( model );
        }

        private IActionResult GetItemsByIds( string ids )
        {
            var numIds = ids.Split( ',' ).Select( id => (Ok: int.TryParse( id, out int x ), Value: x) );

            if (!numIds.All( nid => nid.Ok ))
            {
                return BadRequest( "ids value invalid. Must be comma-separated list of numbers" );
            }

            var idsToSelect = numIds.Select( id => id.Value );

            var items = _catalogDbContext.CatalogItems.Where( ci => idsToSelect.Contains( ci.Id ) ).ToList();

            items = ChangeUriPlaceholder( items );

            return Ok( items );

        }

        [HttpGet]
        [Route( "items/{id:int}" )]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        [ProducesResponseType( typeof( CatalogItem ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> GetItemById( int id )
        {
            if (id <= 0) return BadRequest();

            var item = await _catalogDbContext.CatalogItems.SingleOrDefaultAsync( ci => ci.Id == id );

            var baseUri = _settings.PicBaseUrl;

            var azureStorageEnabled = _settings.AzureStorageEnabled;

            item.FillProductUrl( baseUri, azureStorageEnabled );

            if (item != null)  return Ok( item );

            return NotFound();
        }

        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route( "[action]/withname/{name:minlength(1)}" )]
        [ProducesResponseType( typeof( PaginatedItemsViewModel<CatalogItem> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> Items( string name, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0 )
        {
            var totalItems = await _catalogDbContext.CatalogItems
                .Where( c => c.Name.StartsWith( name ) )
                .LongCountAsync();

            var itemsOnPage = await _catalogDbContext.CatalogItems
                .Where( c => c.Name.StartsWith( name ) )
                .Skip( pageSize * pageIndex )
                .Take( pageSize )
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder( itemsOnPage );

            var model = new PaginatedItemsViewModel<CatalogItem>(
                pageIndex, pageSize, totalItems, itemsOnPage );

            return Ok( model );
        }

        // GET api/v1/[controller]/items/type/1/brand/null[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route( "[action]/type/{catalogTypeId}/brand/{catalogBrandId}" )]
        [ProducesResponseType( typeof( PaginatedItemsViewModel<CatalogItem> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> Items( int? catalogTypeId, int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0 )
        {
            var root = (IQueryable<CatalogItem>)_catalogDbContext.CatalogItems;

            if (catalogTypeId.HasValue)
            {
                root = root.Where( ci => ci.CatalogTypeId == catalogTypeId );
            }

            if (catalogBrandId.HasValue)
            {
                root = root.Where( ci => ci.CatalogBrandId == catalogBrandId );
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip( pageSize * pageIndex )
                .Take( pageSize )
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder( itemsOnPage );

            var model = new PaginatedItemsViewModel<CatalogItem>(
                pageIndex, pageSize, totalItems, itemsOnPage );

            return Ok( model );
        }

        // GET api/v1/[controller]/CatalogTypes
        [HttpGet]
        [Route( "[action]" )]
        [ProducesResponseType( typeof( List<CatalogItem> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogDbContext.CatalogTypes.ToListAsync();

            return Ok( items );
        }

        // GET api/v1/[controller]/CatalogBrands
        [HttpGet]
        [Route( "[action]" )]
        [ProducesResponseType( typeof( List<CatalogItem> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogDbContext.CatalogBrands.ToListAsync();

            return Ok( items );
        }
        
        private List<CatalogItem> ChangeUriPlaceholder( List<CatalogItem> items )
        {
            var baseUri = _settings.PicBaseUrl;

            var azureStorageEnabled = _settings.AzureStorageEnabled;

            foreach (var item in items)
            {
                item.FillProductUrl( baseUri, azureStorageEnabled );
            }

            return items;
        }
    }
}
