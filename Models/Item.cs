namespace ShoppingCart.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public double Price { set; get; }
        public int Quantity { get; set; }
        public string ImageUrl { set; get; }
        public string Category { set; get; }
        public bool IsClearance { set; get; }
        public bool IsBestSeller { set; get; }
    }
}
