namespace ShoppingCart.Models
{
    public class Cart
    {
        public int Id { get; set; } // cart Id, assuming there is only one cart
        public int ItemId { get; set; } // Id of each item added to cart, corresponding to the Id of Item model
        public string Name { get; set; } // name of each item added to cart, corresponding to the Id of Item model
        public double Price { get; set; } // price of each item added to cart, corresponding to the Id of Item model
        public int Quantity { get; set; } // quantity of each item added to cart, corresponding to the Id of Item model
        public string ImageUrl { get; set; }
    }
}
