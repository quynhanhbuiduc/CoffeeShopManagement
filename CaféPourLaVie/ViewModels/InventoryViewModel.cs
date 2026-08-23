namespace CaféPourLaVie.ViewModels
{
    public class InventoryViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Status
        {
            get
            {
                if (Quantity == 0)
                    return "Hết hàng";

                if (Quantity < 10)
                    return "Sắp hết";

                return "Còn hàng";
            }
        }
    }
}