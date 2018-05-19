using Catalog.WebApi.Entities;

namespace Catalog.WebApi.Extensions
{
    public static class CatalogItemExtensions
    {
        public static void FillProductUrl( this CatalogItem item, string picBaseUrl, bool azureStorageEnabled )
        {
            item.ImageUri = azureStorageEnabled
                ? picBaseUrl + item.ImageFileName
                : picBaseUrl.Replace( "[0]", item.Id.ToString() );
        }
    }
}
