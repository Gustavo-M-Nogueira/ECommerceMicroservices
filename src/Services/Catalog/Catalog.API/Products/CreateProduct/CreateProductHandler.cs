﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string name, List<string> category, string description, string imageFile, decimal price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator() 
        { 
            RuleFor(c => c.name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.category).NotEmpty().WithMessage("Category is required");
            RuleFor(c => c.imageFile).NotEmpty().WithMessage("Image is required");
            RuleFor(c => c.price).NotEmpty().WithMessage("Price is required");
        }
    }

    internal class CreateProductCommandHandler
        (IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //create Product entity from command object

            var product = new Product
            {
                Name = command.name,
                Category = command.category,
                Description = command.description,
                ImageFile = command.imageFile,
                Price = command.price
            };

            //save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);


            //return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}
