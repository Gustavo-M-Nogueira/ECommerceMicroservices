
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool isSuccess);

    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.BasketCheckoutDto.Username).NotNull().WithMessage("Username is required");
        }
    }

    public class CheckoutBasketCommandHandler
        (IBasketRepository repository, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            // get existing basket with total price
            var basket = await repository.GetBasket(command.BasketCheckoutDto.Username, cancellationToken);
            if (basket == null)
            {
                return new CheckoutBasketResult(false);
            }

            // set totalprice on basketcheckout event message
            var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            // send basket checkout event to rabbitmq using masstransit
            await publishEndpoint.Publish(eventMessage, cancellationToken);

            // delete the basket
            await repository.DeleteBasket(command.BasketCheckoutDto.Username, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}
