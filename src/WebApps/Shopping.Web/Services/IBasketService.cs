namespace Shopping.Web.Services
{
    public interface IBasketService
    {
        [Get("/basket-service/basket/{username}")]
        Task<GetBasketResponse> GetBasket(string username);

        [Post("/basket-service/basket")]
        Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

        [Post("/basket-service/basket/checkout")]
        Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

        [Delete("/basket-service/basket/checkout")]
        Task<DeleteBasketResponse> DeletBasket(string username);

        public async Task<ShoppingCartModel> LoadUserBasket()
        {
            // Get Basket, If not existe create new basket with default logged in user name
            var username = "swn";
            ShoppingCartModel basket;

            try
            {
                var getBasketResponse = await GetBasket(username);
                basket = getBasketResponse.Cart;
            }
            catch (Exception)
            {
                basket = new ShoppingCartModel
                {
                    Username = username,
                    Items = []
                };
            }

            return basket;
        }
    }
}
