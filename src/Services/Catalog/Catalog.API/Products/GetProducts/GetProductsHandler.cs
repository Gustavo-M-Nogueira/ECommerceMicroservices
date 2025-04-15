
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler.Handle call with {@Query}", query);

            var products = await session.Query<Product>().ToListAsync(cancellationToken);

            return new GetProductByIdResult(products);
        }
    }
}
