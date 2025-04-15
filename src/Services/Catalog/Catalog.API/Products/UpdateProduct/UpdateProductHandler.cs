using JasperFx.Core.Descriptions;
using System.Xml.Linq;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid id, string name, List<string> category, string description, string imageFile, decimal price)
       : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    internal class UpdateProductCommandHandler
        (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);

            var product = await session.LoadAsync<Product>(command.id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            product.Name = command.name;
            product.Category = command.category;
            product.Description = command.description;
            product.ImageFile = command.imageFile;
            product.Price = command.price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
